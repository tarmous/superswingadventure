using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    private GameObject player;
    public float followSpeed;
    private float smoothTime = 0.125f;
    private const float minSmoothTime= 0.125f;
    private const float maxSmoothTime= 0.4f;
    //private Vector3 vectorSpeed;
    private  Vector3 cameraOffset = new Vector3(50,0,-100);

	// Use this for initialization
	void Start () {
        smoothTime = minSmoothTime;
        followSpeed = DebugControls.defaultFollowSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
    void FixedUpdate() {
         if(player==null){
            //Debug.Log("player not found in CameraFollowPlayer.cs");
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        ManageSmoothingTime();
        Vector3 desiredPosition = player.transform.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime);
        transform.position = smoothedPosition;
    }

    private void ManageSmoothingTime(){
        
        Vector3 difference = (player.transform.position + cameraOffset) - transform.position;

        smoothTime =
        minSmoothTime 
        + 
        ( Mathf.Abs( Vector2.Distance(difference, Vector2.zero) ) / Mathf.Abs(cameraOffset.x) ) * (maxSmoothTime - minSmoothTime);

        smoothTime = Mathf.Clamp(smoothTime, minSmoothTime, maxSmoothTime);
        
        
    }
}
