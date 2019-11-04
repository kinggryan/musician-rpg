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

	private int queuedPreviousLoopBeatIndex = -1;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.setPlayerLoopForBeat, DidSetPlayerLoopForBeat);
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.setPreviousPhrasePlayerLoops, DidUpdatePreviousLoopBeats);
		nullCurrentBeatSprite = currentBeatImage.sprite;
		nullPreviousBeatSprite = previousBeatImage.sprite;
		animator = GetComponent<Animator>();
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.setPlayerLoopForBeat, DidSetPlayerLoopForBeat);
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.setPreviousPhrasePlayerLoops, DidUpdatePreviousLoopBeats);
	}
	
	void DidSetPlayerLoopForBeat(object sender, object arg) {
		var playerMoveAndBeat = (RPGGameplayManger.Notifications.SetPlayerLoopForBeatArgs)arg;

		// If this is the start of the measure, reset yourself
		if(playerMoveAndBeat.beatNumber % 8 == 0) {
			currentBeatImage.sprite = nullCurrentBeatSprite;
			if(queuedPreviousLoopBeatIndex >= 0)
				previousBeatImage.sprite = previousBeatSprites[queuedPreviousLoopBeatIndex];
			else 
				previousBeatImage.sprite = nullPreviousBeatSprite;
		}

		// Update your image if this is the current beat
		if(playerMoveAndBeat.beatNumber % 8 == beatNumber) {
			currentBeatImage.sprite = currentBeatSprites[playerMoveAndBeat.playerMoveIndex];
			animator.SetTrigger("pulse");
		}
	}

	// Queue up the previous loop beat so that we can update at the top of the next measure
	void DidUpdatePreviousLoopBeats(object sender, object arg) {
		var playerMoveAndBeat = (RPGGameplayManger.Notifications.SetPreviousPhrasePlayerLoopsArgs)arg;
		if(playerMoveAndBeat.playerMoves != null && playerMoveAndBeat.playerMoves.Count > 0) {
			var index = playerMoveAndBeat.playerMoveIndices[beatNumber % 8];
			queuedPreviousLoopBeatIndex = index;
		} else {
			previousBeatImage.sprite = nullPreviousBeatSprite;
			queuedPreviousLoopBeatIndex = -1;
		}
	}
}
