using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sampler : MonoBehaviour {
    
    public AudioMixerGroup output;
    
    public AudioClip[] layer1;
    public AudioClip[] layer2;
    public AudioClip[] layer3;
    public AudioClip[] layer4;
    public int[] pitchBreakPoint;
    public int[] rootNote;
    public int[] velBreakPoint;
    public int transpose;
    public int clip;
    
    [Range (-1f, 1f)]
    public float velSensitivity;
    
    [Range (1, .05f)]
    public float attack;
    [Range (.05f, .005f)]
    public float decay;
    [Range (0f, 1f)]
    public float sustain;
    [Range (.5f, .005f)]
    public float release;
    

    
    
    public void PlayNote (int pitch, float vel, float length)
    {
        
        //print("Length = " + length);
        AudioSource source = gameObject.AddComponent<AudioSource>();
        //***Clip***//
        pitch = (pitch + transpose);
        int noOfBreakPoints = (pitchBreakPoint.Length - 1);
        //print("vel = " + vel);
        ClipBreak(pitch, noOfBreakPoints);
        //***Velocity***//
        if (vel >= velBreakPoint[2])
        {
            source.clip = layer4[clip];
        } 
        else if (vel >= velBreakPoint[1])
        {
            source.clip = layer3[clip];
        }
        else if (vel >= velBreakPoint[0])
        {
            source.clip = layer2[clip];
        }
        else if (vel < velBreakPoint[0])
        {
            source.clip = layer1[clip];
        }
        //source.volume = ((vel/127)+((1-(vel/127))*(1-velSensitivity)));
        source.volume = 0;
        source.pitch = Mathf.Pow(1.05946f, (pitch - rootNote[clip]));
        
        source.outputAudioMixerGroup = output;
        source.Play();
        StartCoroutine(Attack(source, vel, length));
        //StartCoroutine(Sustain(source, vel, length));
    }
        

void ClipBreak (int pitch, int clipChecked)
    {
        if (pitch <= pitchBreakPoint[0])
        {
            clip = 0;
            
        }
        else if (pitch >= pitchBreakPoint[clipChecked])
        {
            clip = (clipChecked + 1);
            
        }
        else
        {
            int nextClip = (clipChecked - 1);
            ClipBreak(pitch, nextClip);
        }
    }    

    
    

    IEnumerator Attack (AudioSource source, float vel, float length)
    {
        
        if (source.volume < ((vel/127) + ((1 - (vel/127))*velSensitivity)))
        {

             
             //print("att " + source.volume);
             //print("vel = " + vel);
             source.volume = (source.volume + attack);
             yield return new WaitForSeconds(.01f);
             StartCoroutine(Attack(source, vel, length));
             
        }
        else
        {
            StopCoroutine(Attack(source, vel, length)); 
            StartCoroutine(Sustain(source, vel, length));
        }
    }
    IEnumerator Decay (AudioSource source, float vel, float length)
    {
    
        if (source.volume > (((vel/127) + ((1 - (vel/127))*velSensitivity))*sustain))
        {
             source.volume = (source.volume - decay);
             //print("decay vol = " + source.volume);
             //print("decay com = " + ((vel/127)*sustain));
             yield return new WaitForSeconds(.01f);
             StartCoroutine(Decay(source, vel, length));
             //print("dec " + source.volume);
        }
        else
        {
             StartCoroutine(Sustain(source, vel, length));
        }
    }
    IEnumerator Sustain (AudioSource source, float vel, float length)
    {
                //source.volume = ((vel/127)*sustain);        
                yield return new WaitForSeconds(length);
                StartCoroutine(Release(source, vel, length));
                //print("sus " + source.volume);
                //print("sustain");
                
    }
    IEnumerator Release (AudioSource source, float vel, float length)
    {
        
        if (source.volume >= 0.1f)
        {
            source.volume = (source.volume - release);
            yield return new WaitForSeconds(.01f);
            StartCoroutine(Release(source, vel, length));
            //print("rel " + source.volume);
        }
        else
        {

            //print("done");
            Destroy(source);
        }
    }    

}
