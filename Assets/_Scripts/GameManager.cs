using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; //only one instance of GameManager in a scene should exist.
    public enum timeMode { slow, real, paused };
    private timeMode runningType;
    private static bool isTutorialEnabled = true;
    private static int tutorialCounter;
    public AudioSource tutorialStart, tutorialStop;

    private bool isGoSlowMotionRunning, isGoRealTimeRunning;
    private GameObject mainCamera;
    private GameObject tutorialReleaseHookText, tutorialThrowHookText;

    private string[][] levelDataRow; //string[Row][Rowvar]

    //[Range(0, 1)]
    //public float generalVolume;
    public AudioMixer am;
    //public AudioClip transparencyEffectEnding;
    //[Range(0,2)]
    //public float transparencyEffectPitch = 1;
    //public const float defaultTransparencyEffectPitch = 1f;


    public AudioClip startSFX;
    public float audioOffset;
    [SerializeField]
    private AudioClip[] startMusicClips, postMusicClips, magnetSFXClips, spectreSFXClips;// musicClip2, musicClip3;

    [SerializeField]
    private GameObject magnetSFX_GO, spectreSFX_GO;

    private AudioSource audioSource;
    private GameObject tutorialObjectToHandle;


    #region Getters And Setter
    public int GetLevelDataRow(Vector3 playerPos)
    {
        int i = GetPlayerProgression(playerPos);
        //levelDataRow 
        float rand = Random.Range(0, 100);


        if (rand <= int.Parse(levelDataRow[i][1]))
        {
            // Debug.Log("Get Level Data Row:: Rand: "+rand+", Returned:" + 1);
            return 1;
        }
        else if (rand <= int.Parse(levelDataRow[i][1]) + int.Parse(levelDataRow[i][2]))
        {
            //Debug.Log("Get Level Data Row:: Rand: "+rand+", Returned:" + 2);
            return 2;
        }
        else if (rand <= int.Parse(levelDataRow[i][1]) + int.Parse(levelDataRow[i][2]) + int.Parse(levelDataRow[i][3]))
        {
            //Debug.Log("Get Level Data Row:: Rand: "+rand+", Returned:" + 3);
            return 3;
        }
        else if (rand <= int.Parse(levelDataRow[i][1]) + int.Parse(levelDataRow[i][2]) + int.Parse(levelDataRow[i][3]) + int.Parse(levelDataRow[i][4]))
        {
            //Debug.Log("Get Level Data Row:: Rand: "+rand+", Returned:" + 4);
            return 4;
        }

        Debug.Log("Get Level Data Row:: Rand: " + rand + ", Returned:" + 0);
        return 1;

    }
    public void SetTutorialReleaseHookText(GameObject i) { this.tutorialReleaseHookText = i; }
    //private GameObject GetTutorialReleaseHookText(){return this.tutorialReleaseHookText;}
    //public void SetTutorialThrowHookText(GameObject i){this.tutorialThrowHookText = i;}
    //private GameObject GetTutorialThrowHookText(){return this.tutorialThrowHookText;}
    public int GetTutorialCounter() { return tutorialCounter; }
    public void SetTutorialCounter(int i) { tutorialCounter = i; }

    public timeMode GetRunningType() { return this.runningType; }
    public void SetRunningType(timeMode type) { this.runningType = type; }

    public bool GetIsTutorialEnabled()
    {
        isTutorialEnabled = PlayerPrefs.GetInt("tutorialEnabled", 1) == 0 ? false : true;
        return isTutorialEnabled;
    }
    public void SetIsTutorialEnabled(bool i)
    {
        isTutorialEnabled = i;
        PlayerPrefs.SetInt("tutorialEnabled", isTutorialEnabled == false ? 0 : 1);
    }

    public bool GetIsGoSlowMotionRunning() { return this.isGoSlowMotionRunning; }
    public void SetIsGoSlowMotionRunning(bool i) { this.isGoSlowMotionRunning = i; }

    public bool GetIsGorRealTimeRunning() { return this.isGoRealTimeRunning; }
    public void SetIsGoRealTimeRunning(bool i) { this.isGoRealTimeRunning = i; }
    private GameObject GetTutorialObjectToHandle() { return this.tutorialObjectToHandle; }
    private void SetTutorialObjectToHandle(GameObject i) { this.tutorialObjectToHandle = i; }

    #endregion

    private void ResetAudioVariables()
    {

        float defaultPitch = 1, defaultVolume = 1;

        if (!PlayerPrefs.HasKey("Music/Pitch")) { PlayerPrefs.SetFloat("Music/Pitch", defaultPitch); }
        if (!PlayerPrefs.HasKey("Music/Volume")) { PlayerPrefs.SetFloat("Music/Volume", defaultVolume); }
        if (!PlayerPrefs.HasKey("SFX/Volume")) { PlayerPrefs.SetFloat("SFX/Volume", defaultVolume); }

        //am.SetFloat("Music/Pitch", PlayerPrefs.GetFloat("Music/Pitch"));
        am.SetFloat("Music/Volume", PlayerPrefs.GetFloat("Music/Volume"));
        am.SetFloat("SFX/Volume", PlayerPrefs.GetFloat("SFX/Volume"));

    }

    private void LoadCSV()
    {
        TextAsset levelData = Resources.Load<TextAsset>("MapProgressionTable"); //load CSV
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

    public int GetPlayerProgression(Vector3 playerPos)
    {
        //find how far the player has gone in the game
        //and get the correct Row of progression from CSV
        for (int i = 0; i < levelDataRow.Length - 1; i++)
        {
            if (playerPos.x <= int.Parse(levelDataRow[i][0]))
            {
                return i;
            }
        }
        return levelDataRow.Length - 1;
    }



    #region Real Time / Slow Motion
    public IEnumerator GoSlowMotion(GameObject go)
    {

        //OLD GO SLOW MOTION
        /* go.SetActive(true);
        SetTutorialObjectToHandle(go);
        //GetTutorialThrowHookText().SetActive(true);
        if (!isTutorialEnabled) yield break;
        SetIsGoSlowMotionRunning(true);
        float decreaseRate = 0.02f;//0.0095f;
        float perTime = 0.0001f; //0.01f

        do
        {
            Time.timeScale = Time.timeScale * (1 - decreaseRate);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return new WaitForSecondsRealtime(perTime);
        } while (Time.timeScale > 0.2f && runningType == timeMode.slow);

        if (runningType == timeMode.slow) Time.timeScale = 0f;
        SetIsGoSlowMotionRunning(false);
        yield break; */

        //NEW GO SLOW MOTION
        if (!isTutorialEnabled) yield break; // if tutorial isnt enabled fuck off
        tutorialStart.Play();
        audioSource.pitch = 0.7f;
        StopCoroutine("GoRealTime"); // stops real time coroutine
        go.SetActive(true); //sets tutorial text to active
        SetTutorialObjectToHandle(go);


        SetIsGoSlowMotionRunning(true);
        float decreaseRate = 0.02f;//0.0095f;
        float perTime = 0.0001f; //0.01f
        do
        {
            do
            {
                Time.timeScale = Time.timeScale * (1 - decreaseRate);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                yield return new WaitForSecondsRealtime(perTime);
            } while (Time.timeScale > 0.2f && runningType == timeMode.slow);
        } while (runningType == timeMode.paused);

        if (runningType == timeMode.slow) Time.timeScale = 0f;
        SetIsGoSlowMotionRunning(false);

        yield break;
        ;

    }


    public IEnumerator GoRealTime()
    {

        if (!isTutorialEnabled) yield break; // if tutorial isnt enabled fuck off

        //StopCoroutine("GoSlowMotion");
        // tutorialStop.Play();
        //audioSource.pitch = 1f;
        SetIsGoRealTimeRunning(true);
        float increaseRate = 0.0125f;
        float perTime = 0.02f;

        Time.fixedDeltaTime = 0.02f;

        if (Time.timeScale < 0.5f) Time.timeScale = 0.5f;

        do
        {
            do
            {
                Time.timeScale = Time.timeScale * (1 + increaseRate);
                //Time.fixedDeltaTime = 0.02f;
                yield return new WaitForSecondsRealtime(perTime);
            } while (Time.timeScale < 0.9f && runningType == timeMode.real);
        } while (runningType == timeMode.paused);

        // Time.fixedDeltaTime = 0.02f;
        if (runningType == timeMode.real) Time.timeScale = 1;
        SetIsGoRealTimeRunning(false);
        yield break;

    }
    public IEnumerator GoRealTime(GameObject go)
    {

        if (!isTutorialEnabled) yield break; // if tutorial isnt enabled fuck off

        tutorialStop.Play();
        audioSource.pitch = 1f;

        StopCoroutine("GoSlowMotion");
        go.SetActive(false);
        yield return StartCoroutine(GoRealTime());
        // tutorialStop.Play();
        //audioSource.pitch = 1f;

    }

    public IEnumerator GoRealTime_OLD(GameObject go)
    {

        //OLD GO REAL TIME
        /* go.SetActive(false);
        //GetTutorialThrowHookText().SetActive(false);
        if (!isTutorialEnabled) yield break;
        float increaseRate = 0.0125f;
        float perTime = 0.02f;
        SetIsGoRealTimeRunning(true);
        if (Time.timeScale < 0.5f) Time.timeScale = 0.5f;

        do
        {
            Time.timeScale = Time.timeScale * (1 + increaseRate);
            Time.fixedDeltaTime = 0.02f;
            yield return new WaitForSecondsRealtime(perTime);
        } while (Time.timeScale < 0.9f && runningType == timeMode.real);

        Time.fixedDeltaTime = 0.02f;
        if (runningType == timeMode.real) Time.timeScale = 1;
        SetIsGoRealTimeRunning(false);
        yield break; */

        //NEW GO REAL TIME
        if (!isTutorialEnabled) yield break; // if tutorial isnt enabled fuck off

        StopCoroutine("GoSlowMotion");
        go.SetActive(false);

        SetIsGoRealTimeRunning(true);
        float increaseRate = 0.0125f;
        float perTime = 0.02f;

        Time.fixedDeltaTime = 0.02f;

        if (Time.timeScale < 0.5f) Time.timeScale = 0.5f;

        do
        {
            Time.timeScale = Time.timeScale * (1 + increaseRate);
            //Time.fixedDeltaTime = 0.02f;
            yield return new WaitForSecondsRealtime(perTime);
        } while (Time.timeScale < 0.9f && runningType == timeMode.real);

        // Time.fixedDeltaTime = 0.02f;
        if (runningType == timeMode.real) Time.timeScale = 1;
        SetIsGoRealTimeRunning(false);
        yield break;
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        Application.targetFrameRate = 60;
    }
    #endregion

    private void ResetVariables()
    {
        tutorialCounter = 0;
        GetIsTutorialEnabled();
        ResetAudioVariables();
        //if (isTutorialEnabled == null) isTutorialEnabled=true;
        //isTutorialEnabled=true;
        SetIsGoSlowMotionRunning(false);
        SetIsGoRealTimeRunning(false);
        ResetTimeScale();
        runningType = timeMode.real;


    }

    private IEnumerator UnloadResources()
    {
        float timer = 10f;
        while (true)
        {
            Resources.UnloadUnusedAssets();
            yield return new WaitForSeconds(timer);

        }
    }

    #region  SFX and Music

    public void PlayMagnetGateMusic()
    {
        // Play background music during magnet effects
        magnetSFX_GO.GetComponent<AudioSource>().clip = magnetSFXClips[Random.Range(0, magnetSFXClips.Length - 1)];
        magnetSFX_GO.GetComponent<AudioSource>().Play();
    }
    public void StopMagnetGateMusic()
    {
        // Stop background music during magnet effects
        magnetSFX_GO.GetComponent<AudioSource>().Stop();
    }


    public void PlaySpectreGateMusic()
    {
        // Play background music during Spectre effects
        spectreSFX_GO.GetComponent<AudioSource>().clip = spectreSFXClips[Random.Range(0, magnetSFXClips.Length - 1)];
        spectreSFX_GO.GetComponent<AudioSource>().Play();

    }
    public void StopSpectreGateMusic()
    {
        // Stop background music during Spectre effects
        spectreSFX_GO.GetComponent<AudioSource>().Stop();
    }


    private IEnumerator PlaySoundEffects()
    {
        //Play start SFX
        audioSource = GetComponent<AudioSource>();
        //audioSource.volume = generalVolume;
        audioSource.clip = startSFX;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(startSFX.length + audioOffset);

        //if audio is still playing, stop it.
        if (audioSource.isPlaying) { audioSource.Stop(); }

        //Play 1 of three music clips
        int whatToPlay = Random.Range(0, startMusicClips.Length);
        audioSource.clip = startMusicClips[whatToPlay];
        audioSource.Play();
        yield return new WaitForSecondsRealtime(startMusicClips[whatToPlay].length);


        while (true)
        {

            whatToPlay = Random.Range(0, postMusicClips.Length);
            audioSource.clip = postMusicClips[whatToPlay];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(postMusicClips[whatToPlay].length);
        }
    }

    #endregion /SFX and Music

    public void SetMusicPitch(float i)
    {
        am.SetFloat("Music/Pitch", i);
    }

    void Awake()
    {
        instance = this;
        ResetVariables();
        LoadCSV();
        Resources.UnloadUnusedAssets();
        //StartCoroutine(UnloadResources());
        GameAnalytics.Initialize();
    }
    // Use this for initialization
    void Start()
    {

        StartCoroutine(PlaySoundEffects());

        if (AdManager.instance) AdManager.instance.ShowBanner();    // Displays an ad banner

    }



    // Update is called once per frame
    void Update()
    {

        //if player passed through 5 modules, disable tutorial
        if (GetIsTutorialEnabled())
        {
            if (GetTutorialCounter() >= 5) SetIsTutorialEnabled(false);
        }

        //print(runningType);
        if (Map.instance)
        {
            if (Equals(Map.instance.GetPlayerGameObject(), null)) return;
        }
        else if (TestingGrounds.instance)
        {
            if (Equals(TestingGrounds.instance.GetPlayerGameObject(), null)) return;
        }
        //if (runningType == timeMode.real) return;

        if (runningType == timeMode.slow || (runningType == timeMode.real && Time.timeScale < 1))
        {
            if (!isGoRealTimeRunning)
            {
                if (Map.instance)
                {

                    bool triggerOnce = false;
                    if (GetTutorialObjectToHandle().ToString().StartsWith("TutorialReleaseHook"))
                    {
                        if (!Map.instance.GetPlayerGameObject().GetComponent<ThrowHook>().DoesHookExist()) triggerOnce = true;
                    }
                    else if (GetTutorialObjectToHandle().ToString().StartsWith("TutorialThrowHook"))
                    {
                        if (Map.instance.GetPlayerGameObject().GetComponent<ThrowHook>().DoesHookExist()) triggerOnce = true;
                    }

                    //if (Map.instance.GetPlayerGameObject().GetComponent<ThrowHook>().DoesHookExist()){
                    if (triggerOnce)
                    {
                        triggerOnce = false;
                        SetRunningType(timeMode.real);
                        GetTutorialObjectToHandle().SetActive(false);

                        StopCoroutine(GoSlowMotion(GetTutorialObjectToHandle()));
                        SetIsGoSlowMotionRunning(false);

                        StartCoroutine(GoRealTime(GetTutorialObjectToHandle()));
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {

                    if (TestingGrounds.instance.GetPlayerGameObject().GetComponent<ThrowHook>().DoesHookExist())
                    {
                        SetRunningType(timeMode.real);
                        GetTutorialObjectToHandle().SetActive(false);

                        StopCoroutine(GoSlowMotion(GetTutorialObjectToHandle()));
                        SetIsGoSlowMotionRunning(false);

                        StartCoroutine(GoRealTime(GetTutorialObjectToHandle()));
                    }
                }

            }
        }
    }

}
