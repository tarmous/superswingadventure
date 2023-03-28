using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class StartMenu : MonoBehaviour {

	
	public GameObject startGameButton, loadingSceneText;
	public GameObject startMenuCanvas, optionsMenuCanvas;
	public GameObject playerParticles;

	private bool loadScene = true;
	private AsyncOperation asyncLoad;

	//private float relativeVolumeSFX = 0, relativeVolumeMusic = 0;
	public AudioMixer am;
	//public AudioMixerGroup []

	void Awake(){
		
	}

	// Use this for initialization

	//playservices variables
	private string authStatus;
	void Start () {
		startMenuCanvas.SetActive(true);
		//AdManager.instance.ShowBanner();	// Displays an ad banner
		

		if(loadScene){
            if (SceneManager.GetActiveScene().buildIndex == SceneIndexes.startMenuIndex){
                loadScene = !loadScene;
                startGameButton.SetActive(false);
                loadingSceneText.SetActive(true);
                StartCoroutine(LoadAsyncScene(SceneIndexes.sampleSceneIndex));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator LoadAsyncScene(int id){
        asyncLoad = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation=false;

        while (asyncLoad.progress < 0.9f){
            yield return null;
        }

        startGameButton.SetActive(true);
        loadingSceneText.SetActive(false);
        
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone){
            yield return null;
        }
    }

	public void StartGame(){
		if ( !asyncLoad.allowSceneActivation ){
			asyncLoad.allowSceneActivation=true;
		}
	}
/* 
	public void SignInCallback(bool success) {
        if (success) {
            Debug.Log("(SSA) Signed in!");
            // Show the user's name
            authStatus = "Signed in as: " + Social.localUser.userName;
			Debug.Log(authStatus);

        } else {
            Debug.Log("(SSA) Sign-in failed.");
            // Show failure message
            authStatus = "Sign-in failed";
        }
    }
 */
 /* 
	 public void SignIn() {
        if (!PlayGamesPlatform.Instance.localUser.authenticated) {
            // Sign in with Play Game Services, showing the consent dialog
            // by setting the second parameter to isSilent=false.
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        } else {
            // Sign out of play games
         
            
            // Reset UI
            authStatus = "";
        }
    }
 */
	public void ReturnButton(GameObject go){
		go.GetComponentInParent<Canvas>().gameObject.SetActive(false);
		//playerParticles.GetComponent<ParticleSystem>().Play();
		playerParticles.SetActive(true);
	}

	public void OpenOptionsMenu(){
		optionsMenuCanvas.SetActive(true);
		//playerParticles.GetComponent<ParticleSystem>().Stop();
		playerParticles.SetActive(false);
	}

	public void ExtiGame(){
		Application.Quit();
	}

	public void SetMusicVolume(float i){
		//relativeVolumeMusic = i;
		am.SetFloat("Music/Volume", i);
		PlayerPrefs.SetFloat("Music/Volume", i);
	}

	public void SetSFXVolume(float i){
		//relativeVolumeSFX = i;
		am.SetFloat("SFX/Volume", i);
		PlayerPrefs.SetFloat("SFX/Volume", i);
	}

}