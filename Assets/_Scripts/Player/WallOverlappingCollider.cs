using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOverlappingCollider : MonoBehaviour {

	const int wallLayer = 11;
	private PlayerAttributes pa;
	void Start(){
		pa = GetComponentInParent<PlayerAttributes>();
	}

	void Update(){
		if (pa == null){
			pa = GetComponentInParent<PlayerAttributes>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.layer == wallLayer){
			pa.setIsOverlappingWall(true);
		}
	}

	private void OnTriggerStay2D(Collider2D collider){
		if (collider.gameObject.layer == wallLayer){
			pa.setIsOverlappingWall(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collider){
		if (collider.gameObject.layer == wallLayer){
			pa.setIsOverlappingWall(false);
		}
	}
}
