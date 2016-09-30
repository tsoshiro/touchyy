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

	string NEW_ICON = "[NEW]";

	public void showResult (Result pResult) {
		Result _result = pResult;
		UserData _userData = _gameCtrl._userData;

		LBL_SCORE.text = ""+_result.score;
		if (_userData.checkIfIsNewRecord (Const.PREF_BEST_SCORE, _result.score)) {
			LBL_SCORE.text = NEW_ICON + _result.score;
		}

		LBL_MAX_COMBO.text = ""+_result.maxCombo;
		if (_userData.checkIfIsNewRecord (Const.PREF_MAX_COMBO, _result.maxCombo)) {
			LBL_MAX_COMBO.text = NEW_ICON + _result.maxCombo;
		}

		LBL_CUBES.text = ""+_result.deleteCount;
		if (_userData.checkIfIsNewRecord (Const.PREF_MAX_DELETE_COUNT, _result.deleteCount)) {
			LBL_CUBES.text = NEW_ICON + _result.deleteCount;
		}

		LBL_KILL_ALL.text = "" + _result.killAllCount;
		if (_userData.checkIfIsNewRecord (Const.PREF_MAX_KILL_ALL_COUNT, _result.killAllCount)) {
			LBL_KILL_ALL.text = NEW_ICON + _result.killAllCount;
		}

		if (_result.missCount > 0) {
			LBL_MISS.text = "" + _result.missCount;
		} else {
			LBL_MISS.text = "NO MISS!!";
		}

		// TOTAL VALUE
		_userData.addTotalRecords (Const.PREF_TOTAL_SCORE, _result.score);
		_userData.addTotalRecords (Const.PREF_TOTAL_DELETE_COUNT, _result.deleteCount);
		_userData.addTotalRecords (Const.PREF_TOTAL_KILL_ALL_COUNT, _result.killAllCount);
		_userData.addTotalRecords (Const.PREF_PLAY_COUNT, 1);

		// GET COIN
		int coinNow = _userData.getUserDataInt (Const.PREF_COIN);
		int coinAdded = coinNow + _result.score;


		iTween.ValueTo (gameObject, iTween.Hash ("from", coinNow, "to", coinAdded, "time", 1.0f, "onupdate", "CoinValueChange", "oncomplete", "callback"));

		_userData.setUserData (Const.PREF_COIN, coinAdded);

		// SAVE
		_userData.saveUserData ();
	}

	void CoinValueChange (int pValue) {
		LBL_COIN.text = "" + pValue;
	}

	public void SetCoinValue () {
		LBL_COIN.text = ""+_gameCtrl._userParam.coin;
	}

	void callback () {
		_gameCtrl.finishResultAnimation ();
		_gameCtrl._shopCtrl.initUserItems ();
	}

	void actionReplayBtn ()
	{
		this.gameObject.SetActive (false);
		_gameCtrl.SetGame ();
	}

	void actionShopBtn () {
		this.gameObject.SetActive (false);
		_gameCtrl.OpenShop ();
	}

}
