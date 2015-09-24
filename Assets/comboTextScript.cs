using UnityEngine;
using System.Collections;

public class comboTextScript : MonoBehaviour {
	float timeToFade = .5f;

	// Use this for initialization
	void Start () {
		iTween.ScaleFrom(gameObject, iTween.Hash(
			"scale", new Vector3(0,0,0),
			"time", 0.5f,
			"easetype", iTween.EaseType.easeOutBounce,
			"oncomplete", "FadeOut",
			"oncompletetarget", gameObject)
		                 );
	}

	void FadeOut() {
		Color color = gameObject.GetComponent<TextMesh>().color;
		color = new Color(color.r, color.g, color.b, 0);

		iTween.ColorTo(gameObject, color, timeToFade);
	}

	void DestroySelf() {
		Destroy(gameObject);
	}
}
