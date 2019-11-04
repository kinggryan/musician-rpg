using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJamMenu : MonoBehaviour {
	public string soloSong;
	public GameObject menuRow;
	public GameObject styleRow;
	public GameObject menuCursorPrefab;
	public GameObject styleButton;
	public bool isPlayerTurn = false;
	public PlayerMidiController playerMidiController;
	public bool startMuted = true;
	public bool controlsDisabled;
	//public GameObject chordSelector;
	public Animator animator;
	[SerializeField]
	private int menuCursorOffset;
	[SerializeField]
	private Vector2 styleButtonOffset;
	private Move[] moveSet;
	public CharacterJamController player;
	private int rowHeight = 12;
	public int menuIndex = 0;
	private GameObject menuCursor;
	enum List {MoveList,StyleList};
	private List list;
	public JamController jamController;
	private DialogueController dialogueController;
	private EmotionManager emoManager;
	
	// Use this for initialization
	void Start () {
		emoManager = FindObjectOfType<EmotionManager>();
		if(startMuted){
			MutePlayer();
		}
		dialogueController = Object.FindObjectOfType<DialogueController>();
		jamController = Object.FindObjectOfType<JamController>();
		PopulateMoveList();	
		InstantiateCursor();
	}

	void InstantiateCursor(){
		menuCursor = Object.Instantiate(menuCursorPrefab, new Vector3(transform.position.x + menuCursorOffset, transform.position.y + 10 - (rowHeight * 2) + (rowHeight * moveSet.Length), transform.position.z), Quaternion.identity);
		menuCursor.transform.parent = gameObject.transform;
	}

	public void MutePlayer(){
		playerMidiController.mute = true;
	}

	void Update(){
		if(!controlsDisabled){
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
			// if(Input.GetKeyDown("backspace")){
			// 	StopSong();
			// 	jamController.firstMove = true;
			// 	controlsDisabled = true;
			// 	chordSelector.SetActive(true);
			// }
		}
	}

	public void StartSong(){
		playerMidiController.midiPlayer.Play();
	}

	public void StopSong(){
		playerMidiController.midiPlayer.Stop();
		player.jamController.firstMove = true;
		if(startMuted){
			MutePlayer();
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
		if(playerMidiController.mute = true){
			animator.SetBool("playing", true);
			playerMidiController.mute = false;
		}
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

	void ResetMenuCursorPosition(){
		menuIndex = 0;
		if (menuCursor != null){
			menuCursor.transform.position = new Vector3(transform.position.x + menuCursorOffset, transform.position.y + 8 - (rowHeight * 2) + (rowHeight * moveSet.Length), transform.position.z);
		}
		RemoveErrantChildren();
	}

	
	
	public void PopulateMoveList(){
		list = List.MoveList;
		ResetMenuCursorPosition();
		moveSet = player.moveSets.moveSets[player.currentMoveSet].moves;
		float yCoord = transform.position.y - rowHeight + (moveSet.Length * rowHeight);
//		Debug.Log("Creating menu header");
		JamMenuRow menuHeader = CreateNewMenuRow(yCoord,"Pattern","Power","PP");
		yCoord = GetNextYCoord(yCoord);
		foreach(Move move in moveSet){
			JamMenuRow newMenuRow = CreateNewMenuRow(yCoord, move.name, move.power.ToString(), move.Pp.ToString());
			ColorCodeMenuRow(newMenuRow, move);
			yCoord = GetNextYCoord(yCoord);
		}
		GameObject styleButton = CreateStyleButton(yCoord);
	}

	JamMenuRow CreateNewMenuRow(float yCoord, string item1, string item2, string item3){
		var menuRowObject = GameObject.Instantiate(menuRow, new Vector3(transform.position.x, yCoord, transform.position.z), Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = item1;
			newMenuRow.rowItems[1].text = item2;
			newMenuRow.rowItems[2].text = item3;
			yCoord -= rowHeight;
			return newMenuRow;
	}

	float GetNextYCoord(float yCoord){
		yCoord -= rowHeight;
		return yCoord;
	}

	void ColorCodeMenuRow(JamMenuRow menuRow, Move move){
		foreach(Text text in menuRow.rowItems){
			text.color = emoManager.GetEmoColor(move.emo);
		}
	}

	GameObject CreateStyleButton(float yCoord){
		var newStyleButton = GameObject.Instantiate(styleButton, new Vector3(transform.position.x + styleButtonOffset.x, yCoord + styleButtonOffset.y, transform.position.z), Quaternion.identity);
		newStyleButton.transform.parent = gameObject.transform;
		return newStyleButton;
		Debug.Log("Style button made");
	}

	void UpdateMenuValues(){
		int i = 0;
		int moveSetLength = player.moveSets.moveSets[player.currentMoveSet].moves.Length;
		foreach(Transform child in transform){
			if(i == 0){
				i++;
			}
			else if(i < moveSetLength && child.gameObject.tag == "menuRow"){
			var move = player.moveSets.moveSets[player.currentMoveSet].moves[i-1];
			var menuRow = child.gameObject.GetComponent<JamMenuRow>();
			menuRow.rowItems[0].text = move.name;
			menuRow.rowItems[1].text = move.power.ToString();
			menuRow.rowItems[2].text = move.Pp.ToString();
			i++;
			}
		}
	}

	public void PopulateStyleList(){
		list = List.StyleList;
		ResetMenuCursorPosition();
		float yCoord = transform.position.y - (rowHeight*2) + (moveSet.Length * rowHeight);
		var styles = player.moveSets.moveSets;
		foreach(MoveSet style in styles){
			JamMenuRow newMenuRow = CreateNewStyleMenuRow(yCoord,style.name);
			yCoord = GetNextYCoord(yCoord);
		}
		GameObject backButton = CreateStyleButton(yCoord);
		backButton.GetComponent<Text>().text = "Back";
	}

	void RemoveErrantChildren(){
		foreach(Transform child in transform){
			if(child != menuCursor.transform){
				Destroy(child.gameObject);
			}
		}
	}

	JamMenuRow CreateNewStyleMenuRow(float yCoord,string text){
			var menuRowObject = Object.Instantiate(styleRow, new Vector3(transform.position.x + styleButtonOffset.x, yCoord + styleButtonOffset.y, transform.position.z), Quaternion.identity);
			menuRowObject.transform.parent = gameObject.transform;
			JamMenuRow newMenuRow = menuRowObject.GetComponent<JamMenuRow>();
			newMenuRow.rowItems[0].text = text;
			return newMenuRow;
		}
}
