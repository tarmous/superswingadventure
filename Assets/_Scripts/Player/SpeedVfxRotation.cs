using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedVfxRotation : MonoBehaviour
{

    Rigidbody2D rigidbody;
    private const float maxAnimationSpeed = 5,			//
                        minAnimationSpeed = 1,			// if you change theese, things break
                        speedPercentageTrigger = 0.65f;	// 

    void Awake()
    {
        rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Rotate(rigidbody.velocity + (Vector2)this.transform.position);
        ManageAnimationSpeed();
    }

    private void Rotate(Vector2 destiny)
    {
        float angleDeg = Mathf.Atan2(destiny.y - this.transform.position.y, destiny.x - this.transform.position.x) * 180 / Mathf.PI;
        this.transform.rotation = Quaternion.Euler(
                                                    0,
                                                    0,
                                                    angleDeg
                                                );
    }


    private string ManageAnimationSpeed()
    {
        float maxVel = GetComponentInParent<ThrowHook>().GetDenominator();

        float vel = Mathf.Sqrt(
            Mathf.Pow(rigidbody.velocity.x, 2)
            +
            Mathf.Pow(rigidbody.velocity.y, 2)
            );

        if (maxVel * speedPercentageTrigger > vel)
        {
            // hide
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            return "hid sprite renderer";
        }
        else
        {
            // show
            GetComponentInChildren<SpriteRenderer>().enabled = true;

            const float a = 4.4f;
            const float y = a * speedPercentageTrigger;
			const float c = maxAnimationSpeed * y;


            float animamtionSpeed = (vel / maxVel - speedPercentageTrigger) * c;
			//Debug.Log("Animation Speed: " + animamtionSpeed);


            GetComponentInChildren<Animator>().speed = Mathf.Clamp(animamtionSpeed, minAnimationSpeed, maxAnimationSpeed);
            return GetComponentInChildren<Animator>().speed.ToString();
        }
    }

}
