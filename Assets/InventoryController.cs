using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private GameObject inventory;
    private PlayerMovementController player;
    private PlayerJamMenu playerJamMenu;
    private NumbericJamInterface numbericJamInterface;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);  
        foreach(Transform child in transform){
            inventory = child.gameObject;
        }
        player = Object.FindObjectOfType<PlayerMovementController>();
        numbericJamInterface = Object.FindObjectOfType<NumbericJamInterface>();
        //ToggleInventory();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")){
            Debug.Log("ESC");
            ToggleInventory();
        }
    }

    public void ToggleInventory(){
        if(inventory.activeSelf){
            DeactivateInventory();
        }else{
            ActivateInventory();
        }
    }

    public void ActivateInventory(){
        inventory.SetActive(true);
        player.LockMovement(true);
        numbericJamInterface.locked = true;
    }
    public void DeactivateInventory(){
        PlayerMovementController player = Object.FindObjectOfType<PlayerMovementController>();
        player.RemoveInstrument();
        inventory.SetActive(false);
        player.LockMovement(false);
    }
}
