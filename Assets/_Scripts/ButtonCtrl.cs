using UnityEngine;
using System.Collections;

public class ButtonCtrl : MonoBehaviour {
	public GameObject parentObj;
	public bool setMeFlg = false;
	public bool onOffFlg = false;

	GameObject imageObj;
	string imageObjName = "image";

	public Sprite[] onOffSprites; 

	void Start() {
		if (transform.FindChild (imageObjName)) {
			imageObj = transform.FindChild (imageObjName).gameObject;
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();

			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject == this.gameObject) {
					sendAction ();
				}
			}
		}
	}

	public void setOnFlg(bool iFlg) {
		if (onOffSprites.Length <= 0) {
			Debug.LogWarning ("onOffSprites are not set!:" + this.gameObject);
			return;
		}
		if (iFlg) {
			imageObj.GetComponent<SpriteRenderer> ().sprite = onOffSprites [0];
		} else {
			imageObj.GetComponent<SpriteRenderer> ().sprite = onOffSprites [1];
		}
	}

	void sendAction () {
		Debug.Log ("action"+gameObject.name);
		if (setMeFlg) {			
			parentObj.SendMessage ("action" + gameObject.name, this.gameObject);
		} else {
			parentObj.SendMessage ("action" + gameObject.name);	
		}
	}
}
