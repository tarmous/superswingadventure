using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

    protected AudioSource audio;
    
    [SerializeField]
    protected AudioClip death_Audio;

    protected virtual void Awake(){
        if (GetComponent<AudioSource>() != null){
            audio = GetComponent<AudioSource>();
        }else {
            audio = this.gameObject.AddComponent<AudioSource>();
            audio.playOnAwake = false;
        }
        
    }

	protected virtual void OnTriggerEnter2D(Collider2D collision){
 
        if (collision.gameObject.tag == "Player") {
            if (audio){
                audio.clip = death_Audio;
                audio.Play();
            }
            Vector3 playerPos = collision.gameObject.transform.position;
            collision.gameObject.SetActive(false);
            Camera.main.GetComponent<CameraShake>().ShakeIt();

            Death d = new Death(Score.instance.GetScore(), Score.instance.GetTimer(), playerPos, Score.instance.GetCoinsGathered());
            StartCoroutine(d.KillPlayer());

            return;
        } 
      
        
		
    }

}
