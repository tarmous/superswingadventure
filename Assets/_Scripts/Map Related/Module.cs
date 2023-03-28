using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Module : MonoBehaviour
{

    public Module previous, next;
    public Tile entrance;
    public Tile exit;
    public GameObject playerSpawnLocation;
    public GameObject TransparencyGate, bouncyGate, magnetGate;
    public List<GameObject> tutorialSpawnLocation = new List<GameObject>();
    public List<GameObject> tutorialSpawnType = new List<GameObject>();
    public List<int> tutorialSpawnLocationID = new List<int>();
    public List<GameObject> coinSpawnLocations = new List<GameObject>();
    public List<GameObject> goldSpawnLocations = new List<GameObject>();

    const string shaderName = "Tiled2Unity/Default (Instanced)";
    const string wallTag = "TransparentWall";
    const int pathIndex = 1;
    const int solidIndex = 0;

    void Awake()
    {
        if (this.gameObject.transform.Find("Path").transform.childCount>0)
        {
            this.gameObject.transform.Find("Path").transform.GetChild(0).gameObject.GetComponent<Renderer>().material
			=
			this.gameObject.transform.Find("Solid").transform.GetChild(0).gameObject.GetComponent<Renderer>().material
			;
        }
    }

    void Start()
    {

        /* 
		Material m = new Material(Shader.Find(shaderName));
		//m = transparentMat;

		if (gameObject.transform.GetChild(pathIndex).GetChild(0).gameObject.tag == wallTag ){

			m.mainTexture = gameObject.transform.GetChild(solidIndex).GetChild(0).gameObject.GetComponent<Renderer>().material.mainTexture;
			gameObject.transform.GetChild(pathIndex).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
			
		}
		 */
    }
}
