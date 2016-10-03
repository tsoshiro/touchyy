using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdvertisementManager : MonoBehaviour {
	GameObject callBackObj;

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

	public void showInterstitial() {
		
	}
}