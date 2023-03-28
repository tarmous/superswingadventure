using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFogCollider : Hazard {

    public static DeathFogCollider instance;

    protected override void Awake(){
        base.Awake();
        instance = this;
    }

    protected override void OnTriggerEnter2D(Collider2D collision){

        base.OnTriggerEnter2D(collision);    
        return;

    }
                
}
