using UnityEngine;
using System.Collections;

public class Const {
	public const float ApplicationFrameRate = 30f;
	public const int COUNTDOWN_TIME = 3;


	public enum CubeType
	{
		NORMAL,
		BOMB,
		COLOR,
		TIME
	}

	public enum BombType
	{
		VERTICAL, // 縦一列 5コ
		HORIZONTAL, // 横一列 5コ
		CROSS, // 縦横一列
		PLUS, // 上下左右 4コ
		MULTIPLE, // 右上・左上・右下・左下 4コ
		AROUND // 周囲8コ 
	}

	// TYPE
	public const int TYPE_VERTICAL 	= 0;
	public const int TYPE_HORIZONTAL= 1;
	public const int TYPE_CROSS		= 2;
	public const int TYPE_PLUS		= 3;
	public const int TYPE_MULTIPLE 	= 4;
	public const int TYPE_AROUND	= 5;
	public const int TYPE_RENEWAL	= 6;
	public const int TYPE_RESTRICT	= 7;
	public const int TYPE_ADD_TYME	= 8;
	public const int TYPE_NORMAL 	= 9;

	// UserData
	// PlayerPrefsのKey
	// RECORD
	public const string PREF_BEST_SCORE 		= "PREF_BEST_SCORE";
	public const string PREF_MAX_COMBO 			= "PREF_MAX_COMBO";
	public const string PREF_MAX_DELETE_COUNT 	= "PREF_MAX_DELETE_COUNT";
	public const string PREF_MAX_KILL_ALL_COUNT = "PREF_MAX_KILL_ALL_COUNT";
	public const string PREF_TOTAL_SCORE 		= "PREF_TOTAL_SCORE";
	public const string PREF_TOTAL_DELETE_COUNT = "PREF_TOTAL_DELETE_COUNT";
	public const string PREF_TOTAL_KILL_ALL_COUNT = "PREF_TOTAL_KILL_ALL_COUNT";
	public const string PREF_PLAY_COUNT 		= "PREF_PLAY_COUNT";

	// PARAM
	public enum UserParams
	{
		COIN,
		LV_BASE,
		LV_AREA_BOMB,
		LV_LINE_BOMB,
		LV_RENEWAL_BOMB,
		LV_COLOR_LOCK_BOMB,
		LV_TIME_BOMB,
		COUNT
	}

	public const string PREF_COIN				= "PREF_COIN";
	public const string PREF_LV_BASE 			= "PREF_LV_BASE";
	public const string PREF_LV_AREA_BOMB 		= "PREF_LV_AREA_BOMB";
	public const string PREF_LV_LINE_BOMB 		= "PREF_LV_LINE_BOMB";
	public const string PREF_LV_RENEWAL_BOMB 	= "PREF_LV_RENEWAL_BOMB";
	public const string PREF_LV_COLOR_LOCK_BOMB = "PREF_LV_COLOR_LOCK_BOMB";
	public const string PREF_LV_TIME_BOMB 		= "PREF_LV_TIME_BOMB";

	// PARAM LIMIT
	//public const int CUSTOM_LV_MAX = 100; // 100まではゲームデータで管理。それ以降は決め打ちロジック。
	public const int CUSTOM_LV_MAX = 1; // 
	public const int COST_BASE = 19000;
	public const float OVER_LV_COST_ADD_RATE = 1.02f;

	// PARAM ID
	public const int PARAM_COIN 			= 0;
	public const int PARAM_LV_BASE 			= 1;
	public const int PARAM_LV_AREA_BOMB 	= 2;
	public const int PARAM_LV_LINE_BOMB 	= 3;
	public const int PARAM_LV_RENEWAL_BOMB 	= 4;
	public const int PARAM_LV_COLOR_LOCK_BOMB = 5;
	public const int PARAM_LV_TIME_BOMB 	= 6;

	// ADS & REVIEWS
	public const int AD_INTERVAL_INTER = 5;
	public const int AD_INTERVAL_REWARD_MOVIE = 3;
	public const int INTERVAL_REVIEW_REQUEST = 20;


	#region Sound
	public const int SE_GOOD = 0;
	public const int SE_BAD = 1;
	#endregion
}
