using UnityEngine;
using System.Collections;

public class TextCtrl : MonoBehaviour {

	public void init (float pDisplayTime, float pFadeTime) {
		displayTime = pDisplayTime;
		fadeTime = pFadeTime;
		StartCoroutine (startLifeCycle());
	}

	float displayTime;
	float fadeTime;
	float marginTime = 1f; // 表示上消えてから消滅るまでの時間

	IEnumerator startLifeCycle () {
		// WAIT UNTIL DISPLAY TIME
		yield return new WaitForSeconds (displayTime);

		// FADE OUT
		iTween.FadeTo (this.gameObject, 0, fadeTime);

		yield return new WaitForSeconds(marginTime);

		Destroy (this.gameObject);
	}
}
