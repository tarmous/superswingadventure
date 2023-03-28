using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
public class AdManager : MonoBehaviour, IRewardedVideoAdListener
{

    public static AdManager instance { get; private set; }
    public const float showInterstitialchance = 20; // per 100
    public const int perDeathShowAd = 4;
    public int perDeathCounter = 0;

    /* void OnGUI(){
		GUI.Box( new Rect(Screen.width/4, Screen.width/4, Screen.width/2, Screen.height/2), Appodeal.isLoaded(Appodeal.BANNER).ToString() );
	}  */

    private void Awake()
    {
        instance = this;
        perDeathCounter = 0;
    }

    private void Start()
    {
        int consentInt = PlayerPrefs.GetInt("result_gdpr", 0);
        bool consent = consentInt != 0;
        if (consent)
        {
            InitializeAds();
        }
    }

    public void InitializeAds()
    {
        string appKey = ""; // THE APP KEY WAS DELETED FOR SECURITY REASONS
        Appodeal.disableLocationPermissionCheck(); // Only a must Un-comment if we have deleted coarse location from Android Manifest of appodeal
        Appodeal.disableWriteExternalStoragePermissionCheck(); // write external storage permission check
        Appodeal.setTesting(false); // only used for testing and not for release

        int consentInt = PlayerPrefs.GetInt("result_gdpr_sdk", 0);
        bool consent = consentInt != 0;

        Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO, consent); // Enable Different Types of Ads

        Appodeal.setRewardedVideoCallbacks(this); // Tell Appodeal plugin which method has the callback functions for Appodeal Ads


    }

    #region  Show Ads
    public void ShowBanner()
    {
        if (Appodeal.isLoaded(Appodeal.BANNER))
        {
            Appodeal.show(Appodeal.BANNER_BOTTOM); // this shows a banner at the top of screen
        }
    }

    public void HideBanner()
    {
        Appodeal.hide(Appodeal.BANNER);
    }

    public bool ShouldShowInterstitial()
    {
        perDeathCounter++;
        if (perDeathCounter >= perDeathShowAd)
        {
            perDeathCounter = -1;
            return true;
        }
        return false;
    }

    public void ShowInterstitial()
    {
        if (true)
        { // We should have some condition (example: every 5 deaths)
            if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
            {
                Appodeal.show(Appodeal.INTERSTITIAL); // this shows an Interstitial ad
            }
        }
    }

    public void ShowRewardVideo()
    {
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        {
            Appodeal.show(Appodeal.REWARDED_VIDEO); // this shows a banner at the top of screen
        }
    }
    #endregion Show Ads

    #region IRewardedVideoAdListener interface functions
    public void onRewardedVideoLoaded(bool precache) { }
    public void onRewardedVideoFailedToLoad() { }
    public void onRewardedVideoShown() { }
    public void onRewardedVideoFinished(double amount, string name) { /* REWARD PLAYER HERE */ }
    public void onRewardedVideoClosed(bool finished) { }
    public void onRewardedVideoExpired() { }
    public void onRewardedVideoClicked(){}
    #endregion IRewardedVideoAdListener interface functions
}
