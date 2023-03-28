using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Map : MonoBehaviour
{

    #region Audio Variables
    //private AudioSource audioSource;
    //public AudioClip startSFX;
    //public AudioClip[] startMusicClips, postMusicClips;// musicClip2, musicClip3;
    //public float audioOffset;
    #endregion

    public static Map instance;
    public GameObject playerPrefab, mainCameraPrefab, silverCoinPrefab, goldPrefab, transparencyGatePrefab, transparencyGateParticlePrefab, bouncyGatePrefab, tutorialReleaseHookPrefab, tutorialThrowHookPrefab, magnetGatePrefab, magnetGateParticlePrefab;
    private GameObject playerGameObject;

    const int defaultNumOfModules = 1;
    //public static int numToLoad, typeX; //oi metavlites gia to module. (numToLoad = posa modules na loadarei. Ta upolipa metavlites ekfrazoune ta status tou module. Morfi X_X_Module)
    public int typeY, typeX;
    private List<Module> module = new List<Module>(); //A list of files for all the modules.
    private List<GameObject> moduleGO = new List<GameObject>();

    public GameObject GetPlayerGameObject() { return this.playerGameObject; }

    private List<GameObject> poolIntro = new List<GameObject>(), poolEasy = new List<GameObject>(), poolIntermediate = new List<GameObject>(), poolExpert = new List<GameObject>(),
                             poolTransparencyModule = new List<GameObject>(), poolVisualBreaking = new List<GameObject>(), poolBouncyModule = new List<GameObject>(), poolMagnetModule = new List<GameObject>(),
                             poolSilverCoin = new List<GameObject>(), poolGoldCoin = new List<GameObject>(), poolTransparency = new List<GameObject>(), poolBouncy = new List<GameObject>(), poolMagnet = new List<GameObject>();
    private GameObject startModulePool, transparencyGateParticlePool, magnetGateParticlePool;



    #region gold Coin Variables
    const float goldCointBaseProbability = 30f, probabilityIncrement = 10f;
    private float goldCoinProbability;
    private bool resetGoldCoinProbability = false;
    #endregion




    #region Hard Coded Special Module Variables
    private static int counterPerModuleSpawnGate = 0, counterPerModuleSpawnVisualBreaking = 0, counterPerModuleSpawnBouncy = 0, counterPerModuleSpawnMagnet = 0;
    const int perModuleSpawnGate = 11, perModuleSpawnVisualBreaking = 15 /* to be implemented */, perModuleSpawnBouncy = 13 /* to be implemented */, perModuleSpawnMagnet = 15, numOfSpecials = 1, specialPriority = 11; //Per X number of modules spawn something specific 
    const int typeY_Gate = 10, typeY_VisualBreaking = 11, typeY_Bouncy = 12, typeY_Magnet = 13;

    public GameObject GetStartModule() { return this.startModulePool; }
    public void AddToPoolIntro(GameObject go) { this.poolIntro.Add(go); }
    public void AddToPoolEasy(GameObject go) { this.poolEasy.Add(go); }
    public void AddToPoolIntermediate(GameObject go) { this.poolIntermediate.Add(go); }
    public void AddToPoolExpert(GameObject go) { this.poolExpert.Add(go); }
    public void AddToPoolTransparencyModule(GameObject go) { this.poolTransparencyModule.Add(go); }
    public void AddToPoolBouncyModule(GameObject go) { this.poolBouncyModule.Add(go); }
    public void AddToPoolVisualBreaking(GameObject go) { this.poolVisualBreaking.Add(go); }
    public void AddToPoolMagnetModule(GameObject go) { this.poolMagnetModule.Add(go); }
    public void AddToPoolSilver(GameObject go) { this.poolSilverCoin.Add(go); }
    public void AddToPoolGold(GameObject go) { this.poolGoldCoin.Add(go); }
    public void AddToPoolTransparency(GameObject go) { this.poolTransparency.Add(go); }
    public void AddToPoolBouncy(GameObject go) { this.poolBouncy.Add(go); }
    public void AddToPoolMagnet(GameObject go) { this.poolMagnet.Add(go); }

    public int GetCounterPerModuleSpawnGate() { return counterPerModuleSpawnGate; }
    public int GetCounterPerModuleVisualBreaking() { return counterPerModuleSpawnVisualBreaking; }
    public int GetCounterPerModuleSpawnBouncy() { return counterPerModuleSpawnBouncy; }
    public int GetCounterPerModuleSpawnMagnet() { return counterPerModuleSpawnMagnet; }
    public GameObject GetTransparencyParticleEffect() { return this.transparencyGateParticlePool; }
    public GameObject GetMagnetParticleEffect() { return this.magnetGateParticlePool; }

    public void SetCounterPerModuleSpawnGate(int i) { counterPerModuleSpawnGate = i; }
    public void SetCounterPerModuleVisualBreaking(int i) { counterPerModuleSpawnVisualBreaking = i; }
    public void SetCounterPerModuleSpawnBouncy(int i) { counterPerModuleSpawnBouncy = i; }
    public void SetCounterPerModuleSpawnMagnet(int i) { counterPerModuleSpawnMagnet = i; }
    #endregion


    public static bool RollDice(float min, float trueMax, float virtualMax)
    {
        float f = Random.Range(min, trueMax);

        if (f < virtualMax) return true;
        return false;
    }

    private void ResetVariables()
    {
        counterPerModuleSpawnGate = 0;
        counterPerModuleSpawnMagnet = 0;
        //counterPerModuleSpawnVisualBreaking = 0;	// Removed
        //counterPerModuleSpawnBouncy = 0;			// Removed
        poolIntro.Clear();
        poolEasy.Clear();
        poolIntermediate.Clear();
        poolExpert.Clear();
        poolSilverCoin.Clear();
        poolGoldCoin.Clear();
        poolVisualBreaking.Clear();
        poolTransparencyModule.Clear();
        poolBouncy.Clear();
        poolMagnetModule.Clear();
        poolBouncyModule.Clear();
        typeY = 0;
        typeX = 1;
        ResetGoldProbability();
    }

    private void SpawnAllTypes()
    {
        List<GameObject> resourcesListIntro = new List<GameObject>();
        List<GameObject> resourcesListEasy = new List<GameObject>();
        List<GameObject> resourcesListIntermediate = new List<GameObject>();
        List<GameObject> resourcesListExpert = new List<GameObject>();
        List<GameObject> resourcesListTransparency = new List<GameObject>();
        List<GameObject> resourcesListVisualBreaking = new List<GameObject>();
        List<GameObject> resourcesListBouncy = new List<GameObject>();
        List<GameObject> resourcesListMagnet = new List<GameObject>();
        int howManySilver = 200, howManyGolds = 10, howManyTransparencyGates = 6, /* howManyBouncyGates = 6, */ howManyMagnetGates = 6;

        ModuleSelector ms = new ModuleSelector(typeX, 1);
        ms.SelectModules(out resourcesListIntro, out resourcesListEasy, out resourcesListIntermediate, out resourcesListExpert, out startModulePool,
                         out resourcesListTransparency, out resourcesListVisualBreaking, out resourcesListBouncy, out resourcesListMagnet);
        float howFarBackX = 10000;


        //Module Pooling
        startModulePool = Instantiate(startModulePool, new Vector3(-48f, 50f, 0), Quaternion.identity) as GameObject;
        {//encapsulation
            Tile[] list = startModulePool.GetComponentsInChildren<Tile>();  //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2)
                    { //an einai entrance
                        startModulePool.GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                                                                              //print(startModulePool.GetComponent<Module>().entrance);
                    }
                    if (t1.tileType == 3)
                    { //an einai exit
                        startModulePool.GetComponent<Module>().exit = t1; //valto stin lista ws exit
                                                                          //print(startModulePool.GetComponent<Module>().exit);
                    }
                }
            }
        }
        //INTRO MODULES
        foreach (GameObject go in resourcesListIntro)
        {

            poolIntro.Add(Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            Tile[] list = poolIntro[poolIntro.Count - 1].GetComponentsInChildren<Tile>();   //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2)
                    { //an einai entrance
                        poolIntro[poolIntro.Count - 1].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                    }
                    if (t1.tileType == 3)
                    { //an einai exit
                        poolIntro[poolIntro.Count - 1].GetComponent<Module>().exit = t1; //valto stin lista ws exit
                    }
                }
            }
            poolIntro[poolIntro.Count - 1].SetActive(false);

        }
        //EASY MODULES
        foreach (GameObject go in resourcesListEasy)
        {

            poolEasy.Add(Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            Tile[] list = poolEasy[poolEasy.Count - 1].GetComponentsInChildren<Tile>(); //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2) //an einai entrance
                        poolEasy[poolEasy.Count - 1].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                    if (t1.tileType == 3) //an einai exit
                        poolEasy[poolEasy.Count - 1].GetComponent<Module>().exit = t1; //valto stin lista ws exit
                }
            }
            poolEasy[poolEasy.Count - 1].SetActive(false);

        }
        //INTERMEDIATE MODULES
        foreach (GameObject go in resourcesListIntermediate)
        {

            poolIntermediate.Add(Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            Tile[] list = poolIntermediate[poolIntermediate.Count - 1].GetComponentsInChildren<Tile>(); //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2) //an einai entrance
                        poolIntermediate[poolIntermediate.Count - 1].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                    if (t1.tileType == 3) //an einai exit
                        poolIntermediate[poolIntermediate.Count - 1].GetComponent<Module>().exit = t1; //valto stin lista ws exit
                }
            }
            poolIntermediate[poolIntermediate.Count - 1].SetActive(false);

        }
        //EXPERT MODULES
        foreach (GameObject go in resourcesListExpert)
        {

            poolExpert.Add(Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            Tile[] list = poolExpert[poolExpert.Count - 1].GetComponentsInChildren<Tile>(); //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2) //an einai entrance
                        poolExpert[poolExpert.Count - 1].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                    if (t1.tileType == 3) //an einai exit
                        poolExpert[poolExpert.Count - 1].GetComponent<Module>().exit = t1; //valto stin lista ws exit
                }
            }
            poolExpert[poolExpert.Count - 1].SetActive(false);

        }
        //TRANSPARENCY MODULES
        foreach (GameObject go in resourcesListTransparency)
        {

            poolTransparencyModule.Add(Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            Tile[] list = poolTransparencyModule[poolTransparencyModule.Count - 1].GetComponentsInChildren<Tile>(); //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2) //an einai entrance
                        poolTransparencyModule[poolTransparencyModule.Count - 1].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                    if (t1.tileType == 3) //an einai exit
                        poolTransparencyModule[poolTransparencyModule.Count - 1].GetComponent<Module>().exit = t1; //valto stin lista ws exit
                }
            }
            poolTransparencyModule[poolTransparencyModule.Count - 1].SetActive(false);

        }
        //VISUAL BREAKING MODULES
        foreach (GameObject go in resourcesListVisualBreaking)
        {

            poolVisualBreaking.Add(Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            Tile[] list = poolVisualBreaking[poolVisualBreaking.Count - 1].GetComponentsInChildren<Tile>(); //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2) //an einai entrance
                        poolVisualBreaking[poolVisualBreaking.Count - 1].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                    if (t1.tileType == 3) //an einai exit
                        poolVisualBreaking[poolVisualBreaking.Count - 1].GetComponent<Module>().exit = t1; //valto stin lista ws exit
                }
            }
            poolVisualBreaking[poolVisualBreaking.Count - 1].SetActive(false);

        }

        //MAGNET MODULES
        foreach (GameObject go in resourcesListMagnet)
        {

            poolMagnetModule.Add(Instantiate(go, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            Tile[] list = poolMagnetModule[poolMagnetModule.Count - 1].GetComponentsInChildren<Tile>(); //add the tiles of the module in the list.
            if (list.Length > 0)
            {
                foreach (Tile t1 in list)
                {
                    if (t1.tileType == 2) //an einai entrance
                        poolMagnetModule[poolMagnetModule.Count - 1].GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
                    if (t1.tileType == 3) //an einai exit
                        poolMagnetModule[poolMagnetModule.Count - 1].GetComponent<Module>().exit = t1; //valto stin lista ws exit
                }
            }
            poolMagnetModule[poolMagnetModule.Count - 1].SetActive(false);

        }

        //Miscaleneous pooling
        for (int i = 0; i < howManySilver; i++)
        {
            poolSilverCoin.Add(Instantiate(silverCoinPrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            //poolSilverCoin[poolSilverCoin.Count-1].SetActive(false);
        }

        for (int i = 0; i < howManyGolds; i++)
        {
            //poolGoldCoin.Add( Instantiate( goldPrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity ) as GameObject );
            //poolGoldCoin[poolGoldCoin.Count-1].SetActive(false);
        }
        for (int i = 0; i < howManyTransparencyGates; i++)
        {
            poolTransparency.Add(Instantiate(transparencyGatePrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            //poolGoldCoin[poolGoldCoin.Count-1].SetActive(false);
        }

        for (int i = 0; i < howManyTransparencyGates; i++)
        {
            poolBouncy.Add(Instantiate(bouncyGatePrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            //poolGoldCoin[poolGoldCoin.Count-1].SetActive(false);
        }

        for (int i = 0; i < howManyMagnetGates; i++)
        {
            poolMagnet.Add(Instantiate(magnetGatePrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject);
            //poolGoldCoin[poolGoldCoin.Count-1].SetActive(false);
        }

        magnetGateParticlePool = Instantiate(magnetGateParticlePrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject;
        magnetGateParticlePool.SetActive(false);
        transparencyGateParticlePool = Instantiate(transparencyGateParticlePrefab, new Vector3(howFarBackX, 50, 0), Quaternion.identity) as GameObject;
        transparencyGateParticlePool.SetActive(false);


    }


    /* private void SortPickedObjects(List<Module> module){
		//sort pickedObjects (never spawn 2 same in a row)
		for (int i=0; i < module.Count - 2; i++){
			if (module[i].name == module[i+1].name){
				module.RemoveAt(i+1);
				i--;
			}
		}
	}
 */
    public void SpawnSingleModule(Module m)
    {
        //instantiate a single module at the end of the "row" and aligns them
        //also pops the the spawned module from the query list


        GameObject tmpGO = GetTypeBasedModule(playerGameObject.transform.position);
        tmpGO.SetActive(true);
        //Debug.Log(tmpGO.name);
		Debug.Log("Difference: "+Mathf.Abs(tmpGO.GetComponent<Module>().entrance.transform.position.y - tmpGO.transform.position.y));
        tmpGO.transform.position =
                new Vector3(1, 0, 0) * 1f + m.exit.transform.position +
                new Vector3(0,
                Mathf.Abs(tmpGO.GetComponent<Module>().entrance.transform.position.y - tmpGO.transform.position.y),
                0); //+
                //(tmpGO.GetComponent<Module>().exit.transform.position - tmpGO.gameObject.GetComponent<Module>().entrance.transform.position)

        //setup linked list	
        m.next = tmpGO.GetComponent<Module>();
        tmpGO.GetComponent<Module>().previous = m;
        tmpGO.GetComponent<Module>().exit.GetComponent<Tile>().enterOnce = true;

        SpawnMisc(new List<GameObject> { tmpGO });
    }

    private void ResetGoldProbability()
    {
        goldCoinProbability = goldCointBaseProbability;
    }


    private void SpawnMap()
    {
        //spawns map?!
        moduleGO.Add(startModulePool);
        for (int i = 0; i < defaultNumOfModules; i++)
        {
            moduleGO.Add(GetTypeBasedModule(Vector3.zero));
        }
        AlignModules();
    }

    private GameObject GetAndRemovefromList(List<GameObject> list, int i, out List<GameObject> outList)
    {
        //get an object from designated list and remove it at the same time
        //used for pooling
        //print(i);
        GameObject toReturn = list[i];
        list.RemoveAt(i);
        outList = list;
        return toReturn;
    }

    //int i=0;
    private GameObject GetTypeBasedModule(Vector3 playerPos)
    {
        //check if we need to spawn special, else spawn based on player position.
        if ((counterPerModuleSpawnGate == perModuleSpawnGate) || (counterPerModuleSpawnVisualBreaking == perModuleSpawnVisualBreaking) || (counterPerModuleSpawnBouncy == perModuleSpawnBouncy) || (counterPerModuleSpawnMagnet == perModuleSpawnMagnet))
        {
            if ((counterPerModuleSpawnGate == perModuleSpawnGate) && (counterPerModuleSpawnVisualBreaking == perModuleSpawnVisualBreaking) && (counterPerModuleSpawnBouncy == perModuleSpawnBouncy) && (counterPerModuleSpawnMagnet == perModuleSpawnMagnet))
            {
                typeY = specialPriority;
                switch (specialPriority)
                {
                    /* case 10:
                        counterPerModuleSpawnGate = 0;
                        break; */
                    case 11:
                        counterPerModuleSpawnVisualBreaking = 0;
                        break;
                   /*  case 12:
                        counterPerModuleSpawnBouncy = 0;
                        break; */
                    /* case 13:
                        counterPerModuleSpawnMagnet = 0;
                        break; */
                    /* default:
                        counterPerModuleSpawnGate = 0;
                        counterPerModuleSpawnVisualBreaking = 0;
                        counterPerModuleSpawnBouncy = 0;
                        counterPerModuleSpawnMagnet = 0;
                        break; */
                }
            }
            else if (counterPerModuleSpawnGate == perModuleSpawnGate)
            {
                typeY = typeY_Gate;
                counterPerModuleSpawnGate = 0;
            }
            else if (counterPerModuleSpawnVisualBreaking == perModuleSpawnVisualBreaking)
            {
                typeY = typeY_VisualBreaking;
                counterPerModuleSpawnVisualBreaking = 0;
            }
            else if (counterPerModuleSpawnBouncy == perModuleSpawnBouncy)
            {
                typeY = typeY_Bouncy;
                counterPerModuleSpawnBouncy = 0;
            }
            else if (counterPerModuleSpawnMagnet == perModuleSpawnMagnet)
            {
                typeY = typeY_Magnet;
                counterPerModuleSpawnMagnet = 0;
            }
        }
        else
        {
            typeY = GameManager.instance.GetLevelDataRow(playerPos);
        }

        //print("type Y: "+typeY);
        int i = 0;
        switch (typeY)
        {
            case 1:
                i = (int)System.Math.Floor((double)Random.Range(0, poolIntro.Count));
                return GetAndRemovefromList(poolIntro, i, out poolIntro);
            case 2:
                i = (int)System.Math.Floor((double)Random.Range(0, poolEasy.Count));
                return GetAndRemovefromList(poolEasy, i, out poolEasy);
            case 3:
                i = (int)System.Math.Floor((double)Random.Range(0, poolIntermediate.Count));
                return GetAndRemovefromList(poolIntermediate, i, out poolIntermediate);
            case 4:
                i = (int)System.Math.Floor((double)Random.Range(0, poolExpert.Count));
                return GetAndRemovefromList(poolExpert, i, out poolExpert);

            case 10:
                i = (int)System.Math.Floor((double)Random.Range(0, poolTransparencyModule.Count));
                return GetAndRemovefromList(poolTransparencyModule, i, out poolTransparencyModule);
            case 11:
                i = (int)System.Math.Floor((double)Random.Range(0, poolVisualBreaking.Count));
                return GetAndRemovefromList(poolVisualBreaking, i, out poolVisualBreaking);
            case 12:
                i = (int)System.Math.Floor((double)Random.Range(0, poolBouncyModule.Count));
                return GetAndRemovefromList(poolBouncyModule, i, out poolBouncyModule);
            case 13:
                i = (int)System.Math.Floor((double)Random.Range(0, poolMagnetModule.Count));
                return GetAndRemovefromList(poolMagnetModule, i, out poolMagnetModule);
            default:
                return GetAndRemovefromList(poolIntro, i, out poolIntro);
        }
    }

    private void AlignModules()
    {
        //Align modules between entrance and exit (horizontal (local) connection only)

        List<Module> tmpModule = new List<Module>(); //temporary list of modules
        foreach (GameObject m in moduleGO)
        {
            tmpModule.Add(m.GetComponent<Module>()); //add in the temporary list. The module from the in-game list
            m.gameObject.SetActive(true);
        }

        //mexri na ftash sto proteleuteo module
        for (int i = 0; i < tmpModule.Count - 1; i++)
        {
            //vazei to position tou epomenou module. Vriskei tin thesi aferodas tin thesi tou entrance tou epomenou me tin thesi tou exit tou torinou kai meta to anevazei ena epano.
            tmpModule[i + 1].transform.position = new Vector3(1, 0, 0) * 1f + tmpModule[i].exit.transform.position;
            tmpModule[i].next = tmpModule[i + 1];
            tmpModule[i + 1].previous = tmpModule[i];
            //tmpModule[i].gameObject.SetActive(true);
        }

        SpawnMisc(moduleGO);
        moduleGO.Clear();
    }

    private void SpawnMisc(List<GameObject> GoList)
    {
        foreach (GameObject m in GoList)
        {

            if (GameManager.instance.GetIsTutorialEnabled())
            {
                if (m.GetComponent<Module>().tutorialSpawnLocation.Count > 0)
                {
                    TutorialTrigger[] tt = m.GetComponentsInChildren<TutorialTrigger>();
                    for (int i = 0; i < m.GetComponent<Module>().tutorialSpawnLocation.Count; i++)
                    {
                        //spawn tutorial text at location
                        GameObject tempGO = Instantiate(m.GetComponent<Module>().tutorialSpawnType[i],
                                    m.GetComponent<Module>().tutorialSpawnLocation[i].transform.position,
                                    Quaternion.identity) as GameObject;
                        foreach (TutorialTrigger t in tt)
                        {
                            //find all tutorial triggers and if the ID's match, then upon entry to the trigger the above instantiated object will be revealed
                            if (m.GetComponent<Module>().tutorialSpawnLocationID[i] == t.tutorialID)
                            {
                                t.toReveal = tempGO;
                            }
                        }
                        tempGO.SetActive(false);
                    }
                }
            }

            //spawn player
            if (m.GetComponent<Module>().playerSpawnLocation != null)
            {
                playerGameObject = Instantiate(
                    playerPrefab,
                    m.GetComponent<Module>().playerSpawnLocation.transform.position
                        +
                    Vector3.zero,
                    Quaternion.identity
                    ) as GameObject;

                //spawn camera
                Instantiate(
                    mainCameraPrefab,
                    m.GetComponent<Module>().playerSpawnLocation.transform.position
                        +
                    new Vector3(0, 0, -50),
                    Quaternion.identity
                    );
            }

            //spawn coins
            if (m.GetComponent<Module>().coinSpawnLocations != null)
            {
                foreach (GameObject go in m.GetComponent<Module>().coinSpawnLocations)
                {
                    GameObject o = GetAndRemovefromList(poolSilverCoin, 0, out poolSilverCoin);

                    o.GetComponent<Gold>().failSafeSpawning = true;
                    o.SetActive(true);
                    o.transform.position = go.transform.position;
                    o.transform.parent = m.transform;
                    //GameObject o = Instantiate(silverCoinPrefab, go.transform.position, Quaternion.identity) as GameObject;
                }
            }

            /* 
			//spawn gold
			if(m.GetComponent<Module>().goldSpawnLocations !=null) {
				//roll dice.
				//if success spawn and reset probability base.
				//if fail increment probability
				foreach (GameObject go in m.GetComponent<Module>().goldSpawnLocations){

					if ( RollDice( 0, 100, goldCoinProbability ) ){
						//GameObject o = Instantiate(goldPrefab, go.transform.position, Quaternion.identity);
						GameObject o = GetAndRemovefromList(poolGoldCoin, 0, out poolGoldCoin);
						
						o.GetComponent<Gold>().failSafeSpawning = true;
						o.SetActive(true);
						o.transform.position = go.transform.position;
						o.transform.parent = m.transform;
						resetGoldCoinProbability= true;
						//ResetGoldProbability();
					}
				}

				if(resetGoldCoinProbability){
					ResetGoldProbability();
					resetGoldCoinProbability = false;
				}else {
					goldCoinProbability += probabilityIncrement;
				}
			}
			 */


            //spawn transparency gate
            if (m.GetComponent<Module>().TransparencyGate != null)
            {
                GameObject o = GetAndRemovefromList(poolTransparency, 0, out poolTransparency);

                o.GetComponent<Gates>().failSafeSpawning = true;
                o.SetActive(true);
                o.transform.position = m.GetComponent<Module>().TransparencyGate.transform.position;
                o.transform.parent = m.transform;
            }
            //spawn bouncy gate
            if (m.GetComponent<Module>().bouncyGate != null)
            {
                GameObject o = GetAndRemovefromList(poolBouncy, 0, out poolBouncy);

                o.GetComponent<Gates>().failSafeSpawning = true;
                o.SetActive(true);
                o.transform.position = m.GetComponent<Module>().bouncyGate.transform.position;
                o.transform.parent = m.transform;
            }
            //spawn magnet gate
            if (m.GetComponent<Module>().magnetGate != null)
            {
                GameObject o = GetAndRemovefromList(poolMagnet, 0, out poolMagnet);

                o.GetComponent<Gates>().failSafeSpawning = true;
                o.SetActive(true);
                o.transform.position = m.GetComponent<Module>().magnetGate.transform.position;
                o.transform.parent = m.transform;
            }

        }
    }


    /* 	private IEnumerator PlaySoundEffects(){
            //Play start SFX
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = startSFX;
            audioSource.Play();

            yield return new WaitForSecondsRealtime(startSFX.length + audioOffset );

            //if audio is still playing, stop it.
            if (audioSource.isPlaying){ audioSource.Stop(); }

            //Play 1 of three music clips
            int whatToPlay =  Random.Range(0, startMusicClips.Length);
            audioSource.clip = startMusicClips[whatToPlay];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(startMusicClips[whatToPlay].length);

            while (true){
                whatToPlay =  Random.Range(0, postMusicClips.Length);
                audioSource.clip = postMusicClips[whatToPlay];
                audioSource.Play();
                yield return new WaitForSecondsRealtime(postMusicClips[whatToPlay].length);
            }

        }
     */

    // Use this for initialization
    void Awake()
    {
        instance = this;
        ResetVariables();
        SpawnAllTypes();
        SpawnMap();
        //SetupModule (); //Setups the module
        //AlignModules (); //aligns this module with the previous one
        //KeepListUpdated();
        //StartCoroutine(PlaySoundEffects());
    }

    public void LateUpdate()
    {
    }
}
