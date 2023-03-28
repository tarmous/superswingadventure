using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class Death  {
	public static float points, time, finalScore, coinsGathered;
	public static Vector3 playerPos;
	private const float constantTimer = 0.25f;

	public Death( float d_points, float d_time, Vector3 d_playerPos, float d_coinsGathered){
		points = d_points;
		time = d_time;
		playerPos = d_playerPos;
		coinsGathered=d_coinsGathered;
	}

	//"Dead" constructor
	public Death(){}

	private void DeactivateGameplayComponents(){
		Canvas[] canvas = MonoBehaviour.FindObjectsOfType<Canvas>();
		foreach (Canvas go in canvas){
			go.gameObject.SetActive(false);
		}

		GameManager.instance.StopAllCoroutines();
		GameManager.instance.gameObject.GetComponent<AudioSource>().Stop();
		BackgroundManager.instance.StopAllCoroutines();
		BackgroundManager.instance.stopUpdating = true;
	}

	public IEnumerator KillPlayer(){
		
		DeactivateGameplayComponents();

		Module[] moduleArray = MonoBehaviour.FindObjectsOfType<Module>();
		for (int i = 0; i < moduleArray.Length; i++){
			for (int j = 1 ; j< moduleArray.Length; j++){
				if (moduleArray[j].gameObject.transform.position.x < moduleArray[j-1].gameObject.transform.position.x){

					Module temp = moduleArray[j-1];
					moduleArray[j-1] = moduleArray[j];
					moduleArray[j] = temp;

				}
			}
		}
		int k = 0;
		if (moduleArray[k].isActiveAndEnabled){
			while ( moduleArray[k].gameObject.transform.position.x < playerPos.x){
				k++;
			}
		}
		if (k <= 0) k=1; //k should never return a value lower than 0
		string returned = new string(GetModuleNum(moduleArray[k-1]));
	
		//GameAnalytics.Initialize();
		//GameAnalytics.NewDesignEvent("PlayerDeath", int.Parse(returned) );
		float whatToParse;
		if ( float.TryParse(returned, out whatToParse) ){ 
			GameAnalytics.NewDesignEvent("PlayerDeath", float.Parse(returned) ); 
		}
		//this line (46) sometimes has an error
		//uknown char
		
		yield return new WaitForSecondsRealtime( constantTimer );
		//yield return null;
		for (int i = 0; i < SceneManager.sceneCount; i++){
			if (SceneManager.GetSceneAt(i).buildIndex == SceneIndexes.deathSceneIndex) yield break;
		}
		
		SceneManager.LoadSceneAsync( SceneIndexes.deathSceneIndex, LoadSceneMode.Additive);

	}



	
	private char[] GetModuleNum(Module m){
		//Get the id (number) from the end of the name of the module

		char[] charArray = m.name.ToCharArray();
		int nameLength = charArray.Length;
		int i =0;
		foreach (char c in charArray){
			i++;

			if ( c == 'e'){
				string returnArray = "";

				for (int j =i ; j < charArray.Length - 7; j++){ // 7 to remove the (clone) from end of name
					returnArray = returnArray + charArray[j];
				}

				return returnArray.ToCharArray();
			}

		}
		return new char[]{'a'};

	}



}
