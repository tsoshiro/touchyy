using UnityEngine;
using System.Collections;

public class SplashCtrl : MonoBehaviour {
	public GameObject _logo;
	public GameObject _frame;
	bool isSplashStarted = false;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (true);
		iTween.FadeTo (_logo, iTween.Hash ("a", 0f, "time", 0f));
	}

	void Update () {
		if (!Application.isShowingSplashScreen) {
			if (!isSplashStarted) {
				StartCoroutine (splashFlow ());
				isSplashStarted = true;
			}
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

		iTween.FadeTo (_frame, iTween.Hash ("a", 0f, "time", 0.5f));
		yield return new WaitForSeconds (0.5f);

		finishSplashFlow ();
	}

	void finishSplashFlow () {
		// 終了 BGM再生
		GameCtrl.GetInstance ().endSplashThenstartBgm ();
		GameCtrl.GetInstance ().LeaderboardLogin ();
		this.gameObject.SetActive (false);

	}
}