using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UserData {
	#region PRIVATE PlayerPrefs
	int DEFAULT_VALUE_INT = 0;
	float DEFAULT_VALUE_FLOAT = 0.0f;
	string DEFAULT_VALUE_STRING = "";

	private int getUserDataInt (string pKey) {
		return PlayerPrefs.GetInt (pKey, DEFAULT_VALUE_INT);
	}

	private float getUserDataFloat (string pKey)
	{
		return PlayerPrefs.GetFloat (pKey, DEFAULT_VALUE_FLOAT);
	}

	private string getUserDataString (string pKey) {
		return PlayerPrefs.GetString (pKey, DEFAULT_VALUE_STRING);
	}

	private void setUserData (string pKey, int pValue) {
		PlayerPrefs.SetInt (pKey, pValue);
	}

	private void setUserData (string pKey, float pValue)
	{
		PlayerPrefs.SetFloat (pKey, pValue);
	}

	private void setUserData (string pKey, string pValue)
	{
		PlayerPrefs.SetString (pKey, pValue);
	}

	private void saveUserData () {
		PlayerPrefs.Save ();
	}

	private void resetUserData () {
		PlayerPrefs.DeleteAll ();
	}
	#endregion

	#region public interface
	public void save() {
		saveAllData ();
		saveUserData ();
	}

	void saveAllData() {
		setUserData (Const.PREF_BEST_SCORE, bestScore);
		setUserData (Const.PREF_MAX_COMBO, maxCombo) ;
		setUserData (Const.PREF_MAX_DELETE_COUNT, maxDeleteCount);
		setUserData (Const.PREF_MAX_KILL_ALL_COUNT, maxKillAllCount);

		setUserData (Const.PREF_TOTAL_SCORE,totalScore);
		setUserData (Const.PREF_TOTAL_DELETE_COUNT,totalDeleteCount);
		setUserData (Const.PREF_TOTAL_KILL_ALL_COUNT,totalKillAllCount);
		setUserData (Const.PREF_PLAY_COUNT,playCount);

		setUserData (Const.PREF_COIN,coin);

		// _userParamListの中身を更新
		setUserData (Const.PREF_LV_BASE, 			_userParamsList[Const.PARAM_LV_BASE]);
		setUserData (Const.PREF_LV_AREA_BOMB, 		_userParamsList[Const.PARAM_LV_AREA_BOMB]);
		setUserData (Const.PREF_LV_LINE_BOMB, 		_userParamsList[Const.PARAM_LV_LINE_BOMB]);
		setUserData (Const.PREF_LV_RENEWAL_BOMB,	_userParamsList[Const.PARAM_LV_RENEWAL_BOMB]);
		setUserData (Const.PREF_LV_COLOR_LOCK_BOMB,	_userParamsList[Const.PARAM_LV_COLOR_LOCK_BOMB]);
		setUserData (Const.PREF_LV_TIME_BOMB,		_userParamsList[Const.PARAM_LV_TIME_BOMB]);

		setUserData (Const.PREF_NEXT_FREE_GIFT, nextFreeGift);
	}

	public void reset() {
		resetUserData ();
	}
	#endregion

	#region RECORD
	public bool checkIfIsNewRecord (string pKey, int pValue) {
		if ( pValue > getUserDataInt(pKey)) {
			setUserData (pKey, pValue);
			return true;
		}
		return false;
	}

	public void addTotalRecords (string pKey, int pValue) {
		int totalRecord = getUserDataInt (pKey);
		totalRecord += pValue;
		setUserData (pKey, totalRecord);
	}
	#endregion

	#region IN GAME USE
	// public records and parameters
	// RECORDS
	public int bestScore;
	public int maxCombo;
	public int maxDeleteCount;
	public int maxKillAllCount;
	public int maxAllCount;

	// TOTAL RECORDS
	public int totalScore;
	public int totalDeleteCount;
	public int totalKillAllCount;
	public int playCount;

	// PARAMETERS
	public int coin;
	public List<int> _userParamsList = new List<int> ();

	public string nextFreeGift;

	// 初期データ作成
	public void initUserData() {
		checkNewUser ();

		bestScore 			= getUserDataInt (Const.PREF_BEST_SCORE);
		maxCombo 			= getUserDataInt (Const.PREF_MAX_COMBO);
		maxDeleteCount		= getUserDataInt (Const.PREF_MAX_DELETE_COUNT);
		maxKillAllCount		= getUserDataInt (Const.PREF_MAX_KILL_ALL_COUNT);

		totalScore			= getUserDataInt (Const.PREF_TOTAL_SCORE);
		totalDeleteCount	= getUserDataInt (Const.PREF_TOTAL_DELETE_COUNT);
		totalKillAllCount	= getUserDataInt (Const.PREF_TOTAL_KILL_ALL_COUNT);
		playCount			= getUserDataInt (Const.PREF_PLAY_COUNT);

		coin				= getUserDataInt (Const.PREF_COIN);

		// userParamList
		_userParamsList.Clear ();
		_userParamsList.Add (getUserDataInt (Const.PREF_LV_BASE));
		_userParamsList.Add (getUserDataInt (Const.PREF_LV_AREA_BOMB));
		_userParamsList.Add (getUserDataInt (Const.PREF_LV_LINE_BOMB));
		_userParamsList.Add (getUserDataInt (Const.PREF_LV_RENEWAL_BOMB));
		_userParamsList.Add (getUserDataInt (Const.PREF_LV_COLOR_LOCK_BOMB));
		_userParamsList.Add (getUserDataInt (Const.PREF_LV_TIME_BOMB));

		nextFreeGift = getUserDataString (Const.PREF_NEXT_FREE_GIFT);
		if (nextFreeGift == "") {
			nextFreeGift = DateTime.Now.ToString (Const.DATETIME_FORMAT);
		}
	}

	void checkNewUser () {
		if (getUserDataInt (Const.PREF_LV_BASE) <= 0) {
			createBaseUserData ();
		}
	}

	void createBaseUserData () {
		setUserData (Const.PREF_COIN, 0);
		setUserData (Const.PREF_LV_BASE, 1);
	 	setUserData (Const.PREF_LV_AREA_BOMB, 0);
	 	setUserData (Const.PREF_LV_LINE_BOMB, 0);
	 	setUserData (Const.PREF_LV_RENEWAL_BOMB, 0);
	 	setUserData (Const.PREF_LV_COLOR_LOCK_BOMB, 0);
	 	setUserData (Const.PREF_LV_TIME_BOMB, 0);

		setUserData (Const.PREF_NEXT_FREE_GIFT, DateTime.Now.ToString (Const.DATETIME_FORMAT));
	}

	public UserData () {
	}
	#endregion
}