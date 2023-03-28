 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour {

    const float scoreDenominator = 30f;

    public static Score instance;
    private QuestTracker questTrackerRef; //used to track quests

    private GameObject player;
    #region points
        [SerializeField]
        private TextMeshProUGUI scoreGO;
         //text game object
        private TextMeshProUGUI scoreText; //actual text
        private float score, tempScore;// finalScore;
    #endregion
    
    #region coins
        [SerializeField]
        private TextMeshProUGUI coinsGO; //text game object
        private float coinsGathered;
        private TextMeshProUGUI coinsText; //actual text
    #endregion

    #region gems
        [SerializeField]
        private GameObject gemsGO;//text game object
        private float gemsGathered;
        private Text gemsText; //actual text
    #endregion

    private float goldGathered;
    private float multiplier_x=4f, multiplier_y=0f;
    private float timer;

    public void SetGemsGathered(float i){ this.gemsGathered = i; }
    public float GetGemsGathered(){ return this.gemsGathered; }

    public void SetCoinsGathered(float i){
        this.coinsGathered = i;
        if ( this.questTrackerRef ) this.questTrackerRef.SetCoinsGathered(i);
    }
    public float GetCoinsGathered(){ return this.coinsGathered; }

    public void SetMultiplier_x(float i){ this.multiplier_x = i; }

    public void SetMultiplier_y(float i){ this.multiplier_y = i; }

    public float GetScore(){
        return this.score;
    }

    public void SetScore(float i){
        this.score = i;
        if ( this.questTrackerRef ) this.questTrackerRef.SetDistanceTravelled(i);
    }
    public float GetTimer(){
        return this.timer;
    }

    void Awake(){
        instance = this;
        timer=0f;
        SetGemsGathered(0);
        SetCoinsGathered(0);

        scoreText = scoreGO.GetComponent<TextMeshProUGUI>();
        coinsText = coinsGO.GetComponent<TextMeshProUGUI>();
        gemsText = gemsGO.GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        questTrackerRef = QuestTracker.instance; // to track quests
        player = GameObject.FindGameObjectWithTag("Player");
        
        
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if(player==null){
            //Debug.Log("player not found in Score.cs");
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        timer+=Time.deltaTime;
        tempScore = (player.transform.position.x * multiplier_x + player.transform.position.y * multiplier_y) / scoreDenominator; // + GetGoldGathered();

        if (score < tempScore){
            SetScore( Mathf.Floor(tempScore));
        }

        scoreText.text = score.ToString();
        coinsText.text = GetCoinsGathered().ToString();
        gemsText.text = GetGemsGathered().ToString();
        //scoreText.text = "timer: " + timer;
        //finalScore = Mathf.Pow(score,2)/ timer;
    }
}
