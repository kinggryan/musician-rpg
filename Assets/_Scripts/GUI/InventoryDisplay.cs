using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryDisplay : MonoBehaviour
{
    public Color defaultColor;
    public Color highlightColor;
    public GameObject menuRow;
    public GameObject equippedMenuRow;
    public GameObject styleLabel;
    public GameObject equippedStyleLabel;
    private Inventory inventory;
    public MenuCursor menuCursor;
    [SerializeField]
    private float row1XCoord;
    [SerializeField]
    private float row2XCoord;
    [SerializeField]
    private float row3XCoord;
    [SerializeField]
    private float row4XCoord;
    [SerializeField]
    private float styleLabelOffset = 11;
    [SerializeField]
    private float rowHeight = 20;
    [SerializeField]
    
    private float equippedStyleLabelOffset = 64;
    [SerializeField]
    private float cursorXOffSet;
    [SerializeField]
    private float cursorYOffSet;
    private float rowWidth = 40;
    private int noOfMovesInStyle;
    private int noOfMovesEquippedInStyle;
    int knowMoveIndexInit = 0;
    int activeMoveIndexInit = 0;
    private int selectedStyle;
    private List<Text> equippedStyles = new List<Text>();
    private InventoryController inventoryController;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = Object.FindObjectOfType<Inventory>();
        PopulateMoveList(1);
        PopulateStyleList();
        PopulateEquippedStyleList();
        menuCursor = Object.FindObjectOfType<MenuCursor>();
        inventoryController = Object.FindObjectOfType<InventoryController>();
        SetMenuCursorPositionToDefault();
    }

    void Update(){
		if(Input.GetButtonDown("Up")){
			MoveCursorUp();
		}
		if(Input.GetButtonDown("Down")){
			MoveCursorDown();
		}
        if(Input.GetButtonDown("Left")){
			MoveCursorLeft();
		}
		if(Input.GetButtonDown("Right")){
			MoveCursorRight();
		}
		if(Input.GetButtonDown("Select")){
			OnSelect();
		}
        if(Input.GetButtonDown("Space")){
            StartMusicalEncounter();
            inventoryController.ToggleInventory();
        }
	}

    void SetMenuCursorPositionToDefault(){
        menuCursor.transform.localPosition = new Vector3(row1XCoord + cursorXOffSet,cursorYOffSet, menuCursor.transform.position.z);
    }

    void OnSelect(){
        if(menuCursor.x == 0){
            PopulateMoveList(menuCursor.y);
            selectedStyle = menuCursor.y;    
        }else if (menuCursor.x ==1){
            inventory.EquipMove(inventory.knownMoves[MoveIndex()]);
            PopulateEquippedStyleList();
            PopulateEquippedMoveList(selectedStyle);
            // HighlightSelectedStyle();
        }else if (menuCursor.x == 2){
            PopulateEquippedMoveList(menuCursor.y);
            selectedStyle = menuCursor.y;
            // HighlightSelectedStyle();
        }
        else if (menuCursor.x == 3){
            inventory.UnequipMove(inventory.activeMoves[selectedStyle].moves[menuCursor.y],selectedStyle);
            PopulateEquippedStyleList();
            PopulateEquippedMoveList(selectedStyle);
            ResetCursorAfterUnequip();
        }
		
	}

    void StartMusicalEncounter(){
        Debug.Log("Starting musical encounter");
        PersistentInfo persistenInfo = Object.FindObjectOfType<PersistentInfo>();
        persistenInfo.activeMoves = inventory.activeMoves;
        SceneManager.LoadScene(1);
    }


    void MoveCursorUp(){
        if(menuCursor.y > 0){
            menuCursor.y -= 1;
            menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y + rowHeight, menuCursor.transform.localPosition.z);
        }
	}

	void MoveCursorDown(){
        if(menuCursor.x == 0){
			if(menuCursor.y < inventory.styles.Count - 1){
				menuCursor.y += 1;
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y - rowHeight, menuCursor.transform.localPosition.z);
            }
        }else if(menuCursor.x == 1){
            if(menuCursor.y < noOfMovesInStyle - 1){
                menuCursor.y += 1;
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y - rowHeight, menuCursor.transform.localPosition.z);
            }
        }else if (menuCursor.x == 2){
            if(menuCursor.y < noOfEquippedStyles() - 1){
                menuCursor.y += 1;
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y - rowHeight, menuCursor.transform.localPosition.z);
            }
        }else if (menuCursor.x == 3){
            if(menuCursor.y < noOfMovesInSelectedStyle() - 1){
                menuCursor.y += 1;
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y - rowHeight, menuCursor.transform.localPosition.z);
            }
        }
	}

    void MoveCursorLeft(){
        if(menuCursor.x == 1){
            menuCursor.x -= 1;
            menuCursor.transform.localPosition = new Vector3(row1XCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
            if (menuCursor.y > inventory.styles.Count){
                int distanceToMove = menuCursor.y - inventory.styles.Count;
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x + cursorXOffSet,menuCursor.transform.localPosition.y + (rowHeight * distanceToMove), menuCursor.transform.localPosition.z);
                menuCursor.y = inventory.styles.Count;
            }
        }else if(menuCursor.x == 2){
            menuCursor.x -= 1;
            menuCursor.transform.localPosition = new Vector3(row2XCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
            if (menuCursor.y > noOfMovesInStyle -1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfMovesInStyle - 1);
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y + (rowHeight * distanceToMove), menuCursor.transform.localPosition.z);
                menuCursor.y = noOfMovesInStyle -1;
            }
        }else if(menuCursor.x == 3){
            menuCursor.x -= 1;
            menuCursor.transform.localPosition = new Vector3(row3XCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
        }
    }

    void MoveCursorRight(){
        if(menuCursor.x == 0){
            menuCursor.x += 1;
            menuCursor.transform.localPosition = new Vector3(row2XCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
            if (menuCursor.y > noOfMovesInStyle -1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfMovesInStyle - 1);
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y + (rowHeight * distanceToMove), menuCursor.transform.localPosition.z);
                menuCursor.y = noOfMovesInStyle -1;
            }
        }else if(menuCursor.x == 1 && noOfEquippedStyles() > 0){
            menuCursor.x += 1;
            menuCursor.transform.localPosition = new Vector3(row3XCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
            if (menuCursor.y > noOfEquippedStyles() -1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfEquippedStyles() - 1);
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y + (rowHeight * distanceToMove), menuCursor.transform.localPosition.z);
                menuCursor.y = noOfEquippedStyles() -1;
            }
        }else if(menuCursor.x == 2 && noOfMovesInSelectedStyle() > 0){
            menuCursor.x += 1;
            menuCursor.transform.localPosition = new Vector3(row4XCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
            Debug.Log("noOfMovesInSelsscted:" + noOfMovesInSelectedStyle());
            if (menuCursor.y > noOfMovesInSelectedStyle() - 1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfMovesInSelectedStyle() - 1);
                menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y + (rowHeight * distanceToMove), menuCursor.transform.localPosition.z);
                menuCursor.y = noOfMovesInSelectedStyle() -1;
            }
        }
    }
    int MoveIndex(){
        return knowMoveIndexInit + menuCursor.y;
    }

    int noOfMovesInSelectedStyle(){
        if(inventory.activeMoves.Length <= selectedStyle){
            return 0;
        }else{
            return inventory.activeMoves[selectedStyle].moves.Length;
        }
    }

    int noOfEquippedStyles(){
        int count = 0;
        foreach (MoveSet style in inventory.activeMoves){
            count ++;
        }
        return count;
    }

    public void PopulateEquippedStyleList(){
        float yCoord = transform.localPosition.y;
        foreach(Transform child in transform){
            if(child.gameObject.tag == "equippedStyleLabel"){
                Destroy(child.gameObject);
            }
        }
        equippedStyles = new List<Text>();
        for (int i = 0; i < noOfEquippedStyles(); i++){
            Vector3 itemPosition = new Vector3(transform.localPosition.x + row3XCoord + equippedStyleLabelOffset, yCoord + styleLabelOffset, transform.localPosition.z);
            GameObject styleItem = GameObject.Instantiate(equippedStyleLabel,itemPosition, Quaternion.identity);
            styleItem.transform.parent = gameObject.transform;
            styleItem.transform.localPosition = itemPosition;
            styleItem.GetComponent<Text>().text = inventory.equippedStyles[i];
            equippedStyles.Add(styleItem.GetComponent<Text>());
            yCoord = GetNextYCoord(yCoord);
            
        }
    }

    void HighlightSelectedStyle(){
        for (int i = 0; i < equippedStyles.Count; i++){
            if(i == selectedStyle){
                equippedStyles[i].color = highlightColor;
            }else{
               equippedStyles[i].color = defaultColor;
            }
        }
    }

    void ResetCursorAfterUnequip(){
        if(noOfMovesInSelectedStyle() > 0){
            if(menuCursor.y >= noOfMovesInSelectedStyle()){
                MoveCursorUp();
            }
        }else{
            MoveCursorLeft();
            MoveCursorLeft();
        }
    }

    public void PopulateEquippedMoveList(int style){
        if(style < inventory.activeMoves.Length){
        string styleName =  inventory.activeMoves[style].name;
        Debug.Log("Populating equipped moves for style " + styleName);
        foreach(Transform child in transform){
            if(child.gameObject.tag == "equippedMenuRow"){
                Destroy(child.gameObject);
            }
        }
        
        float yCoord = transform.localPosition.y;
        int moveIndex = 0;
        foreach(Move move in inventory.activeMoves[style].moves){
            moveIndex += 1;
            if(move.style == styleName){
                Debug.Log("Label for equipped move " + move.name);
                activeMoveIndexInit = moveIndex;
                JamMenuRow newMenuRow = CreateNewEquippedMenuRow(row4XCoord, yCoord, move.name, move.power.ToString(), move.Pp.ToString());
                yCoord = GetNextYCoord(yCoord);
            }
            
        }
        activeMoveIndexInit = activeMoveIndexInit - inventory.noOfEquippedMovesInStyle(style);
        }else{
            foreach(Transform child in transform){
                if(child.gameObject.tag == "equippedMenuRow"){
                    Destroy(child.gameObject);
                }
            }
        }
	}

    public void PopulateStyleList(){
        float yCoord = transform.localPosition.y;
        for (int i = 0; i < inventory.styles.Count; i++){
            Vector3 itemPosition = new Vector3(row1XCoord, yCoord + styleLabelOffset, transform.position.z);
            GameObject styleItem = GameObject.Instantiate(styleLabel,itemPosition, Quaternion.identity);
            styleItem.transform.parent = gameObject.transform;
            styleItem.transform.localPosition = itemPosition;
            styleItem.GetComponent<Text>().text = inventory.styles[i];
            yCoord = GetNextYCoord(yCoord);
        }
    }

    public void PopulateMoveList(int style){
        foreach(Transform child in transform){
            if(child.gameObject.tag == "menuRow"){
                Destroy(child.gameObject);
            }
        }
        
        float yCoord = transform.localPosition.y;
        int moveIndex = 0;
        noOfMovesInStyle = 0;
        foreach(Move move in inventory.knownMoves){
            moveIndex += 1;
            if(move.style == inventory.styles[style]){
                knowMoveIndexInit = moveIndex;
                JamMenuRow newMenuRow = CreateNewMenuRow(row2XCoord, yCoord, move.name, move.power.ToString(), move.Pp.ToString());
                yCoord = GetNextYCoord(yCoord);
                noOfMovesInStyle += 1;
            }
            
        }
        knowMoveIndexInit = knowMoveIndexInit - noOfMovesInStyle;
	}



    JamMenuRow CreateNewMenuRow(float xCoord, float yCoord, string item1, string item2, string item3){
        Vector3 itemPosition = new Vector3(xCoord, yCoord, transform.position.z);
		var menuRowObject = GameObject.Instantiate(menuRow, itemPosition, Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
            menuRowObject.transform.localPosition = itemPosition;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = item1;
			newMenuRow.rowItems[1].text = item2;
			newMenuRow.rowItems[2].text = item3;
			//yCoord -= rowHeight;
			return newMenuRow;
	}

    JamMenuRow CreateNewEquippedMenuRow(float xCoord, float yCoord, string item1, string item2, string item3){
        Vector3 itemPosition = new Vector3(xCoord, yCoord, transform.position.z);
		var menuRowObject = GameObject.Instantiate(equippedMenuRow, itemPosition, Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
            menuRowObject.transform.localPosition = itemPosition;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = item1;
			newMenuRow.rowItems[1].text = item2;
			newMenuRow.rowItems[2].text = item3;
			//yCoord -= rowHeight;
			return newMenuRow;
	}

    float GetNextYCoord(float yCoord){
		yCoord -= rowHeight;
		return yCoord;
	}
}
