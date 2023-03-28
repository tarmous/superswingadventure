using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Hazard {

     protected override void Awake(){
        //base.Awake();
        //if ( GameManager.spikeDeathSFX != null ) death_Audio = GameManager.spikeDeathSFX;
    }

    protected override void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.GetComponent<RopeScript>()){
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ThrowHook>().DestroyHook(new Vector2(0,0), false );
            return;
        }
        if (collision.gameObject.tag == "Player") {
            ApplicationStartup.instance.PlaySpikeDeath();
            base.OnTriggerEnter2D(collision);    
            return;
        }

    }

}
