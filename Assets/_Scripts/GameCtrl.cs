using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameCtrl : SingletonMonoBehaviour<GameCtrl> {
	public float timeLeft;

	public Result _result;
	public UserData _userData;
	public UserParam _userParam;

	#region Controllers
	public GameObject _readyCtrl;
	public ResultCtrl _resultCtrl;
	public ShopCtrl _shopCtrl;
	#endregion

	#region UI
	public TextMesh guideText;
	public TextMesh scoreText;
	public TextMesh comboText;
	public TextMesh timeText;
	public GameObject timeGauge;
	public GameObject colorTimeGauge;
	public GameObject colorRestrictionGauge;
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
		PURPLE
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

		SetGame();

		// 数値部分算出テスト
		//DisplayTest ();

		// GameCenterログイン
		iOSRankingUtility.Auth ();
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
		_result = new Result ();

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
			StartCoroutine (startCountDown (Const.COUNTDOWN_TIME));
			iTween.FadeTo (_readyCtrl, iTween.Hash ("a", 0, "time", 0.5f));
		}
	}

	public void actionPauseBtn () {
		if (state == STATE.PLAYING) {
			enablePause (true);	
		}
	}

	void actionContinueBtn () {
		if (state == STATE.PAUSE) {
			if (!isCountingDown) {
				// 表示系処理
				// Pauseオブジェクトを非表示
				pauseDiplay.SetActive (false);

				// カウントダウン開始
				StartCoroutine (startCountDown (Const.COUNTDOWN_TIME));
			}
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

		addKillAllBonus ();
		addNoMissBonus ();

		showResult ();

		yield return 0;
	}

	void addKillAllBonus () {
		// Kill All Bonus算出
		PBClass.BigInteger bonusValue = _result.score / Const.KILL_ALL_BONUS_RATE;
		bonusValue = bonusValue * _result.killAllCount;
		_result.score += bonusValue;
	}

	void addNoMissBonus () {
		if (_result.missCount == 0) {
			PBClass.BigInteger bonusValue = _result.score / Const.NO_MISS_BONUS_RATE;
			_result.score += bonusValue;
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
			targetColor = (Colors)UnityEngine.Random.Range (0, 5);
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

	public GameObject colorCubeObj;
	int colorRestrictionCount = 2;
	float colorRestrictionTime = 5f;

	CubeCtrl createCube(Vector3 pPosition) {
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

		int aColor =
			(isColorRestrictionValid)
			? (int)restrictColors[UnityEngine.Random.Range (0, restrictColors.Count)]
	        : UnityEngine.Random.Range (0, 5);
		cubeCtrl.setColor(aColor);

		return cubeCtrl;
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
			targetColor = (Colors)UnityEngine.Random.Range (0, 5);
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

	public void createNew (Vector3 pPosition, int pId)
	{
		_audioMgr.play (Const.SE_GOOD);

		cubes [pId - 1] = createCube (pPosition);
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

		// 表示
		GameObject textObj = Instantiate (killAllText,
										  killAllText.transform.position,
										  killAllText.transform.rotation) as GameObject;
		textObj.GetComponent<TextMesh> ().text = "Kill All!\nx" + _result.killAllCount;
		textObj.SetActive (true);
		textObj.GetComponent<TextCtrl> ().init (0.2f, 0.1f);
	}

	public GameObject mistakeObj;
	public bool mistakePenaltyFlg = false;
	public float mistakePenaltyTime =  .5f;
	float mistakePenaltyTimeLeft;

	public void wrongAnswer (int pId, int pColor) {
//		howWrong = (Colors)pColor;
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

	public GameObject ColorRestrictionGauge;

	bool isColorRestrictionValid = false;
	float colorRestrictionTimeLeft;
	List<Colors> restrictColors = new List<Colors> ();

	public void startColorRestriction (int pColorCount, float pTime) {
		isColorRestrictionValid = true;
		colorRestrictionTimeLeft = pTime;

		restrictColors = new List<Colors> ();
		do {
			Colors aColor = (Colors)(int)UnityEngine.Random.Range (0, 5);
			if (!restrictColors.Contains (aColor)) {
				restrictColors.Add (aColor);		
			}
		} while (restrictColors.Count >= pColorCount);
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
	float rate_vertical		= 2f;
	float rate_horizontal	= 2f;
	float rate_cross		= 50f;
	float rate_plus			= 2f;
	float rate_multiple		= 2f;
	float rate_around		= 1f;
	float rate_renewal		= 1f;
	float rate_restrict		= 1f;
	float rate_add_time		= 3f;
	float [] ranges;

	public bool isRateDebug = false;
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

	#region DEBUG
	public bool isDebugParamSet = false;
	public UserData CustomUserData;
	#endregion
}