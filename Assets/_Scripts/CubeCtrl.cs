using UnityEngine;
using System.Collections;

public class CubeCtrl : MonoBehaviour {
	public Material[] materials;
	int cubeId;

	GameCtrl.Colors myColor;
	GameCtrl _GameCtrl;

	Const.CubeType cubeType;

	public void init() {
		setCollider();
		setCubeType ();

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

	void setCubeType () {
		if (GetComponent<BombCtrl> ()) {
			cubeType = Const.CubeType.BOMB;
		} else if (GetComponent<TimeCubeCtrl> ()) {
			cubeType = Const.CubeType.TIME;
		} else if (GetComponent<ColorCubeCtrl> ()) {
			cubeType = Const.CubeType.COLOR;
		} else {
			cubeType = Const.CubeType.NORMAL;
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
			if (cubeType == Const.CubeType.BOMB) {
				bombCheck ();
			} else if (cubeType == Const.CubeType.TIME) {
				addTime ();
				vanish ();
			} else if (cubeType == Const.CubeType.COLOR) {
				colorEffect();
				vanish ();
			} else {
				vanish ();	
			}
		} else {
			// WRONG
			wrong();
		}
	}

	public GameCtrl.Colors getColor() { return myColor; }

	public void bombCheck () {
		Const.BombType bombType = GetComponent<BombCtrl> ().getBombType ();
		_GameCtrl.gameObject.GetComponent<BombItemCtrl> ().tapBomb (cubeId, bombType);
	}

	void addTime () {
		_GameCtrl.addTime (GetComponent<TimeCubeCtrl> ().getAddTime ());
	}

	void colorEffect () {
		ColorCubeCtrl ctrl = GetComponent<ColorCubeCtrl> ();

		if (ctrl.getColorType() == Const.TYPE_RENEWAL) { // RENEWAL
			_GameCtrl.renewCubes ((int)myColor);
		} else { // COLOR RESTRICTION
			_GameCtrl.startColorRestriction (ctrl.getRestrictColorCount(), ctrl.getRestrictValidTime());
		}
	}

	public void vanish() {
		_GameCtrl.createNew(this.transform.position, cubeId);
		Destroy(this.gameObject);
	}

	void wrong() {
		_GameCtrl.wrongAnswer (cubeId, (int)myColor);
		//_GameCtrl.wrongAnswer();
	}

	public void setId(int pId) {
		cubeId = pId;
	}

	public int getId() {
		return cubeId;
	}

	public void setColor(int pColor) {
		Color aColor;
		// 色情報を取得し、塗る
		if (ColorUtility.TryParseHtmlString("#"+_GameCtrl.colorCodes[pColor], out aColor)) {
			this.transform.GetComponentInChildren<SpriteRenderer> ().color = aColor;
		}
		myColor = (GameCtrl.Colors)pColor;
	}

	public void setGameCtrl(GameCtrl pGameCtrl) {
		_GameCtrl = pGameCtrl;
	}

	public GameCtrl getGameCtrl() {
		return _GameCtrl;
	}
}
