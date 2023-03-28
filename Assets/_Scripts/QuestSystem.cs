using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
//using System;

public class QuestSystem : MonoBehaviour {

	[SerializeField]
	private QuestTracker questTrackerRef;

	[SerializeField]
	private List <Quest> quests = new List <Quest> ();
	[SerializeField]
	private List <Quest> activeQuests = new List <Quest> (); //probably get from a server

	float hiddenLevelMultiplier; //default =1 maybe return a range of difficulties
	const int maxNumberOfQuestsPerDay = 3;

	private string[][] levelDataRow; //string[Row][Rowvar]

	private string path = @"Assets\_CSV\ActiveQuestsData.csv"; //data of active quests

	private float GetDataFromCSV(int index1, int index2){ //TO BE IMPLEMENTED
		//index:
			// 1: MIN Multiplier for the value
			// 2: MAX Multiplier for the value
			// 3: Reward Multiplier in gems

		/* for (int i = 0; i < levelDataRow.Length - 1; i++)
        {
            if (averageFinalScore <= int.Parse(levelDataRow[i][0]))
            {		
                return Random.Range( float.Parse(levelDataRow[i][index1]), float.Parse(levelDataRow[i][index2]) );
            }
        }
		return Random.Range( float.Parse(levelDataRow[levelDataRow.Length - 1][index1]), float.Parse(levelDataRow[levelDataRow.Length - 1][index2]) );
		*/
		return 1;
		//return Random.Range( levelDataRow[i][index1], levelDataRow[i][index2] );
	}

	private float GetDataFromCSV(int index1){ //TO BE IMPLEMENTED
		/* 
		for (int i = 0; i < levelDataRow.Length - 1; i++)
        {
            if (averageFinalScore <= int.Parse(levelDataRow[i][0]))
            {		
                return float.Parse(levelDataRow[i][index1]);
            }
        }
		return float.Parse(levelDataRow[levelDataRow.Length - 1][index1]);
		 */
		return 10;
		//return levelDataRow[i][index1];

	}
	
	private void LoadCSV(){
		//load csv
		TextAsset levelData = Resources.Load<TextAsset>("QuestSystemAttributes"); //load CSV
        string[] levelDataString = levelData.text.Split(new char[] { '\n' }); //split CSV into rows
        levelDataRow = new string[levelDataString.Length - 1][];

        for (int i = 1; i < levelDataString.Length; i++)
        {
            string[] row = levelDataString[i].Split(new char[] { ',' }); //get the row from excel
            levelDataRow[i - 1] = new string[row.Length];
            for (int j = 0; j < row.Length; j++)
            {
                levelDataRow[i - 1][j] = row[j]; // store row variables in a huge public array
            }
        }
	}
	
	
	//private List<Quest> GetActiveQuests(){
	private List<Quest> GetActiveQuests(){	
		//return (A LIST FROM THE SERVER)
		string[] s = File.ReadAllLines(path); // read active quests csv
		/*
		data per csv collumn;
			0: Quest Type
			1: Quest Reward
			2: Number Of Components
			3: Quest Component Type 1
			4: Quest Component Value 1
			5: Quest Component Progress 1
			6: Quest Component Type 2
			7: Quest Component Value 2
			8: Quest Component Progress 2
			....
			N: 
			N+1: 
			N+2: 
		*/

		if (s.Length < 2) return null;
		string[][] sData = new string[s.Length][];

		for (int i = 1; i < s.Length; i++){
			string[] row = s[i].Split(new char[]{ ',' });
			sData[i] = new string[row.Length];
			sData[i] = row;
		}//end for

		List<Quest> q = new List<Quest>();
		for (int i = 0; i<sData.GetLength(0); i++){

			//create quest components from active quests CSV
			List <QuestComponent> qc = new List<QuestComponent>();
			for (int j = 0; j < int.Parse( sData[i][2] ); j++){
				qc.Add(
					new QuestComponent(
						(QuestComponent.questComponentType)System.Enum.Parse( typeof(QuestComponent.questComponentType), sData[i][3 + j*3] ),
						float.Parse( sData[i][4 + j*3] ),
						float.Parse( sData[i][5 + j*3] )
						)
				);//end qc.Add()

			}//end for

			//create quests from active quests CSV
			q.Add (
				new Quest( 
					(Quest.questType)System.Enum.Parse( typeof(Quest.questType), sData[i][0] ),
					int.Parse( sData[i][1] ),
					qc
					)
				);//end q.Add()

		}//end for	

		//PUSH q
		return q;
	}

	private void PushUpdatedQuestsToServer(){

		string [] activeQuestData = File.ReadAllLines(path); //activeQuestData[1] == title of columns
		string [] shitToWriteIn = new string[4]; //MAX 3 QUESTS ANYWAY
		shitToWriteIn[0] = activeQuestData[0];
		int i = 1;

		foreach(Quest q in activeQuests){
			string qcString = "";
			for (int j = 0; j< q.qc.Count; j++){
				qcString = qcString + ", " +  q.qc[j].qct.ToString() + ", " + q.qc[j].GetValue().ToString() + ", " + q.qc[j].progress.ToString();
			}
			shitToWriteIn[i++] = q.qt.ToString() + ", " + q.completionReward.ToString() + ", " + q.qc.Count.ToString() + qcString;
		}
		
		//File.ReadAllLines(path);
		File.WriteAllLines(path, shitToWriteIn );
		//TO BE IMPLEMENTED
	}

	private void GenerateQuests(){
		quests.Clear(); //fresh pool of active quests
		List<Object> questPool = Resources.LoadAll("Quests", typeof(Quest) ).ToList(); // load pool to draw from resources

		while ( quests.Count < maxNumberOfQuestsPerDay ){

			int questPickedIndex = Random.Range( 0, questPool.Count );	
			Quest tmpQuest = questPool[questPickedIndex] as Quest;			//pick a random quest from pool

			Quest.questType tmpQT = tmpQuest.qt;							//Quest type
			int tmpCompletionReward = tmpQuest.completionReward;			//Completion Reward
			List<QuestComponent> tmpQCToParse = new List<QuestComponent>();	//New list of Quest Components
			foreach(QuestComponent q in tmpQuest.qc) {
				tmpQCToParse.Add( new QuestComponent( q.qct, q.GetValue(), 0 ) ); //Create a new quest component Based on above values
			}
			
			Quest newQuest = new Quest(tmpQT, tmpCompletionReward, tmpQCToParse); // Create new quest and leave quest pool intact

			newQuest.completionReward =  (int)GetDataFromCSV(3); // passing it as an int will truncate the decimal places.

			foreach (QuestComponent tmpQC in newQuest.qc){
				tmpQC.SetValue( tmpQC.GetValue() * GetDataFromCSV(1,2) );
			}

			quests.Add(newQuest); // then push this to active ones

		}
		activeQuests.AddRange(quests);
		PushUpdatedQuestsToServer();

	}//end generate quests

	private void UpdateProgress(){
		foreach (Quest q in activeQuests){
			if (q.IsCompleted()) continue;		
			foreach (QuestComponent qc in q.qc){
				//Debug.Log( qc.progress + " || " + qc.GetValue() ); 

				if ( qc.IsCompleted() ) continue;
				
				switch (qc.qct){
					case QuestComponent.questComponentType.coin:
						qc.AddProgress( 
							questTrackerRef.GetDelta( questTrackerRef.GetCoinsGathered(), questTrackerRef.GetCoinsGatheredLU() )
						);
						break;
					case QuestComponent.questComponentType.distance:
					qc.AddProgress( 
							questTrackerRef.GetDelta( questTrackerRef.GetDistanceTravelled(), questTrackerRef.GetDinstanceTravelledLU() )
						);
						break;
					default:
						//I AM ERROR
						break;
				}

			}
		}
		questTrackerRef.SetDinstanceTravelledLU( questTrackerRef.GetDistanceTravelled() );
		questTrackerRef.SetCoinsGatheredLU( questTrackerRef.GetCoinsGathered() );
	}

	void Start(){
		questTrackerRef = QuestTracker.instance;
		
		//GenerateQuests(); //THEN CREATE NEW QUEST IF SLOTS EMPTY (NORMALY THIS SHOULD BE DONE ONCE PER DAY)
		//activeQuests = GetActiveQuests(); //TO BE IMPLEMENTED
		//THIS IS USED TO RESET PROGRESS AND/OR OTHER ATTRIBUTES
		if (activeQuests.Count > 0){
		foreach (Quest aq in activeQuests){
			switch (aq.qt){
				case Quest.questType.collective:
					//DO NOTHING (for now)
					break;
				case Quest.questType.instance:
					//RESET PROGRESS
					foreach(QuestComponent qc in aq.qc){
						qc.SetProgress(0);
					}
					break;
				default:
					//I AM ERROR
					break;
			}
		}}
		//GenerateQuests(); //THEN CREATE NEW QUEST IF SLOTS EMPTY (NORMALY THIS SHOULD BE DONE ONCE PER DAY)

		

	}

	void LateUpdate(){
		UpdateProgress();
	}


}
