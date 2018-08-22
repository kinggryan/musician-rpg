using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIListener {

	void DidChangeAILoop(AIMIDIController ai, AudioLoop loop);

}
