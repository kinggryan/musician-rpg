using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovementController : MonoBehaviour {

	

	enum MovementMode {
		Free,
		Locked,
		Animating
	}

	public float maxSpeed = 1.5f;
	Vector2 movementVector = Vector2.zero;
	public float movementLerpSpeed = 20f;
	public float spellLerpSpeed = 20f;
	MovementMode mode;

	AnimatorSynchroniser animator;
	Rigidbody2D rbody;

	public int walkDirection = 0;

	private SoundEngine soundEngine;

	public GameObject spell;

//	public Spells spells;
//	public SpellUI spellUI;

	private int speed;
	private int radius;
	private int power;
	public int hp;
	public float stamina;
	public float currentStamina;

	public float sprintSpeed;
	public bool hasStamina;

	public bool casting;
	private bool hasCast;
	public string entrancePoint;

	public SoundEvent spellSound;


	private int startingHP;

	private DemoWorldLevelManager levelManager;

	static PlayerMovementController instance = null;

	

	void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print ("Duplicate Spells Destroyed!");
        }
        else 
        {
            instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
        
    }
	void Start () {
		startingHP = hp;
		rbody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<AnimatorSynchroniser> ();
		soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngine>();
		levelManager = GameObject.Find("LevelManager").GetComponentInParent<DemoWorldLevelManager>();
		//spells = GameObject.Find("Spells").GetComponent<Spells>();
	}

	public void FootstepSound(){
		soundEngine.PlaySoundWithName("footstep");
	}



	public void LockMovement(bool lockMovement){
		if(lockMovement){
			mode = MovementMode.Locked;
		}else{
			mode = MovementMode.Free;
		}
	}
	
	void OnParticleCollision(GameObject other){
         
        if (other.gameObject.tag == "EnemyRanged"){
            print("Particle player collision");
            hp --;
            if (hp <= 0){
               Die();
            }
            
        }
    }
	
	public void Die(){
		levelManager.LoadLevel("PlayerHouse", 5);
		entrancePoint = "homeIndoor";
		hp = startingHP;

	}

	// Update is called once per frame
	void Update () {
		
		GameObject[] activeSpells = GameObject.FindGameObjectsWithTag("Spell");

		for(int i = 0; i < activeSpells.Length; i++)
        {
			//print(activeSpells);
            if(GetComponent<ParticleSystem>() && !activeSpells[i].GetComponent<ParticleSystem>().IsAlive()){
            Destroy(activeSpells[i]);
         	}
        }


		// if(Input.GetKeyDown("q") || Input.GetButtonDown("Fire1") && !hasCast){
		// 	//print("CAST!");
		// 	//animator.SetTrigger("cast");
		// 	//float castSpeed = spells.spellSpeed * spellLerpSpeed * Time.deltaTime;
		// 	//animator.SetFloat ("speed", Mathf.Lerp(0.5f, 10f, castSpeed));
		// 	//print("Cast speed: " + castSpeed);
		// }
		if (mode == MovementMode.Locked || mode == MovementMode.Animating) {
			return;
		}

			if (!casting){ 

			var movementInput = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
			if (movementInput.magnitude > 1) {
				movementInput.Normalize ();
			}

			if(currentStamina <= 0){
				hasStamina = false;
			}else if(currentStamina >= stamina){
				hasStamina = true;
			}
			
			rbody.velocity = movementInput * maxSpeed;
			

			// Set animation parameters
			bool walking = movementInput.magnitude > 0.001f;
			animator.SetBool ("walking", walking);
			//if(walking) {
				// The walk directions are:
				// 0 - left
				// 1 - up
				// 2 - right
				// 3 - down
				if (Mathf.Abs (movementInput.x) > Mathf.Abs (movementInput.y)) {
					if (movementInput.x > 0) {
						walkDirection = 2;
					} else if (movementInput.x < 0) {
						walkDirection = 0;
					}
				} else {
					if (movementInput.y > 0) {
						walkDirection = 1;
					} else if (movementInput.y < 0){
						walkDirection = 3;
					}
				}
				SetAnimatorDirection();
			//}
		}else{
			rbody.velocity = new Vector2(0,0);
			walkDirection = 3;
		}
	}

	void SetAnimatorDirection(){
		animator.SetInteger ("direction", walkDirection);
	}
}
