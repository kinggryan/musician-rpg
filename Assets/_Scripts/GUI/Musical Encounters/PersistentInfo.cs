using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PersistentInfo : MonoBehaviour
{
    public MoveSet[] activeMoves;
    

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);   
    }

}
