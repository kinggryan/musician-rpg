using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScorekeeperListener {
	void DidChangeScore(float score);
	void DidSetMaxScore(float maxScore);
	void DidWin();
	void DidLose();
}
