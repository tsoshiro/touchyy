using UnityEngine;
using System.Collections;

public class CubeCtrl : MonoBehaviour {
	public Material[] materials;
	int cubeId;

	GameCtrl.Colors myColor;
	GameCtrl _GameCtrl;

	public void init() {
		setCollider();

		if (gameObject.name == "TARGET_CUBE") {
//			gameObject.transform.localScale = gameObject.transform.localScale * 3;
		}

		iTween.ScaleFrom(gameObject, iTween.Hash(
			"scale", new Vector3(0,0,0),
			"time", 0.5f,
			"easetype", iTween.EaseType.easeOutBounce,
			"oncomplete", "setCollider",
			"oncompletetarget", gameObject)
		                 );
	}

	void setCollider() {
		if (gameObject.GetComponent<BoxCollider>()) {
			gameObject.GetComponent<BoxCollider>().enabled = true;
		} else if (gameObject.GetComponent<SphereCollider>()) {
			gameObject.GetComponent<SphereCollider>().enabled = true;
		}
	}

	void Awake() {
//		init ();
	}

	// Use this for initialization
	void Start () {

	}

	float timer = 0;
	float frameRate = 60;
	bool mistakePenalty = false;

	// Update is called once per frame
	void Update () {
		timer++;

		if (_GameCtrl.mistakePenaltyFlg) {
			return;
		}

		if (_GameCtrl.state != GameCtrl.STATE.PLAYING) {
			return;
		}

		if (Input.GetMouseButton(0)) {        
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
		
			if (Physics.Raycast(ray, out hit)){
				if (hit.collider.gameObject == this.gameObject) {
					this.checkColor();
				}
			}
		}
	}

	void checkColor() {
		if (_GameCtrl.targetColor == myColor) {
			// CORRECT
			vanish();
		} else {
			// WRONG
			wrong();
		}
	}

	public GameCtrl.Colors getColor() { return myColor; }

	void vanish() {
		_GameCtrl.createNew(this.transform.position, cubeId);
		Destroy(this.gameObject);
	}

	void wrong() {
		_GameCtrl.wrongAnswer();
	}

	public void setId(int pId) {
		cubeId = pId;
	}

	public void setColor(int pColor) {
		this.gameObject.GetComponent<Renderer>().material = materials[pColor];
		myColor = (GameCtrl.Colors)pColor;
	}

	public void setGameCtrl(GameCtrl pGameCtrl) {
		_GameCtrl = pGameCtrl;
	}
}
