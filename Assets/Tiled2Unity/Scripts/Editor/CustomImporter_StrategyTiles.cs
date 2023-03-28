using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_StrategyTiles : Tiled2Unity.ICustomTiledImporter {

	private GameObject playerSpawnLocation;
	private GameObject TransparencyGate, bouncyGate, magnetGate;
	private List<GameObject> tutorialSpawnLocation = new List<GameObject>();
	private List<GameObject> tutorialSpawnType = new List<GameObject>();
	private List<int> tutorialSpawnLocationID = new List<int>();
	private List<GameObject> coinSpawnLocations = new List<GameObject>();
	private List<GameObject> goldSpawnLocations = new List<GameObject>();


	public void HandleCustomProperties(GameObject go, IDictionary<string, string> customProperties){

		if(customProperties.ContainsKey("transparent")){
			go.layer=11;
		}

		if(customProperties.ContainsKey("coin")){
			coinSpawnLocations.Add(go);
			go.GetComponent<Collider2D>().enabled=false;
		}

		if(customProperties.ContainsKey("gold")){
			goldSpawnLocations.Add(go);
			go.GetComponent<Collider2D>().enabled=false;
		}

		if(customProperties.ContainsKey("tileType")){
			Tile tile = go.AddComponent<Tile>();
			tile.tileType = int.Parse(customProperties["tileType"]);
			go.GetComponent<BoxCollider2D>().isTrigger=true;

			if ( int.Parse(customProperties["tileType"]) == 3 ){
				go.GetComponent<BoxCollider2D>().size=new Vector2(go.GetComponent<BoxCollider2D>().size.x,99999);
			}
		}

		if (customProperties.ContainsKey ("isHazard")) {
				go.GetComponent<Collider2D> ().isTrigger = true;
				go.AddComponent<Spike> ();
		}

		if (customProperties.ContainsKey ("playerStart")) {
			playerSpawnLocation = go;
			go.GetComponent<Collider2D>().enabled=false;	
		}

		if (customProperties.ContainsKey ("transparencyGate")) {
			TransparencyGate = go;
			go.GetComponent<Collider2D>().enabled=false;
		}

		if (customProperties.ContainsKey ("bouncyGate")) {
			bouncyGate = go;
			go.GetComponent<Collider2D>().enabled=false;
		}

		if (customProperties.ContainsKey ("magnetGate")) {
			magnetGate = go;
			go.GetComponent<Collider2D>().enabled=false;
		}

		if (customProperties.ContainsKey ("tutorialTrigger")) {
				go.GetComponent<Collider2D> ().isTrigger = true;
				go.AddComponent<TutorialTrigger> ();
				go.GetComponent<TutorialTrigger> ().tutorialID = int.Parse( customProperties["tutorialTrigger"] );
		}

		if(customProperties.ContainsKey("tutorialTextSpawnLocation")){
			Object[] tempGO= Resources.LoadAll("Tutorial", typeof(GameObject));
			foreach (Object o in tempGO){
				if (o.ToString().StartsWith(customProperties["tutorialTextSpawnType"]) ){
					tutorialSpawnType.Add ( (GameObject)o );
					break;
				}
			}
			tutorialSpawnLocation.Add(go);
			tutorialSpawnLocationID.Add( int.Parse(customProperties["tutorialTextSpawnLocation"]) );
		}

	}

	public void CustomizePrefab(GameObject go){
		
		Module mo = go.AddComponent<Module> ();

		if ( playerSpawnLocation != null ) mo.playerSpawnLocation = playerSpawnLocation;
		if ( coinSpawnLocations != null ) mo.coinSpawnLocations = coinSpawnLocations;
		if ( goldSpawnLocations != null) mo.goldSpawnLocations = goldSpawnLocations;
		if ( TransparencyGate != null) mo.TransparencyGate = TransparencyGate;
		if ( bouncyGate != null) mo.bouncyGate = bouncyGate;
		if ( magnetGate != null) mo.magnetGate = magnetGate;
		if ( tutorialSpawnLocation != null ) mo.tutorialSpawnLocation = tutorialSpawnLocation;
		if ( tutorialSpawnLocationID != null ) mo.tutorialSpawnLocationID = tutorialSpawnLocationID;
		if ( tutorialSpawnType != null ) mo.tutorialSpawnType = tutorialSpawnType;


	}

}


