using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationStartup : MonoBehaviour {
	public static ApplicationStartup instance;

	#region AudioStuff
		private AudioSource audioSource;

		public AudioClip buttonPressSFX, spikeDeathSFX, questCompleted;

		public void PlayButtonPress(){
			Debug.Log("PlayButtonPress()");
			audioSource.clip = buttonPressSFX;
        audioSource.pitch = Random.Range(1f, 2f);
			audioSource.Play();
		}

		public void PlaySpikeDeath(){
			Debug.Log("PlaySpikeDeath()");
			audioSource.clip = spikeDeathSFX;
			audioSource.Play();
		}

		public void PlayCompletedQuest(){
			Debug.Log("PlayCompletedQuest()");
			audioSource.clip = questCompleted;
			audioSource.Play();


		}

	#endregion

	public void SetActiveSkin(string skinName){
		PlayerPrefs.SetString("BackgroundSkin", skinName);
	}

	private void Awake(){
		
		DontDestroyOnLoad(gameObject);

		#if UNITY_EDITOR					// this works only in unity editor
			PlayerPrefs.DeleteAll();		// deletes all player prefs so we can test
		#endif								// conditions properly

		instance = this;
		Application.targetFrameRate = 60;
		audioSource = GetComponent<AudioSource>();
		if ( !PlayerPrefs.HasKey("BackgroundSkin")){
			PlayerPrefs.SetString("BackgroundSkin", "DefaultSkin");
		}
	}

	public void LoadStartMenu(){
		int consentInt = PlayerPrefs.GetInt("result_gdpr", 0);
        bool consent = consentInt != 0;
        if(!consent) {
			//#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene("CustomGDPR");
			//#else
			//Application.LoadLevel("CustomGDPR");
			//#endif
        } else {
			//#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene(SceneIndexes.startMenuIndex);
			//#else
			//Application.LoadLevel(SceneIndexes.startMenuIndex);
			//#endif
		}

		//Debug.Log("Loading Start Menu");
		//SceneManager.LoadScene(SceneIndexes.startMenuIndex);
	}

	// Use this for initialization
	private void Start () {
		//SceneManager.LoadScene(SceneIndexes.startMenuIndex);		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	
}
