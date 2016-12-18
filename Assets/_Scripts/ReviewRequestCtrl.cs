using UnityEngine;
using System.Collections;

public class ReviewRequestCtrl : MonoBehaviour {
	ResultCtrl _resultCtrl;

	void Start () {
		_resultCtrl = this.gameObject.GetComponent<ResultCtrl> ();
	}

	public void test ()
	{
		LanguageCtrl _lngCtrl = GameCtrl.GetInstance ()._languageCtrl;
		Debug.Log (
			_lngCtrl.getMessageFromCode (Const.answer_01_n)	+ "\n" +
			_lngCtrl.getMessageFromCode (Const.answer_01_y) + "\n" +
			_lngCtrl.getMessageFromCode (Const.dialog_01) 	+ "\n" +
			_lngCtrl.getMessageFromCode (Const.answer_02_n) + "\n" +
			_lngCtrl.getMessageFromCode (Const.answer_02_y) + "\n" +
			_lngCtrl.getMessageFromCode (Const.dialog_02) 	+ "\n" +
			_lngCtrl.getMessageFromCode (Const.answer_03_n) + "\n" +
			_lngCtrl.getMessageFromCode (Const.answer_03_y) + "\n" +
			_lngCtrl.getMessageFromCode (Const.dialog_03)
		);
	}

	public bool ReviewRequest () {
		// プレイ回数がx回以上のユーザーに対して
		int playCount = _resultCtrl._gameCtrl._userData.playCount;
		if (CheckIsPlayCountUnderOrAlreadyReviewed (playCount,
		                                            _resultCtrl._gameCtrl._userData.reviewDoneFlg))
		{
			return false; // 何もしない
		}

		if (CheckIsOkToAskReview(playCount, _resultCtrl._gameCtrl._userData.deniedFlg))
		{ // 規定回の倍数ならレビュー依頼してみる
			// 使う前に setlabel を呼んどく。
			DialogManager.Instance.SetLabel (
				GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_01_n),
				GameCtrl.GetInstance()._languageCtrl.getMessageFromCode(Const.answer_01_y),
				"Close");

			// YES NO ダイアログ
			//「楽しんでいただけていますか？」とのダイアログを出す。
			DialogManager.Instance.ShowSelectDialog (
				GameCtrl.GetInstance()._languageCtrl.getMessageFromCode(Const.dialog_01),
				(bool result) => {
					if (result) {
						// メッセージ送信したことないなら
						if (_resultCtrl._gameCtrl._userData.messageDoneFlg == 0) {
							// Webに飛ばす
							AskForMessage ();
						}
					} else {
						//「ありがとうございます！よければレビューお願いします」
						AskForReview ();
					}
				}
			);
		}
		return true;
	}

	public bool CheckIsPlayCountUnderOrAlreadyReviewed (int playCount, int reviewDoneFlg)
	{
		if (playCount < Const.INTERVAL_REVIEW_REQUEST || // 規定回以上プレイしていない
			reviewDoneFlg == 1) // レビュー済み
		{
			return true; // 何もしない
		}
		return false;
	}

	public bool CheckIsOkToAskReview (int playCount, int deniedFlg)
	{
		// 以前レビュー以来を断ったかどうかによって間隔を変える
		int reviewRequestFreqCount
			= (deniedFlg == 0)
			? Const.INTERVAL_REVIEW_REQUEST
	   		: Const.INTERVAL_REVIEW_REQUEST_CANCELED_ONCE;

		if (playCount % reviewRequestFreqCount == 0) {
			return true;
		}
		return false;
	}

	void AskForReview () {
		DialogManager.Instance.SetLabel (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_02_n),
			GameCtrl.GetInstance()._languageCtrl.getMessageFromCode(Const.answer_02_y),
			"Close");

		// YES NO ダイアログ
		//レビュー依頼
		DialogManager.Instance.ShowSelectDialog (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.dialog_02),
			(bool result) => {
				if (result) {
					_resultCtrl._gameCtrl._userData.deniedFlg = 1;
				} else {
					_resultCtrl._gameCtrl._userData.reviewDoneFlg = 1;
					// ストアへ
					Application.OpenURL (Const.APP_STORE_URL);
				}
				_resultCtrl._gameCtrl._userData.save ();
			}
		);
	}

	void AskForMessage () {
		DialogManager.Instance.SetLabel (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_03_n),
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_03_y),
			"Close");

		// YES NO ダイアログ
		// お問合せ
		DialogManager.Instance.ShowSelectDialog (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.dialog_03),
			(bool result) => {
				if (result) {
					_resultCtrl._gameCtrl._userData.deniedFlg = 1;
				} else {
					_resultCtrl._gameCtrl._userData.messageDoneFlg = 1;
					// Webビュー
					Application.OpenURL (Const.SUPPORT_URL);
				}
				_resultCtrl._gameCtrl._userData.save ();
			}
		);	
	}
}
