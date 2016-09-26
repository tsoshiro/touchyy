using UnityEngine;
using System.Collections;

public class ButtonCtrl : MonoBehaviour {
	public GameObject parentObj;

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

	void sendAction () {
		parentObj.SendMessage ("action" + gameObject.name);
	}
}
