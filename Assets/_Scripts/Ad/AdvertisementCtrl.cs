using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertisementCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;


	void Start () {
		_gameCtrl = GameCtrl.GetInstance ();
	}

	// インタースティシャルを表示するかどうか確認し、表示
	public void checkInterstitial ()
	{
		bool flg = checkInterFlgFromValues (_gameCtrl._userData.playCount, _gameCtrl._userData.restartCount);
		if (flg) {
			Debug.Log ("SHOW INTERSTITIAL!");
			_gameCtrl.gameObject.GetComponent<AdvertisementManager> ().showInterstitial ();
		}
	}

	// 判定処理を切り出し
	public bool checkInterFlgFromValues (int pPlayCount, int pRestartCount) {
		int inter_value = pPlayCount + pRestartCount;
		Debug.Log ("playCount:" + pPlayCount + "\n"
				   + "restartCount:" + pRestartCount + "\n"
				   + "inter_value:" + inter_value);

		if (inter_value % Const.AD_INTERVAL_INTER == 0) {
			return true;
		}
		return false;
	}
}
