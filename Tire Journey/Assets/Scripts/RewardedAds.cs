using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class RewardedAds : MonoBehaviour
{
    private RewardedAd rewardedAd;
    public ShopManager shopManager;
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        string adUnitId;
        #if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
            adUnitId = "unexpected_platform";
        #endif

        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // MonoBehaviour.print(
        //     "HandleRewardedAdFailedToLoad event received with message: "
        //                      + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        // Pause the game
        Time.timeScale = 0;
        audioManager.StopAll();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        // Ad Reward
        PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins", 0) + 50);
        Time.timeScale = 1;
        shopManager.UpdateUI();
        audioManager.PlaySound("MainTheme");


    }

    public void UserChoseToWatchAd()
    {
    if (this.rewardedAd.IsLoaded()) {
        this.rewardedAd.Show();
    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
