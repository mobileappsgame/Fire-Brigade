using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using ConsentManager.Api;
using ConsentManager.Common;
using UnityEngine;
using UnityEngine.UI;

namespace ConsentManager.ConsentManagerDemo.Scripts
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public class ConsentManagerDemo : MonoBehaviour, IConsentFormListener, IConsentInfoUpdateListener,
        IBannerAdListener, IMrecAdListener, IRewardedVideoAdListener, IInterstitialAdListener,
        IPermissionGrantedListener
    {
        #region UI

        [SerializeField] public Toggle tgTesting;
        [SerializeField] public Toggle tgLogging;
        [SerializeField] public Button btnShowInterstitial;
        [SerializeField] public Button btnShowRewardedVideo;
        [SerializeField] public GameObject consentManagerPanel;
        [SerializeField] public GameObject appodealPanel;

        #endregion

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE
        string appKey = "";
#elif UNITY_ANDROID
        string appKey = "fee50c333ff3825fd6ad6d38cff78154de3025546d47a84f";
#elif UNITY_IPHONE
        string appKey = "466de0d625e01e8811c588588a42a55970bc7c132649eede";
#else
	string appKey = "";
#endif

        private ConsentForm consentForm;
        private Api.ConsentManager consentManager;
        private bool isShouldSaveConsentForm;
        public Consent currentConsent;

        private bool isTesting;
        private bool isLogging;

        private void Start()
        {
            isLogging = true;
            isTesting = true;

            consentManagerPanel.gameObject.SetActive(true);
            appodealPanel.gameObject.SetActive(false);


            btnShowInterstitial.GetComponentInChildren<Text>().text = "CACHE INTERSTITIAL";
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = "CACHE REWARDED VIDEO";

            tgTesting.onValueChanged.AddListener(delegate { setTesting(tgTesting); });
            tgLogging.onValueChanged.AddListener(delegate { setLogging(tgLogging); });

            consentManager = Api.ConsentManager.getInstance();
        }

        private void Awake()
        {
            Appodeal.requestAndroidMPermissions(this);
        }

        public void requestConsentInfoUpdate()
        {
            consentManager.requestConsentInfoUpdate(appKey, this);
        }

        public void setCustomVendor()
        {
            var customVendor = new Vendor.Builder(
                    "Appodeal Test",
                    "com.appodeal.test",
                    "https://customvendor.com")
                .setPurposeIds(new List<int> {100, 200, 300})
                .setFeatureId(new List<int> {400, 500, 600})
                .setLegitimateInterestPurposeIds(new List<int> {700, 800, 900})
                .build();

            consentManager.setCustomVendor(customVendor);

            var vendor = consentManager.getCustomVendor("com.appodeal.test");
            if (vendor == null) return;
            print("Vendor getName: " + vendor.getName());
            print("Vendor getBundle: " + vendor.getBundle());
            print("Vendor getPolicyUrl: " + vendor.getPolicyUrl());
            foreach (var purposeId in vendor.getPurposeIds())
            {
                print("Vendor getPurposeIds: " + purposeId);
            }

            foreach (var featureId in vendor.getFeatureIds())
            {
                print("Vendor getFeatureIds: " + featureId);
            }

            foreach (var legitimateInterestPurposeId in vendor.getLegitimateInterestPurposeIds())
            {
                print("Vendor getLegitimateInterestPurposeIds: " + legitimateInterestPurposeId);
            }
        }

        public void shouldShowForm()
        {
            print("shouldShowConsentDialog: " + consentManager.shouldShowConsentDialog());
        }

        public void getConsentZone()
        {
            print("getConsentZone: " + consentManager.getConsentZone());
        }

        public void getConsentStatus()
        {
            print("getConsentStatus: " + consentManager.getConsentStatus());
        }

        public void loadConsentForm()
        {
            consentForm = new ConsentForm.Builder().withListener(this).build();
            consentForm?.load();
        }

        public void isLoadedConsentForm()
        {
            if (consentForm != null)
            {
                print("isLoadedConsentForm:  " + consentForm.isLoaded());
            }
        }

        public void showFormAsActivity()
        {
            if (consentForm != null)
            {
                consentForm.showAsActivity();
            }
            else
            {
                print("showForm - false");
            }
        }

        public void showFormAsDialog()
        {
            if (consentForm != null)
            {
                consentForm.showAsDialog();
            }
            else
            {
                print("showForm - false");
            }
        }

        public void printIABString()
        {
            print("Consent IAB String is: " + consentManager.getConsent().getIabConsentString());
        }

        public void printCurrentConsent()
        {
            if (consentManager.getConsent() == null) return;
            print(
                "consent.getIabConsentString() - " + consentManager.getConsent().getIabConsentString());
            print(
                "consent.hasConsentForVendor() - " +
                consentManager.getConsent().hasConsentForVendor("com.appodeal.test"));
            print("consent.getStatus() - " + consentManager.getConsent().getStatus());
            print("consent.getZone() - " + consentManager.getConsent().getZone());
        }

        public void showAppodealLogic()
        {
            consentManagerPanel.SetActive(false);
            appodealPanel.SetActive(true);
        }

        private void setTesting(Toggle toggle)
        {
            isTesting = toggle.isOn;
        }

        private void setLogging(Toggle toggle)
        {
            isLogging = toggle.isOn;
        }

        public void initialize()
        {
            if (currentConsent != null)
            {
                initWithConsent(true);
            }
            else
            {
                initWithConsent(false);
            }
        }

        public void initWithConsent(bool isConsent)
        {
            Appodeal.setTesting(isTesting);
            Appodeal.setLogLevel(isLogging ? Appodeal.LogLevel.Verbose : Appodeal.LogLevel.None);
            Appodeal.setUserId("1");
            Appodeal.setUserAge(1);
            Appodeal.setUserGender(UserSettings.Gender.OTHER);
            Appodeal.disableLocationPermissionCheck();
            Appodeal.disableWriteExternalStoragePermissionCheck();
            Appodeal.setTriggerOnLoadedOnPrecache(Appodeal.INTERSTITIAL, true);
            Appodeal.setSmartBanners(true);
            Appodeal.setBannerAnimation(true);
            Appodeal.setTabletBanners(true);
            Appodeal.setBannerBackground(true);
            Appodeal.setChildDirectedTreatment(false);
            Appodeal.muteVideosIfCallsMuted(true);
            Appodeal.setAutoCache(Appodeal.INTERSTITIAL, false);
            Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, false);
            Appodeal.setExtraData(ExtraData.APPSFLYER_ID, "1527256526604-2129416");
            if (isConsent)
            {
                Appodeal.initialize(appKey,
                    Appodeal.INTERSTITIAL | Appodeal.BANNER_VIEW | Appodeal.REWARDED_VIDEO | Appodeal.MREC,
                    currentConsent);
            }
            else
            {
                Appodeal.initialize(appKey,
                    Appodeal.INTERSTITIAL | Appodeal.BANNER_VIEW | Appodeal.REWARDED_VIDEO | Appodeal.MREC,
                    true);
            }

            Appodeal.setBannerCallbacks(this);
            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);
            Appodeal.setMrecCallbacks(this);
            Appodeal.setSegmentFilter("newBoolean", true);
            Appodeal.setSegmentFilter("newInt", 1234567890);
            Appodeal.setSegmentFilter("newDouble", 123.123456789);
            Appodeal.setSegmentFilter("newString", "newStringFromSDK");
        }

        public void showInterstitial()
        {
            if (Appodeal.isLoaded(Appodeal.INTERSTITIAL) && !Appodeal.isPrecache(Appodeal.INTERSTITIAL))
            {
                Appodeal.show(Appodeal.INTERSTITIAL);
            }
            else
            {
                Appodeal.cache(Appodeal.INTERSTITIAL);
            }
        }

        public void showRewardedVideo()
        {
            if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
            {
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }
            else
            {
                Appodeal.cache(Appodeal.REWARDED_VIDEO);
            }
        }

        public void showBanner()
        {
            Appodeal.show(Appodeal.BANNER_BOTTOM, "default");
        }

        public void hideBanner()
        {
            Appodeal.hide(Appodeal.BANNER);
        }

        public void showBannerView()
        {
            Appodeal.showBannerView(Screen.currentResolution.height - Screen.currentResolution.height / 10,
                Appodeal.BANNER_HORIZONTAL_CENTER, "default");
        }

        public void hideBannerView()
        {
            Appodeal.hideBannerView();
        }

        public void showMrecView()
        {
            Appodeal.showMrecView(Screen.currentResolution.height - Screen.currentResolution.height / 10,
                Appodeal.BANNER_HORIZONTAL_CENTER, "default");
        }

        public void hideMrecView()
        {
            Appodeal.hideMrecView();
        }

        public void showTestScreen()
        {
            Appodeal.showTestScreen();
        }


        #region ConsentFormListener

        public void onConsentFormLoaded()
        {
            print("ConsentFormListener - onConsentFormLoaded");
        }

        public void onConsentFormError(ConsentManagerException exception)
        {
            print($"ConsentFormListener - onConsentFormError, reason - {exception.getReason()}");
        }

        public void onConsentFormOpened()
        {
            print("ConsentFormListener - onConsentFormOpened");
        }

        public void onConsentFormClosed(Consent consent)
        {
            currentConsent = consent;
            print($"ConsentFormListener - onConsentFormClosed, consentStatus - {consent.getStatus()}");
        }

        #endregion

        #region ConsentInfoUpdateListener

        public void onConsentInfoUpdated(Consent consent)
        {
            currentConsent = consent;
            print("onConsentInfoUpdated");
        }

        public void onFailedToUpdateConsentInfo(ConsentManagerException error)
        {
            print($"onFailedToUpdateConsentInfo");

            if (error == null) return;
            print($"onFailedToUpdateConsentInfo Reason: {error.getReason()}");

            switch (error.getCode())
            {
                case 0:
                    print("onFailedToUpdateConsentInfo - UNKNOWN");
                    break;
                case 1:
                    print(
                        "onFailedToUpdateConsentInfo - INTERNAL - Error on SDK side. Includes JS-bridge or encoding/decoding errors");
                    break;
                case 2:
                    print("onFailedToUpdateConsentInfo - NETWORKING - HTTP errors, parse request/response ");
                    break;
                case 3:
                    print("onFailedToUpdateConsentInfo - INCONSISTENT - Incorrect SDK API usage");
                    break;
            }
        }

        #endregion

        #region Banner callback handlers

        public void onBannerLoaded(int height, bool precache)
        {
            print("banner loaded");
        }

        public void onBannerFailedToLoad()
        {
            print("banner failed");
        }

        public void onBannerShown()
        {
            print("banner opened");
        }

        public void onBannerClicked()
        {
            print("banner clicked");
        }

        public void onBannerExpired()
        {
            print("banner expired");
        }

        #endregion

        #region Interstitial callback handlers

        public void onInterstitialLoaded(bool isPrecache)
        {
            if (!isPrecache)
            {
                btnShowInterstitial.GetComponentInChildren<Text>().text = "SHOW INTERSTITIAL";
            }
            else
            {
                print("Appodeal. Interstitial loaded. isPrecache - true");
            }

            print("Appodeal. Interstitial loaded");
        }

        public void onInterstitialFailedToLoad()
        {
            print("Appodeal. Interstitial failed");
        }

        public void onInterstitialShowFailed()
        {
            print("Appodeal. Interstitial show failed");
        }

        public void onInterstitialShown()
        {
            print("Appodeal. Interstitial opened");
        }

        public void onInterstitialClosed()
        {
            btnShowInterstitial.GetComponentInChildren<Text>().text = "CACHE INTERSTITIAL";
            print("Appodeal. Interstitial closed");
        }

        public void onInterstitialClicked()
        {
            print("Appodeal. Interstitial clicked");
        }

        public void onInterstitialExpired()
        {
            print("Appodeal. Interstitial expired");
        }

        #endregion

        #region Rewarded Video callback handlers

        public void onRewardedVideoLoaded(bool isPrecache)
        {
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = "SHOW REWARDED VIDEO";
            print("Appodeal. Video loaded");
        }

        public void onRewardedVideoFailedToLoad()
        {
            print("Appodeal. Video failed");
        }

        public void onRewardedVideoShowFailed()
        {
            print("Appodeal. RewardedVideo show failed");
        }

        public void onRewardedVideoShown()
        {
            print("Appodeal. Video shown");
        }

        public void onRewardedVideoClosed(bool finished)
        {
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = "SHOW REWARDED VIDEO";
            print("Appodeal. Video closed");
        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            print("Appodeal. Reward: " + amount + " " + name);
        }

        public void onRewardedVideoExpired()
        {
            print("Appodeal. Video expired");
        }

        public void onRewardedVideoClicked()
        {
            print("Appodeal. Video clicked");
        }

        #endregion

        #region Mrec callback handlers

        public void onMrecLoaded(bool precache)
        {
            print("mrec loaded");
        }

        public void onMrecFailedToLoad()
        {
            print("mrec failed");
        }

        public void onMrecShown()
        {
            print("mrec opened");
        }

        public void onMrecClicked()
        {
            print("mrec clicked");
        }

        public void onMrecExpired()
        {
            print("mrec expired");
        }

        #endregion

        #region PermissionGrantedListener

        public void writeExternalStorageResponse(int result)
        {
            if (result == 0)
            {
                Debug.Log("WRITE_EXTERNAL_STORAGE permission granted");
            }
            else
            {
                Debug.Log("WRITE_EXTERNAL_STORAGE permission grant refused");
            }
        }

        public void accessCoarseLocationResponse(int result)
        {
            if (result == 0)
            {
                Debug.Log("ACCESS_COARSE_LOCATION permission granted");
            }
            else
            {
                Debug.Log("ACCESS_COARSE_LOCATION permission grant refused");
            }
        }

        #endregion PermissionGrantedListener
    }
}