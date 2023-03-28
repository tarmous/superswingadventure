using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {

	public GameObject PauseMenuCanvas;
	private float privateTimeScale = 1; // used to check wether player was inside a tutorial box (slow mo) (default = 1)
	private GameManager.timeMode runningType;
	
	public void PauseGame(){
		
		

		if (Time.timeScale < 1){
			privateTimeScale = Time.timeScale;
		}else{
			privateTimeScale = 1;
		}
		this.EliminateTimeScale();
		
	}

	private void EliminateTimeScale(){
		runningType = GameManager.instance.GetRunningType();
		GameManager.instance.SetRunningType(GameManager.timeMode.paused);
		Time.timeScale = 0;
		PauseMenuCanvas.SetActive(true);
	}

	public void ResumeGame(){
		//need to check if player is in tutorial box
		GameManager.instance.SetRunningType(runningType);

		Time.timeScale = privateTimeScale;
		PauseMenuCanvas.SetActive(false);

		
	}

	public void ReturnToMainMenu(){

       SceneManager.LoadSceneAsync(SceneIndexes.startMenuIndex, LoadSceneMode.Single);
    }

	void OnApplicationPause(bool pauseStatus){
		if ( !pauseStatus  ) return; 

		int sceneCount =  SceneManager.sceneCount;
		for (int i = 0; i< sceneCount; i++){
			if (SceneManager.GetSceneAt(i).buildIndex == SceneIndexes.deathSceneIndex) return;
		}

		//this.EliminateTimeScale();
		PauseGame();
	}
}
