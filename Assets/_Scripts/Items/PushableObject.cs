using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour {

public GameObject player;

public float leftCounter = 0;
public float rightCounter = 0;
public float topCounter = 0;
public float bottomCounter = 0;
public float threshold;

public float pushSpeed;
public float fallSpeed;

private Rigidbody2D rbody;
private Vector3 destination;

public bool pushing = false;
public bool falling = false;

public bool pushLeft = false;
public bool pushRight = false;
public bool pushUp = false;
public bool pushDown = false;

public bool fallen = false;
private SoundEngine soundEngine;


// The walk directions are:
			// 0 - left
			// 1 - up
			// 2 - right
			// 3 - down
void Start() {
	rbody = GetComponent<Rigidbody2D> ();
	soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngine>();
	player = GameObject.Find("Player");
	transform.position = new Vector3(RoundToNearestHalf(transform.position.x), RoundToNearestHalf(transform.position.y),transform.position.z);
}

void OnCollisionEnter2D(Collision2D other){
	if (other.gameObject.tag == "RemovableBarrier" && fallen && !falling){
		destination += other.gameObject.GetComponent<RemovableBarrier>().pushableOffset;
		print("Destination: " + destination);
		Destroy(other.gameObject);
		Destroy(gameObject.GetComponent<BoxCollider2D>());
	}else if (other.gameObject == player || other.gameObject.tag == "FallCollider" ||  other.gameObject.tag == "Cliff"){

	}else if (!falling){
		print("Stopped by: " + other.gameObject);
		StopPush();
	}
}

void OnTriggerStay2D(Collider2D other){
	if (!pushing && !falling && other.gameObject.tag == "FallCollider" && !fallen){
			Fall();
			
			//Destroy(other.gameObject);
		}
}

void OnCollisionStay2D(Collision2D other){

		if (other.gameObject.tag == "RemovableBarrier" && fallen && !falling){
			destination += other.gameObject.GetComponent<RemovableBarrier>().pushableOffset;
			print("Removing Barrier");
			Destroy(other.gameObject);
			Destroy(gameObject.GetComponent<BoxCollider2D>());
		}

		if (other.gameObject == player && !pushing){
			int walkDirection = player.GetComponent<PlayerMovementController>().walkDirection;
			if (walkDirection == 0){
				leftCounter += Time.deltaTime;
				rightCounter = 0;
				topCounter = 0;
				bottomCounter = 0;
			}else if (walkDirection == 1){
				leftCounter = 0;
				rightCounter = 0;
				topCounter += Time.deltaTime;
				bottomCounter = 0;
			}else if (walkDirection == 2){
				leftCounter = 0;
				rightCounter += Time.deltaTime;
				topCounter = 0;
				bottomCounter = 0;
			}else if (walkDirection == 3){
				leftCounter = 0;
				rightCounter = 0;
				topCounter = 0;
				bottomCounter += Time.deltaTime;
			}
			if (leftCounter >= threshold){
				print("Left buffer");
				PushObject(0);
			} else if (topCounter >= threshold){
				print("Up buffer");
				PushObject(1);
			}else if (rightCounter >= threshold){
				print("Right buffer");
				PushObject(2);
			}else if (bottomCounter >= threshold){
				print("Down buffer");
				PushObject(3);
			}
		}

	}

	// The walk directions are:
	// 0 - left
	// 1 - up
	// 2 - right
	// 3 - down

	void PushObject(int direction){
		GetComponent<Rigidbody2D>().isKinematic = false;
		soundEngine.PlaySoundWithName("push");
		leftCounter = 0;
		rightCounter = 0;
		topCounter = 0;
		bottomCounter = 0;
		pushing = true;
		gameObject.layer = 9;
		
		if (direction == 0){
			print("moving left pt1");
			destination = new Vector3(transform.position.x - 1,transform.position.y,transform.position.z);
			pushLeft = true;
		}else if (direction == 1){
			print("moving up pt1");
			destination = new Vector3(transform.position.x,transform.position.y + 1,transform.position.z);
			pushUp = true;
		}else if (direction == 2){
			print("moving right pt1");
			destination = new Vector3(transform.position.x + 1,transform.position.y,transform.position.z);
			pushRight = true;
		}else if (direction == 3){
			print("moving down pt1");
			destination = new Vector3(transform.position.x,transform.position.y - 1,transform.position.z);
			pushDown = true;
		}
		
		
	}

	void Fall(){
		gameObject.layer = 9;
		print("Falling");
		GetComponent<Rigidbody2D>().isKinematic = false;
		destination = new Vector3(transform.position.x,transform.position.y - 2,transform.position.z);
		falling = true;
	}

	void Update(){

		if(pushLeft){
			GetComponent<Rigidbody2D>().isKinematic = false;
			if(transform.position.x > destination.x){
				transform.position = new Vector3(transform.position.x - (pushSpeed * Time.deltaTime),transform.position.y,transform.position.z);
			}else{
				StopPush();
			}
		}else if(pushUp){
			if(transform.position.y < destination.y){
				GetComponent<Rigidbody2D>().isKinematic = false;
				transform.position = new Vector3(transform.position.x,transform.position.y + (pushSpeed * Time.deltaTime),transform.position.z);
			}else{
				StopPush();
			}
		}else if(pushRight){
			if(transform.position.x < destination.x){
				GetComponent<Rigidbody2D>().isKinematic = false;
				transform.position = new Vector3(transform.position.x + (pushSpeed * Time.deltaTime),transform.position.y,transform.position.z);
			}else{
				StopPush();
			}
		} else if(pushDown){
			if(transform.position.y > destination.y){
				GetComponent<Rigidbody2D>().isKinematic = false;
				transform.position = new Vector3(transform.position.x,transform.position.y - (pushSpeed * Time.deltaTime),transform.position.z);
			}else{
				StopPush();
			}
		}else if(falling){
			if(transform.position.y > destination.y){
				GetComponent<Rigidbody2D>().isKinematic = false;
				transform.position = new Vector3(transform.position.x,transform.position.y - (fallSpeed * Time.deltaTime),transform.position.z);
			}else{
				StopPush();
				fallen = true;
			}
		}
	}

	void StopPush (){

		GetComponent<Rigidbody2D>().isKinematic = true;
		transform.position = new Vector3(RoundToNearestHalf(transform.position.x), RoundToNearestHalf(transform.position.y),transform.position.z);
		gameObject.layer = 11;

		if(pushLeft){
			pushLeft = false;
		}else if(pushUp){
			pushUp = false;
		}else if(pushRight){
			pushRight = false;
		}else if(pushDown){
			pushDown = false;
		}
		else if(falling){
			falling = false;
			CheckForBarriers();
			
		}
		if (fallen){
			CheckForBarriers();
		}

		pushing = false;
		
		print("Push stopped");
		
		
	}

	void CheckForBarriers(){
			GameObject[] barriers = GameObject.FindGameObjectsWithTag("RemovableBarrier");
        	foreach (GameObject barrier in barriers) {
			if (barrier.transform.position.x == transform.position.x && barrier.transform.position.y == transform.position.y){
				print("Barrier match found");
				Destroy(barrier);
				Destroy(gameObject.GetComponent<BoxCollider2D>());
			}
	}
	}

	void OnCollisionExit2D(Collision2D other){

		if (other.gameObject == player){
			leftCounter = 0;
			rightCounter = 0;
			topCounter = 0;
			bottomCounter = 0;
		}
	}

	public static float RoundToNearestHalf(float a)
	{
		return a = Mathf.Round(a - 0.5f) + 0.5f;
	}
	
}
