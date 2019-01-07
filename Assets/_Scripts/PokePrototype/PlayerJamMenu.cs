using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJamMenu : MonoBehaviour {
	public GameObject menuRow;
	public GameObject styleRow;
	public GameObject menuCursorPrefab;
	public GameObject styleButton;
	public bool isPlayerTurn = false;
	public PlayerMidiController playerMidiController;
	public bool startMuted = true;

	private Move[] moveSet;
	public CharacterJamController player;
	private int rowHeight = 12;
	public int menuIndex = 0;
	private GameObject menuCursor;
	enum List {MoveList,StyleList};
	private List list;
	private JamController jamController;
	private DialogueController dialogueController;
	// Use this for initialization
	void Start () {
		if(startMuted){
			playerMidiController.mute = true;
		}
		dialogueController = Object.FindObjectOfType<DialogueController>();
		PopulateMoveList();	
		InstantiateCursor();
	}

	void InstantiateCursor(){
		menuCursor = Object.Instantiate(menuCursorPrefab, new Vector3(transform.position.x - 90, transform.position.y + 10 - rowHeight + (rowHeight * moveSet.Length), transform.position.z), Quaternion.identity);
		menuCursor.transform.parent = gameObject.transform;
	}

	void Update(){
		if(Input.GetButtonDown("Up")){
			MoveCursorUp();
		}
		if(Input.GetButtonDown("Down")){
			MoveCursorDown();
		}
		if(Input.GetButtonDown("Select")){
			if(isPlayerTurn){
				OnSelect();
			}
		}
	}

	void OnSelect(){
		if(list == List.MoveList){
			if(menuIndex == moveSet.Length){
				PopulateStyleList();
			}else{
				Move currentMove = player.moveSets.moveSets[player.currentMoveSet].moves[menuIndex];
				if(currentMove.Pp > 0){
					ChangeMove(currentMove);
				}else{
					dialogueController.UpdateDialogue("Out of PP!",2);
				}
			}
		}else{
			if(menuIndex == player.moveSets.moveSets.Length){
				PopulateMoveList();
			}else{
				player.ChangeMoveSet(menuIndex);
				moveSet = player.moveSets.moveSets[player.currentMoveSet].moves;
				dialogueController.UpdateDialogue("You changed styles!",2);
				PopulateMoveList();
			}
		}
	}

	void ChangeMove(Move currentMove){
		playerMidiController.mute = false;
		player.SelectMove(menuIndex);
		playerMidiController.SetCurrentMidiFileWithName(currentMove.loopName);
		dialogueController.UpdateDialogue("You used " + currentMove.name + "!",2);
		UpdateMenuValues();
	}

	void MoveCursorUp(){
		if(menuIndex > 0){
			menuIndex -= 1;
			menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y + rowHeight,menuCursor.transform.position.z);
		}
	}

	void MoveCursorDown(){
		if(list == List.MoveList){
			if(menuIndex < moveSet.Length){
				menuIndex += 1;
				menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y - rowHeight,menuCursor.transform.position.z);
			}
		}else if(list == List.StyleList){
			if(menuIndex < player.moveSets.moveSets.Length){
				menuIndex += 1;
				menuCursor.transform.position = new Vector3(menuCursor.transform.position.x,menuCursor.transform.position.y - rowHeight,menuCursor.transform.position.z);
			}
		}
	}
	
	public void PopulateMoveList(){
		list = List.MoveList;
		menuIndex = 0;
		if (menuCursor != null){
				menuCursor.transform.position = new Vector3(transform.position.x - 100, transform.position.y + 8 - rowHeight + (rowHeight * moveSet.Length), transform.position.z);
			}
		foreach(Transform child in transform){
			if(child != menuCursor.transform){
				Destroy(child.gameObject);
			}
		}
		moveSet = player.moveSets.moveSets[player.currentMoveSet].moves;
		float yCoord = transform.position.y - rowHeight + (moveSet.Length * rowHeight);

		foreach(Move move in moveSet){
			var menuRowObject = GameObject.Instantiate(menuRow, new Vector3(transform.position.x, yCoord, transform.position.z), Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = move.name;
			newMenuRow.rowItems[1].text = move.power.ToString();
			newMenuRow.rowItems[2].text = move.emo.ToString();
			newMenuRow.rowItems[3].text = move.Pp.ToString();
			yCoord -= rowHeight;
		}

		var newStyleButton = Object.Instantiate(styleButton, new Vector3(transform.position.x, yCoord, transform.position.z), Quaternion.identity);
		newStyleButton.transform.parent = gameObject.transform;
	}

	void UpdateMenuValues(){
		int i = 0;
		foreach(Transform child in transform){
			if(i < player.moveSets.moveSets[player.currentMoveSet].moves.Length && child.gameObject.tag == "menuRow"){
			var move = player.moveSets.moveSets[player.currentMoveSet].moves[i];
			var menuRow = child.gameObject.GetComponent<JamMenuRow>();
			menuRow.rowItems[0].text = move.name;
			menuRow.rowItems[1].text = move.power.ToString();
			menuRow.rowItems[2].text = move.emo.ToString();
			menuRow.rowItems[3].text = move.Pp.ToString();
			i++;
			}
		}
	}

	public void PopulateStyleList(){
		list = List.StyleList;
		menuIndex = 0;
		if (menuCursor != null){
				menuCursor.transform.position = new Vector3(transform.position.x - 100, transform.position.y + 8 - rowHeight + (rowHeight * moveSet.Length), transform.position.z);
			}
		foreach(Transform child in transform){
			if(child != menuCursor.transform){
				Destroy(child.gameObject);
			}
		}
		float yCoord = transform.position.y - rowHeight + (moveSet.Length * rowHeight);
		var styles = player.moveSets.moveSets;
		foreach(MoveSet style in styles){
			var menuRowObject = Object.Instantiate(styleRow, new Vector3(transform.position.x, yCoord, transform.position.z), Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = style.name;
			yCoord -= rowHeight;
		}

		var newStyleButton = Object.Instantiate(styleButton, new Vector3(transform.position.x, yCoord, transform.position.z), Quaternion.identity);
		newStyleButton.transform.parent = gameObject.transform;
		newStyleButton.GetComponent<Text>().text = "Back";
	}
}
