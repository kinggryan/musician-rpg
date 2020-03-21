using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private Animator animator;
    private SoundEvent sound;
    void Start(){
        animator = GetComponent<Animator>();
        sound = GetComponent<SoundEvent>();
    }
    public void PlayAnimation(){
        animator.SetBool("Wind",true);
        sound.PlaySound();
    }

    public void StopAnimation(){
        animator.SetBool("Wind",false);
        sound.StopSound();
    }

}
