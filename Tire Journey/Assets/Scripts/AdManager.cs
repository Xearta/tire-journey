using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitial;


    public static AdManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        this.RequestBanner();
    
    }

    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        this.bannerAd = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerAd.LoadAd(request);
    }

    public void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        } 
    }

    // Update is called once per frame
    void Update()
    {
    }
}
