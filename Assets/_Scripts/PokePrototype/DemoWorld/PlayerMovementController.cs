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

	Animator animator;
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

	// Use this for initialization

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
		animator = GetComponent<Animator> ();
		soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngine>();
		levelManager = GameObject.Find("LevelManager").GetComponentInParent<DemoWorldLevelManager>();
		//spells = GameObject.Find("Spells").GetComponent<Spells>();
	}

	public void FootstepSound(){
		soundEngine.PlaySoundWithName("footstep");
	}

	public void StartCast(){
		print("Start cast");
		casting = true;
	}

	public void EndCast(){
		if(casting){
		print("End cast");
		casting = false;
		hasCast = false;
		}
	}

	// public void CastSpell(){
	// 	hasCast = true;
	// 	if (spells.spellSpeed + spells.spellPower <= spells.power && spells.spellRadius + spells.spellRange <= spells.area && spells.spellLight + spells.spellDark <= spells.energy){
	// 		GameObject spellInstance = Instantiate(spell) as GameObject;
	// 		spellInstance.transform.position = transform.position;
			
	// 		if (walkDirection == 0){
	// 			spellInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (180 - spells.spellRadius * 10)));
	// 		} else if (walkDirection == 1){
	// 			spellInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (90 - spells.spellRadius * 10)));
	// 		} else if (walkDirection == 2){
	// 			spellInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (0 - spells.spellRadius * 10)));
	// 		} else if (walkDirection == 3){
	// 			spellInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (270 - spells.spellRadius * 10)));
	// 		}

	// 		ParticleSystem spellParticle = spellInstance.GetComponent<ParticleSystem>();

	// 		ParticleSystem.ShapeModule shapeModule = spellInstance.GetComponent<ParticleSystem>().shape;
	// 		ParticleSystem.EmissionModule emissionModule = spellInstance.GetComponent<ParticleSystem>().emission;

	// 		//Radius
	// 		shapeModule.arc = spells.spellRadius * 18;
	// 		//Power/Density
	// 		emissionModule.rate = spells.spellPower * 10;
	// 		spellSound.externalPitchModifier = spells.spellPower * -0.1f;
	// 		//Range
	// 		spellParticle.startLifetime = spells.spellRange * 0.05f + 0.1f;
	// 		//Speed
	// 		spellParticle.startSpeed = (spells.spellSpeed) + 7;
			
	// 		spells.power -= spells.spellSpeed + spells.spellPower;
	// 		spells.area -= spells.spellRadius + spells.spellRange;
	// 		spells.energy -= spells.spellLight + spells.spellDark;
	// 		spells.SetUIText();
	// 		spellUI.SetUIText();
	// 		soundEngine.PlaySoundWithName("castSpell");
			
			
	// 	}else{
	// 		print("NOT ENOUGH RESOURCES!");
	// 		spellUI.SetUIText();
	// 	}

	// }

	
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


		if(Input.GetKeyDown("q") || Input.GetButtonDown("Fire1") && !hasCast){
			//print("CAST!");
			//animator.SetTrigger("cast");
			//float castSpeed = spells.spellSpeed * spellLerpSpeed * Time.deltaTime;
			//animator.SetFloat ("speed", Mathf.Lerp(0.5f, 10f, castSpeed));
			//print("Cast speed: " + castSpeed);
		}
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

			//SPRINT
			float sprintModifier;
			if(Input.GetButton("BButton") && hasStamina){
				sprintModifier = sprintSpeed;
				currentStamina -= Time.deltaTime * 2;
				animator.speed = sprintModifier;
			}else{
				sprintModifier = 1;
				if(currentStamina < stamina) currentStamina += Time.deltaTime;
				animator.speed = 1;
			}



			rbody.velocity = Vector2.Lerp (rbody.velocity, movementInput*maxSpeed * sprintModifier, movementLerpSpeed * Time.deltaTime);

			// Set animation parameters
			var walking = movementInput.magnitude > 0.01f;
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
				animator.SetInteger ("direction", walkDirection);
			//}
		}else{
			rbody.velocity = new Vector2(0,0);
		}
	}
}
