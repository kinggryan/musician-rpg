using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private GameObject inventory;
    private PlayerMovementController player;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);  
        foreach(Transform child in transform){
            inventory = child.gameObject;
        }
        player = Object.FindObjectOfType<PlayerMovementController>();
        ToggleInventory();
        
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
            inventory.SetActive(false);
            player.LockMovement(false);
        }else{
            inventory.SetActive(true);
            player.LockMovement(true);
        }
    }
}
