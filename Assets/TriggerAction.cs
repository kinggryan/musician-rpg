using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAction : MonoBehaviour
{
    public UnityEvent runOnEnter;
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Triggered");
        if(runOnEnter != null){
            runOnEnter.Invoke();
        }
    }
}
