using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestReviewRequest {

	[Test]
	public void EditorTest()
	{
		//Arrange
		var gameObject = new GameObject();

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}

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

		//do {
		//	gameCtrl._userData.playCount = n;
		//	gameCtrl._userData.save ();

		//	flg = reviewRequestCtrl.ReviewRequest();
		//	Assert.IsTrue (flg);

		//	n++;
		//} while (n <= 10);
	}

	[Test]
	public void ResultIntervalSimulate () {
		int playCount = 0;
		int reviewDoneFlg = 0;
		int deniedFlg = 0;
		int messageDoneFlg = 0;

		// 0: DO NOTHING
		// 1: ASK IF HAVING FUN
		int actionNumber = getActionNumber ();

		Assert.AreEqual (0, getActionNumber (1, 0, 0, 0));
		Assert.AreEqual (1, getActionNumber (20, 0, 0, 0));
		Assert.AreEqual (0, getActionNumber (25, 0, 1, 0));
		Assert.AreEqual (0, getActionNumber (40, 1, 0, 0));
		Assert.AreEqual (0, getActionNumber (40, 0, 1, 0));
		Assert.AreEqual (1, getActionNumber (60, 0, 1, 0));
	}

	int getActionNumber (int playCount = 0,
	                     int reviewDoneFlg = 0,
	                     int deniedFlg = 0,
	                     int messageDoneFlg = 0)
	{
		// プレイ回数がx回以上のユーザーに対して
		if (playCount < Const.INTERVAL_REVIEW_REQUEST || // 規定回以上プレイしていない
			reviewDoneFlg == 1) // レビュー済み
		{
			return 0;// 何もしない
		}

		int reviewRequestFreqCount
		= (deniedFlg == 0)
				? Const.INTERVAL_REVIEW_REQUEST
	   			: Const.INTERVAL_REVIEW_REQUEST_CANCELED_ONCE;

		Debug.Log ("reviewRequestFreqCount:" + reviewRequestFreqCount);

		if (playCount % reviewRequestFreqCount == 0) { // 規定回の倍数ならレビュー依頼してみる
			return 1;
		}
		return 0;
	}

}
