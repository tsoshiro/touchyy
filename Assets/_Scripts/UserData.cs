using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserData {
	int DEFAULT_VALUE_INT = 0;
	float DEFAULT_VALUE_FLOAT = 0.0f;
	string DEFAULT_VALUE_STRING = "";
	public List<string> _userParamsList = new List<string> ();

	public int getUserDataInt (string pKey) {
		return PlayerPrefs.GetInt (pKey, DEFAULT_VALUE_INT);
	}

	public float getUserDataFloat (string pKey)
	{
		return PlayerPrefs.GetFloat (pKey, DEFAULT_VALUE_FLOAT);
	}

	public string getUserDataString (string pKey) {
		return PlayerPrefs.GetString (pKey, DEFAULT_VALUE_STRING);
	}

	public void setUserData (string pKey, int pValue) {
		PlayerPrefs.SetInt (pKey, pValue);
	}

	public void setUserData (string pKey, float pValue)
	{
		PlayerPrefs.SetFloat (pKey, pValue);
	}

	public void setUserData (string pKey, string pValue)
	{
		PlayerPrefs.SetString (pKey, pValue);
	}

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

	public void saveUserData () {
		PlayerPrefs.Save ();
	}

	public void resetUserData () {
		PlayerPrefs.DeleteAll ();
	}

	// 初期データ作成
	public void checkNewUser () {
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
	}

	public UserData () {
		_userParamsList.Clear ();

		_userParamsList.Add (Const.PREF_COIN);
		_userParamsList.Add (Const.PREF_LV_BASE);
		_userParamsList.Add (Const.PREF_LV_AREA_BOMB);
		_userParamsList.Add (Const.PREF_LV_LINE_BOMB);
		_userParamsList.Add (Const.PREF_LV_RENEWAL_BOMB);
		_userParamsList.Add (Const.PREF_LV_COLOR_LOCK_BOMB);
		_userParamsList.Add (Const.PREF_LV_TIME_BOMB);
	}
}