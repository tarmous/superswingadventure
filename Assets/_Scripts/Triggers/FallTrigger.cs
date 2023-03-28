using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour {
	
	private bool doOnce;
	protected virtual void Awake(){
		doOnce = false;
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision){

		if (doOnce) return;

		if (collision.gameObject.tag == "Player") {
			GameManager.instance.StartCoroutine(GameManager.instance.GoRealTime());

			Camera.main.gameObject.GetComponent<CameraFollowPlayer>().enabled = false;
			collision.gameObject.GetComponent<DistanceJoint2D>().enabled = false;
			collision.gameObject.GetComponent<HingeJoint2D>().enabled = false; 

			if (collision.gameObject.GetComponent<ThrowHook>().DoesHookExist()) {
				collision.gameObject.GetComponent<ThrowHook>().ReturnCurrentHook().GetComponent<RopeScript>().vertexCount --;
				collision.gameObject.GetComponent<ThrowHook>().ReturnCurrentHook().GetComponent<RopeScript>().fellInTrigger = true;
			}

			collision.gameObject.GetComponent<ThrowHook>().enabled = false;
			doOnce = true;
		}
	}
}
