using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates : MonoBehaviour
{
    [SerializeField]
    private GameObject repeatableAnim, oneShotAnim1, oneShotAnim2;
    public enum gateType { transparency, bouncy, magnet };
    public gateType _gateType;
    private bool wasLeftBehind, playerCollided = false;
    private AudioSource audioSource;
    public bool failSafeSpawning; //used for when is left behind and then spawned. //true: spawned by Map.cs //false: left behind

    private static Coroutine spectreCoroutine, magnetCoroutine;


    void OnDisable()
    {

        if (playerCollided)
        {
            wasLeftBehind = false;
        }
        else
        {
            wasLeftBehind = true;
        }
        playerCollided = false;
        failSafeSpawning = false;

        //wasLeftBehind = true;
        switch (_gateType)
        {
            case gateType.transparency:
                if (Map.instance) Map.instance.AddToPoolTransparency(this.gameObject);
                break;


            case gateType.magnet:
                if (Map.instance) Map.instance.AddToPoolMagnet(this.gameObject);
                break;


            case gateType.bouncy:
                if (Map.instance) Map.instance.AddToPoolBouncy(this.gameObject);
                break;


            default:
                Debug.Log("I AM ERROR OnDisable()");
                break;
        }


    }

    void OnEnable()
    {
        if (wasLeftBehind && !failSafeSpawning)
        {
            //wasLeftBehind = false;
            this.gameObject.transform.parent = null;
            this.gameObject.transform.position = new Vector3(10000, 10000, 0);
        }
        failSafeSpawning = false;
        wasLeftBehind = false;

        //GetComponent<SpriteRenderer>().enabled = true;

        repeatableAnim.GetComponent<SpriteRenderer>().enabled = true;   // Enable repeatable animation's sprite renderer
        oneShotAnim1.GetComponent<SpriteRenderer>().enabled = false;    // Disable one shot animation's sprite renderer
        oneShotAnim2.GetComponent<SpriteRenderer>().enabled = false;    // Disable one shot animation's sprite renderer

        oneShotAnim1.GetComponent<Animator>().StopPlayback();           // Stop animation playback
        oneShotAnim2.GetComponent<Animator>().StopPlayback();           // Stop animation playback
    }

    void Start()
    {
        //GetComponent<SpriteRenderer>().enabled = true; //turn on graphics
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            PlayerAttributes pa = collision.gameObject.GetComponent<PlayerAttributes>();
            GameObject tmp;
            float dur;

            switch (_gateType)
            {

                case gateType.transparency:
                    if (spectreCoroutine != null) pa.StopCoroutine(spectreCoroutine);
                    //pa.StopCoroutine(pa.ChangePlayerLayer());
                    spectreCoroutine = pa.StartCoroutine(pa.ChangePlayerLayer());
                    tmp = Map.instance.GetTransparencyParticleEffect();
                    dur = PlayerAttributes.transparencyDuration;

                    break;


                case gateType.magnet:
                    if (magnetCoroutine != null) pa.StopCoroutine(magnetCoroutine);
                    //pa.StopCoroutine("SpawnMagnet");
                    magnetCoroutine = pa.StartCoroutine("SpawnMagnet");
                    tmp = Map.instance.GetMagnetParticleEffect();
                    dur = PlayerAttributes.magnetDuration;
                    break;


                case gateType.bouncy:
                    pa.StopCoroutine("ChangePlayerBounciness");
                    pa.StartCoroutine("ChangePlayerBounciness");
                    tmp = Map.instance.GetMagnetParticleEffect();
                    dur = PlayerAttributes.magnetDuration;
                    break;


                default:
                    Debug.Log("I AM ERROR OnTriggerEnter2D()");
                    tmp = Map.instance.GetMagnetParticleEffect();
                    dur = 1;
                    break;
            }

            tmp.transform.parent = collision.gameObject.transform;
            tmp.transform.localPosition = Vector3.zero;
			if (tmp.GetComponent<ParticleScript>()) tmp.GetComponent<ParticleScript>().Duration = dur;
            tmp.SetActive(true);


            //GetComponent<SpriteRenderer>().enabled = false;
            audioSource.Play(); //play pick up sound



            playerCollided = true;
            this.gameObject.transform.parent = null;
            failSafeSpawning = false;

            repeatableAnim.GetComponent<SpriteRenderer>().enabled = false;  // Disable repeatable animation's sprite renderer
            oneShotAnim1.GetComponent<SpriteRenderer>().enabled = true;     // Enable one shot animation's sprite renderer
            oneShotAnim2.GetComponent<SpriteRenderer>().enabled = true;     // Enable one shot animation's sprite renderer

            //oneShotAnim1.GetComponent<Animation>().Rewind();
            oneShotAnim1.GetComponent<Animator>().Play("default", -1, 0f);

            //oneShotAnim2.GetComponent<Animation>().Rewind();
            oneShotAnim2.GetComponent<Animator>().Play("default", -1, 0f);

            yield return new WaitForSeconds(audioSource.clip.length);
            this.gameObject.SetActive(false);
        }

    }
}
