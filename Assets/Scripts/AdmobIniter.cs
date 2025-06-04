using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Exception = System.Exception;
using PlayerPrefs = UnityEngine.PlayerPrefs;
using Time = UnityEngine.Time;

public class AdmobIniter : MonoBehaviour
{
    private string _bannerUnitId = "ca-app-pub-3940256099942544/6300978111";

    private readonly int _triesToInit = 3;
    private int _currentTries;
    private BannerView _bannerView;
    private bool _isBannerVisible;

    public void Awake()
    {
        InitializeMobileAds();
    }

    private void CreateBannerView()
    {
        try
        {
            Debug.Log("banner created with id = " + _bannerUnitId);
            AdSize adaptiveSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            _bannerView = new BannerView(_bannerUnitId, adaptiveSize, AdPosition.Bottom);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while creating banner: " + e.Message);
        }
    }

    public void LoadBannerAd()
    {
        if (_isBannerVisible)
            return;

        CreateBannerView();
        AdRequest adRequest = new();

        try
        {
            _bannerView.LoadAd(adRequest);
            _isBannerVisible = true;
            Debug.Log("banner created with id = " + _bannerUnitId);
        }
        catch (Exception e)
        {
            Debug.Log("banner error: " + e);
        }
    }

    public void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
            _isBannerVisible = false;
        }
    }

    private void InitializeMobileAds()
    {
        if (_currentTries >= _triesToInit)
        {
            Debug.LogError("Failed to initialize Mobile Ads SDK after maximum attempts.");
            return;
        }

        MobileAds.Initialize(initStatus =>
        {
            bool allInitialized = true;

            foreach (KeyValuePair<string, AdapterStatus> adapter in initStatus.getAdapterStatusMap())
            {
                string className = adapter.Key;
                AdapterState status = adapter.Value.InitializationState;

                Debug.Log($"Adapter: {className}, Status: {status}");

                if (status != AdapterState.Ready)
                    allInitialized = false;
            }

            if (allInitialized)
            {
                LoadBannerAd();
                Debug.Log("INITED - admob somekey");
            }
            else
            {
                _currentTries++;
                Debug.LogWarning($"AdMob initialization incomplete. Attempt {_currentTries} of {_triesToInit}.");
                InitializeMobileAds();
            }
        });
    }
}