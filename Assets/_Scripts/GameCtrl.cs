﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameCtrl : SingletonMonoBehaviour<GameCtrl> {
	public float timeLeft;

	public GameResult _result;
	public UserData _userData;
	public UserParam _userParam;

	#region Controllers
	public GameObject _readyCtrl;
	public ResultCtrl _resultCtrl;
	public ShopCtrl _shopCtrl;
	public CubeAnimationManager _cubeAnimationManager;
	#endregion

	#region UI
	public TextMesh guideText;
	public TextMesh scoreText;
	public TextMesh comboText;
	public TextMesh timeText;
	public GameObject timeGauge;
	public GameObject colorTimeGauge;
	public TextMesh resultText;
	public GameObject cubeObj;
	public GameObject bombObj;
	public GameObject timeObj;
	public GameObject comboShowTextObj;
	public GameObject countDownTextObj;
	public GameObject touchableSign;

	public GameObject arrowguide;
	public GameObject pauseBtn;

	float timeGaugeBaseWidth;
	float colorTimeGaugeBaseWidth;
	float mistakeGaugeBaseWidth;

	Vector3 colorTimeGaugeBaseScaleSphere;

	public GameObject mistakeGauge;
	public GameObject mistakeText;

	public GameObject killAllText;
	#endregion

	#region timerflg
	int COUNT_DOWN_NUM = 5;
	int countDownNum;
	#endregion

	public float TIME = 30;
	public int HEIGHT = 5;
	public int WIDTH = 5;

	//AUDIO
	public AudioMgr _audioMgr;

	// LANGUAGE
	public LanguageCtrl _languageCtrl;


	public GameObject cubeGroup;
	GameObject targetCube;
	CubeCtrl targetCubeCtrl;

	public List<CubeCtrl> cubes = new List<CubeCtrl>();

	public Colors targetColor;
	public enum Colors {
		RED,
		BLUE,
		GREEN,
		YELLOW,
		PURPLE,
		NUM
	}
	public string[] colorCodes = {
		Const.COLOR_CODE_RED,
		Const.COLOR_CODE_BLUE,
		Const.COLOR_CODE_GREEN,
		Const.COLOR_CODE_YELLOW,
		Const.COLOR_CODE_PURPLE
	};

	public enum STATE {
		READY,
		PLAYING,
		PAUSE,
		RESULT,
		SHOP
	}

	public STATE state;
	float CHANGE_TIME = 3;
	float changeTimeLeft;

	string TARGET_CUBE = "TARGET_CUBE";

	public GameObject colorCubeObj;
	/// <summary>
	/// 色制限状態の生成可能色数上限
	/// </summary>
	int colorRestrictionCount = 2;
	float colorRestrictionTime = 5f;


	void Awake () {
		// MasterData取得
		this.GetComponent<UserMasterDataCtrl> ().initMasterData ();
		setLanguageTexts ();

		// UserData初期化
		_userData = new UserData ();
		_userData.initUserData ();
		_userData.debugDataSetUp ();

		// UserParam初期化
		_userParam = new UserParam (_userData, this.GetComponent<UserMasterDataCtrl>());
	}

	// Use this for initialization
	void Start () {
		setUpDisplays ();

		// SHOP初期化
		_shopCtrl.initUserItems ();

		// 演出用Cubeの初期化
		_cubeAnimationManager.init ();

		SetGame();

		// 数値部分算出テスト
		//DisplayTest ();
		DEBUG_FinishBtn.SetActive (isPlayDebug);
	}

	public void LeaderboardLogin () {
		// GameCenter/Google Play Serviceログイン
		GPGSManager.Auth ();
	}

	public void endSplashThenstartBgm () {
		_audioMgr.playBGM ();
	}

	void setLanguageTexts () {
		// Localization
		_languageCtrl = this.GetComponent<LanguageCtrl> ();
		_languageCtrl.initLocalization ();

		guideText.text = _languageCtrl.getMessageFromCode (Const.instruction + "_01");
	}

	// 表示系初期化
	void setUpDisplays() {
		timeGaugeBaseWidth = timeGauge.transform.localScale.x;
		colorTimeGaugeBaseWidth = colorTimeGauge.transform.localScale.x;
		mistakeGaugeBaseWidth = mistakeGauge.transform.localScale.x;
		countDownTextObj.SetActive (false);

		mistakeObj.SetActive (true);
		iTween.FadeTo(mistakeObj, iTween.Hash("a", 0f, "time", 0));

		pauseBtn.SetActive (false);

		// BLOCKER
		ColorEditor.setFade (touchableSign, 0.8f);
	}


	// TEST
	void DisplayTest () {
		IntValueConverter testClass = new IntValueConverter ();
		testClass.test ();
	}

	public void SetGame() {
		state = STATE.READY;

		initRate ();

		// Resultクラスの初期化
		_result = new GameResult ();

		// Display
		scoreText.text = "SCORE : " + _result.score;
		comboText.text = "";
		
		timeLeft = TIME;
		timeText.text = "TIME : "+timeLeft;
		setTimeGaugeRate ();

		resetColorRestriction ();

		_readyCtrl.SetActive (true);
		iTween.FadeTo (_readyCtrl, iTween.Hash("a", 1, "time", 0.5f));
	}

	void setTimeGaugeRate () {
		float gaugeX = timeGaugeBaseWidth * (timeLeft / TIME);
		Vector3 gaugeScale = new Vector3 (gaugeX,
										 timeGauge.transform.localScale.y,
										 timeGauge.transform.localScale.z);
		timeGauge.transform.localScale = gaugeScale;
	}

	void setColorTimeGaugeRate ()
	{
		float gaugeX = colorTimeGaugeBaseWidth * (changeTimeLeft / CHANGE_TIME);
		Vector3 gaugeScale = new Vector3(gaugeX,
		                                 colorTimeGauge.transform.localScale.y,
										 colorTimeGauge.transform.localScale.z);
		colorTimeGauge.transform.localScale = gaugeScale;

		Vector3 targetCubeScale = colorTimeGaugeBaseScaleSphere * (changeTimeLeft / CHANGE_TIME);
		targetCube.transform.localScale = targetCubeScale;
	}

	void setMistakeTimeGaugeRate ()
	{
		float gaugeX = mistakeGaugeBaseWidth * (mistakePenaltyTimeLeft / mistakePenaltyTime);
		Vector3 gaugeScale = new Vector3(gaugeX,
		                                 mistakeGauge.transform.localScale.y,
										 mistakeGauge.transform.localScale.z);
		mistakeGauge.transform.localScale = gaugeScale;
	}

	void StartGame() {
		if (cubeGroup.transform.childCount > 0) {
			for (int i = 0; i < cubeGroup.transform.childCount; i++) {
				Destroy (cubeGroup.transform.GetChild(i).gameObject);
			}
			cubes.Clear();
			Destroy (targetCube);
		}

		resultText.gameObject.SetActive(false);

		changeTimeLeft = CHANGE_TIME;
		setCubes();

		countDownNum = COUNT_DOWN_NUM;
		touchableSign.SetActive (false);
		arrowguide.SetActive (false);

		pauseBtn.SetActive (true);

		state = STATE.PLAYING;
	}

	void Update() {
#if UNITY_ANDROID
		// パックポタンが押された時の処理
		CheckBackButtonPressed ();
#endif

		if (state == STATE.READY) {
			updateReady();
		} else if (state == STATE.PAUSE) {
			updatePause ();
		} else if (state == STATE.PLAYING) {
			updatePlaying();
		} else if (state == STATE.RESULT && canGoNext) {
			updateResult ();
		}
	}

	bool isCountingDown = false;
	void updateReady() {
	}

	void updatePause () {
		
	}

	#region PAUSE
	void enablePause (bool iFlg) {
		if (iFlg) {
			state = STATE.PAUSE;
			// 表示系処理
			// Pauseオブジェクトを表示
			pauseDiplay.SetActive (true);
		} else {
			state = STATE.PLAYING;
		}
	}
	public GameObject pauseDiplay;

	#region actionBtn
	public void actionStartBtn() {
		if (!isCountingDown) {
			_audioMgr.play (Const.SE_BUTTON);

			StartCoroutine (startCountDown (Const.COUNTDOWN_TIME));
			iTween.FadeTo (_readyCtrl, iTween.Hash ("a", 0, "time", 0.5f));
		}
	}

	public void actionPauseBtn () {
		if (state == STATE.PLAYING) {
			_audioMgr.play (Const.SE_BUTTON);
			enablePause (true);	
		}
	}

	void actionContinueBtn () {
		if (state == STATE.PAUSE) {
			if (!isCountingDown) {
				_audioMgr.play (Const.SE_BUTTON);
				// 表示系処理
				// Pauseオブジェクトを非表示
				pauseDiplay.SetActive (false);

				// カウントダウン開始
				StartCoroutine (startCountDown (Const.COUNTDOWN_TIME));
			}
		}
	}

	void actionRestartBtn () {
		if (state == STATE.PAUSE) {
			_audioMgr.play (Const.SE_BUTTON);

			// Analyticsに送る
			new AnalyticsManager ().SendCounterEvent ("playFromRestart", 1);

			// RestartCountを一回加算してセーブ
			_userData.restartCount++;
			_userData.save ();

			// インタースティシャルチェック
			this.GetComponent<AdvertisementCtrl> ().checkInterstitial ();

			// Pauseオブジェクトを非表示
			pauseDiplay.SetActive (false);

			replay ();
		}
	}

	void actionDEBUG_FinishBtn () {
		if (state == STATE.PLAYING) {
			finishGame ();
		}
	}

	#endregion

	IEnumerator startCountDown (int pSecond) {
		isCountingDown = true;
		float second = pSecond;

		for (int i = (int)second; i > 0; i--) {
			// Display Object
			displayCountDownNumber (i);
			yield return new WaitForSeconds (1.0f);
		}

		isCountingDown = false;
		if (state == STATE.PAUSE) { // PAUSE 
			enablePause (false);
		} else { // READY
			_readyCtrl.SetActive(false);
			StartGame ();
		}
	}

	void displayCountDownNumber (int pNum) {
		Vector3 pos = countDownTextObj.transform.position;
		float fadeValue = 0.4f;
		if (state != STATE.PLAYING) {
			pos.z = _readyCtrl.transform.position.z;
			fadeValue = 0.8f;
		}

		// SHOW TEXT
		GameObject textObj = Instantiate (countDownTextObj,
										  pos,
										  gameObject.transform.rotation) as GameObject;
		textObj.SetActive (true);
		textObj.GetComponent<TextMesh> ().text = "" + pNum;
		ColorEditor.setFade (textObj, fadeValue);
		textObj.GetComponent<TextCtrl> ().init (0.5f, 0.5f);
	}

	#endregion

	void updatePlaying() {
		if (mistakePenaltyFlg) {
			updateMistake ();
		}

		if (isColorRestrictionValid) {
			updateColorRestriction ();
		}
		
		timeLeft -= Time.deltaTime;
		timeText.text = "TIME :"+timeLeft.ToString("F2");
		if (timeLeft <= 0) {
			timeLeft = 0;
			timeText.text = "TIME :" + timeLeft.ToString ("F2");

			//GAME OVER
			StartCoroutine(StopGame());
		}
		setTimeGaugeRate ();
		countDown();

		changeTimeLeft -= Time.deltaTime;
		setColorTimeGaugeRate ();
		setColorTimeGaugeRate ();
		if (changeTimeLeft <= 0) {
			changeTargetColor();
		}
	}

	void countDown () {
		if (timeLeft <= countDownNum) {
			if (timeLeft <= 0) {
				return;
			}
			// SHOW TEXT
			displayCountDownNumber (countDownNum);
			// COUNT DOWN
			countDownNum--;
		}
	}

	void updateResult() {
	}

	void updateMistake () {
		mistakePenaltyTimeLeft -= Time.deltaTime;
		setMistakeTimeGaugeRate ();
		if (mistakePenaltyTimeLeft <= 0) {
			mistakePenaltyFlg = false;

			iTween.FadeTo(mistakeObj, iTween.Hash("a", 0f, "time", 0));
			touchableSign.SetActive (false);
		}
	}

	public bool canGoNext = false;

	IEnumerator StopGame() {

		state = STATE.RESULT;
		canGoNext = false;

		pauseBtn.SetActive (false);
		touchableSign.SetActive (true);

		addNoMissBonus ();

		showResult ();

		yield return 0;
	}

	// ================================================================================
	// Kill All が発生するごとに、その時点での獲得ポイントの2%を加算
	// ================================================================================
	PBClass.BigInteger addSingleKillAllBonus () {
		PBClass.BigInteger bonusValue = _result.score / Const.KILL_ALL_BONUS_RATE;
		bonusValue = bonusValue * _result.killAllCount;
		_result.score += bonusValue;
		return bonusValue;
	}

	void addNoMissBonus () {
		if (_result.missCount == 0) {
			PBClass.BigInteger bonusValue = _result.score / Const.NO_MISS_BONUS_RATE;
			_result.noMissBonusValue = bonusValue;
			_result.score += _result.noMissBonusValue;
		} else {
			_result.noMissBonusValue = 0;
		}
	}

	public void finishResultAnimation () {
		canGoNext = true;
	}

	Vector3 _screenBasePos = new Vector3(0,0,-20);

	void showResult () {
		iTween.MoveTo (_resultCtrl.gameObject, 
		               iTween.Hash(
			               "position", _screenBasePos,
			               "time", SHORT_ANIMATION_TIME,
			               "islocal", true,
			               "oncomplete", "showResult",
			               "oncompleteparams", _result
			              )
		              );
	}

	public void replay() {
		iTween.MoveTo (_resultCtrl.gameObject, 
			iTween.Hash(
				"position", new Vector3(-Const.GAME_SCREEN_POSITION_X, _screenBasePos.y, _screenBasePos.z), "time", SHORT_ANIMATION_TIME, "islocal", true
			)
		);
		iTween.FadeTo(mistakeObj, iTween.Hash("a", 0f, "time", 0));
		SetGame ();
	}

	public STATE lastState = STATE.READY;
	public void OpenShop () {
		lastState = state;
		_shopCtrl.gameObject.SetActive (true);
		_shopCtrl.initUserItems ();

		iTween.MoveTo (_shopCtrl.gameObject, 
			iTween.Hash (
				"position", _screenBasePos, "time", SHORT_ANIMATION_TIME, "islocal", true
			)
		);

		iTween.MoveTo (_resultCtrl.gameObject, 
			iTween.Hash(
				"position", new Vector3(-Const.GAME_SCREEN_POSITION_X, _screenBasePos.y, _screenBasePos.z), "time", SHORT_ANIMATION_TIME, "islocal", true
			)
		);

		state = STATE.SHOP;
	}

	public void backFromShop () {
		// ShopCtrlを移動
		iTween.MoveTo (_shopCtrl.gameObject,
			iTween.Hash (
				"position", new Vector3 (Const.GAME_SCREEN_POSITION_X, _screenBasePos.y, _screenBasePos.z), "time", SHORT_ANIMATION_TIME, "islocal", true
			)
		);

		// ショップからリザルト画面か、READY画面かで切り替え
		// 基本はSTATE.RESULTの方を通る想定
		if (lastState == STATE.READY) {
			state = STATE.READY;
		} else if (lastState == STATE.RESULT) {
			// Resultを開く
			state = STATE.RESULT;
			_resultCtrl.SetCoinValue();
			_resultCtrl.checkShop ();

			iTween.MoveTo (_resultCtrl.gameObject, 
				iTween.Hash(
					"position", _screenBasePos, "time", SHORT_ANIMATION_TIME, "islocal", true
				)
			);
		}
	}

	#region coin
	public void getCoin (int pScore) {
		// LOGIC
		_userData.coin += pScore;
		_userData.save ();
	}

	public void spendCoin (PBClass.BigInteger pCost) {
		_userData.coin -= pCost;
		_userData.save ();
	}

	public void reloadUserData () {
		_userData = new UserData ();
		_userData.initUserData ();
		_userParam.setUserParam (_userData);
	}

	#endregion

	float SHORT_ANIMATION_TIME = 0.1f;
	float ANIMATION_TIME = 0.2f;
	void changeTargetColor() {
		do {
			targetColor = (Colors)UnityEngine.Random.Range (0, (int)Colors.NUM);
		} while (!hasEnableCube(targetColor));

		changeTimeLeft = CHANGE_TIME;
		setColorTimeGaugeRate ();

		targetCubeCtrl.setColor((int)targetColor);
		iTween.ScaleFrom(targetCube, iTween.Hash(
			"scale", new Vector3(0,0,0),
			"time", ANIMATION_TIME,
			"easetype", iTween.EaseType.easeOutBounce)
		                 );
	}

	bool hasEnableCube(Colors pColor) {
		for (int i = 0; i < cubes.Count; i++) {
			if (cubes[i].getColor() == targetColor) {
				return true;
			}
		}
		return false;
	}

	bool isBomb (int pConstType) {
		if (pConstType >= 0 &&
			pConstType <= 5) {
			return true;
		}
		return false;
	}

	bool isTime (int pConstType) {
		if (pConstType == Const.TYPE_ADD_TYME) {
			return true;
		}
		return false;
	}

	bool isColorEffect (int pConstType) {
		if (pConstType == Const.TYPE_RENEWAL ||
		    pConstType == Const.TYPE_RESTRICT) {
			return true;
		}
		return false;
	}

	Const.BombType bombRate () {
		float rate = UnityEngine.Random.Range (0, 100);
		if (rate <= 33) {
			return Const.BombType.PLUS;
		} else if (rate <= 66) {
			return Const.BombType.MULTIPLE;
		} else {
			return Const.BombType.AROUND;
		}
	}

	Const.BombType getBombType (int pConstType) {
		if (pConstType == Const.TYPE_VERTICAL) {
			return Const.BombType.VERTICAL;
		} else if (pConstType == Const.TYPE_HORIZONTAL) {
			return Const.BombType.HORIZONTAL;
		} else if (pConstType == Const.TYPE_CROSS) {
			return Const.BombType.CROSS;
		} else if (pConstType == Const.TYPE_PLUS) {
			return Const.BombType.PLUS;
		} else if (pConstType == Const.TYPE_MULTIPLE) {
			return Const.BombType.MULTIPLE;
		} else { // (pConstType == Const.TYPE_AROUND)
			return Const.BombType.AROUND;
		}
	}

	// 新規ベリーの生成。
	// pFadingColorには、ベリーを消す場合、消える色が入る。
	// 特にない場合はColors.NUMを入れるとする。
	CubeCtrl createCube(Vector3 pPosition, Colors pFadingColor = Colors.NUM) { 
		GameObject obj;

		int cubeType = decideCubeType ();
		if (isBomb(cubeType)){
			obj = Instantiate(bombObj);
			obj.GetComponent<BombCtrl> ().setBombType (getBombType (cubeType));
		} else if (isTime(cubeType)) {
			obj = Instantiate (timeObj);
			obj.GetComponent<TimeCubeCtrl> ().setAddTime (0.5f);
		} else if (isColorEffect(cubeType)) {
			obj = Instantiate (colorCubeObj);
			ColorCubeCtrl ctrl = obj.GetComponent<ColorCubeCtrl> ();
			if (cubeType == Const.TYPE_RESTRICT) {
				ctrl.setRestrictParameters (colorRestrictionCount, colorRestrictionTime);
			}
		} else {
			obj = Instantiate(cubeObj);
		}
		obj.transform.position = pPosition;
		obj.transform.parent = cubeGroup.transform;
		obj.transform.name = TARGET_CUBE;

		CubeCtrl cubeCtrl = obj.GetComponent<CubeCtrl>();
		cubeCtrl.init();
		cubeCtrl.setGameCtrl(this);

		int aColor = decideColor (isColorRestrictionValid,
		                          restrictColors,
		                          pFadingColor);
		cubeCtrl.setColor(aColor);

		return cubeCtrl;
	}

	// 色設定メソッド
	public int _decideColor (bool pIsColorRestrictionValid, List<Colors> pRestrictColors, Colors pBeforeColor) {
		int aColor;
		do {
			aColor =
				(pIsColorRestrictionValid)
				? (int)restrictColors [UnityEngine.Random.Range (0, pRestrictColors.Count)]
				: UnityEngine.Random.Range (0, (int)Colors.NUM);
		} while (aColor == (int)pBeforeColor); // beforeColorと同じ色だったら、やり直す
		// Colors.NUMが入ってきた場合は、初回の生成時
		
		return aColor;
	}

	//
	public int decideColor (bool pIsColorRestrictionValid, List<Colors> pRestrictColors, Colors pBeforeColor)
	{
		int aColor;

		List<int> availableColors = new List<int> ();

		for (int i = 0; i < (int)Colors.NUM; i++) {
			if (i != (int)pBeforeColor) { // 前の色と同じでなければOK
				if (pIsColorRestrictionValid) { // 色制限状態の場合
					if (!pRestrictColors.Contains ((Colors)i)) { // 利用可能色でないならばスキップ
						continue;
					}
				}
				availableColors.Add (i);
			}
		}

		aColor = availableColors [UnityEngine.Random.Range (0, availableColors.Count)];

		return aColor;
	}
	
	void setCubes() {
		// NORMAL CUBES
		int cubeId = 1;
		for (int i = 0; i < WIDTH; i++) {
			for (int j = 0; j < HEIGHT; j++) {
				Vector3 position = new Vector3(i,j,0);
				cubes.Add(createCube(position));
				cubes[cubeId-1].setId(cubeId);
				cubes[cubeId-1].gameObject.name = "CUBE_"+cubeId.ToString("D2");
				cubeId++;
			}
		}

		// TARGET CUBE
		do {
			targetColor = (Colors)UnityEngine.Random.Range (0, (int)Colors.NUM);
		} while (!hasEnableCube (targetColor));

		targetCube = Instantiate(cubeObj);
		targetCube.transform.position = new Vector3(2, 6.9f, 0);
		targetCube.transform.localScale = targetCube.transform.localScale * 2.5f;
		enableCollider(targetCube, false);
		targetCubeCtrl = targetCube.GetComponent<CubeCtrl>();
		targetCubeCtrl.setGameCtrl(this);
		targetCubeCtrl.setColor((int)targetColor);
		targetCubeCtrl.gameObject.name = TARGET_CUBE;
		colorTimeGaugeBaseScaleSphere = targetCube.transform.localScale;
	}

	void enableCollider(GameObject iObj, bool iFlg) {
		if (iObj.GetComponent<BoxCollider>()) {
			iObj.GetComponent<BoxCollider>().enabled = iFlg;
		} else if (iObj.GetComponent<SphereCollider>()) {
			iObj.GetComponent<SphereCollider>().enabled = iFlg;
		}
	}

	public void createNew (Vector3 pPosition, int pId, Colors pFadingColor)
	{
		_audioMgr.play (Const.SE_GOOD);

		cubes [pId - 1] = createCube (pPosition, pFadingColor);
		cubes [pId - 1].setId (pId);
		cubes [pId - 1].gameObject.name = "CUBE_" + pId.ToString ("D2");

		_result.deleteCount++;
		_result.comboCount++;

		if (_result.comboCount > _result.maxCombo) {
			_result.maxCombo = _result.comboCount;
		}

		if (_result.comboCount >= 2) {
			comboText.text = _result.comboCount.ToString ();

			string comboShowTextStr = _result.comboCount.ToString ();

			Vector3 textPosition = pPosition + new Vector3 (0, 0, -5f);
			GameObject textObj = Instantiate (comboShowTextObj,
											 textPosition,
											 gameObject.transform.rotation) as GameObject;
			textObj.GetComponent<TextMesh> ().text = comboShowTextStr;
			textObj.GetComponent<TextCtrl> ().init (2, 1);
		}

		// コンボの算出
		PBClass.BigInteger comboBonusValue = _userParam.basePoint / Const.COMBO_BONUS_RATE;
		comboBonusValue = comboBonusValue * (_result.comboCount - 1);

		_result.score += _userParam.basePoint + comboBonusValue;

		//scoreText.text = "SCORE : " + String.Format ("{0:#,0}", _result.score);
		scoreText.text = "SCORE : " + new IntValueConverter().FixBigInteger(_result.score);

		if (!hasEnableCube (targetColor)) {
			// 色全滅ボーナス
			colorClearBonus ();
			changeTargetColor ();
		}
	}

	void colorClearBonus () {
		_result.killAllCount++;

		_audioMgr.play (Const.SE_KILL_ALL);

		// 表示
		GameObject textObj = Instantiate (killAllText,
										  killAllText.transform.position,
										  killAllText.transform.rotation) as GameObject;
		textObj.GetComponent<TextMesh> ().text = "Kill All!\n+"
			+ (1/(float)Const.KILL_ALL_BONUS_RATE).ToString("P0");
		
		textObj.SetActive (true);
		textObj.GetComponent<TextCtrl> ().init (0.2f, 0.1f);
	}

	public GameObject mistakeObj;
	public bool mistakePenaltyFlg = false;
	public float mistakePenaltyTime =  .5f;
	float mistakePenaltyTimeLeft;

	public void wrongAnswer (int pId, int pColor) {
		setHowWrong (pColor);
		wrongAnswer ();
	}

	public SpriteRenderer miss_target;
	public SpriteRenderer miss_touched;

	void setHowWrong(int pColor) {
		ColorEditor.setColorFromColorCode (miss_target.gameObject, colorCodes [(int)targetColor]);
		ColorEditor.setColorFromColorCode (miss_touched.gameObject, colorCodes [pColor]);
	}

	public void wrongAnswer() {
		_audioMgr.play(Const.SE_BAD);

		_result.comboCount = 0;
		_result.missCount++;
		comboText.text = "";
		mistakePenaltyFlg = true;
		mistakePenaltyTimeLeft = mistakePenaltyTime;

		// 表示
		iTween.FadeTo(mistakeObj, iTween.Hash("a", 1.0f, "time", 0));
		touchableSign.SetActive (true);
	}

	public void addTime (float pTime) {
		timeLeft += pTime;
	}

	public void renewCubes (int pColor) {
		for (int i = 0; i < cubes.Count; i++) {
			cubes [i].setColor (pColor);
		}
	}

	bool isColorRestrictionValid = false;
	float colorRestrictionTimeLeft;
	List<Colors> restrictColors = new List<Colors> ();

	/// <summary>
	/// 色制限状態スタート。
	/// </summary>
	/// <param name="pColorCount">色制限状態時の使用可能色数</param>
	/// <param name="pTime">色制限状態の持続時間</param>
	public void startColorRestriction (int pColorCount, float pTime) {
		isColorRestrictionValid = true;
		colorRestrictionTimeLeft = pTime;

		restrictColors = new List<Colors> ();
		restrictColors = getAvailableColors (pColorCount);
	}

	public List<Colors> getAvailableColors(int pColorCount) {
		List<Colors> colors = new List<Colors> ();
		int n = pColorCount;

		// ランダムにいくつかの色を選ぶ処理
		// 色リストを確保
		List<int> availableColorNums = new List<int> ();
		for (int i = 0; i < (int)Colors.NUM; i++) {
			availableColorNums.Add (i);
		}

		for (int i = 0; i < n; i++) {
			// 色を選ぶ
			int colorNum = availableColorNums[UnityEngine.Random.Range (0, availableColorNums.Count)];

			// 選んだ色を利用可能リストに追加
			colors.Add ((Colors)colorNum);

			// 色リストから、該当の色を取り除く
			availableColorNums.RemoveAll (s => s == colorNum);
		}

		return colors;
	}

	void logList (List<int> list, string pName = "") {
		string ls = "";
		if (pName != "") {
			ls += pName + ": ";
		}

		for (int i = 0; i < list.Count; i++) {
			ls += "[" + i + "]:" + list [i] + "\n";
		}
		Debug.Log (ls);
	}

	void logList (List<Colors> list, string pName = "")
	{
		string ls = "";
		if (pName != "") {
			ls += pName + ": ";
		}
		for (int i = 0; i < list.Count; i++) {
			ls += "[" + i + "]:" + list [i] + "\n";
		}
		Debug.Log (ls);
	}


	void updateColorRestriction () {
		colorRestrictionTimeLeft -= Time.deltaTime;

		// 表示系処理

		// Reset
		if (colorRestrictionTimeLeft <= 0) {
			resetColorRestriction ();
		}
	}

	void resetColorRestriction () {
		colorRestrictionTimeLeft = 0;
		if (restrictColors.Count > 0) {
				restrictColors.Clear ();
		}
		isColorRestrictionValid = false;
	}

	// 確率設定
	// 0 - 5	: BOMB
	// 6 - 7	: COLOR
	// 8		: TIME

	public float rate_vertical		= 2f;
	public float rate_horizontal	= 2f;
	public float rate_cross		= 50f;
	public float rate_plus			= 2f;
	public float rate_multiple		= 2f;
	public float rate_around		= 1f;
	public float rate_renewal		= 1f;
	public float rate_restrict		= 1f;
	public float rate_add_time		= 3f;
	float [] ranges;

	void initRate () {
		if (!isRateDebug) {
			rate_vertical 	= _userParam.lineBombRate;
			rate_horizontal = _userParam.lineBombRate;
			rate_cross		= _userParam.lineBombRate;
			rate_plus		= _userParam.areaBombRate;
	     	rate_multiple	= _userParam.areaBombRate;
	     	rate_around 	= _userParam.areaBombRate;
			rate_renewal	= _userParam.renewalBombRate;
			rate_restrict 	= _userParam.colorLockBombRate;	
			rate_add_time	= _userParam.timeBombRate;
		}

		ranges = new float [] {
			rate_vertical,
			rate_horizontal,
			rate_cross,
			rate_plus,
			rate_multiple,
			rate_around,
			rate_renewal,
			rate_restrict,
			rate_add_time
		};
	}

	int decideCubeType () {
		float value = UnityEngine.Random.Range (0, 100);

		int fixId = -1;
		float rangeValue = 0;
		for (int i = 0; i < ranges.Length; i++) {
			rangeValue += ranges [i];
			if (value < rangeValue) {
				fixId = i;			
				break;
			}
		}
		if (fixId < 0) {
			// NORMAL
			return Const.TYPE_NORMAL;
		} else {
			// ITEM
			return fixId;
		}
	}

	Const.CubeType _decideCubeType () {
		Const.CubeType aType = Const.CubeType.NORMAL;
		return aType;
	}

	#region background
	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			//ホームボタンを押してアプリがバックグランドに移行した時
		} else {
			//アプリを終了しないでホーム画面からアプリを起動して復帰した時
			if (state == STATE.PLAYING) {
				enablePause (true);	
			}
		}
	}
	#endregion


	#region Android backbutton
	void CheckBackButtonPressed () {
		// プラットフォームがアンドロイドかチェック
		if (Application.platform == RuntimePlatform.Android) {
			// バックボタンが押されたか検知
			if (Input.GetKeyDown (KeyCode.Escape)) {
				// アプリケーション終了確認
				QuitApplicationConfirm ();
			}
		}
	}

	void QuitApplicationConfirm () {
		//ゲームプレイ中ならポーズにして、終了確認ダイアログを出す
		if (state == STATE.PLAYING) {
			enablePause (true);
		}
		// SHOP画面なら、RESULT画面に戻る
		else if (state == STATE.SHOP) {
			backFromShop ();
			return;
		}

		// 使う前に setlabel を呼んどく。
		DialogManager.Instance.SetLabel (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode ("quit_yes"),
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode ("quit_no"),
			"Close");

		// YES NO ダイアログ
		//「終了しますか？」とのダイアログを出す。
		DialogManager.Instance.ShowSelectDialog (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode ("quit_confirm"),
			(bool result) => {
				if (result) { // YES
					Application.Quit ();
					return;
				}
			}
		);
	}

	#endregion

	#region DEBUG
	[Header ("##### DEBUG Settings #####")]
	public bool isRateDebug = false;
	public bool isDebugParamSet = false;
	public UserData CustomUserData;
	public GameObject DEBUG_FinishBtn;
	public bool isPlayDebug = false;

	void finishGame () {
		timeLeft = 0;
	}
	#endregion
}