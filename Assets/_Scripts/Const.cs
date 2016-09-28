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

	// 
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
	public const string PREF_COIN 				= "PREF_COIN";
	public const string PREF_TOTAL_SCORE 		= "PREF_TOTAL_SCORE";
	public const string PREF_TOTAL_DELETE_COUNT = "PREF_TOTAL_DELETE_COUNT";
	public const string PREF_TOTAL_KILL_ALL_COUNT = "PREF_TOTAL_KILL_ALL_COUNT";
	public const string PREF_PLAY_COUNT 		= "PREF_PLAY_COUNT";

	// STATUS
	public const string PREF_LV_BASE 			= "PREF_LV_BASE";
	public const string PREF_LV_AREA_BOMB 		= "PREF_LV_AREA_BOMB";
	public const string PREF_LV_LINE_BOMB 		= "PREF_LV_LINE_BOMB";
	public const string PREF_LV_RENEWAL_BOMB 	= "PREF_LV_RENEWAL_BOMB";
	public const string PREF_LV_COLOR_LOCK_BOMB = "PREF_LV_COLOR_LOCK_BOMB";
	public const string PREF_LV_TIME_BOMB 		= "PREF_LV_TIME_BOMB";


	#region Sound
	public const int SE_GOOD = 0;
	public const int SE_BAD = 1;
	#endregion
}
