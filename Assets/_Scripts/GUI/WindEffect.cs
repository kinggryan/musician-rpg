using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour {
	
	public float minWaitTime;
	public float maxWaitTime;
	public float xPosRange;
	public float yPosRange;
	public GameObject camera;



	private ParticleSystem particleSystem;

	private SoundEngine soundEngine;

	// Use this for initialization
	void Start () {
		particleSystem = GetComponent<ParticleSystem>();
		soundEngine = GameObject.Find("SoundEngine").GetComponent<SoundEngine>();
		StartCoroutine("SpawnWind");	
	}

	IEnumerator SpawnWind(){
		yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
		transform.position = new Vector3(camera.transform.position.x + Random.Range(- xPosRange, xPosRange),camera.transform.position.y + Random.Range(-yPosRange, yPosRange),transform.position.z);
		soundEngine.PlaySoundWithName("windGust");
		yield return new WaitForSeconds(1);
		StartCoroutine(AnimateTrees(4));
		particleSystem.Play();
		soundEngine.PlaySoundWithName("windWhistle");
		StartCoroutine("SpawnWind");
	}

	IEnumerator AnimateTrees(float duration){
		StartAnimatingTrees();
		yield return new WaitForSeconds(duration);
		StopAnimatingTrees();
	}

	void StartAnimatingTrees(){
		Tree[] trees = FindObjectsOfType<Tree>();
		foreach(Tree tree in trees){
			tree.PlayAnimation();
		}
	}

	void StopAnimatingTrees(){
		Tree[] trees = FindObjectsOfType<Tree>();
		foreach(Tree tree in trees){
			tree.StopAnimation();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
