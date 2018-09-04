using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatTracker : MonoBehaviour {

	/// <summary>
	/// The beat number is 0-indexed
	/// </summary>
	public int beatNumber;
	public UnityEngine.UI.Image currentBeatImage;
	public UnityEngine.UI.Image previousBeatImage;

	public Sprite[] currentBeatSprites;
	public Sprite[] previousBeatSprites;

	private Sprite nullCurrentBeatSprite;
	private Sprite nullPreviousBeatSprite;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.setPlayerLoopForBeat, DidSetPlayerLoopForBeat);
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.setPreviousPhrasePlayerLoops, DidUpdatePreviousLoopBeats);
		nullCurrentBeatSprite = currentBeatImage.sprite;
		nullPreviousBeatSprite = previousBeatImage.sprite;
		animator = GetComponent<Animator>();
	}
	
	void DidSetPlayerLoopForBeat(object sender, object arg) {
		var playerMoveAndBeat = (RPGGameplayManger.Notifications.SetPlayerLoopForBeatArgs)arg;
		if(playerMoveAndBeat.beatNumber % 8 == beatNumber) {
			currentBeatImage.sprite = currentBeatSprites[playerMoveAndBeat.playerMoveIndex];
			animator.SetTrigger("pulse");
		} else if(playerMoveAndBeat.beatNumber % 8 == 0) {
			currentBeatImage.sprite = nullCurrentBeatSprite;
		}
	}

	void DidUpdatePreviousLoopBeats(object sender, object arg) {
		var playerMoveAndBeat = (RPGGameplayManger.Notifications.SetPreviousPhrasePlayerLoopsArgs)arg;
		var index = playerMoveAndBeat.playerMoveIndices[beatNumber % 8];
		if(index >= 0)
			previousBeatImage.sprite = previousBeatSprites[index];
		else 
			previousBeatImage.sprite = nullPreviousBeatSprite;
		
	}
}
