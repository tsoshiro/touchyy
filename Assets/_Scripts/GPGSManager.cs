using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

/// <summary>
/// ランキング用便利クラス
/// Androidの場合はこのクラスで処理
/// iOSの場合はiOSRankingUtilityに飛ばす
/// </summary>
public static class GPGSManager
{

	//=================================================================================
	//初期化
	//=================================================================================

	/// <summary>
	/// ユーザー認証
	/// </summary>
	public static void Auth (Action<bool> callBack = null)
	{
		//コールバックが設定されていない場合はログを設定
		if (callBack == null) {
			callBack = (success) => {
				Debug.Log (success ? "認証成功" : "認証失敗");
			};
		}
#if UNITY_ANDROID
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
			.Build ();
		PlayGamesPlatform.InitializeInstance (config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();

		Social.localUser.Authenticate (callBack);
#else
		iOSRankingUtility.Auth (callBack);
#endif

	}

	//=================================================================================
	//ランキング
	//=================================================================================

	/// <summary>
	/// リーダーボードを表示する
	/// </summary>
	public static void ShowLeaderboardUI ()
	{
#if UNITY_ANDROID
		Social.ShowLeaderboardUI ();
#else
		iOSRankingUtility.ShowLeaderboardUI();
#endif
	}

	/// <summary>
	/// リーダーボードにスコアを送信する
	/// </summary>
	public static void ReportScore (string leaderboardID, long score, Action<bool> callBack = null)
	{
		//コールバックが設定されていない場合はログを設定
		if (callBack == null) {
			callBack = (success) => {
				Debug.Log (success ? "スコア送信成功" : "スコア送信失敗");
			};
		}
#if UNITY_ANDROID
		//送信
		Social.ReportScore (score, leaderboardID, callBack);
#else
		iOSRankingUtility.ReportScore (leaderboardID, score, callBack);
#endif
	}

	//=================================================================================
	//実績
	//=================================================================================

	/// <summary>
	/// 実績一覧を表示する
	/// </summary>
	public static void ShowAchievementsUI ()
	{
#if UNITY_ANDROID
		Social.ShowAchievementsUI ();
#else
		iOSRankingUtility.ShowAchievementsUI ();
#endif
}

	/// <summary>
	/// 実績の進捗状況を送信する
	/// </summary>
	public static void ReportProgress (string achievementKey, float percent, Action<bool> callBack = null)
	{
#if UNITY_ANDROID
		//コールバックが設定されていない場合はログを設定
		if (callBack == null) {
			callBack = (success) => {
				Debug.Log (success ? "進捗送信成功" : "進捗送信失敗");
			};
		}

		//送信
		Social.ReportProgress (achievementKey, percent, callBack);
#else
		iOSRankingUtility.ReportProgress (achievementKey, percent, callBack);
#endif
	}
}