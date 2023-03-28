using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {
	//public enum timeMode {slow, real};
    private GameManager.timeMode runningType;  
	private bool triggerOnce=false; // turns true when is used
	public int tutorialID;
	public GameObject toReveal;

	private void OnTriggerEnter2D(Collider2D collision){
		//If tutorial is enabled
		if(collision.gameObject.tag == "Player"){
			
			if ( toReveal.ToString().StartsWith("TutorialReleaseHook") ){
				if ( !collision.gameObject.GetComponent<ThrowHook>().DoesHookExist() ) triggerOnce = true;
			}else if ( toReveal.ToString().StartsWith("TutorialThrowHook") ){
				if ( collision.gameObject.GetComponent<ThrowHook>().DoesHookExist() ) triggerOnce = true;
			}

			//if ( collision.gameObject.GetComponent<ThrowHook>().DoesHookExist() ) triggerOnce = true;

			if (GameManager.instance.GetIsTutorialEnabled() && !triggerOnce){

				triggerOnce = !triggerOnce;
				GameManager.instance.SetRunningType(GameManager.timeMode.slow);
				
				StopCoroutine(GameManager.instance.GoRealTime(toReveal));
				GameManager.instance.SetIsGoRealTimeRunning(false);

				StartCoroutine(GameManager.instance.GoSlowMotion(toReveal));
			}	
		}
	}

}
