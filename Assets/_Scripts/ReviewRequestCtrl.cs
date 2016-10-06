using UnityEngine;
using System.Collections;

public class ReviewRequestCtrl : MonoBehaviour {
	ResultCtrl _resultCtrl;

	void Start () {
		_resultCtrl = this.gameObject.GetComponent<ResultCtrl> ();
	}

	public bool ReviewRequest () {
		// プレイ回数がx回以上のユーザーに対して
		int playCount = _resultCtrl._gameCtrl._userData.playCount;
		if (playCount <= Const.INTERVAL_REVIEW_REQUEST || // 規定回以上プレイていない
		    _resultCtrl._gameCtrl._userData.reviewDoneFlg == 1) // レビュー済み
		{
			return false; // 何もしない
		}

		if (playCount % Const.INTERVAL_REVIEW_REQUEST == 0) { // 規定回の倍数ならレビュー依頼してみる															  
			// 使う前に setlabel を呼んどく。
			DialogManager.Instance.SetLabel (
				GameCtrl.GetInstance()._languageCtrl.getMessageFromCode(Const.answer_01_y),
				GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_01_n),
				"Close");

			// YES NO ダイアログ
			//「楽しんでいただけていますか？」とのダイアログを出す。
			DialogManager.Instance.ShowSelectDialog (
				GameCtrl.GetInstance()._languageCtrl.getMessageFromCode(Const.dialog_01),
				(bool result) => {
					if (result) {
						//「ありがとうございます！よければレビューお願いします」
						AskForReview ();
					} else {
						// メッセージ送信したことないなら
						if (_resultCtrl._gameCtrl._userData.messageDoneFlg == 0) {
							// Webに飛ばす
							AskForMessage ();
						}
					}
				}
			);
		}
		return true;
	}

	void AskForReview () {
		DialogManager.Instance.SetLabel (
			GameCtrl.GetInstance()._languageCtrl.getMessageFromCode(Const.answer_02_y),
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_02_n),
			"Close");

		// YES NO ダイアログ
		//レビュー依頼
		DialogManager.Instance.ShowSelectDialog (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.dialog_02),
			(bool result) => {
				if (result) {
					_resultCtrl._gameCtrl._userData.reviewDoneFlg = 1;
					_resultCtrl._gameCtrl._userData.save ();

					// ストアへ
					Application.OpenURL (Const.APP_STORE_URL);
				}
			}
		);
	}

	void AskForMessage () {
		DialogManager.Instance.SetLabel (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_03_y),
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.answer_03_n),
			"Close");

		// YES NO ダイアログ
		// お問合せ
		DialogManager.Instance.ShowSelectDialog (
			GameCtrl.GetInstance ()._languageCtrl.getMessageFromCode (Const.dialog_03),
			(bool result) => {
				if (result) {
					_resultCtrl._gameCtrl._userData.messageDoneFlg = 1;
					_resultCtrl._gameCtrl._userData.save ();

					// Webビュー
					Application.OpenURL (Const.SUPPORT_URL);
				}
			}
		);	
	}
}
