using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbericJamInterface : MonoBehaviour
{
    private PlayerJamMenu jamMenu;
    public bool locked = true;
    void Start()
    {
        jamMenu = GetComponent<PlayerJamMenu>();
    }

    void Update(){
        if(!locked){
            if(Input.GetKeyDown("1")){
                ChangeMove(0);
            } else if(Input.GetKeyDown("2")){
                ChangeMove(1);
            } else if(Input.GetKeyDown("3")){
                ChangeMove(2);
            } else if(Input.GetKeyDown("4")){
                ChangeMove(3);
            } else if(Input.GetKeyDown("5")){
                ChangeMove(4);
            } else if(Input.GetKeyDown("6")){
                ChangeMove(5);
            } else if(Input.GetKeyDown("7")){
                ChangeMove(6);
            } else if(Input.GetKeyDown("8")){
                ChangeMove(7);
            } else if(Input.GetKeyDown("9")){
                ChangeMove(8);
            } else if(Input.GetKeyDown("0")){
                ChangeMove(9);
            }
        }
    }

    void ChangeMove(int key){
        jamMenu.NumericChangeMove(key);
    }
}
