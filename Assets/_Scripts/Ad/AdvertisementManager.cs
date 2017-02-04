using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

using GoogleMobileAds.Api;

public class AdvertisementManager : MonoBehaviour {
	GameObject callBackObj;
	public string TEST_DEVICE_ID = "6befb76ee0c14f275cd09f2702c88528";

	public void ShowRewardedAd(GameObject pObj = null)
	{
		if (Advertisement.isInitialized &&
			Advertisement.IsReady ()) {
			if (pObj != null) {
				callBackObj = pObj;
			}
			Advertisement.Show (null, new ShowOptions {
				////trueだとUnityが止まり、音もミュートになる
				//pause = true, 
				//広告が表示された後のコールバック設定
				resultCallback = HandleShowResult
			});
		} else {
			Debug.Log ("NOT INITIALIZED!");
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", "0");
			}
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", "1");
			}
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", "2");
			}
			break;
		}
	}


	// UnityAds
	public string Android_gameId;
	public string ios_gameId;
	public bool isUnityAdsTestMode = false; // 本番=false テスト=true
	[SerializeField]
	string gameId;

	// AdMob
	public string Android_interstitial;
	public string ios_interstitial;
	public bool isAdMobInterTestMode = false; // 本番=false テスト=true

	[SerializeField]
	string adUnitId;

	private InterstitialAd _interstitial;
	private AdRequest request;

	bool is_close_interstitial = false;

	void Awake() {
		// 起動時にロード
		RequestInterstitial();
		setMovieReward ();
	}

	public void setMovieReward () {
	#if UNITY_ADROID
		gameId = Android_gameId;
	#elif UNITY_IPHONE
		gameId = ios_gameId;
	#else
		gameId = ios_gameId;
	#endif
		if (Advertisement.isSupported) { // If the platform is supported,
			Advertisement.Initialize (gameId, isUnityAdsTestMode); // initialize Unity Ads.
		}
	}

	public void RequestInterstitial() {
		#if UNITY_ANDROID
		adUnitId = Android_interstitial;
		#elif UNITY_IOS
		adUnitId = ios_interstitial;
		#else
		adUnitId = "unexpected_platform";
		#endif

		Debug.Log ("RequestInterstitial adUnitId:"+adUnitId);

		if (is_close_interstitial) {
			Debug.Log ("Destroy ad");
			_interstitial.Destroy ();
		}

		// Init
		_interstitial = new InterstitialAd(adUnitId);
		// create an empty request
		request = createRequest (isAdMobInterTestMode);
			
		// load inters
		_interstitial.LoadAd(request);
		_interstitial.OnAdClosed += HandleInterstitialClosed;

		is_close_interstitial = false;
	}

	AdRequest createRequest (bool isTest) {
		if (isTest) {
			return new AdRequest.Builder ().
							   AddTestDevice(TEST_DEVICE_ID).
							   Build ();
		}
		return new AdRequest.Builder ().
						  Build ();
	}

	public void showInterstitial() {
		_interstitial.Show ();
	}

	public void HandleInterstitialClosed(object sender, System.EventArgs args) {
		Debug.Log ("HandleInterstitialClosed");
		is_close_interstitial = true;
		RequestInterstitial ();
	}

}