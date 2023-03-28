using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSelector {
	private TextAsset levelData;
	private string[] levelDataString;
	//private string[][] levelDataRow; //string[Row][Rowvar]
	public List<GameObject> pickedObjects = new List<GameObject> (); //N: dimiourgise lista aop game objects
	public int typeX,typeY,numToLoad;
	protected static int prev_typeX = 54512;// prev_typeY = 0;
	private static Object[] loadedAssetsStart;
	/* private static Object[] loadedAssetsConnectors1; //intro
	private static Object[] loadedAssetsConnectors2; //easy
	private static Object[] loadedAssetsConnectors3; //intermediate
	private static Object[] loadedAssetsConnectors4; //expert */
	private static Object[] loadedAssetsConnectors10; //transparency gate
	private static Object[] loadedAssetsConnectors11; //visual breaking
	private bool spawnSpecial = false;

	public ModuleSelector(int numToLoad){

		SetupVariables(numToLoad,0,0, false);
		//LoadCSV();	
	}

	public ModuleSelector(int numToLoad, int typeX, int typeY){
		SetupVariables(numToLoad,typeX,typeY, true);
		//LoadCSV();
	}

	public ModuleSelector(int typeX, int typeY){
		SetupVariables(0, typeX,typeY, true);
		//LoadCSV();
	}

	//private

	private void SetupVariables(int numToLoad, int typeX, int typeY, bool spawnSpecial){
		this.numToLoad = numToLoad;
		this.typeX = typeX;
		this.typeY =typeY;
		this.spawnSpecial = spawnSpecial;

	}
/* 	private void LoadCSV(){
		levelData = Resources.Load<TextAsset>("MapProgressionTable"); //load CSV
		levelDataString = levelData.text.Split (new char[]{'\n'}); //split CSV into rows
		levelDataRow = new string[levelDataString.Length-1][]; 

		for (int i = 1; i < levelDataString.Length; i++) {
			string[] row = levelDataString[i].Split (new char[]{','}); //get the row from excel
			levelDataRow [i - 1] = new string[row.Length];
			for (int j = 0; j < row.Length; j++){
				levelDataRow [i - 1] [j] = row [j]; // store row variables in a huge public array
			}
		}
	} */

	/* private int GetPlayerProgression(Vector3 playerPos){
		//find how far the player has gone in the game
		//and get the correct Row of progression from CSV
		for (int i=0; i < levelDataRow.Length - 1; i++){
			if ( playerPos.x <= int.Parse(levelDataRow [i] [0]) ){
				return i;
			}
		}
		return levelDataRow.Length - 1;
	} */

	/* private int GetType(float a, float b, float c, float d){
		//"roll a dice" and get a random type Y
		//based on probability from the CSV.
		float num = Random.Range(0,100);
		if (num <a){
			return 1;
		}
		if (num <a+b){
			return 2;
		} 
		if (num <a+b+c){
			return 3;
		} 
			return 4;
	}

	private void ProgressionBasedTypes (Vector3 playerPos, out int typeX, out int typeY){
		
		//UpdateProfileStatistics ups = new UpdateProfileStatistics ();
		int playerProgression = GetPlayerProgression (playerPos);

		typeX = 1;
		typeY = GetType(
			int.Parse(levelDataRow [playerProgression] [1]),
			int.Parse(levelDataRow [playerProgression] [2]),
			int.Parse(levelDataRow [playerProgression] [3]),
			int.Parse(levelDataRow [playerProgression] [4])
						);
		//Debug.Log("Type picked: "+typeY);
		//Debug.Log("Player Progression: "+playerProgression);
		//numToLoad=int.Parse(levelDataRow [playerLevel] [3]);
		//typeX = 1;
        //typeY = 1;     
    }
 */
	//int numToLoad, int typeX, int typeY
	public void SelectModules(out List<GameObject> intro, out List<GameObject> easy, out List<GameObject> intermediate, out List<GameObject> expert, out GameObject startModule, out List<GameObject> transparecy, out List<GameObject> visualBreaking, out List<GameObject> bouncy, out List<GameObject> magnet){
		//Start Module	
			Object[] startModules = Resources.LoadAll ("Modules/Start", typeof(GameObject));
			GameObject go0 = new GameObject();
		//intro 1
			List<GameObject> go1 = new List<GameObject>();
			Object[] loadedAssetsConnectors1 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_1", typeof(GameObject));
		//easy 2
			List<GameObject> go2 = new List<GameObject>();
			Object[] loadedAssetsConnectors2 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_2", typeof(GameObject));
		//intermediate 3
			List<GameObject> go3 = new List<GameObject>();
			Object[] loadedAssetsConnectors3 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_3", typeof(GameObject));
		//expert 4
			List<GameObject> go4 = new List<GameObject>();
			Object[] loadedAssetsConnectors4 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_4", typeof(GameObject));
		//transparency gate 10
			List<GameObject> go5 = new List<GameObject>();
			Object[] loadedAssetsConnectors5 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_10", typeof(GameObject));
		//visual breaking 11
			List<GameObject> go6 = new List<GameObject>();
			Object[] loadedAssetsConnectors6 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_11", typeof(GameObject));
		//bouncy 12
			List<GameObject> go7 = new List<GameObject>();
			Object[] loadedAssetsConnectors7 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_12", typeof(GameObject));
		//magnet 13
			List<GameObject> go8 = new List<GameObject>();
			Object[] loadedAssetsConnectors8 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_13", typeof(GameObject));
		
		
		
		//Start Module
		foreach(Object ob in startModules){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 1) ){
						go0 = (GameObject) ob; 
						break; 
				}
		}
		//intro 1
		foreach(Object ob in loadedAssetsConnectors1){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 1) ){
						go1.Add ((GameObject) ob );  
				}
		}
		//easy 2
		foreach(Object ob in loadedAssetsConnectors2){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 2) ){
						go2.Add ((GameObject) ob );  
				}
		}
		//intermediate 3
		foreach(Object ob in loadedAssetsConnectors3){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 3) ){
						go3.Add ((GameObject) ob );  
				}
		}
		//expert 4
		foreach(Object ob in loadedAssetsConnectors4){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 4) ){
						go4.Add ((GameObject) ob );  
				}
		}
		//transparency 10
		foreach(Object ob in loadedAssetsConnectors5){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 10) ){
						go5.Add ((GameObject) ob );  
				}
		}
		//visual breaking 11
		foreach(Object ob in loadedAssetsConnectors6){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 11) ){
						go6.Add ((GameObject) ob );  
				}
		}
		// bouncy 12
		foreach(Object ob in loadedAssetsConnectors7){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 12) ){
						go7.Add ((GameObject) ob );  
				}
		}
		// magnet 13
		foreach(Object ob in loadedAssetsConnectors8){
				if ( ob.ToString().StartsWith(typeX.ToString()+"_" + 13) ){
						go8.Add ((GameObject) ob );  
				}
		}
			
		intro = go1;
		easy = go2;
		intermediate = go3;
		expert = go4;
		startModule = go0;
		transparecy = go5;
		visualBreaking = go6;
		bouncy = go7;
		magnet = go8;
	}


	/* public List<GameObject> SelectModules(bool b, Vector3 playerPos){
		//false: also gets a start module
		//true: gets only connector modules


		//numToLoad = 5;
		pickedObjects.Clear (); //N: adiase tin lista me ta epilegmena modules
		//Object[] loadedAssetsStart; //N: travixe to info gia ola ta start modules
		//Object[] loadedAssetsConnectors; //used to draw connector modules from the right folder
		int randomCounter=0;

		//Start Module
		if(!b){
			loadedAssetsStart = Resources.LoadAll ("Modules/Start", typeof(GameObject));
			if (loadedAssetsStart.Length != 0) {	
				randomCounter= Random.Range (0, loadedAssetsStart.Length);
				pickedObjects.Add ((GameObject)loadedAssetsStart[randomCounter]);
			}
		}

		//Connector Modules
		
			//if dont want to alter types if we want to spawn special
			//if(!spawnSpecial){ ProgressionBasedTypes(playerPos, out typeX,out typeY); }

			//check if typeX(skin maybe?!) are equal before loading (no need if its the same)
			//Debug.Log(prev_typeX);
			//if (prev_typeX != typeX){
				
				//intro 1
					Object[] loadedAssetsConnectors1 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_1", typeof(GameObject));

				//easy 2
					Object[] loadedAssetsConnectors2 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_2", typeof(GameObject));
				
				//intermediate 3
					Object[] loadedAssetsConnectors3 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_3", typeof(GameObject));
				
				//expert 4
					Object[] loadedAssetsConnectors4 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_4", typeof(GameObject));
				
				//transparencygate 10
					Object[] loadedAssetsConnectors10 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_10", typeof(GameObject));
				
				//visualbreaking 11
					Object[] loadedAssetsConnectors11 = Resources.LoadAll ("Modules/" + typeX.ToString() + "_11", typeof(GameObject));
				
				//prev_typeX = typeX;			
			//}
			
			for (int i = 0; i < numToLoad;) {
			int j; //N: pare stin tixi kapio apo ta loaded modules
			 //N: Kitaei gia to module pou xekinaei me to sigekrimeno onoma
			 //N: vale to epilegmeno loaded module sto epilegmena modules
			switch (typeY){
				//intro
				case 1:{
					j = Random.Range (0, loadedAssetsConnectors1.Length);
					if ( loadedAssetsConnectors1[j].ToString().StartsWith(typeX.ToString()+"_"+typeY.ToString()) ){
						pickedObjects.Add ((GameObject)loadedAssetsConnectors1 [j]);  
						i++;
					}
					break;
				}
				//easy
				case 2:{
					j = Random.Range (0, loadedAssetsConnectors2.Length);
					if ( loadedAssetsConnectors2[j].ToString().StartsWith(typeX.ToString()+"_"+typeY.ToString()) ){
						pickedObjects.Add ((GameObject)loadedAssetsConnectors2 [j]);  
						i++;
					}
					break;
				}
				//intermediate	
				case 3:{
					j = Random.Range (0, loadedAssetsConnectors3.Length);
					if ( loadedAssetsConnectors3[j].ToString().StartsWith(typeX.ToString()+"_"+typeY.ToString()) ){
						pickedObjects.Add ((GameObject)loadedAssetsConnectors3 [j]);  
						i++;
					}
					break;
				}
				//expert	
				case 4:{
					j = Random.Range (0, loadedAssetsConnectors4.Length);
					if ( loadedAssetsConnectors4[j].ToString().StartsWith(typeX.ToString()+"_"+typeY.ToString()) ){
						pickedObjects.Add ((GameObject)loadedAssetsConnectors4 [j]);  
						i++;
					}
					break;
				}
				//transparency gate	
				case 10:{
					j = Random.Range (0, loadedAssetsConnectors10.Length);
					if ( loadedAssetsConnectors10[j].ToString().StartsWith(typeX.ToString()+"_"+typeY.ToString()) ){
						pickedObjects.Add ((GameObject)loadedAssetsConnectors10 [j]);  
						i++;
					}
					break;
				}
				//visual breaking	
				case 11:{
					j = Random.Range (0, loadedAssetsConnectors11.Length);
					if ( loadedAssetsConnectors11[j].ToString().StartsWith(typeX.ToString()+"_"+typeY.ToString()) ){
						pickedObjects.Add ((GameObject)loadedAssetsConnectors11 [j]);  
						i++;
					}
					break;
				}
										
				default:
					break;
			}
		}

		SortPickedObjects();
		return pickedObjects;
	} */

	private void SortPickedObjects(){
		//sort pickedObjects (never spawn 2 same in a row)
		for (int i=0; i < pickedObjects.Count - 1; i++){
			if (pickedObjects[i].name == pickedObjects[i+1].name){
				pickedObjects.RemoveAt(i+1);
				i--;
			}
		}
	}

}
