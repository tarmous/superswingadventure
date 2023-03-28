using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    private enum TimeMark
    {
        S = 25,
        A = 20,
        B = 15,
        C = 10,
        D = 5,
        E = 0
    }

    public AudioClip[] musicClips;

    [Range(0, 1)]
    public float musicVolume;
    new private AudioSource audio;
    public GameObject youDied, finalScore, points, timer, line, numOfCoins, coinTotalValue, coinObjectWrapper; //text objects
    public GameObject[] buttons;
    //const float minTimerSize=35f, maxTimerSize=110f, anticipatedRatio=70/5;
    const int youDiedStartSize = 1000;
    public float d_points, d_time, d_finalScore, d_coinsGathered;
    const float coinValue = 10, coinValueMultiplier = 0.8f, gemValue = 1, timerValueMultiplier = 0.7f;
    private const float startPivot = 2, endPivot = 1f, virtualEndPivot = endPivot - 0.1f, pivotRate = 0.08f, pivotX = 0.5f, slideUpRate = 0.00005f;
    private const float elementRevealRate = 0.30f;

    private float tempcoinscore;

    public void RestartLevel()
    {
        if (GameManager.instance)
        {
            if (!GameManager.instance.GetIsTutorialEnabled())
            {
                if (AdManager.instance)
                {
                    if (AdManager.instance.ShouldShowInterstitial())
                    {
                        AdManager.instance.ShowInterstitial();
                        return;
                    }
                }
            }
        }

        SceneManager.LoadSceneAsync(SceneIndexes.sampleSceneIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(SceneIndexes.startMenuIndex);
    }

    void Awake()
    {
        Application.targetFrameRate = 60;

        audio = GetComponentInParent<AudioSource>();

        d_points = Death.points;
        d_coinsGathered = Death.coinsGathered;
        d_time = Death.time;

        d_finalScore = CalculateFinalScore(d_points, d_time * timerValueMultiplier, d_coinsGathered * coinValue * coinValueMultiplier);

    }

    // Use this for initialization
    IEnumerator Start()
    {

        if (AdManager.instance) AdManager.instance.HideBanner();    // Hides an ad banner

        float ratio = d_points / d_time; // the higher the ratio, the lower the font (shows that the player does well, else he does poorly)

        if (ratio >= (int)TimeMark.S)
        {
            timer.GetComponent<Text>().text = /*  "Time: " + */TimeMark.S.ToString();
        }
        else if (ratio >= (int)TimeMark.A)
        {
            timer.GetComponent<Text>().text = /*  "Time: " + */TimeMark.A.ToString();
        }
        else if (ratio >= (int)TimeMark.B)
        {
            timer.GetComponent<Text>().text = /*  "Time: " + */TimeMark.B.ToString();
        }
        else if (ratio >= (int)TimeMark.C)
        {
            timer.GetComponent<Text>().text = /*  "Time: " + */TimeMark.C.ToString();
        }
        else if (ratio >= (int)TimeMark.D)
        {
            timer.GetComponent<Text>().text = /*  "Time: " + */TimeMark.D.ToString();
        }
        else if (ratio >= (int)TimeMark.E)
        {
            timer.GetComponent<Text>().text = /*  "Time: " + */ TimeMark.E.ToString();
        }
        else
        {
            //derp?!
        }

        /* if(ratio < anticipatedRatio){
			timer.GetComponent<Text>().fontSize = (int)( Mathf.Clamp( 
															(anticipatedRatio - ratio) * (minTimerSize / maxTimerSize) + minTimerSize,
															minTimerSize,
															maxTimerSize) );
		} */

        points.GetComponent<Text>().text =/* "Points: " +*/ int.Parse(d_points.ToString()).ToString();
        // + " + (" +d_coinsGathered.ToString() + "$ ) X " + coinValue.ToString();
        //timer.GetComponent<Text>().text = "Time: " + System.Math.Round(d_time, 2).ToString();
        numOfCoins.GetComponent<Text>().text = d_coinsGathered.ToString();
        coinTotalValue.GetComponent<Text>().text = (d_coinsGathered * coinValue).ToString(); //" x" + coinValue.ToString() + " = " + d_coinsGathered * coinValue;


        points.SetActive(false);
        timer.SetActive(false);
        finalScore.SetActive(false);
        line.SetActive(false);
        coinObjectWrapper.SetActive(false);
        foreach (GameObject go in buttons)
        {
            go.SetActive(false);
        }

        //Slide up effect
        //currentPivot= startPivot;
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(pivotX, startPivot);
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        while (gameObject.GetComponent<RectTransform>().pivot.y > endPivot)
        {
            gameObject.GetComponent<RectTransform>().pivot = Vector2.Lerp(gameObject.GetComponent<RectTransform>().pivot, new Vector2(pivotX, virtualEndPivot), pivotRate); //new Vector2 (pivotX, currentPivot); 
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, Vector2.zero, pivotRate);
            yield return null;
        }
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(pivotX, endPivot);
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        //start playing music after canvas slides up
        int j = Random.Range(0, musicClips.Length);
        audio.clip = musicClips[j];
        audio.volume = musicVolume;
        audio.Play();

        //yield return StartCoroutine( YouDiedEffect( youDied.GetComponent<Text>().fontSize ) );
        yield return new WaitForSecondsRealtime(elementRevealRate);

        points.SetActive(true);
        yield return new WaitForSecondsRealtime(elementRevealRate);

        coinObjectWrapper.SetActive(true);
        yield return new WaitForSecondsRealtime(elementRevealRate / 3);

        timer.SetActive(true);
        yield return new WaitForSecondsRealtime(elementRevealRate);

        line.SetActive(true);
        yield return new WaitForSecondsRealtime(elementRevealRate / 3);

        finalScore.SetActive(true);
        finalScore.GetComponent<Text>().text = System.Math.Round(d_finalScore, 0).ToString();
        yield return StartCoroutine(ShowFinalScore(float.Parse(System.Math.Round(d_finalScore, 0).ToString())));
        //yield return new WaitForSeconds(elementRevealRate/3);
        foreach (GameObject go in buttons)
        {
            go.SetActive(true);
        }
    }

    /* 	private IEnumerator YouDiedEffect(int actualSize){

            youDied.GetComponent<Text>().fontSize = youDiedStartSize;
            while (youDied.GetComponent<Text>().fontSize > actualSize){

                youDied.GetComponent<Text>().fontSize -= 10;
                yield return new WaitForSeconds(0.001f);

            }
        }
     */
    private IEnumerator ShowFinalScore(float fin)
    {
        //if (fin == null) fin = 0;
        if (fin < 0) fin = 0;
        GooglePlayServices.AddScoreToLeaderboard(googleplaygames.leaderboard_all_time, long.Parse(fin.ToString()));
        float f = 0;
        float inHowManySeconds = 0.8f;
        float interpSpeed = fin / inHowManySeconds, ticksPerUpdate = 10f;

        finalScore.GetComponent<Text>().text = "xxxxx";

        if (fin > ticksPerUpdate)
        {
            while (f < fin)
            {
                f = (float)System.Math.Round(f + interpSpeed / ticksPerUpdate);
                finalScore.GetComponent<Text>().text = f.ToString();
                yield return new WaitForSecondsRealtime(1 / ticksPerUpdate);
            }
        }
        else
        {
            finalScore.GetComponent<Text>().text = f.ToString();
        }

        if (f != fin)
        {
            f = fin;
            finalScore.GetComponent<Text>().text = f.ToString();
        }

    }

    private IEnumerator ShowFinalScore(char[] c)
    {
        finalScore.GetComponent<Text>().text = "xxxxx";

        for (int i = 0; i < c.Length; i++)
        {
            finalScore.GetComponent<Text>().text += c[i];
            yield return new WaitForSeconds(0.5f);
        }
    }

    private float CalculateFinalScore(float score, float timer, float goldAmount)
    {
        return Mathf.Pow(score + goldAmount, 2) / (timer * 10);
    }



}
