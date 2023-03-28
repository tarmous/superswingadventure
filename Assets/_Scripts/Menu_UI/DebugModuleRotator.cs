using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModuleRotator : MonoBehaviour {


	void Start () {
        if (PlayerPrefs.HasKey("mapRotation"))
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, PlayerPrefs.GetFloat("mapRotation") ) );
    }
	
}
