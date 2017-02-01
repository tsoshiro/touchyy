using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppealAnimation : MonoBehaviour {
	GameObject objOrg;
	public float scaleRate = 1.2f;
	public float scaleTime = 0.6f;
	public float fadeTime = 0.6f;

	// Use this for initialization
	void Start () {
		if (objOrg == null) {
			objOrg = this.gameObject;
		}
	}

	public void activate (bool pFlg) {
		flg = pFlg;
		if (flg) {
			StartCoroutine (loop ());
		}
	}

	bool flg;
	IEnumerator loop () {
		while (flg) {
			blinkAnimation (objOrg);
			yield return new WaitForSeconds (1.5f);
		}
		yield return 0;
	}


	// ボタンアピールアニメーション
	void blinkAnimation (GameObject obj)
	{
		Vector3 pos = obj.transform.position;
		pos.z -= 0.1f;

		// コピー
		GameObject go = (GameObject)Instantiate (obj,
												 pos,
												 obj.transform.rotation);
		Destroy (go.GetComponent<BoxCollider> ());
		iTween.ScaleTo (go, iTween.Hash ("scale", obj.transform.localScale * scaleRate, "time", scaleTime));
		iTween.FadeTo (go, iTween.Hash ("alpha", 0, "time", fadeTime));

		Destroy (go, 2f);
	}
}