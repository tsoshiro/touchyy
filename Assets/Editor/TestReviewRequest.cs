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
		bool flg;

		int n = 0;

		do {
			gameCtrl._userData.playCount = n;
			reviewRequestCtrl.ReviewRequest ();
			n++;
		} while (n <= 10);
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
		if (playCount <= Const.INTERVAL_REVIEW_REQUEST || // 規定回以上プレイしていない
			reviewDoneFlg == 1) // レビュー済み
		{
			return 0;// 何もしない
		}

		int reviewRequestFreqCount
		= (deniedFlg == 0)
				? Const.INTERVAL_REVIEW_REQUEST
	   			: Const.INTERVAL_REVIEW_REQUEST_CANCELED_ONCE;

		if (playCount % reviewRequestFreqCount == 0) { // 規定回の倍数ならレビュー依頼してみる
			return 1;
		}
		return 0;
	}

}
