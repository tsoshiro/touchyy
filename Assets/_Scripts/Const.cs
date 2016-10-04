using UnityEngine;
using System.Collections;

public class Const {
	#region STORE
#if UNITY_ANDROID
	public const string APP_ID = "jp.pixelbeat.touchyy";
	public const string APP_STORE_URL = "jp.pixelbeat.touchyy";
	public const string APP_STORE_REVIEW_URL = "";
#elif UNITY_IPHONE
	//public const string APP_ID = "1161341169";
	public const string APP_ID = "493470467"; //テスト用:パズドラが開くはず
	public const string APP_STORE_URL = 
		"https://itunes.apple.com/jp/app/pazuru-doragonzu/id"
		+APP_ID
		+"?mt=8";
	public const string APP_STORE_REVIEW_URL =
		"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id="
		+APP_ID
		+"&onlyLatestVersion=true&pageNumber=0&sortOrdering=1&type=Purple+Software";
#endif
	public const string SUPPORT_URL = "pixelbeat.jp";
	#endregion


	public const float GAME_SCREEN_POSITION_X = 6.4f;

	public const float ApplicationFrameRate = 30f;
	public const int COUNTDOWN_TIME = 3;

	#region color codes
	// MaterialColors From https://www.materialui.co/colors
	public const string COLOR_CODE_BASE_COLOR = "37474F"; // Blue... 800
	public const string COLOR_CODE_LIGHTER_BASE = "78909C"; // Blue... 400

	public const string COLOR_CODE_BG_GRAY = "F5F5F5"; // Gray : 100

	// COLOR 400
	public const string COLOR_CODE_PINK = "EC407A";	
	public const string COLOR_CODE_RED = "ef5350";
	public const string COLOR_CODE_BLUE = "42A5F5";
	public const string COLOR_CODE_GREEN = "66BB6A";
	public const string COLOR_CODE_YELLOW = "FFCA28";
	public const string COLOR_CODE_PURPLE = "7E57C2";
	public const string COLOR_CODE_GRAY = "BDBDBD";


	#endregion

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

	public const string PREF_NEXT_FREE_GIFT	 	= "PREF_NEXT_FREE_GIFT";
	public const string PREF_REVIEW_DONE = "PREF_REVIEW_DONE";
	public const string PREF_MESSAGE_DONE = "PREF_MESSAGE_DONE";

	public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

	// PARAM LIMIT
	// 100まではゲームデータで管理。それ以降は決め打ちロジック。
	public const int CUSTOM_LV_MAX = 100; // 
	public const float OVER_LV_COST_ADD_RATE = 1.02f;

	// PARAM ID
	public const int PARAM_LV_BASE 				= 0;
	public const int PARAM_LV_AREA_BOMB 		= 1;
	public const int PARAM_LV_LINE_BOMB 		= 2;
	public const int PARAM_LV_RENEWAL_BOMB 		= 3;
	public const int PARAM_LV_COLOR_LOCK_BOMB 	= 4;
	public const int PARAM_LV_TIME_BOMB 		= 5;

	// ADS & REVIEWS
	public const int AD_INTERVAL_INTER = 5;
	public const int AD_INTERVAL_REWARD_MOVIE = 3;
	public const int INTERVAL_REVIEW_REQUEST = 20;

	public const string AD_MOVIE_STATE_FINISHED = "0";
	public const string AD_MOVIE_STATE_SKIPPED = "1";
	public const string AD_MOVIE_STATE_FAILED = "2";


	#region Sound
	public const int SE_GOOD = 0;
	public const int SE_BAD = 1;
	public const int SE_UP = 2;
	public const int SE_NO = 3;
	#endregion

}
