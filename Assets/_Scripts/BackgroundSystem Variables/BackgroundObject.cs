using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Background Object", menuName = "Background Object")]
public class BackgroundObject : ScriptableObject {

	[SerializeField]
	public Texture2D tileset;

	[SerializeField]
	public List <backgroundComponent> bc;

	[System.Serializable]
	public struct backgroundComponent{

		/* public backgroundComponent(GameObject go, float vel){
			this.backgroundAssetPrefab = go;
			this.backroundAssetVelocity = vel;
		} */

		public GameObject backgroundAssetPrefab;
			
		public float backroundAssetVelocity;


	} 
}
