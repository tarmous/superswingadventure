﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back_hills : MonoBehaviour {
	private float colorDenominator = 255f;
	
	public Color[] color_active;  	//Array for the skin being used.
	public Color[] color_base;		//Array for base skin


	void Start(){
		//Base Skin Array
		color_base = new Color[4];
			color_base[0]= new Color(232, 173, 131, 255)/colorDenominator;
			color_base[1]= new Color(168, 129, 125, 255)/colorDenominator;
			color_base[2]= new Color(117, 143, 201, 255)/colorDenominator;
			color_base[3]= new Color(62, 103, 255, 255)/colorDenominator;
			color_active=color_base;
		// FUTURE: Space Skin Array
	}
	
	public void setActiveSkin(int skinID){ // Function to select the active color array depending on the skin.
		switch(skinID){         //switch based on skinID provided, base skin is 0, future skins are 1,2,3.. etc
			case 0: //Base Skin
				color_active=color_base;
				break;
		/*   case 1: //Space Skin
				color_active=color_space;
				return true;
			*/
		}
	}
}
