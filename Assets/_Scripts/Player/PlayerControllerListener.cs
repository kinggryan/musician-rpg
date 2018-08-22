using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerControllerListener {

	void DidChangeLoop(AudioLoop newLoop);
}
