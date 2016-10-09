using UnityEngine;
using System.Collections;

public class SplashCtrl : MonoBehaviour {
	public GameObject _logo;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (true);
		iTween.FadeTo (_logo, iTween.Hash ("a", 0f, "time", 0f));
	}

	void Update () {
		if (!Application.isShowingSplashScreen) {
			StartCoroutine (splashFlow ());
		}
	}

	IEnumerator splashFlow () {
		yield return 0;

		// フェードイン
		iTween.FadeTo (_logo, iTween.Hash ("a", 1.0f, "time", 0.5f));

		yield return new WaitForSeconds (0.5f);

		// 待機
		yield return new WaitForSeconds(1f);

		// フェードアウト
		iTween.FadeTo (_logo, iTween.Hash ("a", 0f, "time", 0.5f));
		yield return new WaitForSeconds (0.5f);

		iTween.FadeTo (this.gameObject, iTween.Hash ("a", 0f, "time", 0.5f));
		yield return new WaitForSeconds (0.5f);

		// 終了 BGM再生
		GameCtrl.GetInstance ().endSplashThenstartBgm ();
		this.gameObject.SetActive (false);
	}
}