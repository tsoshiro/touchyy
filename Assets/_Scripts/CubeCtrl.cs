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

	// Update is called once per frame
	void Update () {
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
			if (isBomb ()) {
				bombCheck();
			} else {
				vanish ();	
			}
		} else {
			// WRONG
			wrong();
		}
	}

	public GameCtrl.Colors getColor() { return myColor; }

	bool isBomb() {
		if (GetComponent<BombCtrl>()) {
			return true;
		}
		return false;
	}

	public void bombCheck () {
		Const.BombType bombType = GetComponent<BombCtrl> ().getBombType ();
		_GameCtrl.gameObject.GetComponent<BombItemCtrl> ().tapBomb (cubeId, bombType);
	}

	public void vanish() {
		_GameCtrl.createNew(this.transform.position, cubeId);
		Destroy(this.gameObject);
	}

	void wrong() {
		_GameCtrl.wrongAnswer();
	}

	public void setId(int pId) {
		cubeId = pId;
	}

	public int getId() {
		return cubeId;
	}

	public void setColor(int pColor) {
		this.gameObject.GetComponent<Renderer>().material = materials[pColor];
		myColor = (GameCtrl.Colors)pColor;

		return;
		if (isBomb ()) {
			for (int i = 0; i < this.transform.childCount; i++) {
				Transform t = this.transform.GetChild (i);
				t.gameObject.GetComponent<Renderer> ().material = materials [pColor];
			}
		}
	}

	public void setGameCtrl(GameCtrl pGameCtrl) {
		_GameCtrl = pGameCtrl;
	}

	public GameCtrl getGameCtrl() {
		return _GameCtrl;
	}
}
