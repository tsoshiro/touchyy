using UnityEngine;
using System.Collections;

public class ResultCtrl : MonoBehaviour {
	public GameCtrl _gameCtrl;

	public TextMesh LBL_SCORE;
	public TextMesh LBL_MAX_COMBO;
	public TextMesh LBL_CUBES;
	public TextMesh LBL_KILL_ALL;
	public TextMesh LBL_MISS;

	public TextMesh LBL_COIN;

	public GameObject BEST_ICON;
	string BEST_ICON_NAME = "BEST_ICON";
	Vector3 BEST_ICON_POS = new Vector3 (0, -0.4f, 0);

	public GameObject SHOP_BADGE;

	GiftCtrl _giftCtrl;

	void Start() {
		_giftCtrl = this.GetComponent<GiftCtrl> ();
		_giftCtrl.Init ();
	}
		
	public void showResult (Result pResult) {
		Result _result = pResult;
		new AnalyticsManager ().SendGameResult (pResult);

		UserData _userData = _gameCtrl._userData;
		cleanBestIcons ();

		LBL_SCORE.text = ""+_result.score;
		if (checkBestRecord (_userData.bestScore, _result.score, LBL_SCORE)) {
			_userData.bestScore = _result.score;		
		};

		LBL_MAX_COMBO.text = ""+_result.maxCombo;
		if (checkBestRecord (_userData.maxCombo, _result.maxCombo, LBL_MAX_COMBO)) {
			_userData.maxCombo = _result.maxCombo;
		}

		LBL_CUBES.text = ""+_result.deleteCount;
		if (checkBestRecord(_userData.maxDeleteCount, _result.deleteCount, LBL_CUBES)) {
			_userData.maxDeleteCount = _result.deleteCount;	
		}

		LBL_KILL_ALL.text = "" + _result.killAllCount;
		if (checkBestRecord(_userData.maxKillAllCount, _result.killAllCount, LBL_KILL_ALL)) {
			_userData.maxKillAllCount = _result.killAllCount;	
		}


		if (_result.missCount == 0) {
			LBL_MISS.text = "NO MISS";
		} else {
			LBL_MISS.text = "" + _result.missCount;
		}

		// TOTAL VALUE
		_userData.totalScore += _result.score;
		_userData.totalDeleteCount += _result.deleteCount;
		_userData.totalKillAllCount += _result.killAllCount;
		_userData.playCount++;

		// GET COIN
		PBClass.BigInteger coinNow = _userData.coin;
		PBClass.BigInteger coinAdded = coinNow + _result.score;

		//iTween.ValueTo (gameObject,
		//                iTween.Hash (
		//	                "from", coinNow, 
		//	                "to", coinAdded, 
		//	                "time", 1.0f, 
		//	                "onupdate", "CoinValueChange", 
		//	                "oncomplete", "callback"
		//	               )
		//               );
		StartCoroutine (resultMove (coinNow, coinAdded));
		_userData.coin = coinAdded;

		// SAVE
		_userData.save ();

		// SHOP
		_gameCtrl._shopCtrl.initUserItems ();
		// GIFT REWARD CHECK
		_giftCtrl.statusCheck ();
		checkShop ();
	}

	// ショップで購入できるものがあれば「!」マークをショップボタン上につける
	public void checkShop () {
		if (_gameCtrl._shopCtrl.checkIsAffordSomething ()) {
			SHOP_BADGE.SetActive (true);
		} else {
			SHOP_BADGE.SetActive (false);
		}
	}

	// COIN付与演出
	IEnumerator resultMove (PBClass.BigInteger pFrom, PBClass.BigInteger pValue) {
		CoinValueChange (pFrom);
		GameObject cp = Instantiate (LBL_SCORE.gameObject,
							 LBL_SCORE.transform.position,
							 LBL_SCORE.transform.rotation) as GameObject;
		cp.name = "ScoreMoving";

		// BESTアイコンが表示されている場合は、消してておく
		if (cp.transform.childCount > 0) {
			cp.transform.FindChild (BEST_ICON_NAME).gameObject.SetActive (false);	
		}

		iTween.MoveTo (cp, iTween.Hash ("position", LBL_COIN.transform.position, "time", 1.0f, "islocal", true));
		yield return new WaitForSeconds (1.0f);
		iTween.ScaleTo (cp, iTween.Hash ("scale", cp.transform.localScale * 1.5f, "time", 0.2f));
		iTween.FadeTo (cp, iTween.Hash ("a", 0, "time", 0.2f));
		CoinValueChange (pValue);
		yield return new WaitForSeconds (0.5f);
		callback ();
		Destroy (cp);
	}

	// インタースティシャルを表示するかどうか確認し、表示
	void checkInterstitial() {
		if (_gameCtrl._userData.playCount % Const.AD_INTERVAL_INTER == 0) {
			_gameCtrl.gameObject.GetComponent<AdvertisementManager> ().showInterstitial ();
		}
	}

	bool checkBestRecord(PBClass.BigInteger pBestScore, PBClass.BigInteger pScore, TextMesh pScoreTextMesh) {
		if (pScore > pBestScore) {
			showBestScoreLabel (pScoreTextMesh);
			return true;
		}
		return false;
	}

	void showBestScoreLabel(TextMesh pScoreTextMesh){
		if (!pScoreTextMesh.transform.FindChild (BEST_ICON_NAME)) {
			GameObject go = Instantiate (BEST_ICON,
				BEST_ICON.transform.position,
				BEST_ICON.transform.rotation) as GameObject;
			go.transform.parent = pScoreTextMesh.transform;
			go.transform.localPosition = BEST_ICON_POS;
			go.name = BEST_ICON_NAME;
			ColorEditor.setColorFromColorCode (go, Const.COLOR_CODE_PINK);
		} else {
			pScoreTextMesh.transform.Find (BEST_ICON_NAME).gameObject.SetActive (true);
		}
	}
		
	void cleanBestIcon(GameObject pScoreTextObj) {
		if (pScoreTextObj.transform.FindChild (BEST_ICON_NAME)) {
			pScoreTextObj.transform.Find (BEST_ICON_NAME).gameObject.SetActive (false);
		}
	}

	void cleanBestIcons() {
		TextMesh[] values = {
			LBL_SCORE,
			LBL_CUBES,
			LBL_MAX_COMBO,
			LBL_KILL_ALL
		};
		for (int i = 0; i < values.Length; i++) {
			cleanBestIcon (values [i].gameObject);
		}
	}

	void CoinValueChange (PBClass.BigInteger pValue) {
		LBL_COIN.text = "" + pValue;
	}

	// todo 時間別で
	//void CoinValueChange (PBClass.BigInteger pValueFrom, PBClass.BigInteger pValueTo, float pTime) {
	//	StartCoroutine (coinValueChangeCoroutine (pValueTo, pValueTo, pTime);
		
	//}

	//IEnumerator coinValueChangeCoroutine (PBClass.BigInteger pValueFrom, PBClass.BigInteger pValueTo, float pTime) {
	//	// 時間を掛けて算出
	//	PBClass.BigInteger frameRate = new PBClass.BigInteger (Application.targetFrameRate);
	//	PBClass.BigInteger unit = (pValueFrom - pValueTo) / frameRate;

	//	for (int i = 0; i < PBClass.BigInteger.ToInt32 (frameRate); i++) {
	//		PBClass.BigInteger value = pValueFrom + (new PBClass.BigInteger (i) * frameRate);

	//		yield return 0;
	//	}
	//}

	public void SetCoinValue () {
		LBL_COIN.text = ""+_gameCtrl._userData.coin;
	}

	void callback () {
		// INTERSTITIAL CHECK
		checkInterstitial ();

		_gameCtrl.finishResultAnimation ();
	}

	void giveFreeCoin(PBClass.BigInteger pCoin) {
		PBClass.BigInteger coinNow = _gameCtrl._userData.coin;

		// todo Animation
		//iTween.ValueTo (gameObject, iTween.Hash ("from", coinNow, "to", coinNow + pCoin, "time", 1.0f, "onupdate", "CoinValueChange"));

		_gameCtrl._userData.coin += pCoin;
		_gameCtrl._userData.nextFreeGift = _giftCtrl.nextTimeFreeGift.ToString ("yyyy/MM/dd HH:mm:ss");

		// SAVE
		_gameCtrl._userData.save ();
	}

	public void giveRewardCoin(PBClass.BigInteger pCoin) {
		PBClass.BigInteger coinNow = _gameCtrl._userData.coin;

		//iTween.ValueTo (gameObject, iTween.Hash ("from", coinNow, "to", coinNow + pCoin, "time", 1.0f, "onupdate", "CoinValueChange"));

		_gameCtrl._userData.coin += pCoin;

		// SAVE
		_gameCtrl._userData.save ();
	}



	#region action
	void actionReplayBtn ()
	{
		if (!_gameCtrl.canGoNext) {
			return;
		}
		_gameCtrl.replay ();
	}

	void actionShopBtn () {
		if (!_gameCtrl.canGoNext) {
			return;
		}
		_gameCtrl.OpenShop ();
	}

	void actionSoundBtn(GameObject pObj) {
		if (!_gameCtrl.canGoNext) {
			return;
		}
		bool flg = _gameCtrl._audioMgr.isMute;	
		_gameCtrl._audioMgr.isMute = !flg;
		pObj.GetComponent<ButtonCtrl> ().setOnFlg (flg); // ミュートなし=ON=true
	}

	void actionGiftBtn() {
		if (!_gameCtrl.canGoNext) {
			return;	
		}
		if (_giftCtrl.checkIsReward()) {
			_giftCtrl.playMovieReward ();
			return;
		}
		if (_giftCtrl.giveGiftFree ()) {
			PBClass.BigInteger addValue =_giftCtrl.getGiftCoinValue();
			giveFreeCoin (addValue);
		}
	}
	#endregion
}