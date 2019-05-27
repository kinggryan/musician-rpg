using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour {

	public int hp;

	void OnParticleCollision(GameObject other){
        print("Destructable colllision");
        if (other.gameObject.tag == "Spell"){
            //hit = true;
            print("Particle object collision");
            hp--;
            if (hp <= 0){
                print("Enemy killed");
                OnDie();
            }
            //spriteRenderer.color = hitColor;
        }
    }

	void OnDie(){
        Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
