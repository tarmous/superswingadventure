using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressScript : MonoBehaviour {

	public void PlayButtonPress(){
		ApplicationStartup.instance.PlayButtonPress();
	}

	public void ShowLeaderBoards(){
		GooglePlayServices.ShowLeaderboardsUI();
	}
}
