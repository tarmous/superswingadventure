using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyGate : MonoBehaviour {

	private bool wasLeftBehind;
	private AudioSource audioSource;
	public bool failSafeSpawning; //used for when is left behind and then spawned. //true: spawned by Map.cs //false: left behind
	

	void OnDisable(){
		
		wasLeftBehind = true;
		if (Map.instance) Map.instance.AddToPoolBouncy(this.gameObject);
		
	}

	void OnEnable(){
		if ( wasLeftBehind && !failSafeSpawning){
			//wasLeftBehind = false;
			this.gameObject.transform.parent=null;
			this.gameObject.transform.position= new Vector3(10000,10000,0);
		}
		failSafeSpawning = false;
		wasLeftBehind = false;

	}

	void Start(){
		audioSource = GetComponent<AudioSource>();
	}
	
	private void OnTriggerEnter2D(Collider2D collision){

		if (collision.gameObject.tag == "Player") {
			PlayerAttributes pa = collision.gameObject.GetComponent<PlayerAttributes>();
			pa.StopCoroutine("ChangePlayerBounciness");
			pa.StartCoroutine("ChangePlayerBounciness");
			audioSource.Play(); //play pick up sound
		}
		
	}
}
