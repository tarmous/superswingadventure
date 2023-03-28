using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject{
	
	public enum questType { instance, collective }; //instace: finish it in one go || collective: finish it through multiple playthroughs
	public questType qt; //variable so that the designer can define the type of quest or picked pseudorandomly	
	public int completionReward; //Reward the player gems for completing the quest
	/* new */ public List <QuestComponent> qc = new List<QuestComponent>(); //a list of quest components for the quest (example gather 10 coins and run 10k distance, each objective is a different component)

	private bool playOnce= false; //Used to play quest Completed audio clip

	public bool IsCompleted () {
		//Check if quest is completed (all components must be completed)
		foreach (QuestComponent qc in qc){
			if ( !qc.IsCompleted() ){ 
				Debug.Log("Quest Incomplete");
				return false;
				}
		}

		if (!playOnce) {
			playOnce = !playOnce;
			ApplicationStartup.instance.PlayCompletedQuest();
			Debug.Log("Quest Completed");
		}
		
		return true;
	}

	public Quest ( questType _qt, int _completionReward, List<QuestComponent> _qc ){
		qt = _qt;
		completionReward = _completionReward;
		qc = _qc;
		playOnce= false;	
	}
}

[System.Serializable]
	public class QuestComponent{

		public enum questComponentType { coin, distance }; //coin: (example) gather 23 coins || distance: (example) run a total of 5000 distance
		public questComponentType  qct; //public variable so that the designer can define the type of quest

		[SerializeField]
		private float value; //Number of coins to gather or distance to travel, based on qct;	


		[HideInInspector] // TO BE MADE PRIVATE IN FUTURE
		public float progress; //Variable used to keep track of quest component progress

		public bool IsCompleted(){
			//if progress equal or higher than target value the quest is completed
			if (progress >= value) {
				Debug.Log("Quest Component Completed");
				return true;
			}
			return false;
		}
		public float GetValue(){
			return this.value;
		}

		public void SetValue(float _value){
			this.value = _value;
		}

		public void SetProgress(float i){
			this.progress = i;
		}
		
		public void AddProgress(float i){
			this.progress += i;
		}
		
		public QuestComponent ( questComponentType  _qct, float _value, float _progress ){
			this.qct = _qct;
			this.value = _value;	
			this.progress = _progress;
		}

	}
