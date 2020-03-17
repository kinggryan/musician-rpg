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
    [SerializeField]
    private float fadeAmount;
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

    public void FadeEquipIcons(){
        foreach(EquipIcon equipIcon in equipIcons){
            equipIcon.icon.color = new Color (equipIcon.icon.color.r,equipIcon.icon.color.g,equipIcon.icon.color.b,equipIcon.icon.color.a - fadeAmount);
            equipIcon.background.color = new Color (equipIcon.background.color.r,equipIcon.background.color.g,equipIcon.background.color.b,equipIcon.background.color.a - fadeAmount);
            equipIcon.number.color = new Color (equipIcon.number.color.r,equipIcon.number.color.g,equipIcon.number.color.b,equipIcon.number.color.a - fadeAmount);
        }
    }

    public void UnfadeEquipIcons(){
        foreach(EquipIcon equipIcon in equipIcons){
            equipIcon.icon.color = new Color (equipIcon.icon.color.r,equipIcon.icon.color.g,equipIcon.icon.color.b,equipIcon.icon.color.a + fadeAmount);
            equipIcon.background.color = new Color (equipIcon.background.color.r,equipIcon.background.color.g,equipIcon.background.color.b,equipIcon.background.color.a + fadeAmount);
            equipIcon.number.color = new Color (equipIcon.number.color.r,equipIcon.number.color.g,equipIcon.number.color.b,equipIcon.number.color.a + fadeAmount);
        }
    }
}
