using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameCtrl : MonoBehaviour {
	public float timeLeft;
	public float frameRate = 30;


	float score = 0;
	public int comboCount = 0;
	int maxCombo = 0;

	#region UI
	public TextMesh scoreText;
	public TextMesh comboText;
	public TextMesh timeText;
	public GameObject timeGauge;
	public GameObject colorTimeGauge;
	public TextMesh resultText;
	public GameObject cubeObj;
	public GameObject bombObj;
	public GameObject comboShowTextObj;
	public GameObject countDownTextObj;
	public GameObject touchableSign;

	float timeGaugeBaseWidth;
	float colorTimeGaugeBaseWidth;
	Vector3 colorTimeGaugeBaseScaleSphere;
	#endregion

	#region timerflg
	int COUNT_DOWN_NUM = 3;
	int countDownNum;
	#endregion

	public float TIME = 20;
	public int HEIGHT = 5;
	public int WIDTH = 5;
	float basePoint = 10;

	//AUDIO
	public AudioMgr _audioMgr;
	int SE_GOOD = 0;
	int SE_BAD = 1;

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
		RESULT
	}

	public STATE state;
	float CHANGE_TIME = 3;
	float changeTimeLeft;

	string TARGET_CUBE = "TARGET_CUBE";

	string SCENE_TITLE  = "TEST_TITLE";
	string SCENE_MAIN   = "TEST_MAIN";
	string SCENE_RESULT = "TEST_RESULT";

	// Use this for initialization
	void Start () {

		timeGaugeBaseWidth = timeGauge.transform.localScale.x;
		colorTimeGaugeBaseWidth = colorTimeGauge.transform.localScale.x;
		countDownTextObj.SetActive (false);

		SetGame();
	}

	void SetGame() {
		state = STATE.READY;
		
		scoreText.text = "SCORE : "+score;
		comboText.text = "";
		comboCount = 0;
		
		timeLeft = TIME;
		timeText.text = "TIME : "+timeLeft;
		setTimeGaugeRate ();
		
		resultText.text = "TAP\nTO\nSTART";
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
		Vector3 gaugeScale = new Vector3 (gaugeX,
		                                  colorTimeGauge.transform.localScale.y,
										 colorTimeGauge.transform.localScale.z);
		colorTimeGauge.transform.localScale = gaugeScale;

		Vector3 targetCubeScale = colorTimeGaugeBaseScaleSphere * (changeTimeLeft / CHANGE_TIME);
		targetCube.transform.localScale = targetCubeScale;
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
		targetColor = (Colors)Random.Range(0,5);
		setCubes();

		countDownNum = COUNT_DOWN_NUM;
		touchableSign.SetActive (false);

		state = STATE.PLAYING;
	}

	void Update() {
		if (state == STATE.READY) {
			updateReady();
		} else if (state == STATE.PLAYING) {
			updatePlaying();
		} else if (state == STATE.RESULT && canGoNext) {
			updateResult ();
		}
	}

	void updateReady() {
		if (Input.GetMouseButtonDown(0)) {
			StartGame();
		}
	}

	void updatePlaying() {
		if (mistakePenaltyFlg) {
			mistakePenaltyTimeLeft -= Time.deltaTime;
			if (mistakePenaltyTimeLeft <= 0) {
				mistakePenaltyFlg = false;

				touchableSign.SetActive (false);
			}
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
			GameObject textObj = Instantiate (countDownTextObj,
			                                  countDownTextObj.transform.position,
			                                  gameObject.transform.rotation) as GameObject;
			textObj.SetActive (true);
			textObj.GetComponent<TextMesh> ().text = ""+countDownNum;
			textObj.GetComponent<TextCtrl> ().init (0.5f, 0.5f);

			// COUNT DOWN
			countDownNum--;
		}
	}

	void updateResult() {
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("LogScreen (SCENE_RESULT)");
			// SETTING
			SetGame();
//			Application.LoadLevel(Application.loadedLevelName);
		}
	}

	bool canGoNext = false;

	IEnumerator StopGame() {

		state = STATE.RESULT;
		canGoNext = false;
		touchableSign.SetActive (true);

		resultText.text = "RESULT\n"+"SCORE:"+score+"\nMAX COMBO:"+maxCombo;
		resultText.gameObject.SetActive(true);

		yield return new WaitForSeconds(2);

		resultText.text += "\nTAP TO REPLAY";
		canGoNext = true;
	}
	
	void changeTargetColor() {
		do {
			targetColor = (Colors)Random.Range (0, 5);
		} while (!hasEnableCube(targetColor));

		changeTimeLeft = CHANGE_TIME;
		setColorTimeGaugeRate ();

		targetCubeCtrl.setColor((int)targetColor);
		iTween.ScaleFrom(targetCube, iTween.Hash(
			"scale", new Vector3(0,0,0),
			"time", 0.5f,
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

	bool isBomb() {
		return false;
		float rate = Random.Range (0,100);
		if (rate <= 10) {
			return true;
		}
		return false;
	}

	CubeCtrl createCube(Vector3 pPosition) {
		GameObject obj;
		if (isBomb()){
			obj = Instantiate(bombObj);
		} else {
			obj = Instantiate(cubeObj);
		}
		obj.transform.position = pPosition;
		obj.transform.parent = cubeGroup.transform;
		obj.transform.name = TARGET_CUBE;

		CubeCtrl cubeCtrl = obj.GetComponent<CubeCtrl>();
		cubeCtrl.init();
		cubeCtrl.setGameCtrl(this);
		cubeCtrl.setColor(Random.Range(0,5));

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

	public void createNew(Vector3 pPosition, int pId) {
		_audioMgr.play(SE_GOOD);

		cubes[pId-1] = createCube(pPosition);
		cubes[pId-1].setId(pId);
		cubes[pId-1].gameObject.name = "CUBE_"+pId.ToString("D2");

		comboCount++;

		if (comboCount > maxCombo) {
			maxCombo = comboCount;
		}

		if (comboCount >= 2) {
			comboText.text = comboCount.ToString() + " COMBO!!";

			string comboShowTextStr = comboCount.ToString() + "\nCOMBO!!";

			Vector3 textPosition = pPosition + new Vector3(0, 0, -5f);
			GameObject textObj = Instantiate(comboShowTextObj,						
			                                 textPosition,
			                                 gameObject.transform.rotation) as GameObject;
			textObj.GetComponent<TextMesh>().text = comboShowTextStr;
			textObj.GetComponent<TextCtrl> ().init (2, 1);
		}

		score += basePoint * comboCount;
		scoreText.text = "SCORE : "+score;

		if (!hasEnableCube(targetColor)) {
			changeTargetColor();
		}
	}

	public bool mistakePenaltyFlg = false;
	public float mistakePenaltyTime =  .5f;
	float mistakePenaltyTimeLeft;

	public void wrongAnswer() {
		_audioMgr.play(SE_BAD);

		comboCount = 0;
		comboText.text = "";
		mistakePenaltyFlg = true;
		mistakePenaltyTimeLeft = mistakePenaltyTime;

		touchableSign.SetActive (true);
	}
}
