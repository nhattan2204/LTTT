using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using System;

public class AdsControl : MonoBehaviour
{


	protected AdsControl ()
	{
	}

	private static AdsControl _instance;
	InterstitialAd interstitial;
	RewardBasedVideoAd rewardBasedVideo;
	BannerView bannerView;
	public string AdmobID_Android, AdmobID_IOS, BannerID_Android, BannerID_IOS;

	public static AdsControl Instance { get { return _instance; } }

	void Awake ()
	{
		if (FindObjectsOfType (typeof(AdsControl)).Length > 1) {
			Destroy (gameObject);
			return;
		}

		_instance = this;
		MakeNewInterstial ();
		RequestRewardBasedVideo ();

		RequestBanner ();
		ShowBanner ();
		DontDestroyOnLoad (gameObject); //Already done by CBManager


	}


	public void HandleInterstialAdClosed (object sender, EventArgs args)
	{

		if (interstitial != null)
			interstitial.Destroy ();
		MakeNewInterstial ();



	}

	void MakeNewInterstial ()
	{


		#if UNITY_ANDROID
		interstitial = new InterstitialAd (AdmobID_Android);
		#endif
		#if UNITY_IPHONE
		interstitial = new InterstitialAd (AdmobID_IOS);
		#endif
		interstitial.OnAdClosed += HandleInterstialAdClosed;
		AdRequest request = new AdRequest.Builder ().Build ();
		interstitial.LoadAd (request);


	}


	public void showAds ()
	{
		int Ads_Remove = PlayerPrefs.GetInt ("RemoveAds");

		if (Ads_Remove == 0) {

			int randomNum = UnityEngine.Random.Range (0, 2);
			if (randomNum == 0)
				interstitial.Show ();
		}


	}
//	public void HideBannerAds ()
//	{
//
//	}
//
//	public void ShowBannerAds ()
//	{
//	}

	private void RequestBanner ()
	{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = BannerID_Android;
		#elif UNITY_IPHONE
		string adUnitId = BannerID_IOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView (adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().Build ();
		// Load the banner with the request.
		bannerView.LoadAd (request);

		}

		public void ShowBanner ()
		{
		bannerView.Show ();
		}

		public void HideBanner ()
		{
		bannerView.Hide ();
		}

		private void RequestRewardBasedVideo ()
		{

		#if UNITY_ANDROID
		//string adUnitId = RewardVideo_Android;
		#elif UNITY_IPHONE
		string adUnitId = RewardVideo_IOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif
		rewardBasedVideo = RewardBasedVideoAd.Instance;

		AdRequest request = new AdRequest.Builder ().Build ();
		//rewardBasedVideo.LoadAd (request, adUnitId);
		//rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		//rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
		}

		public void HandleRewardBasedVideoRewarded (object sender, Reward args)
		{
		string type = args.Type;
		double amount = args.Amount;
		print ("User rewarded with: " + amount.ToString () + " " + type);
		//FindObjectOfType<Menu> ().Purchase (3);
		}

		public void HandleRewardBasedVideoClosed (object sender, EventArgs args)
		{
		RequestRewardBasedVideo ();
		}

	}

