using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{
    public enum Emo {
		Happy,
		Romantic,
		Sentimental,
		Sad,
		Angry,
		Energetic
	}



    public Emo[] happyWeaknesses;
    public Emo[] happyStrengths;
    public Emo[] romanticWeaknesses;
    public Emo[] romanticStrengths;
    public Emo[] sentimentalWeaknesses;
    public Emo[] sentimentalStrengths;
    public Emo[] sadWeaknesses;
    public Emo[] sadStrengths;
    public Emo[] angryWeaknesses;
    public Emo[] angryStrengths;
    public Emo[] energeticWeaknesses;
    public Emo[] energeticStrengths;
    public Color[] emoColors = new Color[6];

    public bool checkEmoWeaknesses(Emo newEmo, Emo currentEmo){
        Emo[] emoToCheck = new Emo[0];
        
        switch(newEmo){
            case Emo.Happy:
                emoToCheck = happyWeaknesses;
                break;
            case Emo.Romantic:
                emoToCheck = romanticWeaknesses;
                break;
            case Emo.Sentimental:
                emoToCheck = sentimentalWeaknesses;
                break;
            case Emo.Sad:
                emoToCheck = sadWeaknesses;
                break;
            case Emo.Angry:
                emoToCheck = angryWeaknesses;
                break;
            case Emo.Energetic:
                emoToCheck = energeticWeaknesses;
                break;
            }
            foreach(Emo emo in emoToCheck){
                if(emo == newEmo){
                    return true;
                }
            }
            return false;
        }

    public bool checkEmoStrengths(Emo newEmo, Emo currentEmo){
        Emo[] emoToCheck = new Emo[0];
        
        switch(newEmo){
            case Emo.Happy:
                emoToCheck = happyStrengths;
                break;
            case Emo.Romantic:
                emoToCheck = romanticStrengths;
                break;
            case Emo.Sentimental:
                emoToCheck = sentimentalStrengths;
                break;
            case Emo.Sad:
                emoToCheck = sadStrengths;
                break;
            case Emo.Angry:
                emoToCheck = angryStrengths;
                break;
            case Emo.Energetic:
                emoToCheck = energeticStrengths;
                break;
            }
            foreach(Emo emo in emoToCheck){
                if(emo == currentEmo){
                    return true;
                }
            }
            return false;
        }

        public Color GetEmoColor(Emo emo){
            Debug.Log("Getting emo color " + emo.ToString());
            switch(emo){
            case Emo.Happy:
                return emoColors[0];
                break;
            case Emo.Romantic:
                return emoColors[1];
                break;
            case Emo.Sentimental:
                return emoColors[2];
                break;
            case Emo.Sad:
                return emoColors[3];
                break;
            case Emo.Angry:
                return emoColors[4];
                break;
            case Emo.Energetic:
                return emoColors[5];
                break;
            }
            Debug.LogError("Emo color not found!");
            return Color.white;
        }
    

}
