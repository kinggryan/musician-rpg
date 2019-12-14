using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListeningZone : MonoBehaviour
{
    MovableObject parent;
    
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.GetComponentInParent<MovableObject>();
        
    }

    void OnTriggerEnter2D(Collider2D other){
        parent.OnListeningAreaEnter(other);
    }

    void OnTriggerExit2D(Collider2D other){
        parent.OnListeningAreaExit(other);
    }
}
