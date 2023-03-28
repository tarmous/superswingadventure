using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GooglePlayServices : MonoBehaviour {

    public static GooglePlayServices instance;

    private string debugText;

    void Awake(){
        instance=this;
    }

    void OnGUI(){
		GUI.Box( new Rect(Screen.width/4, Screen.width/4, Screen.width/2, Screen.height/2), debugText );
	}

	 // Use this for initialization
	void Start () {

        debugText = "Building Configuration";
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames() // Enables saving game progress
            .Build();

        debugText = "Initializing";
        PlayGamesPlatform.InitializeInstance(config);
        debugText = "Activating";
        PlayGamesPlatform.Activate();
        
        if ( !Social.localUser.authenticated ){
            debugText = "Authenticating";
            Social.localUser.Authenticate( (bool success, string s) => {
                                                        if (success){
                                                            debugText = "Authentication success: " + s;
                                                        }else{
                                                            debugText = "Authentication failed: " + s;
                                                        }
                                                        ApplicationStartup.instance.LoadStartMenu();  
                                                    // loginTextGo.GetComponent<Text>().text = ""+s;
                                                    });
        }else{
            ApplicationStartup.instance.LoadStartMenu();
        }
        Debug.Log("Finished?!");
                                                      
	}

	public void SignIn(){
        if ( !Social.localUser.authenticated ){
            Social.localUser.Authenticate( (bool success, string s) => {
                                                    Debug.Log("sign in function: "+ success +":"+s);
                                                        //loginTextGo.GetComponent<Text>().text = "Signed in: "+success;
                                                    });
        }
    }
 
    #region Achievements
    public static void UnlockAchievement(string id){
        Social.ReportProgress(id, 100, success => { });
    }
 
    public static void IncrementAchievement(string id, int stepsToIncrement){
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
    }
 
    public static void ShowAchievementsUI(){
        Social.ShowAchievementsUI();
    }
    #endregion /Achievements
 
    #region Leaderboards
    public static void AddScoreToLeaderboard(string leaderboardId, long score){
        
        if (!Social.localUser.authenticated) return;

        Social.ReportScore(score, leaderboardId, success => {
            if (success){
                Debug.Log("sucessfully reported score");
            }else{
                Debug.Log("failed to report score");
            }
         });
        //Social.ReportScore(score, googleplaygames.leaderboard_all_time, success => { });
    }
 
    public static void ShowLeaderboardsUI(){
        Social.ShowLeaderboardUI();
    }
    #endregion /Leaderboards



    #region Saving/Loading Cloud
    const string gemsFilename = "gems";

    public void OpenSavedGame(string filename){
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void SaveGame (ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);
        
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void LoadGameData (ISavedGameMetadata game) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }



    ////////////////////////////////////////////////////////////////////////////////////
    // Callbacks
    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game){
        if (status == SavedGameRequestStatus.Success) {
            // handle reading or writing of saved game.
        } else {
            // handle error
        }
    }

    private void OnSavedGameWritten (SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            // handle reading or writing of saved game.
        } else {
            // handle error
        }
    }

    private void OnSavedGameDataRead (SavedGameRequestStatus status, byte[] data) {
        if (status == SavedGameRequestStatus.Success) {
            // handle processing the byte array data
        } else {
            // handle error
        }
    }

    public byte[] SplitGemsIntoBytes(int gems){
        int g = gems;
        byte[] byteArray = new byte[1 + g/byte.MaxValue];

        for (int i = 0; i < byteArray.Length; i++){
            if (g > byte.MaxValue){
                byteArray[i] = 255;
                g -= 255;
            }else{
                byteArray[i] = (byte)g;
            }
        }

        return byteArray;
    }

    public int TurnBytesIntoGems(byte[] byteArray){
        int gems = 0;
        
        for (int i = 0; i < byteArray.Length; i++){
            gems += byteArray[i];
        }

        return gems;
    }

    #endregion Saving/Loading Cloud
	
}
