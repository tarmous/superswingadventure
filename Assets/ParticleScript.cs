using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{

    private float _duration;

    public float Duration
    {
        get
        {
			return this._duration;
        }
        set
        {
			this._duration = value;
        }
    }
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private IEnumerator ShowSpiral()
    {
        ps.Play();
        yield return new WaitForSeconds(_duration);
        transform.parent = null;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //ParticleSystem.ShapeModule pss = ps.shape;
        StartCoroutine(ShowSpiral());

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(speed* Time.deltaTime);
    }
}
