using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowHook : MonoBehaviour {

    #region Audio Variables
    public AudioClip launchHook_Audio, releaseHook_Audio;
    private AudioSource audio;
    public float audioVolume=0.4f;
   //public float audioPitch = 1;
    #endregion

    public GameObject hook;
    private GameObject curHook;
    private Rigidbody2D r2d;
    public float directionReleaseForce, velocityReleaseForceX, velocityReleaseForceY, hookForceX, hookForceY;
    private float maxVelocity, maxDirectionReleaseForce, denominator;

    public GameObject ReturnCurrentHook(){
        return this.curHook;
    }

    public bool DoesHookExist(){
        if (curHook == null){
            return false;
        }else {
            return true;
        }
    }

    private bool isHooked;

    public bool GetIsHooked(){ return this.isHooked;}
    public void SetIsHooked(bool i){ this.isHooked=i; }

    public float GetDenominator(){return this.denominator;}

    public AudioSource GetAudioSource(){return this.audio;}

    public Rigidbody2D getRigidBody(){return this.r2d;}

    // Use this for initialization
    void Awake () {
        maxDirectionReleaseForce = DebugControls.defaultDirectionReleaseForce;
        directionReleaseForce = maxDirectionReleaseForce; // FOR MANAGING DIRECTION RELEASE FORCE

        velocityReleaseForceX = DebugControls.defaultVelocityReleaseForceX;
        velocityReleaseForceY = DebugControls.defaultVelocityReleaseForceY;
        hookForceX = DebugControls.defaultHookForceX;
        hookForceY = DebugControls.defaultHookForceY;
        denominator=150f;   // FOR MANAGING DIRECTION RELEASE FORCE

        r2d = gameObject.GetComponent<Rigidbody2D>();       
        gameObject.GetComponent<DistanceJoint2D>().enabled = false;
        gameObject.GetComponent<HingeJoint2D>().enabled = false; // disable hinge joint component
        audio = GetComponent<AudioSource>();
    }   

	
	// Update is called once per frame
	void Update () {

        if ( Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() ) {return;}
        if ( Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) ) {return;}


        if ( GameManager.instance.GetRunningType() != GameManager.timeMode.paused
            &&
            ( Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) )){

            Vector2 destiny;
            if (Input.touchCount > 0){
                destiny =  Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }else{
                destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            //remove previous hook-ropes
            if (curHook != null) {
               DestroyHook(destiny, true);
                return;
             }
                ThrowTheHook(destiny);
        }
    }

    void FixedUpdate(){
        ManageReleaseForce();
    }

    private void ManageReleaseForce(){
        //need a y=-ax + maxDirectionRealeaseForce
        //get max velocity (which is x)
        //a= maxDir/x
        //for x = 0, we get max force,
        //for x = max velocity we get 0 force

        float vel= Mathf.Sqrt( 
            Mathf.Pow(r2d.velocity.x , 2) 
            + 
            Mathf.Pow(r2d.velocity.y , 2) 
            );   

         if (vel > denominator){
            denominator = vel;
        }
        float a = maxDirectionReleaseForce / denominator; 
        directionReleaseForce = -a * vel + maxDirectionReleaseForce;
        
    }

    public void ApplyForce(){
        //hook forces applies
        r2d.AddForce(r2d.velocity.normalized * new Vector2(hookForceX, hookForceY) );
    }

    public void ThrowTheHook(Vector2 destiny){

        //spawn new hook
        //angle between 2 points
        //in degrees
        float angleDeg = Mathf.Atan2(destiny.y - this.transform.position.y, destiny.x - this.transform.position.x)*180/Mathf.PI;
            //angleDeg = 45f;
        
        //turn degress to Quaternion
        Quaternion q = Quaternion.Euler(
                0,
                0,
                angleDeg + 90f
                );


        curHook = (GameObject)Instantiate( hook, this.transform.position, q);
        curHook.GetComponent<RopeScript>().direction = new Vector2(destiny.x- this.transform.position.x, destiny.y - this.transform.position.y);
        curHook.GetComponent<RopeScript>().SetPlayerGameObject(this.gameObject);

        curHook.layer= gameObject.layer;
        

        //play hook launch sound
        if (audio != null) {
        audio.clip = launchHook_Audio;
        audio.volume = audioVolume;
           audio.pitch = Random.Range(0.8f , 1.6f);
        //audio.pitch = audioPitch;
        audio.Play();
            
        }
    }

    public void DestroyHook(Vector2 destiny, bool playSound){
        gameObject.GetComponent<DistanceJoint2D>().enabled = false; //disable distance joint component
        gameObject.GetComponent<HingeJoint2D>().enabled = false; // disable hinge joint component
        Destroy(curHook);
        curHook = null;

        //play hook release sound
        if(playSound){
            audio.clip=releaseHook_Audio;
            audio.volume = audioVolume;
            audio.pitch = Random.Range(0.8f, 1.6f);
           // audio.pitch = audioPitch;
            audio.Play();
        }

        if(GetIsHooked()){
            
        r2d.AddForce(
            r2d.velocity.normalized * new Vector2(velocityReleaseForceX, velocityReleaseForceY) 
            + 
            new Vector2(
                destiny.x - this.transform.position.x,
                destiny.y - this.transform.position.y).normalized * directionReleaseForce
                );

        SetIsHooked(false);
        }
    }
}
