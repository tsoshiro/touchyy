using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestReviewRequest {

	[Test]
	public void ReviewRequestFeedbackRequestTest ()
	{
		GameCtrl gameCtrl = GameObject.Find ("GameCtrl").GetComponent<GameCtrl> ();
		ResultCtrl resultCtrl = gameCtrl._resultCtrl;

		ReviewRequestCtrl reviewRequestCtrl = resultCtrl.gameObject.GetComponent<ReviewRequestCtrl>();

		// テストで書き換えられてしまうデータを一時保存
		int playCountTmp = gameCtrl._userData.playCount;
		int reviewDoneFlgTmp = gameCtrl._userData.reviewDoneFlg;
		int deniedFlgTmp = gameCtrl._userData.deniedFlg;
		int messageDoneFlgTmp = gameCtrl._userData.messageDoneFlg;


		bool flg;
		// プレイ1回目 - false
		gameCtrl._userData.playCount = 1;
		gameCtrl._userData.save ();
		flg = reviewRequestCtrl.ReviewRequest();
		Assert.IsFalse (flg);

		// プレイ20回目 - true
		gameCtrl._userData.playCount = 20;
		gameCtrl._userData.save ();
		flg = reviewRequestCtrl.ReviewRequest ();
		Assert.IsTrue (flg);

		// 40回目・レビュー済み - false
		gameCtrl._userData.playCount = 40;
		gameCtrl._userData.reviewDoneFlg = 1;
		gameCtrl._userData.save ();
		flg = reviewRequestCtrl.ReviewRequest ();
		Assert.IsFalse (flg);

		// 40回目・楽しんでないORレビューしないを選択済み - false
		gameCtrl._userData.playCount = 40;
		gameCtrl._userData.reviewDoneFlg = 0;
		gameCtrl._userData.deniedFlg = 1;
		gameCtrl._userData.save ();
		flg = reviewRequestCtrl.ReviewRequest ();
		Assert.IsFalse (flg);

		// 120回目・楽しんでないORレビューしないを選択済み - true
		gameCtrl._userData.playCount = 120;
		gameCtrl._userData.reviewDoneFlg = 0;
		gameCtrl._userData.deniedFlg = 1;
		gameCtrl._userData.save ();
		flg = reviewRequestCtrl.ReviewRequest ();
		Assert.IsTrue (flg);

		// Test が完了したので設定を元に戻す
		gameCtrl._userData.playCount = playCountTmp;
		gameCtrl._userData.reviewDoneFlg = reviewDoneFlgTmp;
		gameCtrl._userData.deniedFlg = deniedFlgTmp;
		gameCtrl._userData.messageDoneFlg = messageDoneFlgTmp;
	}

	[Test]
	public void ResultIntervalSimulate () {
		// 0: DO NOTHING
		// 1: ASK IF HAVING FUN
		int actionNumber = getActionNumber ();

		Assert.AreEqual (0, getActionNumber (1, 0, 0, 0));
		Assert.AreEqual (0, getActionNumber (20, 0, 0, 0));
		Assert.AreEqual (0, getActionNumber (30, 1, 0, 0));
		Assert.AreEqual (0, getActionNumber (30, 1, 0, 1));
		Assert.AreEqual (1, getActionNumber (30, 0, 0, 0));
		Assert.AreEqual (1, getActionNumber (30, 0, 0, 1));
		Assert.AreEqual (1, getActionNumber (60, 0, 0, 0));
		Assert.AreEqual (0, getActionNumber (60, 1, 0, 0));
		Assert.AreEqual (0, getActionNumber (60, 0, 1, 0));
		Assert.AreEqual (0, getActionNumber (65, 0, 1, 0));
		Assert.AreEqual (1, getActionNumber (70, 0, 1, 0));
		Assert.AreEqual (1, getActionNumber (140, 0, 1, 0));
	}

	int getActionNumber (int playCount = 0,
	                     int reviewDoneFlg = 0,
	                     int deniedFlg = 0,
	                     int messageDoneFlg = 0)
	{
		GameCtrl gameCtrl = GameObject.Find ("GameCtrl").GetComponent<GameCtrl> ();
		ResultCtrl resultCtrl = gameCtrl._resultCtrl;
		ReviewRequestCtrl reviewRequestCtrl = resultCtrl.gameObject.GetComponent<ReviewRequestCtrl> ();

		// プレイ回数がx回以上のユーザーに対して
		if (reviewRequestCtrl.CheckIsPlayCountUnderOrAlreadyReviewed(playCount, reviewDoneFlg))		    
		{
			return 0;// 何もしない
		}
		if (reviewRequestCtrl.CheckIsOkToAskReview(playCount, deniedFlg))
		{
			return 1;
		}
		return 0;
	}

}
