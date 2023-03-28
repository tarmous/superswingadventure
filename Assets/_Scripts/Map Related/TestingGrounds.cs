using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class TestingGrounds : MonoBehaviour {

	public static TestingGrounds instance;
	public GameObject playerPrefab, mainCameraPrefab, silverCoinPrefab, goldPrefab, transparencyGatePrefab, bouncyGatePrefab, tutorialReleaseHookPrefab, tutorialThrowHookPrefab;
	private GameObject playerGameObject;

	public GameObject[] whatToSpawn;
	private List<GameObject> poolWhatToSpawn = new List<GameObject>();

	//const int defaultNumOfModules = 1;
	//public static int numToLoad, typeX; //oi metavlites gia to module. (numToLoad = posa modules na loadarei. Ta upolipa metavlites ekfrazoune ta status tou module. Morfi X_X_Module)
	public int typeY, typeX;
	private List<GameObject> moduleGO = new List<GameObject>();

	public GameObject GetPlayerGameObject(){return this.playerGameObject;}

	private List<GameObject> poolSilverCoin = new List<GameObject>(), poolGoldCoin = new List<GameObject>(), poolTransparency = new List<GameObject>(), poolBouncy = new List<GameObject>();
	private GameObject startModulePool;



	#region gold Coin Variables
	const float goldCointBaseProbability = 30f, probabilityIncrement = 10f;
	private float goldCoinProbability;
	private bool resetGoldCoinProbability = false;
	#endregion


	private void ResetVariables(){
		poolSilverCoin.Clear();
		poolGoldCoin.Clear();
		typeY=0;
		typeX=1;
	}

	private void SpawnAllTypes(){
		int howManySilver = 30, howManyGolds = 10, howManyGates = 3, howManyTransparencyGates = 3;
		float howFarBackX =10000;
		bool doOnce = true;

		foreach (GameObject go in whatToSpawn){
			if (doOnce){
				doOnce=false;
				poolWhatToSpawn.Add( Instantiate(go, new Vector3(-48f, 50, 0), Quaternion.identity ) as GameObject );
			}else{
				poolWhatToSpawn.Add( Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity ) as GameObject );
			}
			Tile[] list = poolWhatToSpawn[ poolWhatToSpawn.Count-1 ].GetComponentsInChildren<Tile> ();	//add the tiles of the module in the list.
			if (list.Length > 0) {
				foreach (Tile t1 in list) {
					if (t1.tileType == 2){ //an einai entrance
						poolWhatToSpawn[ poolWhatToSpawn.Count-1 ].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
					}
					if (t1.tileType == 3){ //an einai exit
						poolWhatToSpawn[ poolWhatToSpawn.Count-1 ].GetComponent<Module>().exit = t1; //valto stin lista ws exit
					}
				}
			}
		}


		//Miscaleneous pooling
		for (int i = 0; i < howManySilver; i++){
			poolSilverCoin.Add( Instantiate( silverCoinPrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity ) as GameObject );
		}

		for (int i = 0; i < howManyGolds; i++){
			poolGoldCoin.Add( Instantiate( goldPrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity ) as GameObject );
		}
		for (int i = 0; i < howManyGates; i++){
			poolTransparency.Add( Instantiate( transparencyGatePrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity ) as GameObject );
		}
		for (int i = 0; i < howManyTransparencyGates; i++){
			poolBouncy.Add( Instantiate( bouncyGatePrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity ) as GameObject );
			//poolGoldCoin[poolGoldCoin.Count-1].SetActive(false);
		}

	}
	
	public void SpawnSingleModule(Module m){
		//do nothing
	}

	private void ResetGoldProbability(){
		goldCoinProbability = goldCointBaseProbability;
	}


	private void SpawnMap(){
		//spawns map?!
		foreach (GameObject go in poolWhatToSpawn){
			moduleGO.Add(go);
		}
		AlignModules();
	}

	private GameObject GetAndRemovefromList(List<GameObject> list, int i, out List<GameObject> outList){
		//get an object from designated list and remove it at the same time
		//used for pooling
		//print(i);
		GameObject toReturn= list[i];
		list.RemoveAt(i);
		outList=list;
		return toReturn;
	}

	//int i=0;

	private void AlignModules(){
		//Align modules between entrance and exit (horizontal (local) connection only)

		List<Module> tmpModule = new List<Module> (); //temporary list of modules
		foreach (GameObject m in moduleGO) {
			tmpModule.Add (m.GetComponent<Module> ()); //add in the temporary list. The module from the in-game list
			m.gameObject.SetActive(true);
		}
	
		//mexri na ftash sto proteleuteo module
		for (int i = 0; i < tmpModule.Count - 1; i++) {
			//vazei to position tou epomenou module. Vriskei tin thesi aferodas tin thesi tou entrance tou epomenou me tin thesi tou exit tou torinou kai meta to anevazei ena epano.
			tmpModule [i + 1].transform.position = new Vector3(1,0,0) * 1f + tmpModule[i].exit.transform.position;
			tmpModule[i].next= tmpModule[i+1];
			tmpModule[i+1].previous=tmpModule[i];
			//tmpModule[i].gameObject.SetActive(true);
		}

		SpawnMisc(moduleGO);
		moduleGO.Clear();
	}

	private void SpawnMisc(List<GameObject> GoList){
		foreach(GameObject m in GoList){

			//spawn player
			if(m.GetComponent<Module>().playerSpawnLocation !=null) {
				playerGameObject= Instantiate(
					playerPrefab,
					m.GetComponent<Module>().playerSpawnLocation.transform.position
						+
					Vector3.zero,
					Quaternion.identity 
					) as GameObject;

				//spawn camera
				Instantiate(
					mainCameraPrefab,
					m.GetComponent<Module>().playerSpawnLocation.transform.position 
						+
					new Vector3(0,0,-50),
					Quaternion.identity 
					);
			}

			//spawn coins
			if(m.GetComponent<Module>().coinSpawnLocations != null){
				foreach (GameObject go in m.GetComponent<Module>().coinSpawnLocations){
					GameObject o = GetAndRemovefromList(poolSilverCoin, 0, out poolSilverCoin);
					
					o.GetComponent<Gold>().failSafeSpawning = true;
					o.SetActive(true);
					o.transform.position = go.transform.position;
					o.transform.parent = m.transform;
					//GameObject o = Instantiate(silverCoinPrefab, go.transform.position, Quaternion.identity) as GameObject;
				}
			}

			//spawn gold
			if(m.GetComponent<Module>().goldSpawnLocations !=null) {
				//roll dice.
				//if success spawn and reset probability base.
				//if fail increment probability
				foreach (GameObject go in m.GetComponent<Module>().goldSpawnLocations){
					GameObject o = GetAndRemovefromList(poolGoldCoin, 0, out poolGoldCoin);

					o.GetComponent<Gold>().failSafeSpawning = true;
					o.SetActive(true);
					o.transform.position = go.transform.position;
					o.transform.parent = m.transform;
				}
			}
			//spawn transparency gate
			if (m.GetComponent<Module>().TransparencyGate !=null) {
				GameObject o = GetAndRemovefromList(poolTransparency, 0, out poolTransparency);
					
				o.GetComponent<TransparencyGate>().failSafeSpawning = true;
				o.SetActive(true);
				o.transform.position = m.GetComponent<Module>().TransparencyGate.transform.position;
				o.transform.parent = m.transform;
			}
			//spawn bouncy gate
			if (m.GetComponent<Module>().bouncyGate !=null) {
				GameObject o = GetAndRemovefromList(poolBouncy, 0, out poolBouncy);
					
				o.GetComponent<BouncyGate>().failSafeSpawning = true;
				o.SetActive(true);
				o.transform.position = m.GetComponent<Module>().bouncyGate.transform.position;
				o.transform.parent = m.transform;
			}


		}
	}
	
	// Use this for initialization
	void Start () {
		instance = this;
		ResetVariables();
		SpawnAllTypes();
		SpawnMap();
		//SetupModule (); //Setups the module
		//AlignModules (); //aligns this module with the previous one
		//KeepListUpdated();
		//StartCoroutine(PlaySoundEffects());
	}
		
	public void LateUpdate(){
	}	
}
