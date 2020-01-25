using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipReadout : MonoBehaviour
{
    public EquipIcon[] equipIcons;
    [SerializeField]
    private int margin;
    [SerializeField]
    private int yPos;
    private Camera camera;
    void Start()
    {
        equipIcons = GetComponentsInChildren<EquipIcon>();
        camera = Object.FindObjectOfType<Camera>();

        //AlignEquipIcons();
    }

    void AlignEquipIcons(){
        float noOfIcons = equipIcons.Length;
        float screenWidth = Screen.width;
        float spacing = (screenWidth - margin*2)/noOfIcons;
        float count = 1;
        foreach(EquipIcon equipIcon in equipIcons){
            float xPosReference = (1/noOfIcons) * count;
            Debug.Log("Count: " + count + " xRef: " + xPosReference);
            Vector3 newPosition = Camera.main.ViewportToWorldPoint(new Vector3(xPosReference, yPos, -1));
            Debug.Log(" New pos: " + newPosition + "NoOfIcons: " + noOfIcons);
            equipIcon.transform.position = new Vector3 (newPosition.x,newPosition.y,0);
            count++;
        }
    }
}
