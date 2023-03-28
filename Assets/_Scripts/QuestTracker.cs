using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuestTracker : MonoBehaviour {
	public static QuestTracker instance;
	private int coinsGathered;
	

	private float distanceTravelled;

	//The LU variables are used so that extra progress doesnt accidentaly gets pushed
	private float distanceTravelledLU; //value of coins pushed to quest component progressions. LU:Last Update
	private int coinsGatheredLU; //value of coins pushed to quest component progressions. LU:Last Update

	public float GetDelta(float a, float b){
		//Debug.Log(a - b);
		return a-b;
	}

	public float GetDinstanceTravelledLU(){
		return this.distanceTravelledLU;
	}
	
	public float GetCoinsGatheredLU(){
		return this.coinsGatheredLU;
	}

	public void SetDinstanceTravelledLU(float i){
		this.distanceTravelledLU = i;
	}
	
	public void SetCoinsGatheredLU(float i){
		this.coinsGatheredLU = (int)i;
	}

	public void SetCoinsGathered(float i){
		this.coinsGathered = (int)i;
	}

	public void SetDistanceTravelled(float i){
		this.distanceTravelled = i;
	}

	public int GetCoinsGathered(){
		return this.coinsGathered;
	}

	public float GetDistanceTravelled(){
		return this.distanceTravelled;
	}

	void Awake() {
		instance = this;
		coinsGathered = 0;
		distanceTravelled = 0;
		coinsGatheredLU = 0;
		distanceTravelledLU = 0;
	}

	void Start () {
	}

	void Update () {
	//	print ("Coins: "+coinsGathered); print ("Coins LU: "+coinsGatheredLU);
		//print ("distance: "+distanceTravelled); print ("distance LU: "+distanceTravelledLU);
		
	}
	
	void LateUpdate(){
	}
}
