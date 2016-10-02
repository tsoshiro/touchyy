using UnityEngine;
using System.Collections;

public class MissCtrl : MonoBehaviour {
	public GameObject _gauge;
	public GameObject _label;
	public GameObject _largeCircle;
	public GameObject _smallCircle;

	public TextMesh stateNow;
	float timeToTake = 2f;

	void Start() {
//		Invoke ("fadeOut",timeToTake);
	}

	void fadeOut() {
		stateNow.text = "fadeOut\niTween";
		iTween.FadeTo (this.gameObject, iTween.Hash ("a", 0, "time", timeToTake, "oncomplete", "fadeIn"));
	}

	void fadeIn() {
		stateNow.text = "fadeIn\niTween";
		iTween.FadeTo(this.gameObject,  iTween.Hash("a", 1, "time", timeToTake, "oncomplete", "fadeOutColorEditor"));
	}


	void fadeOutColorEditor() {
		stateNow.text = "fadeOut\nColorEditor";
		ColorEditor.setFade (this.gameObject, 0f);
		Invoke ("fadeInColorEditor", timeToTake);
	}

	void fadeInColorEditor() {
		stateNow.text = "fadeIn\nColorEditor";
		ColorEditor.setFade (this.gameObject, 1f);
		Invoke ("fadeOut", timeToTake);
	}
}
