using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestReviewRequest {
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
