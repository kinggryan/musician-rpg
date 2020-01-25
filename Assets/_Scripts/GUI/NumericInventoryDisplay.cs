using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NumericInventoryDisplay : MonoBehaviour
{
    public Color defaultColor;
    public Color highlightColor;
    public GameObject menuRow;
    public GameObject equippedMenuRow;
    public GameObject styleLabel;
    public GameObject equippedStyleLabel;
    private NumericInventory numericInventory;
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
    private float songListCoord;
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
    private int maxMoves = 10;
    private SongSelector songSelector;
    
    // Start is called before the first frame update
    void Start()
    {
        numericInventory = Object.FindObjectOfType<NumericInventory>();
        songSelector = Object.FindObjectOfType<SongSelector>();
        PopulateMoveList(0);
        PopulateSongList();
        //PopulateStyleList();
        //PopulateEquippedStyleList();
        //menuCursor = Object.FindObjectOfType<MenuCursor>();
        inventoryController = Object.FindObjectOfType<InventoryController>();
        songSelector = Object.FindObjectOfType<SongSelector>();
        SetMenuCursorPositionToDefault();
    }

    void Update(){
		if(Input.GetButtonDown("Up")){
			MoveCursorUp();
		}
		if(Input.GetButtonDown("Down")){
			MoveCursorDown();
		}
        if(Input.GetButtonDown("Right")){
            MoveCursorRight();
        }
        if(Input.GetButtonDown("Left")){
            MoveCursorLeft();
        }
        if(Input.GetButtonDown("Select")){
            Select();
        }
		if(Input.GetKeyDown("1")){
            Equip(0);
        } else if(Input.GetKeyDown("2")){
            Equip(1);
        } else if(Input.GetKeyDown("3")){
            Equip(2);
        } else if(Input.GetKeyDown("4")){
            Equip(3);
        } else if(Input.GetKeyDown("5")){
            Equip(4);
        } else if(Input.GetKeyDown("6")){
            Equip(5);
        } else if(Input.GetKeyDown("7")){
            Equip(6);
        } else if(Input.GetKeyDown("8")){
            Equip(7);
        } else if(Input.GetKeyDown("9")){
            Equip(8);
        } else if(Input.GetKeyDown("0")){
            Equip(9);
        }
	}

    void Select(){
        songSelector.ChangeSong(numericInventory.learnedSongs[menuCursor.y]);
        UpdateMovesAndSongs();
    }

    void SetMenuCursorPositionToDefault(){
        menuCursor.transform.localPosition = new Vector3(row1XCoord + cursorXOffSet,cursorYOffSet, menuCursor.transform.position.z);
    }

    void Equip(int key){
        numericInventory.EquipMove(numericInventory.knownMoves[menuCursor.y], key);
    }


    void StartMusicalEncounter(){
        Debug.Log("Starting musical encounter");
        PersistentInfo persistenInfo = Object.FindObjectOfType<PersistentInfo>();
        persistenInfo.activeMoves = numericInventory.activeMoves;
        SceneManager.LoadScene(1);
    }


    void MoveCursorUp(){
        if(menuCursor.y > 0){
            menuCursor.y -= 1;
            menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y + rowHeight, menuCursor.transform.localPosition.z);
        }
	}

	void MoveCursorDown(){
        if(menuCursor.x == 0 && menuCursor.y < noOfMovesInStyle - 1){
            menuCursor.y += 1;
            menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y - rowHeight, menuCursor.transform.localPosition.z);
        }else if(menuCursor.x == 1 & menuCursor.y < numericInventory.learnedSongs.Count - 1){
            menuCursor.y += 1;
            menuCursor.transform.localPosition = new Vector3(menuCursor.transform.localPosition.x,menuCursor.transform.localPosition.y - rowHeight, menuCursor.transform.localPosition.z);
        };
	}

    void MoveCursorLeft(){
        if(menuCursor.x == 1){
            menuCursor.x -= 1;
            if (menuCursor.y + 1 > noOfMovesInStyle){
                int distanceToMove = (menuCursor.y + 1) - noOfMovesInStyle;
                menuCursor.transform.localPosition = new Vector3(row1XCoord + cursorXOffSet,menuCursor.transform.localPosition.y + (rowHeight * distanceToMove), menuCursor.transform.localPosition.z);
                menuCursor.y = noOfMovesInStyle - 1;
            }else{
                menuCursor.transform.localPosition = new Vector3(row1XCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
            }
            
        }
    }

    void MoveCursorRight(){
        int noOfLearnedSongs = numericInventory.learnedSongs.Count;
        Debug.Log("NoOfSongs: " + noOfLearnedSongs + ". MenuY: " + menuCursor.y);
        if(menuCursor.x == 0){
            menuCursor.x += 1;
            if (menuCursor.y + 1> noOfLearnedSongs){
                int distanceToMove = (menuCursor.y + 1) - noOfLearnedSongs;
                menuCursor.transform.localPosition = new Vector3(songListCoord + cursorXOffSet,menuCursor.transform.localPosition.y + (rowHeight * distanceToMove), menuCursor.transform.localPosition.z);
                menuCursor.y = noOfLearnedSongs - 1;
            }else{
                menuCursor.transform.localPosition = new Vector3(songListCoord + cursorXOffSet, menuCursor.transform.localPosition.y, menuCursor.transform.localPosition.z);
            }
            
        }
    }

    void PopulateSongList(){
        // foreach(Transform child in transform){
        //     if(child.gameObject.tag == "menuRow"){
        //         Destroy(child.gameObject);
        //     }
        // }
        
        float yCoord = transform.localPosition.y;
        string activeSoloSong = songSelector.playerJamMenu.soloSong;
        foreach(string song in numericInventory.learnedSongs){
            JamMenuRow newMenuRow = CreateNewMenuRow(songListCoord, yCoord, song, "", "", "", null);
            yCoord = GetNextYCoord(yCoord);
            if("Songs/" + song == activeSoloSong){
                foreach(Text text in newMenuRow.rowItems){
                    text.color = highlightColor;
                }
            }
        }
    }

    public void UpdateMovesAndSongs(){
        PopulateMoveList(0);
        PopulateSongList();
        
    }

    int MoveIndex(){
        return knowMoveIndexInit + menuCursor.y;
    }

    int noOfMovesInSelectedStyle(){
        if(numericInventory.activeMoves.Length <= selectedStyle){
            return 0;
        }else{
            return numericInventory.activeMoves[selectedStyle].moves.Length;
        }
    }

    int noOfEquippedStyles(){
        int count = 0;
        foreach (MoveSet style in numericInventory.activeMoves){
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
            styleItem.GetComponent<Text>().text = numericInventory.equippedStyles[i];
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
        if(style < numericInventory.activeMoves.Length){
        string styleName =  numericInventory.activeMoves[style].name;
        Debug.Log("Populating equipped moves for style " + styleName);
        foreach(Transform child in transform){
            if(child.gameObject.tag == "equippedMenuRow"){
                Destroy(child.gameObject);
            }
        }
        
        float yCoord = transform.localPosition.y;
        int moveIndex = 0;
        foreach(Move move in numericInventory.activeMoves[style].moves){
            moveIndex += 1;
            if(move.style == styleName){
                Debug.Log("Label for equipped move " + move.name);
                activeMoveIndexInit = moveIndex;
                JamMenuRow newMenuRow = CreateNewEquippedMenuRow(row4XCoord, yCoord, move.name, move.power.ToString(), move.Pp.ToString());
                yCoord = GetNextYCoord(yCoord);
            }
            
        }
        activeMoveIndexInit = activeMoveIndexInit - numericInventory.noOfEquippedMovesInStyle(style);
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
        for (int i = 0; i < numericInventory.styles.Count; i++){
            Vector3 itemPosition = new Vector3(row1XCoord, yCoord + styleLabelOffset, transform.position.z);
            GameObject styleItem = GameObject.Instantiate(styleLabel,itemPosition, Quaternion.identity);
            styleItem.transform.parent = gameObject.transform;
            styleItem.transform.localPosition = itemPosition;
            styleItem.GetComponent<Text>().text = numericInventory.styles[i];
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
        foreach(Move move in numericInventory.knownMoves){
            moveIndex += 1;
            knowMoveIndexInit = moveIndex;
            JamMenuRow newMenuRow = CreateNewMenuRow(row1XCoord, yCoord, move.name, move.power.ToString(), move.Pp.ToString(), move.equipKey, move.icon);
            yCoord = GetNextYCoord(yCoord);
            noOfMovesInStyle += 1;
        }
        //knowMoveIndexInit = knowMoveIndexInit - noOfMovesInStyle;
	}



    JamMenuRow CreateNewMenuRow(float xCoord, float yCoord, string item1, string item2, string item3, string item4, Sprite icon){
        Vector3 itemPosition = new Vector3(xCoord, yCoord, transform.position.z);
		var menuRowObject = GameObject.Instantiate(menuRow, itemPosition, Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
            menuRowObject.transform.localPosition = itemPosition;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = item1;
			newMenuRow.rowItems[1].text = item2;
			newMenuRow.rowItems[2].text = item3;
            newMenuRow.rowItems[3].text = item4;
            newMenuRow.icon.sprite = icon;
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
