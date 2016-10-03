﻿using UnityEngine;
using System.Collections;

public class ReviewRequestCtrl : MonoBehaviour {
	ResultCtrl _resultCtrl;

	void Start () {
		_resultCtrl = this.gameObject.GetComponent<ResultCtrl> ();
	}

	void ReviewRequest () {
		// プレイ回数がx回以上のユーザーに対して
		int playCount = _resultCtrl._gameCtrl._userData.playCount;
		if (playCount <= Const.INTERVAL_REVIEW_REQUEST || // 規定回以上プレイていない
		    _resultCtrl._gameCtrl._userData.reviewDoneFlg == 1) // レビュー済み
		{
			return; // 何もしない
		}

		if (playCount % Const.INTERVAL_REVIEW_REQUEST == 0) { // 規定回の倍数ならレビュー依頼してみる															  
			// 使う前に setlabel を呼んどく。
			DialogManager.Instance.SetLabel ("Yes", "It's ok...", "No");

			// YES NO ダイアログ
			//「楽しんでいただけていますか？」とのダイアログを出す。
			DialogManager.Instance.ShowSelectDialog (
				"Are you having fun?",
				(bool result) => {
					if (result) {
						//「ありがとうございます！よければレビューお願いします」
						AskForReview ();
					} else {
						// メッセージ送信したことないなら
						if (_resultCtrl._gameCtrl._userData.messageDoneFlg == 1) {
							// Webに飛ばす
							AskForMessage ();
						}
					}
				}
			);
		}
	}

	void AskForReview () {
		DialogManager.Instance.SetLabel ("Yes", "No", "Later");

		// YES NO ダイアログ
		//レビュー依頼
		DialogManager.Instance.ShowSelectDialog (
			"Would you review our app?  We'll be so happy to hear from you!",
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
		DialogManager.Instance.SetLabel ("Send message", "No", "Later");

		// YES NO ダイアログ
		// お問合せ
		DialogManager.Instance.ShowSelectDialog (
			"We'd love to hear your comment, advice, bug report and so in order to make our app better.  If you don't mind:",
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
