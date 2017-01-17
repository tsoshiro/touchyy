using UnityEngine;
using System.Collections;
using System;

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
	Vector3 BEST_ICON_POS = new Vector3 (0, -0.5f, 0);

	public GameObject SHOP_BADGE;

	GiftCtrl _giftCtrl;
	ReviewRequestCtrl _reviewCtrl;

	void Start() {
		_reviewCtrl = this.GetComponent<ReviewRequestCtrl> ();
		_giftCtrl = this.GetComponent<GiftCtrl> ();
		_giftCtrl.Init ();
	}
			
	public void showResult (GameResult pResult) {
		GameResult _result = pResult;
		new AnalyticsManager ().SendGameResult (pResult);

		UserData _userData = _gameCtrl._userData;
		cleanBestIcons ();

		// スコアラベルには一時的に、NO MISS BONUS加算前の値を入れる
		// _result.scoreの値はそのままで、見栄えだけ元に戻す
		Debug.Log ("_result.score: " + _result.score + "\n_result.noMissBonusValue: "+_result.noMissBonusValue
		          +"LBL_SCORE.text:" + new IntValueConverter ().FixBigInteger (_result.score - _result.noMissBonusValue));
		LBL_SCORE.text = new IntValueConverter ().FixBigInteger (_result.score - _result.noMissBonusValue);

		// スコアのBESTチェック
		if (checkBestRecord (_userData.bestScore, _result.score, LBL_SCORE)) {
			_userData.bestScore = _result.score;

			// Leaderboard
			GPGSManager.ReportScore (Const.LB_HIGH_SCORE_SCORE, 
			                               PBClass.BigInteger.ToInt64(_result.score));
		};

		// コンボのBESTチェック
		LBL_MAX_COMBO.text = ""+_result.maxCombo;
		if (checkBestRecord (_userData.maxCombo, _result.maxCombo, LBL_MAX_COMBO)) {
			_userData.maxCombo = _result.maxCombo;

			// Leaderboard
			GPGSManager.ReportScore (Const.LB_HIGH_SCORE_MAX_COMBO,
			                               (long)_result.maxCombo);
		}

		// deleteCountのBESTチェック
		LBL_CUBES.text = ""+_result.deleteCount;
		if (checkBestRecord(_userData.maxDeleteCount, _result.deleteCount, LBL_CUBES)) {
			_userData.maxDeleteCount = _result.deleteCount;

			// Leaderboard
			GPGSManager.ReportScore (Const.LB_HIGH_SCORE_COUNT,
			                              (long)_result.deleteCount);
		}

		// killAllCountのBESTチェック
		LBL_KILL_ALL.text = "" + _result.killAllCount;
		if (checkBestRecord(_userData.maxKillAllCount, _result.killAllCount, LBL_KILL_ALL)) {
			_userData.maxKillAllCount = _result.killAllCount;

			// Leaderboard
			GPGSManager.ReportScore (Const.LB_HIGH_SCORE_KILL_ALL,
			                              (long)_result.killAllCount);
		}
			
		// NO MISSなら「NO MISS」ラベルを付ける
		if (_result.missCount == 0) {
			Debug.Log ("NO MISS");
			float noMissRate = 1 / (float)Const.NO_MISS_BONUS_RATE;
			LBL_MISS.text = "NO MISS\n+"+noMissRate.ToString("P0");
		} else {
			LBL_MISS.text = "" + _result.missCount;
		}

		// TOTAL VALUE
		_userData.totalScore += _result.score;
		_userData.totalDeleteCount += _result.deleteCount;
		_userData.totalKillAllCount += _result.killAllCount;
		_userData.playCount++;

		// TotalCountをLeaderboardに送信
		GPGSManager.ReportScore (Const.LB_TOTAL_COUNT,
		                               (long)_userData.totalDeleteCount);

		// Send Achievement
		sendAchievement (_userData.playCount);

		// GET COIN
		PBClass.BigInteger coinNow = _userData.coin;
		PBClass.BigInteger coinAdded = coinNow + _result.score;

		// RESULT MOTION FLOW
		StartCoroutine (resultAnimation(_result, coinNow));

		_userData.coin = coinAdded;

		// SAVE
		_userData.save ();

		// SHOP
		_gameCtrl._shopCtrl.initUserItems ();
		// GIFT REWARD CHECK
		_giftCtrl.isRewardMovieWatched = false;
		_giftCtrl.statusCheck ();
		checkShop ();
	}


	void sendAchievement (int pScore)
	{
		GameResultAchievement [] achievements = {
			new GameResultAchievement(Const.AC_PLAY_10_TIMES, 10),
			new GameResultAchievement(Const.AC_PLAY_20_TIMES, 20),
			new GameResultAchievement(Const.AC_PLAY_50_TIMES, 50),
			new GameResultAchievement(Const.AC_PLAY_100_TIMES, 100),
			new GameResultAchievement(Const.AC_PLAY_200_TIMES, 200)
		};

		for (int i = 0; i < achievements.Length; i++) {
			achievements [i].sendProgress (pScore);
		}
	}

	// ショップで購入できるものがあれば「!」マークをショップボタン上につける
	public void checkShop () {
		if (_gameCtrl._shopCtrl.checkIsAffordSomething ()) {
			SHOP_BADGE.SetActive (true);
		} else {
			SHOP_BADGE.SetActive (false);
		}
	}

	// Resultアニメーション演出まとめ
	IEnumerator resultAnimation (GameResult pResult, PBClass.BigInteger pCoinNow) {
		Debug.Log ("resultAnimation Start");

		if (pResult.missCount == 0) {
			// NO MISS BONUS 演出
			yield return StartCoroutine (NoMissBonusMotion (LBL_MISS, LBL_SCORE, pResult.score));

			// 待機
			yield return new WaitForSeconds(0.5f);
		}

		// COIN ADD 演出
		yield return StartCoroutine(coinAddMotion(pCoinNow, pResult.score, LBL_SCORE.gameObject, LBL_COIN.gameObject, true));

		yield return 0;
	}

	// NO MISS BONUS + SCOREカウントアップ演出
	IEnumerator NoMissBonusMotion (TextMesh pBonusLabel,
	                               TextMesh pScoreText, 
	                               PBClass.BigInteger pScore)
	{
		Debug.Log ("NoMissBonusMotion start");

		yield return StartCoroutine (addValueMotion (pBonusLabel, pScoreText, pScore));
	}

	// シンプルに、
	// Fromのオブジェクトを複製・色付けして、
	// Toのオブジェクトまで持っていき、
	// Toのオブジェクトを複製・色付けして拡大し、最終的に複製されたオブジェクトは破棄される、
	// 一連の演出を作る
	IEnumerator addValueMotion (TextMesh pFromPositionObj,
	                                    TextMesh pToPositionObj,
	                                    PBClass.BigInteger pToValue) 
	{
		// Get position data from FROM OBJ and TO OBJ
		Vector3 pFromPosition = pFromPositionObj.transform.position;
		Vector3 pToPosition = pToPositionObj.transform.position;

		float zPadding = -1f;
		pFromPosition.z = pFromPosition.z + zPadding;
		pToPosition.z = pToPosition.z + zPadding;

		// ターゲットの文字色を薄くしておく
		ColorEditor.setColorFromColorCode (pToPositionObj.gameObject, Const.COLOR_CODE_LIGHTER_BASE);


		// 演出用文字オブジェクトを生成し、pToPositionObjまで動かす
		GameObject cp = Instantiate (pFromPositionObj.gameObject,
									 pFromPosition,
									 pFromPositionObj.transform.rotation) as GameObject;
		cp.name = "ObjMoving";

		iTween.MoveTo (cp, iTween.Hash ("position", pToPosition,
										"time", 1.0f,
										"easetype", iTween.EaseType.easeInOutQuart,
										"islocal", true)
					  );


		// Wait until cp moves to the target position
		yield return new WaitForSeconds (1.0f);

		// 演出文字用オブジェクトが拡大して消える
		ColorEditor.setColorFromColorCode (pToPositionObj.gameObject, Const.COLOR_CODE_LIGHTER_BASE);
		iTween.ScaleTo (cp, iTween.Hash ("scale", cp.transform.localScale * 1.5f,
										 "time", 0.2f)
					   );
		iTween.FadeTo (cp, iTween.Hash ("a", 0, "time", 0.2f));

		// ターゲットの文字色を戻し、文字列を書き換える
		ColorEditor.setColorFromColorCode (pToPositionObj.gameObject, Const.COLOR_CODE_BASE_COLOR);
		pToPositionObj.text = new IntValueConverter ().FixBigInteger (pToValue);

		// ターゲットの文字を一瞬拡大し、戻す
		iTween.ScaleTo (pToPositionObj.gameObject, iTween.Hash ("scale", pToPositionObj.transform.localScale * 2f,
										 "time", 0.1f));
		yield return new WaitForSeconds (0.1f);
		iTween.ScaleTo (pToPositionObj.gameObject, iTween.Hash ("scale", pToPositionObj.transform.localScale / 2f,
										 "time", 0.1f));
		yield return new WaitForSeconds (0.5f);

		Destroy (cp);
	}

	// COIN付与演出
	IEnumerator coinAddMotion (PBClass.BigInteger pCoinNow,
	                           PBClass.BigInteger pAddCoinValue,
	                           GameObject pFromPositionObj,
	                           GameObject pToPositionObj,
	                           bool pIsCallback = true) 
	{
		Debug.Log ("CoinAddMotion starts");

		CoinValueChange (pCoinNow); // Change COIN Label's value to NOW COIN VALUE

		// Get position data from FROM OBJ (=SCORE OR BUTTON) and TO OBJ (COIN)
		Vector3 pFromPosition = pFromPositionObj.transform.position;
		Vector3 pToPosition = pToPositionObj.transform.position;

		float zPadding = -1f;
		pFromPosition.z = pFromPosition.z + zPadding;
		pToPosition.z = pToPosition.z + zPadding;

		// ターゲットの文字色を薄くしておく
		ColorEditor.setColorFromColorCode (pToPositionObj, Const.COLOR_CODE_LIGHTER_BASE);


		// 演出用文字オブジェクトを生成し、pToPositionObjまで動かす
		GameObject cp = Instantiate (LBL_SCORE.gameObject,
		                             pFromPosition,
									 LBL_SCORE.transform.rotation) as GameObject;
		cp.name = "ScoreMoving";
		// 演出用文字オブジェクトの数値を修正
		cp.GetComponent<TextMesh>().text = "+"+pAddCoinValue;


		// BESTアイコンが表示されている場合は、消しておく
		if (cp.transform.childCount > 0) {
			cp.transform.FindChild (BEST_ICON_NAME).gameObject.SetActive (false);	
		}
		iTween.MoveTo (cp, iTween.Hash ("position", pToPosition, 
		                                "time", 1.0f, 
		                                "easetype", iTween.EaseType.easeInOutQuart, 
		                                "islocal", true)
		              );


		// Wait until cp moves to the target position
		yield return new WaitForSeconds (1.0f);

		// 演出文字用オブジェクトが拡大して消える
		ColorEditor.setColorFromColorCode (pToPositionObj, Const.COLOR_CODE_LIGHTER_BASE);
		iTween.ScaleTo (cp, iTween.Hash ("scale", cp.transform.localScale * 1.5f,
		                                 "time", 0.2f)
		               );
		iTween.FadeTo (cp, iTween.Hash ("a", 0, "time", 0.2f));

		// ターゲットの文字を戻し、数値を書き換える
		ColorEditor.setColorFromColorCode (pToPositionObj, Const.COLOR_CODE_BASE_COLOR);
		CoinValueChange (pCoinNow+pAddCoinValue);

		// ターゲットの文字を一瞬拡大し、戻す
		iTween.ScaleTo (pToPositionObj, iTween.Hash ("scale", pToPositionObj.transform.localScale * 2f,
		                                 "time", 0.1f));
		yield return new WaitForSeconds (0.1f);
		iTween.ScaleTo (pToPositionObj, iTween.Hash ("scale", pToPositionObj.transform.localScale / 2f,
		                                 "time", 0.1f));
		yield return new WaitForSeconds (0.5f);

		Destroy (cp);
		if (pIsCallback) {
			resultMotionCallback ();
		}
		_gameCtrl.finishResultAnimation ();
	}

	// ベストレコードかチェック
	// trueならBESTテキスト表示+結果テキストの色変更
	// falseなら結果テキストの色を元の色にする
	bool checkBestRecord(PBClass.BigInteger pBestScore, PBClass.BigInteger pScore, TextMesh pScoreTextMesh) {
		if (pScore > pBestScore) {
			showBestScoreLabel (pScoreTextMesh);
			return true;
		} else {
			resetLabelColor (pScoreTextMesh);
			return false;
		}
	}

	void resetLabelColor (TextMesh pScoreTextMesh) {
		ColorEditor.setColorFromColorCode (pScoreTextMesh.gameObject, Const.COLOR_CODE_BASE_COLOR);
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
		ColorEditor.setColorFromColorCode (pScoreTextMesh.gameObject, Const.COLOR_CODE_PINK);
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
		LBL_COIN.text = new IntValueConverter().FixBigInteger(pValue);
	}

	public void SetCoinValue () {
		LBL_COIN.text = new IntValueConverter ().FixBigInteger (_gameCtrl._userData.coin);
	}

	void resultMotionCallback () {
		// REVIEW CHECK
		// x回に一回レビューリクエストポップアップ表示
		// レビュー依頼が無い時にインタースティシャル表示のチェック
		if (!_reviewCtrl.ReviewRequest ()) {
			// INTERSTITIAL CHECK
			_gameCtrl.gameObject.GetComponent<AdvertisementCtrl>().checkInterstitial ();
		}
	}

	void giveFreeCoin(PBClass.BigInteger pCoin) {
		PBClass.BigInteger coinNow = _gameCtrl._userData.coin;

		StartCoroutine (coinAddMotion (coinNow, pCoin, GameObject.Find ("GiftBtn"), LBL_COIN.gameObject, false));

		_gameCtrl._userData.coin += pCoin;
		_gameCtrl._userData.nextFreeGift = _giftCtrl.nextTimeFreeGift.ToString ("yyyy/MM/dd HH:mm:ss");

		// SAVE
		_gameCtrl._userData.save ();
	}

	public void giveRewardCoin(PBClass.BigInteger pCoin) {
		PBClass.BigInteger coinNow = _gameCtrl._userData.coin;

		StartCoroutine(coinAddMotion (coinNow, pCoin, GameObject.Find ("GiftBtn"), LBL_COIN.gameObject, false));

		_gameCtrl._userData.coin += pCoin;

		// SAVE
		_gameCtrl._userData.save ();
	}

	void sendAchievement () {
		
	}

	#region action
	void actionReplayBtn ()
	{
		if (!_gameCtrl.canGoNext) {
			return;
		}
		_gameCtrl._audioMgr.play (Const.SE_BUTTON);
		_gameCtrl.replay ();
	}

	void actionShopBtn () {
		if (!_gameCtrl.canGoNext) {
			return;
		}
		_gameCtrl._audioMgr.play (Const.SE_BUTTON);
		_gameCtrl.OpenShop ();
	}

	void actionSoundBtn(GameObject pObj) {
		if (!_gameCtrl.canGoNext) {
			return;
		}
		bool flg = _gameCtrl._audioMgr.isMute;	
		_gameCtrl._audioMgr.isMute = !flg;
		_gameCtrl._audioMgr.muteBgm (!flg);

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
		_gameCtrl._audioMgr.play (Const.SE_BUTTON);
		if (_giftCtrl.giveGiftFree ()) {
			PBClass.BigInteger addValue =_giftCtrl.getGiftCoinValue();
			giveFreeCoin (addValue);
		}
	}

	void actionShareBtn () {
		if (!_gameCtrl.canGoNext) {
			return;
		}
		_gameCtrl._audioMgr.play (Const.SE_BUTTON);
		ShareCtrl shareCtrl = this.gameObject.GetComponent<ShareCtrl> ();
		shareCtrl.shareResult ();
	}

	void actionLeaderboardBtn () {
		_gameCtrl._audioMgr.play (Const.SE_BUTTON);
		GPGSManager.ShowLeaderboardUI ();
	}
	#endregion
}