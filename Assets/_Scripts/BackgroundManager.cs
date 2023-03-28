using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BackgroundManager : MonoBehaviour {

	public static BackgroundManager instance;



	public BackgroundObject backgroundSGO;
	public GameObject[] backgroundComponentAsset;
	public float[] backgroundComponentVelocity;
	public float [] backgroundComponentPosition;
	public bool [] repeat;


	public bool stopUpdating=false;

	private GameObject camera;
	private GameObject player;
	private Vector3 startPosition;
	private bool startCoroutineOnce;


    [Range (0, 1) ]
	private float velocityMultiplier=1f;
	const float scrollSpeed=0.05f;
	const float autoScrollSpeed=65f;
	const float dayCycleTimer = 10f;
	const float colorDenominator = 255f;

	void Awake(){
		instance = this;
		player=null;
		stopUpdating = false;
		startPosition = transform.position;

		if ( !PlayerPrefs.HasKey("BackgroundSkin")){
			PlayerPrefs.SetString("BackgroundSkin", "DefaultSkin");
			backgroundSGO = (BackgroundObject) Resources.Load("BackgroundSkins/DefaultSkin", typeof(BackgroundObject));
		}else {
			backgroundSGO = (BackgroundObject) Resources.Load("BackgroundSkins/" + PlayerPrefs.GetString("BackgroundSkin"), typeof(BackgroundObject)); 
			//print(PlayerPrefs.GetString("BackgroundSkin"));
		}

		//check player prefs
		//Load skin and generally set up everything needed for spawn
	}

    // Use this for initialization
    void Start () {
		if (Map.instance == null ){ }else{
			if ( Map.instance.GetPlayerGameObject() == null ) {}else { 
				player = Map.instance.GetPlayerGameObject(); 
				}
		}
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		
		
		SpawnBackground();
		SetColorActive(); //Pari's fault :P

		startCoroutineOnce = true;	
	}
	
	// Update is called once per frame
	void Update () {

		if (stopUpdating) return;
		//dont alter the 7 lines below. //failsafe if the player isnt found
		if ( !Map.instance ){
			if ( TestingGrounds.instance ){
				//do nothing
			}else{
				return;
			}
		} 

		if ( player == null ){
			if (Map.instance){
				if ( Map.instance.GetPlayerGameObject() == null ){return;}else {} 
			
				player = Map.instance.GetPlayerGameObject();
				return;
			}else if (TestingGrounds.instance){
				if ( TestingGrounds.instance.GetPlayerGameObject() == null ){return;}else {} 
			
				player = TestingGrounds.instance.GetPlayerGameObject();
				return;
			}
		} 
		if ( camera == null ){
			camera = GameObject.FindGameObjectWithTag("MainCamera");
			return;
			}
		if (  player == null ){return;}else {}

		gameObject.transform.position = new Vector3(
			camera.transform.position.x,
			this.gameObject.transform.position.y,
			this.gameObject.transform.position.z 
		);

		startPosition = gameObject.transform.position;

		if (startCoroutineOnce){
			StartCoroutine(changeBackground());
			startCoroutineOnce=!startCoroutineOnce;	
		} 	
	}

	void LateUpdate(){
		if ( stopUpdating ) return;
		if ( player== null ){return;}else {} 
		manageScrollSpeed();
		moveBackground();
	}

	private bool ContinueLerping (Color target, Color intented) {
		
		//print("target: " + target.r +"|| intented: " + intented);

		float colorOffset = 5 /colorDenominator;
		bool isRedGood = ( target.r  >= (intented.r - colorOffset) ) && ( target.r  <= (intented.r + colorOffset) );
		bool isGreenGood = ( target.g  >= (intented.g - colorOffset) ) && ( target.g  <= (intented.g + colorOffset) );
		bool isBlueGood = ( target.b  >= (intented.b - colorOffset) ) && ( target.b  <= (intented.b + colorOffset) );
		//if any isnt good then return true

		//print ("red: " + isRedGood + " green: " + isGreenGood + " blue: " + isBlueGood);
		return !(isRedGood && isGreenGood && isBlueGood);
	}

	private IEnumerator changeBackground(){ // Color swapping
		
		int i = 0;
		float lerpSpeed = 0.01f;
		float waitForColorTimer = lerpSpeed ;
		bool repeatCamera, repeatAll;
		
		camera.GetComponent<Camera>().backgroundColor = 
		backgroundComponentAsset[0].GetComponent<BackgroundAssetColor>().color_active[0];// Camera background color;

		for (int j = 0; j < backgroundComponentAsset.Length; j++){
			foreach (SpriteRenderer sr in backgroundComponentAsset[j].GetComponentsInChildren<SpriteRenderer>() ){
				sr.color =
				backgroundComponentAsset[j].
				GetComponent<BackgroundAssetColor>().
				color_active[0];
			}
		} 
		
		while(true){
			repeatCamera = false;
			repeatAll = false;
			for (int j = 0; j < repeat.Length; j++){
				repeat[j]=false;
			}

			//print("fuck all");

			do {
				camera.GetComponent<Camera>().backgroundColor = Color.Lerp(camera.GetComponent<Camera>().backgroundColor, backgroundComponentAsset[0].
																															GetComponent<BackgroundAssetColor>().
																															color_active[i], lerpSpeed);// Camera background color;
				repeatCamera = ContinueLerping(camera.GetComponent<Camera>().backgroundColor, backgroundComponentAsset[0].GetComponent<BackgroundAssetColor>().color_active[i]);
				
				for (int j = 0; j < backgroundComponentAsset.Length; j++){
					foreach (SpriteRenderer sr in backgroundComponentAsset[j].GetComponentsInChildren<SpriteRenderer>() ){
						sr.color = Color.Lerp(sr.color, backgroundComponentAsset[j].GetComponent<BackgroundAssetColor>().color_active[i], lerpSpeed);
						repeat[j] = ContinueLerping(sr.color,backgroundComponentAsset[j].GetComponent<BackgroundAssetColor>().color_active[i]);
					}
				}

				yield return new WaitForSeconds( waitForColorTimer );

				if (repeatCamera == false) {
					repeatAll = false;
				}else {
					repeatAll = true;
					continue;
				}
				for (int j = 0; j < repeat.Length; j++){ 
					if (repeat[i] == false ){
						repeatAll = false;
					}else {
						repeatAll = true;
						continue;
					}
				}

			} while(repeatAll);

			i = i==3 ? 0 : i+1;	
			yield return new WaitForSeconds(dayCycleTimer);
		}
	}

	private float customAbsClamp(float rate, float length, float value){
		
		float tempf = value + rate;

		if (Mathf.Abs(tempf) > Mathf.Abs(length) ){
			tempf=0;
		}
		return tempf;

	}

	private void moveBackground(){//BG V2 needs completing

	 	for (int i = 0; i < backgroundComponentAsset.Length; i++){
			 if (backgroundComponentAsset[i].tag=="AutoScrolling"){
			 backgroundComponentPosition[i] = customAbsClamp(
				Time.deltaTime * scrollSpeed * velocityMultiplier * backgroundComponentVelocity[i] 
				 + 
				Time.deltaTime * scrollSpeed *autoScrollSpeed* backgroundComponentVelocity[i],
				backgroundComponentAsset[i].GetComponent<BackgroundAssetColor>().spriteTileSizeX,
				backgroundComponentPosition[i]);
			 }
			 else{
			 backgroundComponentPosition[i] = customAbsClamp(Time.deltaTime * scrollSpeed * velocityMultiplier * backgroundComponentVelocity[i], backgroundComponentAsset[i].GetComponent<BackgroundAssetColor>().spriteTileSizeX, backgroundComponentPosition[i]);
			 }
			 backgroundComponentAsset[i].transform.position = startPosition + Vector3.left * backgroundComponentPosition[i];
		 }
	}




	private void clearBackground(){ //BG V2 Done
	}

	private void SpawnBackground(){

		backgroundComponentAsset = new GameObject[backgroundSGO.bc.Count];
		backgroundComponentVelocity	= new float[backgroundSGO.bc.Count];
		backgroundComponentPosition = new float[backgroundSGO.bc.Count];
		repeat = new bool[backgroundSGO.bc.Count];

		for (int i = 0; i < backgroundSGO.bc.Count; i++){
			backgroundComponentAsset[i] = Instantiate(backgroundSGO.bc[i].backgroundAssetPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			backgroundComponentVelocity[i] = backgroundSGO.bc[i].backroundAssetVelocity;
			backgroundComponentPosition[i] = 0f;
		}

	}

	private void SetColorActive(){

		for (int i = 0; i < backgroundComponentAsset.Length; i++){
			backgroundComponentAsset[i].GetComponent<BackgroundAssetColor>().color_active
			=
			backgroundComponentAsset[i].GetComponent<BackgroundAssetColor>().color_base;
		}

	}
	private float minPlayerSpeed = -20, maxPlayerSpeed= 20; 
	private void manageScrollSpeed(){
		if ( player.GetComponent<Rigidbody2D>().velocity.x <= maxPlayerSpeed && player.GetComponent<Rigidbody2D>().velocity.x >= minPlayerSpeed ){
			this.velocityMultiplier = 0;
		}else{
			this.velocityMultiplier = player.GetComponent<Rigidbody2D>().velocity.x;
		}
		//print (this.velocityMultiplier + "		"+ player.GetComponent<Rigidbody2D>().velocity.x );
		//this.velocityMultiplier = player.GetComponent<Rigidbody2D>().velocity.normalized.x;
		
	}
}
