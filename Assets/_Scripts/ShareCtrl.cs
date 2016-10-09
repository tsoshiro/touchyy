using UnityEngine;
using System.Collections;

public class ShareCtrl : MonoBehaviour {
	ResultCtrl _resultCtrl;
	// Use this for initialization
	void Start () {
		_resultCtrl = this.GetComponent<ResultCtrl> ();
	}

	string SCORE_KEY = "[SCORE]";
	public void shareResult () {
		string score = ""+_resultCtrl._gameCtrl._result.score;
		string msg = _resultCtrl._gameCtrl._languageCtrl.getMessageFromCode ("share");

		// 書き換え
		if (msg.Contains (SCORE_KEY)) {
			msg = msg.Replace (SCORE_KEY, score);
		}

		SocialConnector.SocialConnector.Share (msg, Const.APP_STORE_URL);
	}
}
