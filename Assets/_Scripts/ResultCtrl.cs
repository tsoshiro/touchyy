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
	GiftCtrl _giftCtrl;

	void Start() {
		_giftCtrl = this.GetComponent<GiftCtrl> ();
		_giftCtrl.Init ();
	}
		
	public void showResult (Result pResult) {
		Result _result = pResult;
		UserData _userData = _gameCtrl._userData;

		LBL_SCORE.text = ""+_result.score;
		if (checkBestRecord (_userData.bestScore, _result.score,LBL_SCORE)) {
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

		LBL_MISS.text = "" + _result.missCount;
		if (_result.missCount == 0) {
			LBL_MISS.text += "\nPERFECT!";
		}

		// TOTAL VALUE
		_userData.totalScore += _result.score;
		_userData.totalDeleteCount += _result.deleteCount;
		_userData.totalKillAllCount += _result.killAllCount;
		_userData.playCount++;

		// GET COIN
		int coinNow = _userData.coin;
		int coinAdded = coinNow + _result.score;

		iTween.ValueTo (gameObject, iTween.Hash ("from", coinNow, "to", coinAdded, "time", 1.0f, "onupdate", "CoinValueChange", "oncomplete", "callback"));
		_userData.coin = coinAdded;

		// SAVE
		_userData.save ();
	}

	bool checkBestRecord(int pBestScore, int pScore, TextMesh pScoreTextMesh) {
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
		} else {
			pScoreTextMesh.transform.Find (BEST_ICON_NAME).gameObject.SetActive (true);
		}
	}

	void checkBestRecord(UserData pUserData, string pKey, int pScore, TextMesh pScoreTextMesh) {
		UserData _userData = pUserData;
		if (_userData.checkIfIsNewRecord (pKey, pScore)) {
//		if (true) {
			if (!pScoreTextMesh.transform.FindChild (BEST_ICON_NAME)) {
				GameObject go = Instantiate (BEST_ICON,
					                BEST_ICON.transform.position,
					                BEST_ICON.transform.rotation) as GameObject;
				go.transform.parent = pScoreTextMesh.transform;
				go.transform.localPosition = BEST_ICON_POS;
				go.name = BEST_ICON_NAME;		
			} else {
				pScoreTextMesh.transform.Find (BEST_ICON_NAME).gameObject.SetActive (true);
			}
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

	void CoinValueChange (int pValue) {
		LBL_COIN.text = "" + pValue;
	}

	public void SetCoinValue () {
		LBL_COIN.text = ""+_gameCtrl._userData.coin;
	}

	void callback () {
		_gameCtrl.finishResultAnimation ();
		_gameCtrl._shopCtrl.initUserItems ();
	}

	#region action
	void actionReplayBtn ()
	{
		if (!_gameCtrl.canGoNext) {
			return;
		}
		this.gameObject.SetActive (false);
		_gameCtrl.SetGame ();
	}

	void actionShopBtn () {
		if (!_gameCtrl.canGoNext) {
			return;
		}
		this.gameObject.SetActive (false);
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
		if (_giftCtrl.giveGift ()) {
			int addValue =_giftCtrl.getGiftCoinValue();
			Debug.Log ("addValue: "+addValue);
			int coinNow = _gameCtrl._userData.coin;
			iTween.ValueTo (gameObject, iTween.Hash ("from", coinNow, "to", coinNow + addValue, "time", 1.0f, "onupdate", "CoinValueChange"));
			_gameCtrl._userData.coin += addValue;	
			_gameCtrl._userData.nextFreeGift = _giftCtrl.nextTimeFreeGift.ToString ("yyyy/MM/dd HH:mm:ss");

			// SAVE
			_gameCtrl._userData.save ();
		}
	}



	#endregion
}
