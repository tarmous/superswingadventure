using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour {

    #region  Audio Variables
	public AudioClip hitWall_Audio;

    public float hitWallMinPitch = 0.5f;
    public float hitWallMaxPitch = 1;
    public float hitWallMinVolume = 0f;
    public float hitWallMaxVolume = 1;

    private float hitWallPitch, hitWallVolume;
    #endregion

    private ThrowHook th;


   // private const float hitWallVolume=0.7f;
    new private AudioSource audio;
	private Rigidbody2D r2d;
	
	void Start(){
		r2d = gameObject.GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        th = GetComponent<ThrowHook>();
	}

	// Update is called once per frame
	void LateUpdate () {
		//ManageWallBounceVolume();
       // ManageWallBouncePitch();
	}



    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag=="Player" || collision.gameObject.tag == "Rope") {
           // print("1");
            return;      
        }

        //player collided with wall.
		//Play sound effect
        ManageWallBounceVolume();
        ManageWallBouncePitch();
		audio.clip=hitWall_Audio;
		audio.pitch= hitWallPitch;
        audio.volume=hitWallVolume;
        audio.Play();

       // print("2");
        return;

    }


    private void ManageWallBounceVolume(){
        //Manage Volume Via line function
        //also gets variable from ThrowHook.cs

        float vel= Mathf.Sqrt( 
            Mathf.Pow(r2d.velocity.x , 2) 
            + 
            Mathf.Pow(r2d.velocity.y , 2) 
            );   
            
        float a = hitWallMaxVolume / th.GetDenominator(); 
        hitWallVolume = a * vel + hitWallMinVolume;  
    }


    private void ManageWallBouncePitch(){
        //Manage Volume Via line function
        //also gets variable from ThrowHook.cs

        float vel= Mathf.Sqrt( 
            Mathf.Pow(r2d.velocity.x , 2) 
            + 
            Mathf.Pow(r2d.velocity.y , 2) 
            );   
            
        float a = hitWallMinVolume / th.GetDenominator(); 
        hitWallPitch = a * vel + hitWallMinPitch; 
    }

}
