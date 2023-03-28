using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

    public float distance;
    new private AudioSource audio;
    public GameObject nodePrefab;
    private GameObject player;
    private GameObject lastNode;


    public void SetPlayerGameObject(GameObject gameObject){ this.player = gameObject; }



    #region Hook Movement Variables
    public float speed = 1;
    public Vector2 direction;
    private bool done = false;
    private bool translateHook = true;
    private Rigidbody2D rb;
    private HingeJoint2D hj;
    #endregion


    #region Line Renderer Variables
    private LineRenderer lr;
    public int vertexCount=2;
	public List<GameObject> Nodes = new List<GameObject>();
    public bool fellInTrigger;  // check for FallTrigger.CS
    #endregion

    // Use this for initialization
    void Start(){
        fellInTrigger = false;
        //hinge joints neeeds to be disabled because it causes weird behaviour
        hj = GetComponent<HingeJoint2D>();
        hj.enabled=false;

        if (player == null ) player = GameObject.FindGameObjectWithTag("Player");
        lastNode = transform.gameObject;

        lr = GetComponent<LineRenderer>();
        Nodes.Add (gameObject);
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if (player == null ){
            player = GameObject.FindGameObjectWithTag("Player");
            rb.velocity = Vector3.zero;
            return;
        }

            if (translateHook) {
               // gameObject.transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
                rb.velocity = direction.normalized * speed;

                /* if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance) {
                   // CreateNode();
                } */
            }else if (done == false){
                done = true;

                    //while (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance ) {
                        CreateNode();
                    //}

                    //lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
                    if (player.GetComponent<ThrowHook>().enabled == true ){
                        player.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
                        player.GetComponent<DistanceJoint2D>().connectedBody = gameObject.GetComponent<Rigidbody2D>();
                        player.GetComponent<DistanceJoint2D>().enabled = true; // enable distance joint component
                        player.GetComponent<HingeJoint2D>().enabled = true; // enable hinge joint component
                    }
                    if(player.GetComponent<ThrowHook>().GetIsHooked()){
                        player.GetComponent<ThrowHook>().ApplyForce();
                    }
             }
    }

    void LateUpdate(){
        if (player == null ) return;
        PlayerHookLayerConnection(player.GetComponent<HingeJoint2D>());
        RenderLine();
    }


    void RenderLine(){

		lr.positionCount = vertexCount;
        //if ( !player.activeSelf ) lr.positionCount--;   // if player is dead we have 1 less position to take into account
		int i;
		for (i = 0; i < Nodes.Count; i++) {

			lr.SetPosition (i, Nodes [i].transform.position);

		}
		if ( player.activeSelf && !fellInTrigger ) lr.SetPosition (i, player.transform.position); // if player is dead dont draw up to him
	}

    void CreateNode(){
        while (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance ) {
            Vector2 pos2Create = player.transform.position - lastNode.transform.position;
            pos2Create.Normalize();
            pos2Create *= distance;
            pos2Create += (Vector2)lastNode.transform.position;

            GameObject go = (GameObject)Instantiate(nodePrefab, pos2Create, this.transform.rotation);
            go.transform.SetParent(transform);
            go.layer = gameObject.layer;

            //lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>(); // connect hinje joint to next
            go.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
            go.GetComponent<DistanceJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>(); //connect distance joint to previous
            lastNode = go;
            //lastNode.GetComponent<DistanceJoint2D>().connectedBody = gameObject.GetComponent<Rigidbody2D>();

            Nodes.Add (lastNode);
            vertexCount++;
        }
    }

    private void PlayerHookLayerConnection(HingeJoint2D hj2d){
       // if (hj2d == null ) return;
        this.gameObject.layer = player.layer;
        if (hj2d.connectedBody) HookRopesLayerConnection(hj2d.connectedBody.GetComponent<HingeJoint2D>());
    }

    private void HookRopesLayerConnection(HingeJoint2D hj2d){
        hj2d.gameObject.layer = player.layer;
        if (hj2d.connectedBody ){
            if ( hj2d.connectedBody.gameObject != null && hj2d.connectedBody.gameObject.tag != "Player" ){
                HookRopesLayerConnection(hj2d.connectedBody.gameObject.GetComponent<HingeJoint2D>());
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (player == null ) return;
         if (collision.gameObject.tag=="Player" || collision.gameObject.tag == "Rope" ) {
           // print("1");
            return;      
        }

        hj.enabled=true; //enable hinje joint
        player.GetComponent<ThrowHook>().SetIsHooked(true);
        player.GetComponent<ThrowHook>().GetAudioSource().Stop(); //intended to stop "throw rope" sfx

        //rope landed. play sound effect
        audio.pitch = Random.Range(0.7f, 1.6f);
        audio.volume = 1f;
        audio.Play();


        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        translateHook = false;

        return;
    }

}
