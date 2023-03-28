using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{

    #region Variables
    private Rigidbody2D r2d;
    private Collider2D cc2d;

    [SerializeField]
    private GameObject magnet;
    public Material transparencyMat;
    [SerializeField]
    private Material trailMatSpecter, trailMatMagnet;
    private Material trailMatDefault;
    private Material defaultMat;
    const int layer_Ball = 8;
    const int layer_BallTransparent = 9;
    public const float transparencyDuration = 5f;
    public const float transparencyProlongDuration = 0.5f;
    public const float bouncyDuration = 7f;
    public const float bouncyValue = 2;// 0:full damp, 1:perfect bouncy(no energy loss)
    public const float magnetDuration = 10;
    #endregion

    private bool isOverlappingWall = false;
    void Awake()
    {
        trailMatDefault = GetComponent<TrailRenderer>().material;

        //get components
        r2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<Collider2D>();

        //set attributes
        //mass
        //gravity scale
        //linear drag
        //angular drag
        //bounciness

        r2d.mass = DebugControls.defaultMass;
        r2d.gravityScale = DebugControls.defaultGravityScale;
        r2d.drag = DebugControls.defaultLinearDrag;
        r2d.angularDrag = DebugControls.defaultAngularDrag;
        cc2d.sharedMaterial.bounciness = DebugControls.defaultBounciness;

        if (!magnet) { this.magnet = this.gameObject.transform.Find("Magnet").gameObject; }

    }

    void Start()
    {

        magnet.SetActive(false);

    }

    #region  trail related
    private void ChangeTrailMaterial(Material mat)
    {
        // Debug.Log("changing trail mat");
        GetComponent<TrailRenderer>().material = mat;
    }
    #endregion trail related

    #region  Magnet Effect
    private void EnableMagnet()
    {
        this.magnet.SetActive(true);
        GameManager.instance.PlayMagnetGateMusic();
    }
    private void DisableMagnet()
    {
        this.magnet.SetActive(false);
        GameManager.instance.StopMagnetGateMusic();
    }

    public IEnumerator SpawnMagnet()
    {
        EnableMagnet();
        ChangeTrailMaterial(trailMatMagnet);
        yield return new WaitForSeconds(magnetDuration);
        DisableMagnet();
        ChangeTrailMaterial(trailMatDefault);
    }
    #endregion Magnet Effect

    #region Transparency Effect


    public void setIsOverlappingWall(bool i) { this.isOverlappingWall = i; }


    public IEnumerator ChangePlayerBounciness()
    {
        r2d.sharedMaterial.bounciness = bouncyValue;
        yield return new WaitForSeconds(bouncyDuration);
        r2d.sharedMaterial.bounciness = DebugControls.defaultBounciness;

    }
    public IEnumerator ChangePlayerLayer()
    {
        //start and stop coroutine to reset
        gameObject.layer = layer_BallTransparent;
        MakeWallsTransparent();
        ChangeTrailMaterial(trailMatSpecter);

        yield return new WaitForSeconds(transparencyDuration);

        //	gameObject.GetComponent<AudioSource>().clip = GameManager.instance.transparencyEffectEnding;
        //     gameObject.GetComponent<AudioSource>().Play();
        // yield return new WaitForSeconds(GameManager.instance.transparencyEffectEnding.length + 1f);
        while (isOverlappingWall)
        {
            yield return new WaitForSeconds(transparencyProlongDuration);
        }

        MakeWallsSolid();
        ChangeTrailMaterial(trailMatDefault);
        gameObject.layer = layer_Ball;
    }

    private void MakeWallsTransparent()
    {

        GameManager.instance.PlaySpectreGateMusic();

        Module[] objectsFound = FindObjectsOfType<Module>() as Module[];
        GameObject[] walls = new GameObject[objectsFound.Length];
        for (int i = 0; i < objectsFound.Length; i++)
        {

            if (objectsFound[i].gameObject.transform.Find("Path").transform.childCount > 0) walls[i] = objectsFound[i].gameObject.transform.Find("Path").transform.GetChild(0).gameObject;
        }

        foreach (GameObject go in walls)
        {
			if (!go) continue;
            if (!go.GetComponent<Renderer>()) continue; //FAILSAFE

            if (defaultMat == null)
            {
                if (!go.GetComponent<Renderer>().material)
                {
                    print("no default material..... w8 wat?");
                    continue;
                }
                defaultMat = go.GetComponent<Renderer>().material;
            }
            go.GetComponent<Renderer>().material = transparencyMat;
        }
    }

    private void MakeWallsSolid()
    {

        GameManager.instance.StopSpectreGateMusic();

        Module[] objectsFound = FindObjectsOfType<Module>() as Module[];
        GameObject[] walls = new GameObject[objectsFound.Length];
        for (int i = 0; i < objectsFound.Length; i++)
        {
            if (objectsFound[i].gameObject.transform.Find("Path").transform.childCount > 0) walls[i] = objectsFound[i].gameObject.transform.Find("Path").transform.GetChild(0).gameObject;
        }
        //GameObject[] walls = GameObject.FindGameObjectsWithTag("TransparentWall");
        foreach (GameObject go in walls)
        {
            if (!go) continue;
            if (!go.GetComponent<Renderer>()) continue; //FAILSAFE

            go.GetComponent<Renderer>().material = defaultMat;
        }
    }
    #endregion

}
