using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFogFollow : MonoBehaviour {

    private GameObject player;
    private float x,y;
    private float VerticalOffset = 100f;
    private float followSpeed = 1f;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        x = gameObject.transform.position.x;
        y = gameObject.transform.position.y;
        //background = transform.GetChild(0).gameObject;
        //startPosition = background.transform.position;


    }
	
	// Update is called once per frame
	void Update () {
            //dont alter the 2 lines below. //failsafe if the player isnt found
            if ( player == null ) player = GameObject.FindGameObjectWithTag("Player");
            if ( player == null ) return;

            x = player.transform.position.x;

        if (y + VerticalOffset < player.transform.position.y){
            y = player.transform.position.y;
        }

        gameObject.transform.position = new Vector3(
           Mathf.Lerp(gameObject.transform.position.x, x, followSpeed),
             //Mathf.Lerp(gameObject.transform.position.y, y, followSpeed),
            gameObject.transform.position.y,
            gameObject.transform.position.z
        );

    }

    void LateUpdate(){
        //manageScrollSpeed();
		//moveBackground();
    }
/* 
    private float customAbsClamp(float rate, float length, float value){
		
		float tempf = value + rate;

		if (Mathf.Abs(tempf) > Mathf.Abs(length) ){
			tempf=0;
		}
		return tempf;

	}
	private void moveBackground(){
    	backgroundPosition = customAbsClamp(Time.deltaTime * scrollSpeed * velocityMultiplier * backgroundSpeed, tileSizeX, backgroundPosition); //= Mathf.Repeat(Time.deltaTime*scrollSpeed, tileSizeX);
        background.transform.position = startPosition + Vector3.left * backgroundPosition;
	}

    const float scrollSpeed = 10f;
	const float backgroundSpeed=1.3f;
    const float tileSizeX = 500.3f;
    private float velocityMultiplier=2f;
	private float backgroundPosition=0;
    private Vector3 startPosition;
    private GameObject background;

	 private void manageScrollSpeed(){
		this.velocityMultiplier = player.GetComponent<Rigidbody2D>().velocity.normalized.x;
		
	}  */
}
