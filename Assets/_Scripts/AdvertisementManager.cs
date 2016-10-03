using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

using GoogleMobileAds.Api;

public class AdvertisementManager : MonoBehaviour {
	GameObject callBackObj;
	public string TEST_DEVICE_ID = "6befb76ee0c14f275cd09f2702c88528";

	public void ShowRewardedAd(GameObject pObj = null)
	{
		if (Advertisement.IsReady("rewardedVideoZone"))
		{
			if (pObj != null) {
				callBackObj = pObj;
			}
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideoZone", options);
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
				callBackObj.SendMessage ("movieCallBack", 0);
			}
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", 1);
			}
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", 2);
			}
			break;
		}
	}

	public string Android_interstitial;
	public string ios_interstitial;

	private InterstitialAd _interstitial;
	private AdRequest request;

	bool is_close_interstitial = false;


	void Awake() {
		// 起動時にロード
		RequestInterstitial();
	}

	public void RequestInterstitial() {
		#if UNITY_ADROID
		string adUnitId = Android_interstitial;
		#elif UNITY_IPHONE
		string adUnitId = ios_interstitial;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		if (is_close_interstitial) {
			_interstitial.Destroy ();
		}

		// Init
		_interstitial = new InterstitialAd(adUnitId);
		// create an empty request
		request = new AdRequest.Builder().
		                       AddTestDevice(TEST_DEVICE_ID).
		                       Build();
		// load inters
		_interstitial.LoadAd(request);
		_interstitial.OnAdClosed += HandleInterstitialClosed;

		is_close_interstitial = false;	
	}

	public void showInterstitial() {
		_interstitial.Show ();
	}

	public void HandleInterstitialClosed(object sender, System.EventArgs args) {
		is_close_interstitial = true;
		RequestInterstitial ();
	}

}