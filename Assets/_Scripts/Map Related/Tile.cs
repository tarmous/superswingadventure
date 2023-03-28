using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour {
	public int tileType;
	private int counter;
	public bool enterOnce=true;

	private Module GetLastModule(Module m){
		//get the last module in the "row"
		if (m.next == null ){
			return m;
		}else {
			return GetLastModule(m.next);
		}
	}

	private Module GetFirstModule(Module m){
		//get the first module in the "row"
		if (m.previous == null ){
			return m;
		}else {
			counter++;
			return GetFirstModule(m.previous);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision){
		if ( collision.gameObject.tag != "Player" ){
            return;      
        }	

		if ( tileType == 3  && enterOnce){
			enterOnce=false;
		//destroy 1 modules back
		//spawn 1 module at end of row
		//destroy this
		//increment special module counters by 1

		

		counter=0;
		Module first = GetFirstModule(GetComponentInParent<Module>());
		if ( counter >= 1) {
			if (Map.instance){
				if (first.name == Map.instance.GetStartModule().name){
					//print("destroyed");
					first.next.previous = null;
					first.next = null;
					first.gameObject.SetActive(false);
				}else {
					//figure out which TypeY of module it is.
					//Then add it to its aproppriate pool.
					char[] charArray = first.gameObject.name.ToCharArray();
					int nameLength = charArray.Length;
					int i = 0, j = 0;
					bool foundTypeY = false;

					foreach (char c in charArray){
						if (c == '_'){
							for (j = i + 1; j < nameLength; j++){
								if (charArray[j] == '_'){
									foundTypeY = true;
									break;
								}
							}
							if (foundTypeY) break;
						}
						i++;
					}

					string modulesTypeY = null;
					for (int k = i + 1; k < j; k++){
						modulesTypeY += charArray[k].ToString();
					}
					//print (modulesTypeY);
					switch ( int.Parse(modulesTypeY) ){
						case 1:
							Map.instance.AddToPoolIntro(first.gameObject);
							break;
						case 2:
							Map.instance.AddToPoolEasy(first.gameObject);
							break;	
						case 3:
							Map.instance.AddToPoolIntermediate(first.gameObject);
							break;	
						case 4:
							Map.instance.AddToPoolExpert(first.gameObject);
							break;
						case 10:
							Map.instance.AddToPoolTransparencyModule(first.gameObject);
							break;
						case 11:
							Map.instance.AddToPoolVisualBreaking(first.gameObject);
							break;
						case 12:
							Map.instance.AddToPoolBouncyModule(first.gameObject);
							break;
						case 13:
							Map.instance.AddToPoolMagnetModule(first.gameObject);
							break;	
						default:
							break;
						}
					
					first.next.previous = null;
					first.next = null;
					first.gameObject.SetActive(false);
					//Destroy(first.gameObject);
				}
			}

			GameManager.instance.SetTutorialCounter( GameManager.instance.GetTutorialCounter() + 1 );
			if (Map.instance) Map.instance.SetCounterPerModuleSpawnGate( Map.instance.GetCounterPerModuleSpawnGate() + 1 );
			//if (Map.instance) Map.instance.SetCounterPerModuleVisualBreaking( Map.instance.GetCounterPerModuleVisualBreaking() + 1 );
			//if (Map.instance) Map.instance.SetCounterPerModuleSpawnBouncy( Map.instance.GetCounterPerModuleSpawnBouncy() + 1 );
			if (Map.instance) Map.instance.SetCounterPerModuleSpawnMagnet( Map.instance.GetCounterPerModuleSpawnMagnet() + 1 );
			
			//(this); //we dont need this trigger anymore
			}

			Module last = GetLastModule(GetComponentInParent<Module>());
			//spawn a single module
			if (Map.instance){
				Map.instance.SpawnSingleModule(last);
			}else if (TestingGrounds.instance){
				TestingGrounds.instance.SpawnSingleModule(last);
			}


		}//end if tile type == 3
	}
}
