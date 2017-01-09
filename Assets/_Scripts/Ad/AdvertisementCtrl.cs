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
		int inter_value = _gameCtrl._userData.playCount + _gameCtrl._userData.restartCount;
		Debug.Log ("playCount:" + _gameCtrl._userData.playCount + "\n"
				   + "restartCount:" + _gameCtrl._userData.restartCount + "\n"
				   + "inter_value:" + inter_value);

		if (inter_value % Const.AD_INTERVAL_INTER == 0) {
			Debug.Log ("SHOW INTERSTITIAL!");
			_gameCtrl.gameObject.GetComponent<AdvertisementManager> ().showInterstitial ();
		}
	}
}
