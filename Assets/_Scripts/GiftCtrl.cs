using UnityEngine;
using System.Collections;
using System;

public class GiftCtrl : MonoBehaviour {
	public ResultCtrl _resultCtrl;
	public GameObject _giftBtn;
	GameObject image;
	TextMesh leftTimeText;
	GameObject button_small;

	float xPos = -1.5f;

	public DateTime nextTimeFreeGift;
	public DateTime intervalDateTime;

	enum GiftButtonStatus {
		FREE_AVAILABLE,
		FREE_AWAIT,
		REWARD
	}
	GiftButtonStatus status;

	public void Init() {
		_resultCtrl = this.GetComponent<ResultCtrl>();

		image = _giftBtn.transform.FindChild ("image").gameObject;
		leftTimeText = _giftBtn.transform.FindChild ("leftTime").gameObject.GetComponent<TextMesh>();
		button_small = _giftBtn.transform.FindChild ("button_small").gameObject;

		nextTimeFreeGift = DateTime.Parse(_resultCtrl._gameCtrl._userData.nextFreeGift);
		isRewardMovieWatched = false;

		statusCheck ();
	}

	public void statusCheck() {
		if (checkIsMovieRewardAvailable()) { // 動画広告
			status = GiftButtonStatus.REWARD;
		} else if (checkIsFreeGiftAvailable()) { // フリーギフト
			status = GiftButtonStatus.FREE_AVAILABLE;
		} else { // 待機中
			status = GiftButtonStatus.FREE_AWAIT;
		}
		setButton (status);
	}

	bool checkIsFreeGiftAvailable() {
		if (nextTimeFreeGift - DateTime.Now <= TimeSpan.Zero) {
			return true;
		} else {
			return false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!checkIsFreeGiftAvailable()) {
			TimeSpan leftTime = nextTimeFreeGift - DateTime.Now;
			leftTimeText.text = String.Format ("{0:00}:{1:00}:{2:00}",
				leftTime.Hours,
				leftTime.Minutes,
				leftTime.Seconds);
			
			if (leftTime <= TimeSpan.Zero) {
				leftTime = TimeSpan.Zero;
				if (status == GiftButtonStatus.FREE_AWAIT) {
					status = GiftButtonStatus.FREE_AVAILABLE;
					setButton (status);
				}
			}
		}
	}
		
	public bool giveGiftFree() {
		if (checkIsFreeGiftAvailable()) {
			nextTimeFreeGift = DateTime.Now.AddHours(1);
			statusCheck ();
			return true;
		}
		return false;
	}

	public bool checkIsReward() {
		if (status == GiftButtonStatus.REWARD) {
			return true;
		}
		return false;
	}

	int coinRate = 1000;
	public int getGiftCoinValue() {
		UserData userData =	_resultCtrl._gameCtrl._userData;
		int totalUserLevel = 0;
		for (int i = 0; i < userData._userParamsList.Count; i++) {
			totalUserLevel += userData._userParamsList [i];
			Debug.Log ("add: " + userData._userParamsList [i]);
		}
		Debug.Log ("totalUserLevel: " + totalUserLevel);
		return totalUserLevel * coinRate;
	}

	void setButton(GiftButtonStatus pStatus) {
		Vector3 pos;
		switch (pStatus) {
		case GiftButtonStatus.FREE_AVAILABLE:
			ColorEditor.setColor(button_small, Color.white);
			ColorEditor.setColorFromColorCode (image, Const.COLOR_CODE_BASE_COLOR);
			ColorEditor.setFade (_giftBtn, 1.0f);
			leftTimeText.gameObject.SetActive (false);
			pos = image.transform.localPosition;
			pos.x = 0f;
			image.transform.localPosition = pos;

			new AnalyticsManager ().SendCounterEvent ("freeGiftOffer", 1);

			break;
		case GiftButtonStatus.FREE_AWAIT:
			ColorEditor.setColor(button_small, Color.white);
			ColorEditor.setColorFromColorCode (image, Const.COLOR_CODE_BASE_COLOR);
			ColorEditor.setFade (_giftBtn, 0.8f);
			pos = image.transform.localPosition;
			pos.x = xPos;
			image.transform.localPosition = pos;
			leftTimeText.gameObject.SetActive (true);
			break;
		case GiftButtonStatus.REWARD:
			ColorEditor.setColorFromColorCode(button_small, Const.COLOR_CODE_PINK);
			ColorEditor.setColor (image, Color.white);
			ColorEditor.setFade (_giftBtn, 1.0f);

			leftTimeText.gameObject.SetActive (false);
			pos = image.transform.localPosition;
			pos.x = 0f;
			image.transform.localPosition = pos;

			new AnalyticsManager ().SendCounterEvent ("movieRewardOffer", 1);

			break;
		}	
	}

	#region movie reward
	public bool isRewardMovieWatched = false;
	public bool checkIsMovieRewardAvailable() {
		Debug.Log ("reached");
		Debug.Log ("playCount:"+_resultCtrl._gameCtrl._userData.playCount);
		// Play回数が3回に1回、出す
		if (_resultCtrl._gameCtrl._userData.playCount % Const.AD_INTERVAL_REWARD_MOVIE == 0 &&
			!isRewardMovieWatched)
		{
			Debug.Log ("reached");
			return true;
		}
		return false;
	}

	public void playMovieReward() {
		Debug.Log ("reached");
		AdvertisementManager adMng = _resultCtrl._gameCtrl.gameObject.GetComponent<AdvertisementManager> ();
		adMng.ShowRewardedAd (this.gameObject);

		new AnalyticsManager ().SendCounterEvent ("movieRewardPlayed", 1);
	}

	public void movieCallBack(string state) { // 0:finished, 1:skipped, 2:failed
		if (state == "1") {
			int addCoin = getGiftCoinValue ();
			_resultCtrl.giveRewardCoin (addCoin);
			isRewardMovieWatched = true;
			statusCheck ();

			new AnalyticsManager ().SendCounterEvent ("movieRewardFinished", 1);
		} else if (state == "2") {
			new AnalyticsManager ().SendCounterEvent ("movieRewardSkipped", 1);
		} else if (state == "3") {
			new AnalyticsManager ().SendCounterEvent ("movieRewardFailed", 1);
		}
	}
	#endregion
}