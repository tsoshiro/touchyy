using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppealAnimation : MonoBehaviour {
	GameObject objOrg;
	public float scaleRate = 1.2f;
	public float scaleTime = 0.6f;
	public float fadeTime = 0.6f;

	public float fadeTimeFast = 0.3f;
	public float scaleTimeFast = 0.3f;

	bool loopFlg;

	enum MotionSpeed {
		SLOW,
		FAST
	}

	// Use this for initialization
	void Start () {
		if (objOrg == null) {
			objOrg = this.gameObject;
		}
	}

	void setMotionSpeed (int speedType) {
		if ((MotionSpeed)speedType == MotionSpeed.FAST) {
			scaleTime = scaleTimeFast;
			fadeTime = fadeTimeFast;
		}
	}

	/// <summary>
	/// アニメーションさせるオブジェクトを引数に入れる
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="localScale">Local scale.</param>
	/// <param name="position">Position.</param>
	/// <param name="speedType">Speed type.</param>
	public void playOnceOriginal (GameObject obj, Vector3 localScale, Vector3 position, int speedType = 0) {
		setMotionSpeed (speedType);
		animateObject (obj, localScale, position);
	}

	public void playOnce (GameObject obj = null, int speedType = 0) {
		if (obj == null) {
			obj = objOrg;
		}

		setMotionSpeed (speedType);
		blinkAnimation (obj);
	}

	public void activate (bool pFlg) {
		loopFlg = pFlg;
		if (loopFlg) {
			StartCoroutine (loop ());
		}
	}

	IEnumerator loop () {
		while (loopFlg) {
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

		// 不要なコンポーネントを削除
		DestroyCollider (go);
		DestroyCubeCtrl (go);

		animateObject (go, obj.transform.localScale, pos);
	}

	void animateObject (GameObject go, Vector3 localScale, Vector3 pos) {
		go.transform.position = pos;

		iTween.ScaleTo (go, iTween.Hash ("scale", localScale * scaleRate, "time", scaleTime));
		iTween.FadeTo (go, iTween.Hash ("alpha", 0, "time", fadeTime));

		Destroy (go, 2f);
	}


	void DestroyComponents (GameObject go) {
		DestroyCollider (go);
		DestroyCubeCtrl (go);
	}

	void DestroyCollider (GameObject go) {
		Component [] components = go.GetComponents<Collider> ();
		for (int i = 0; i < components.Length; i++) {
			Destroy (components [i]);
		}
	}

	void DestroyCubeCtrl (GameObject go) {
		if (go.GetComponent<CubeCtrl> ()) {
			Destroy (go.GetComponent<CubeCtrl> ());	
		}
	}
}