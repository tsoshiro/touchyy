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
		Debug.Log ("playCount:" + _gameCtrl._userData.playCount);
		if (_gameCtrl._userData.playCount % Const.AD_INTERVAL_INTER == 0) {
			Debug.Log ("SHOW INTERSTITIAL!");
			_gameCtrl.gameObject.GetComponent<AdvertisementManager> ().showInterstitial ();
		}
	}
}
