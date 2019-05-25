using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float rowWidth = 40;
    private int noOfMovesInStyle;
    private int noOfMovesEquippedInStyle;
    int knowMoveIndexInit = 0;
    int activeMoveIndexInit = 0;
    private int selectedStyle;
    private List<Text> equippedStyles = new List<Text>();
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = Object.FindObjectOfType<Inventory>();
        PopulateMoveList(1);
        PopulateStyleList();
        PopulateEquippedStyleList();
        menuCursor = Object.FindObjectOfType<MenuCursor>();
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


    void MoveCursorUp(){
        if(menuCursor.y > 0){
            menuCursor.y -= 1;
            menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y + rowHeight, menuCursor.transform.position.z);
        }
	}

	void MoveCursorDown(){
        if(menuCursor.x == 0){
			if(menuCursor.y < inventory.styles.Count - 1){
				menuCursor.y += 1;
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y - rowHeight, menuCursor.transform.position.z);
            }
        }else if(menuCursor.x == 1){
            if(menuCursor.y < noOfMovesInStyle - 1){
                menuCursor.y += 1;
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y - rowHeight, menuCursor.transform.position.z);
            }
        }else if (menuCursor.x == 2){
            if(menuCursor.y < noOfEquippedStyles() - 1){
                menuCursor.y += 1;
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y - rowHeight, menuCursor.transform.position.z);
            }
        }else if (menuCursor.x == 3){
            if(menuCursor.y < noOfMovesInSelectedStyle() - 1){
                menuCursor.y += 1;
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y - rowHeight, menuCursor.transform.position.z);
            }
        }
	}

    void MoveCursorLeft(){
        if(menuCursor.x == 1){
            menuCursor.x -= 1;
            menuCursor.transform.position = new Vector3(row1XCoord, menuCursor.transform.position.y, menuCursor.transform.position.z);
            if (menuCursor.y > inventory.styles.Count){
                int distanceToMove = menuCursor.y - inventory.styles.Count;
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y + (rowHeight * distanceToMove), menuCursor.transform.position.z);
                menuCursor.y = inventory.styles.Count;
            }
        }else if(menuCursor.x == 2){
            menuCursor.x -= 1;
            menuCursor.transform.position = new Vector3(row2XCoord, menuCursor.transform.position.y, menuCursor.transform.position.z);
            if (menuCursor.y > noOfMovesInStyle -1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfMovesInStyle - 1);
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y + (rowHeight * distanceToMove), menuCursor.transform.position.z);
                menuCursor.y = noOfMovesInStyle -1;
            }
        }else if(menuCursor.x == 3){
            menuCursor.x -= 1;
            menuCursor.transform.position = new Vector3(row3XCoord, menuCursor.transform.position.y, menuCursor.transform.position.z);
        }
    }

    void MoveCursorRight(){
        if(menuCursor.x == 0){
            menuCursor.x += 1;
            menuCursor.transform.position = new Vector3(row2XCoord, menuCursor.transform.position.y, menuCursor.transform.position.z);
            if (menuCursor.y > noOfMovesInStyle -1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfMovesInStyle - 1);
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y + (rowHeight * distanceToMove), menuCursor.transform.position.z);
                menuCursor.y = noOfMovesInStyle -1;
            }
        }else if(menuCursor.x == 1 && noOfEquippedStyles() > 0){
            menuCursor.x += 1;
            menuCursor.transform.position = new Vector3(row3XCoord, menuCursor.transform.position.y, menuCursor.transform.position.z);
            if (menuCursor.y > noOfEquippedStyles() -1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfEquippedStyles() - 1);
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y + (rowHeight * distanceToMove), menuCursor.transform.position.z);
                menuCursor.y = noOfEquippedStyles() -1;
            }
        }else if(menuCursor.x == 2 && noOfMovesInSelectedStyle() > 0){
            menuCursor.x += 1;
            menuCursor.transform.position = new Vector3(row4XCoord, menuCursor.transform.position.y, menuCursor.transform.position.z);
            Debug.Log("noOfMovesInSelsscted:" + noOfMovesInSelectedStyle());
            if (menuCursor.y > noOfMovesInSelectedStyle() - 1){
                //Debug.Log("Too much thing!!");
                int distanceToMove = menuCursor.y - (noOfMovesInSelectedStyle() - 1);
                menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y + (rowHeight * distanceToMove), menuCursor.transform.position.z);
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
        float yCoord = transform.position.y;
        foreach(Transform child in transform){
            if(child.gameObject.tag == "equippedStyleLabel"){
                Destroy(child.gameObject);
            }
        }
        equippedStyles = new List<Text>();
        for (int i = 0; i < noOfEquippedStyles(); i++){
            GameObject styleItem = GameObject.Instantiate(equippedStyleLabel,new Vector3(transform.position.x + row3XCoord, yCoord + styleLabelOffset, transform.position.z), Quaternion.identity);
            styleItem.transform.parent = gameObject.transform;
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
        
        float yCoord = transform.position.y;
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
        float yCoord = transform.position.y;
        for (int i = 0; i < inventory.styles.Count; i++){
            GameObject styleItem = GameObject.Instantiate(styleLabel,new Vector3(transform.position.x, yCoord + styleLabelOffset, transform.position.z), Quaternion.identity);
            styleItem.transform.parent = gameObject.transform;
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
        
        float yCoord = transform.position.y;
        int moveIndex = 0;
        noOfMovesInStyle = 0;
        foreach(Move move in inventory.knownMoves){
            moveIndex += 1;
            if(move.style == inventory.styles[style]){
                knowMoveIndexInit = moveIndex;
                JamMenuRow newMenuRow = CreateNewMenuRow(transform.position.x + rowWidth, yCoord, move.name, move.power.ToString(), move.Pp.ToString());
                yCoord = GetNextYCoord(yCoord);
                noOfMovesInStyle += 1;
            }
            
        }
        knowMoveIndexInit = knowMoveIndexInit - noOfMovesInStyle;
	}



    JamMenuRow CreateNewMenuRow(float xCoord, float yCoord, string item1, string item2, string item3){
		var menuRowObject = GameObject.Instantiate(menuRow, new Vector3(xCoord, yCoord, transform.position.z), Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = item1;
			newMenuRow.rowItems[1].text = item2;
			newMenuRow.rowItems[2].text = item3;
			//yCoord -= rowHeight;
			return newMenuRow;
	}

    JamMenuRow CreateNewEquippedMenuRow(float xCoord, float yCoord, string item1, string item2, string item3){
		var menuRowObject = GameObject.Instantiate(equippedMenuRow, new Vector3(xCoord, yCoord, transform.position.z), Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
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
