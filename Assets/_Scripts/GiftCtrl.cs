using UnityEngine;
using System.Collections;
using System;

public class GiftCtrl : MonoBehaviour {
	public ResultCtrl _resultCtrl;
	public bool isFreeGiftAvailable;
	public GameObject _giftBtn;
	GameObject image;
	TextMesh leftTimeText;
	float xPos = -1.5f;

	public DateTime nextTimeFreeGift;

	public DateTime intervalDateTime;

	public void Init() {
		_resultCtrl = this.GetComponent<ResultCtrl>();

		image = _giftBtn.transform.FindChild ("image").gameObject;
		leftTimeText = _giftBtn.transform.FindChild ("leftTime").gameObject.GetComponent<TextMesh>();
		nextTimeFreeGift = DateTime.Parse(_resultCtrl._gameCtrl._userData.nextFreeGift);

		statusCheck ();
	}

	void statusCheck() {
		setFreeGiftFlg ();

		if (isFreeGiftAvailable) {

		} else {
			ColorEditor.setFade (_giftBtn, 0.8f);
			Vector3 pos = image.transform.localPosition;
			pos.x = xPos;
			image.transform.localPosition = pos;
			leftTimeText.gameObject.SetActive (true);
		}
	}

	void setFreeGiftFlg() {
		if (nextTimeFreeGift - DateTime.Now <= TimeSpan.Zero) {
			isFreeGiftAvailable = true;
		} else {
			isFreeGiftAvailable = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isFreeGiftAvailable) {
			TimeSpan leftTime = nextTimeFreeGift - DateTime.Now;
			leftTimeText.text = String.Format ("{0:00}:{1:00}:{2:00}",
				leftTime.Hours,
				leftTime.Minutes,
				leftTime.Seconds);
			
			if (leftTime <= TimeSpan.Zero) {
				leftTime = TimeSpan.Zero;
				isFreeGiftAvailable = true;
				ColorEditor.setFade (_giftBtn, 1.0f);
				leftTimeText.gameObject.SetActive (false);
				Vector3 pos = image.transform.localPosition;
				pos.x = 0f;
				image.transform.localPosition = pos;
			}
		}
	}

	public bool giveGift() {
		if (isFreeGiftAvailable) {
			isFreeGiftAvailable = false;
			nextTimeFreeGift = DateTime.Now.AddHours(1);
			statusCheck ();
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
}
