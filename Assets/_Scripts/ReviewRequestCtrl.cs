using UnityEngine;
using System.Collections;

public class ReviewRequestCtrl : MonoBehaviour {
	ResultCtrl _resultCtrl;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ReviewRequest () {
		// プレイ回数がx回以上のユーザーに対して
		int playCount = _resultCtrl._gameCtrl._userData.playCount;
		if (playCount <= Const.INTERVAL_REVIEW_REQUEST) {
			return;
		}

		if (playCount % Const.INTERVAL_REVIEW_REQUEST == 0) { // 20の倍数ならレビュー依頼															  
			// 使う前に setlabel を呼んどく。
			DialogManager.Instance.SetLabel ("Yes", "It's ok...", "No");

			// YES NO ダイアログ
			//「楽しんでいただけていますか？」とのダイアログを出す。
			DialogManager.Instance.ShowSelectDialog (
				"Are you having fun?",
				(bool result) => {
					if (result) {
						//「ありがとうございます！よければレビューお願いします」
					} else {
						// Webに飛ばす
					}
				}
			);
			//
			// 確認のみのダイアログ
			DialogManager.Instance.ShowSubmitDialog (
				"submit dialog",
				(bool result) => { Debug.Log ("submited!"); }
			);

			// タイトルを表示する場合
			DialogManager.Instance.ShowSubmitDialog (
				"dialog title",
				"submit dialog",
				(bool result) => { Debug.Log ("submited!"); }
			);
		}
	}
	//「はい」の場合

	//レビューする
	//レビュー画面に飛ばす
	//Reviewed
	//今はしない。
	//終了
	//もう表示しない。
	//DontAskフラグを立てて終了
	//「いいえ」の場合
	//「改善のため、不具合や気になる点があればお問い合わせまでご連絡いただければお願いします」
	//報告・問い合わせ (Webブラウザが起動します)
	//問い合わせフォームに飛ばす
	//今はしない。
	//終了
	//今後もしない。
	//DontAskフラグを立てて終了

	bool dontAskFlg;
	bool reviewDoneFlg;
	void AskForReview () {
		DialogManager.Instance.SetLabel ("Yes", "No", "Later");

		// YES NO ダイアログ
		//レビュー依頼
		DialogManager.Instance.ShowSelectDialog (
			"Would you review our app?  We'll be so happy to hear from you!",
			(bool result) => {
				if (result) {
					setReviewDoneFlg (true);
					// ストアへ

				} else {
					//
				}
			}
		);
	}

	void AskForFAQ () {
		DialogManager.Instance.SetLabel ("Send message", "No", "Later");

		// YES NO ダイアログ
		// お問合せ
		DialogManager.Instance.ShowSelectDialog (
			"We'd love to hear your comment, advice, bug report and so in order to make our app better.  If you don't mind:",
			(bool result) => {
				if (result) {
					setReviewDoneFlg (true);
					// Webビュー

				} else {
					//
				}
			}
		);	
	}

	void setReviewDoneFlg (bool pFlg) {
		reviewDoneFlg = pFlg;
	}
}
