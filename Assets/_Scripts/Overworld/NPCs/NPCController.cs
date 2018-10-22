using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
// This class controls 1 NPC, and contains all the references to the NPCs components. 
// Classes outside of the NPC should only interact with this class, rather than those components individually.
//</summary>
public class NPCController : MonoBehaviour {

	public NPCMovementController movementController;
	public PlayerCountoffDisplay countoffDisplay;
	
}
