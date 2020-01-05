using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AnimatorSynchroniser : MonoBehaviour {
    
    public Animator[] animators;
 
    private void Start()
    {
        //Assumes all Animator components are children
        //You can change this or remove this line and assign your animators in the inspector
        animators = GetComponentsInChildren<Animator>();
    }
 
    //Set a boolean parameter for all animators
    public void SetBool(string name, bool value)
    {
        foreach (Animator anim in animators)
        {
            anim.SetBool(name, value);
        }
    }
    //Set a float parameter for all animators
    public void SetFloat(string name, float value)
    {
        foreach (Animator anim in animators)
        {
            anim.SetFloat(name, value);
        }
    }
    //Set an integer parameter for all animators
    public void SetInteger(string name, int value)
    {
        foreach (Animator anim in animators)
        {
            anim.SetInteger(name, value);
        }
    }
    //Set a trigger parameter for all animators
    public void SetTrigger(string name)
    {
        foreach (Animator anim in animators)
        {
            anim.SetTrigger(name);
        }
    }
 
    //The get functions just return the first animators value - no point looping here
    public bool GetBool(string name)
    {
        return animators[0].GetBool(name);
    }
 
    public float GetFloat(string name)
    {
        return animators[0].GetFloat(name);
    }
 
    public int GetInteger(string name)
    {
        return animators[0].GetInteger(name);
    }
}