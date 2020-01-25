using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipIcon : MonoBehaviour
{
    public Image background;
    public Image icon;
    public Image number;
    
    void Start()
    {
        Image[] sprites = GetComponentsInChildren<Image>();
        background = sprites[0];
        icon = sprites[1];
        number = sprites[2];   
    }

}
