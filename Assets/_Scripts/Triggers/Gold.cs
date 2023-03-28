using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {

	//public float goldAmount = 0f;
	public enum currencyType {coin, gem};
	public currencyType goldType; //0: coin, 1:gem
	private AudioSource audioSource;
	private bool wasLeftBehind=false, playerCollided=false ;//,firstInstatiation=false;
	public bool failSafeSpawning; //used for when a coin is left behind and then spawned. //true: spawned by Map.cs //false: left behind
	private GameObject magnetPlayerGameObject;

	/* void OnAwake(){
		//firstInstatiation = true;
		//wasLeftBehind = false;
		//playerCollided = false;
	} */

	void OnDisable(){
		//print("coin disabled");
		//if (transform.parent == null) return;
		//print("Yolo");
		/* if (firstInstatiation){
			firstInstatiation=false;
			return;
		} */
		
		if (playerCollided){
			wasLeftBehind = false;
		}else{
			wasLeftBehind = true;
		}
		playerCollided = false;
		failSafeSpawning = false;

		if (this.goldType == currencyType.gem){
			//print("this is gold");
			if (Map.instance) Map.instance.AddToPoolGold(this.gameObject);
						
		}else if (this.goldType == currencyType.coin){		
			//print("this is silver");
			if (Map.instance) Map.instance.AddToPoolSilver(this.gameObject);
		}
		
	}

	void OnEnable(){
		if ( wasLeftBehind && !failSafeSpawning){
			//wasLeftBehind = false;
			this.gameObject.transform.parent=null;
			this.gameObject.transform.position= new Vector3(10000,10000,0);
		}
		failSafeSpawning = false;
		wasLeftBehind = false;

		GetComponentInChildren<SpriteRenderer>().enabled = true; //turn on graphics
	}
	void Start(){
		/* if ( wasLeftBehind ){
			wasLeftBehind = false;
			this.gameObject.transform.parent=null;
			this.gameObject.transform.position= new Vector3(1000000,10000000,0);
		}
		playerCollided = false; */

		GetComponentInChildren<SpriteRenderer>().enabled = true; //turn on graphics
		audioSource = GetComponent<AudioSource>();
	}

	private IEnumerator MoveTowardsPlayer(){
		float lerpValue = 0.15f;
		const float lerpIncreaseRate = 0.01f;
		float x1 = this.gameObject.transform.position.x;
		float y1 = this.gameObject.transform.position.y;
		float x2 = this.magnetPlayerGameObject.transform.position.x;
		float y2 = this.magnetPlayerGameObject.transform.position.y;

		while (true){

			this.gameObject.transform.position = new Vector3(
														Mathf.Lerp(this.gameObject.transform.position.x, this.magnetPlayerGameObject.transform.position.x, lerpValue ),
														Mathf.Lerp(this.gameObject.transform.position.y, this.magnetPlayerGameObject.transform.position.y, lerpValue ),
														this.gameObject.transform.position.z
															);
			
			x2 = this.magnetPlayerGameObject.transform.position.x;
			y2 = this.magnetPlayerGameObject.transform.position.y;

			lerpValue += lerpIncreaseRate;			

			yield return null;
		}

	}
	
	private IEnumerator OnTriggerEnter2D(Collider2D collision){

		if (collision.gameObject.tag == "Magnet") {
			magnetPlayerGameObject = collision.gameObject;
			//kane toumpes
			StartCoroutine("MoveTowardsPlayer");
		}

        if (collision.gameObject.tag == "Player" ) {
			StopCoroutine("MoveTowardsPlayer");

			GetComponentInChildren<SpriteRenderer>().enabled = false; //turn off graphics
            audioSource.pitch = Random.Range(0.8f, 1.6f);
			audioSource.Play(); //play pick up sound

			if (this.goldType == currencyType.gem ){
				Score.instance.SetGemsGathered( Score.instance.GetGemsGathered() + 1 );
			}else if (this.goldType == currencyType.coin){
				Score.instance.SetCoinsGathered( Score.instance.GetCoinsGathered() + 1 );
			}

			//Score.instance.SetGoldGathered( Score.instance.GetGoldGathered() + this.goldAmount );
			
			
			//wasLeftBehind = false;
			playerCollided = true;
			this.gameObject.transform.parent = null;
			failSafeSpawning = false;
			
			yield return new WaitForSeconds(audioSource.clip.length);
			this.gameObject.SetActive(false);
		}
	}
}
