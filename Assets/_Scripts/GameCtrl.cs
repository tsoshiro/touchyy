using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameCtrl : MonoBehaviour {
	public float timeLeft;

	float COLOR_CLEAR_BONUS_RATE = 2f;

	Result _result;
	public UserData _userData;
	public UserParam _userParam;

	#region Controllers
	public ResultCtrl _resultCtrl;
	public ShopCtrl _shopCtrl;
	#endregion

	#region UI
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
	int basePoint = 10;

	//AUDIO
	public AudioMgr _audioMgr;


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
		// UserData初期化
		_userData = new UserData ();
		_userData.checkNewUser ();

		// UserParam初期化
		_userParam = new UserParam (_userData);
	}

	// Use this for initialization
	void Start () {
		timeGaugeBaseWidth = timeGauge.transform.localScale.x;
		colorTimeGaugeBaseWidth = colorTimeGauge.transform.localScale.x;
		mistakeGaugeBaseWidth = mistakeGauge.transform.localScale.x;
		countDownTextObj.SetActive (false);
		mistakeText.SetActive (false);
		mistakeGauge.SetActive (false);
		pauseBtn.SetActive (false);

		// SHOP初期化
		_shopCtrl.initUserItems ();

		SetGame();
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

		resultText.gameObject.SetActive (true);
		resultText.text = "TAP\nTO\nSTART\n\n画面上部のものと\n同じ色を触って\n消しましょう"; ;
		//arrowguide.SetActive (true);
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
		if (Input.GetMouseButtonDown(0)) {
			if (!isCountingDown) {
				StartCoroutine (startCountDown (Const.COUNTDOWN_TIME));
			}
		}
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

			// 表示系処理
			// Pauseオブジェクトを非表示
			pauseDiplay.SetActive (false);
		}
	}
	public GameObject pauseDiplay;

	#region actionBtn
	public void actionPauseBtn () {
		if (state == STATE.PLAYING) {
			enablePause (true);	
		}
	}

	void actionContinueBtn () {
		if (state == STATE.PAUSE) {
			if (!isCountingDown) {
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
			StartGame ();	
		}
	}

	void displayCountDownNumber (int pNum) {
		// SHOW TEXT
		GameObject textObj = Instantiate (countDownTextObj,
										  countDownTextObj.transform.position,
										  gameObject.transform.rotation) as GameObject;
		textObj.SetActive (true);
		textObj.GetComponent<TextMesh> ().text = "" + pNum;
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
		if (Input.GetMouseButtonDown(0)) {
			// SETTING
			//SetGame();
//			Application.LoadLevel(Application.loadedLevelName);
		}
	}

	void updateMistake () {
		mistakePenaltyTimeLeft -= Time.deltaTime;
		setMistakeTimeGaugeRate ();
		if (mistakePenaltyTimeLeft <= 0) {
			mistakePenaltyFlg = false;

			mistakeGauge.SetActive (false);
			touchableSign.SetActive (false);
		}
	}

	bool canGoNext = false;

	IEnumerator StopGame() {

		state = STATE.RESULT;
		canGoNext = false;

		pauseBtn.SetActive (false);
		touchableSign.SetActive (true);

		showResult ();

		yield return 0;
	}

	public void finishResultAnimation () {
		//yield return new WaitForSeconds (2);

		resultText.text += "\nTAP TO REPLAY";
		canGoNext = true;
	}

	void showResult () {
		_resultCtrl.gameObject.SetActive (true);
		iTween.ScaleFrom (_resultCtrl.gameObject, Vector3.zero, 0.1f);

		//TODO アニメーション中に触らせない処理

		// saveData

		// showResult
		_resultCtrl.showResult (_result);

	}

	public STATE lastState = STATE.READY;
	public void OpenShop () {
		lastState = state;
		_shopCtrl.gameObject.SetActive (true);
		state = STATE.SHOP;
	}

	public void backFromShop () {
		_shopCtrl.gameObject.SetActive (false);
		if (lastState == STATE.READY) {
			// 閉じるのみ
			state = STATE.READY;
		} else if (lastState == STATE.RESULT) {
			// Resultを開く
			state = STATE.RESULT;
			_resultCtrl.gameObject.SetActive (true);
			_resultCtrl.SetCoinValue();;
		}
	}

	#region coin
	public void getCoin (int pScore) {
		// LOGIC
		_userData.addTotalRecords (Const.PREF_COIN, pScore);
	}

	public void spendCoin (int pCost) {
		_userData.addTotalRecords (Const.PREF_COIN, - pCost);
	}

	public void reloadUserData () {
		_userData = new UserData ();
		_userParam.setUserParam (_userData);
	}

	#endregion

	float ANIMATION_TIME = 0.5f;
	void changeTargetColor() {
		do {
			targetColor = (Colors)Random.Range (0, 5);
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
		float rate = Random.Range (0, 100);
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
			? (int)restrictColors[Random.Range (0, restrictColors.Count)]
	        : Random.Range (0, 5);
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
			targetColor = (Colors)Random.Range (0, 5);
		} while (!hasEnableCube (targetColor));

		targetCube = Instantiate(cubeObj);
		targetCube.transform.position = new Vector3(2, 6.5f, 0);
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
			comboText.text = _result.comboCount.ToString () + " COMBO!!";

			string comboShowTextStr = _result.comboCount.ToString () + "\nCOMBO!!";

			Vector3 textPosition = pPosition + new Vector3 (0, 0, -5f);
			GameObject textObj = Instantiate (comboShowTextObj,
											 textPosition,
											 gameObject.transform.rotation) as GameObject;
			textObj.GetComponent<TextMesh> ().text = comboShowTextStr;
			textObj.GetComponent<TextCtrl> ().init (2, 1);
		}

		_result.score += basePoint * _result.comboCount;
		scoreText.text = "SCORE : " + _result.score;

		if (!hasEnableCube (targetColor)) {
			// 色全滅ボーナス
			colorClearBonus ();
			changeTargetColor ();
		}
	}

	void colorClearBonus () {
		_result.killAllCount++;
		_result.score += (int)((float)_result.killAllCount * COLOR_CLEAR_BONUS_RATE);

		// 表示
		GameObject textObj = Instantiate (killAllText,
										  killAllText.transform.position,
										  killAllText.transform.rotation) as GameObject;
		textObj.GetComponent<TextMesh> ().text = "Kill All!\n+" + (float)_result.killAllCount * COLOR_CLEAR_BONUS_RATE;
		textObj.SetActive (true);
		textObj.GetComponent<TextCtrl> ().init (0.2f, 0.1f);
	}

	public bool mistakePenaltyFlg = false;
	public float mistakePenaltyTime =  .5f;
	float mistakePenaltyTimeLeft;
	string howWrong = "";

	public void wrongAnswer (int pId, int pColor) {
		howWrong = pId + "\n" + (Colors)pColor;
		wrongAnswer ();
	}

	public void wrongAnswer() {
		_audioMgr.play(Const.SE_BAD);

		_result.comboCount = 0;
		_result.missCount++;
		comboText.text = "";
		mistakePenaltyFlg = true;
		mistakePenaltyTimeLeft = mistakePenaltyTime;

		// 表示
		GameObject textObj = Instantiate (mistakeText,
		                                  mistakeText.transform.position,
		                                  mistakeText.transform.rotation) as GameObject;
		textObj.SetActive (true);
		textObj.GetComponent<TextCtrl> ().init (0.1f, mistakePenaltyTime);
		textObj.GetComponent<TextMesh> ().text += "\n"+howWrong;

		mistakeGauge.SetActive (true);
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
			Colors aColor = (Colors)(int)Random.Range (0, 5);
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
	float rate_cross		= 1f;
	float rate_plus			= 2f;
	float rate_multiple		= 2f;
	float rate_around		= 1f;
	float rate_renewal		= 1f;
	float rate_restrict		= 1f;
	float rate_add_time		= 3f;
	float [] ranges;

	void initRate () {
		rate_vertical 	= _userParam.lineBombRate;
		rate_horizontal = _userParam.lineBombRate;
		rate_cross		= _userParam.lineBombRate;
		rate_plus		= _userParam.areaBombRate;
     	rate_multiple	= _userParam.areaBombRate;
     	rate_around 	= _userParam.areaBombRate;
		rate_renewal	= _userParam.renewalBombRate;
		rate_restrict 	= _userParam.colorLockBombRate;	
		rate_add_time	= _userParam.timeBombRate;

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
		float value = Random.Range (0, 100);

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
}