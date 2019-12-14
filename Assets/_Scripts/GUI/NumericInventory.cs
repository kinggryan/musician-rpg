using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NumericInventory : MonoBehaviour
{
    public Move[] knownMoves;
    public MoveSet[] activeMoves;
    private int maxNoOfMoves = 9;
    public List<string> styles;
    public List<string> equippedStyles;
    public NumericInventoryDisplay inventoryDisplay;
    public MoveSets playerMoves;
    public List<string> learnedSongs;
    // Start is called before the first frame update
    void Awake()
    {
        GenerateStylesList();
        
    }

    void Start(){
        inventoryDisplay = gameObject.GetComponent<NumericInventoryDisplay>();
        if(inventoryDisplay == null){
            Debug.LogError("Inventory Display not found!!");
        }
    }

    void GenerateStylesList(){
        foreach(Move move in knownMoves){
            if(styles.Count > 0){
                foreach (string style in styles){
                    if(style == move.style){
                        break;
                    }else{
                        styles.Add(move.style);
                    }
                }
            }else{
                styles.Add(move.style);
            }
        }
    }

    public int noOfEquippedStyles(){
        return equippedStyles.Count;
    }

    public int noOfEquippedMovesInStyle(int style){
        return activeMoves[style].moves.Length;
    }

    void GenerateEquippedStylesList(){
        equippedStyles = new List<string>();
        foreach(MoveSet moveSet in activeMoves){
            foreach(Move move in moveSet.moves){
                equippedStyles.Add(moveSet.name);
            }   
        }
        equippedStyles = equippedStyles.Distinct().ToList();
    }
    

    public void EquipMove(Move move, int key){
        string keyReadout = (key + 1).ToString();
        foreach(Move activeMove in activeMoves[0].moves){
            if(activeMove.equipKey == keyReadout){
                activeMove.equipKey = null;
            }
        }
        move.equipKey = keyReadout;
        activeMoves[0].moves[key] = move;
        playerMoves.moveSets = activeMoves;
        inventoryDisplay.UpdateMovesAndSongs();
    }

    public void UnequipMove(Move move, int style){
        MoveSet moveSetToUpdate = activeMoves[style];
        moveSetToUpdate.moves =  RemoveMoveFromArray(moveSetToUpdate.moves,move,moveSetToUpdate);
        if(moveSetToUpdate.moves.Length < 1){
            activeMoves = RemoveMoveSetFromArray(activeMoves,moveSetToUpdate);
            GenerateEquippedStylesList();
        }
    }

    

    bool isMoveEquipped(Move moveToCheck){
        foreach(MoveSet moveSet in activeMoves){
            foreach(Move move in moveSet.moves){
                if(move.name == moveToCheck.name){
                    return true;
                }
            }
        }
        return false;
    }

    Move[] AddMoveToArray(Move[] original, Move itemToAdd, bool newMoveSet){
        Debug.Log(original[0].name);
        int originalArrayLength = 0;
        foreach(Move moveToCount in original){
            originalArrayLength += 1;
        }
        int newArrayLength = 1;
        if(!newMoveSet){
            newArrayLength = originalArrayLength + 1;
        }        
        Move[] finalArray = new Move[newArrayLength];
        for(int i = 0; i < original.Length; i ++ ) {
            Debug.Log("Adding Move " + original[i].name);
            finalArray[i] = original[i];
        }
        Debug.Log("final array length: " + finalArray.Length + "itemToAdd: " + itemToAdd.name);
        finalArray[finalArray.Length - 1] = itemToAdd;
        return finalArray;
    }

    Move[] RemoveMoveFromArray(Move[] original, Move itemToRemove,MoveSet moveSet){
        Debug.Log(original[0].name);
        int originalArrayLength = 0;
        foreach(Move moveToCount in original){
            originalArrayLength += 1;
        }
        int newArrayLength = 0;
        newArrayLength = originalArrayLength - 1;        
        Move[] finalArray = new Move[newArrayLength];
        int finalArrayIndex = 0;
        for(int i = 0; i < original.Length; i ++ ) {
            if(original[i].name != itemToRemove.name){
                Debug.Log("Adding Move " + original[i].name);
                finalArray[finalArrayIndex] = original[i];
                finalArrayIndex++;
            }
        }
        Debug.Log("final array length: " + finalArray.Length + "itemToAdd: " + itemToRemove.name);
        //finalArray[finalArray.Length - 1] = itemToAdd;
        return finalArray;
    }

    MoveSet[] AddMoveSetToArray(MoveSet[] original, MoveSet itemToAdd){
        MoveSet[] finalArray = new MoveSet[ original.Length + 1 ];
        for(int i = 0; i < original.Length; i ++ ) {
            finalArray[i] = original[i];
        }
        finalArray[finalArray.Length - 1] = itemToAdd;
        return finalArray;
    }

    MoveSet[] RemoveMoveSetFromArray(MoveSet[] original, MoveSet itemToRemove){
        MoveSet[] finalArray = new MoveSet[ original.Length - 1 ];
        int finalArrayIndex = 0;
        for(int i = 0; i < original.Length; i ++ ) {
            if(original[i] != itemToRemove)
                finalArray[finalArrayIndex] = original[i];
                finalArrayIndex++;
        }
        return finalArray;
    }




}
