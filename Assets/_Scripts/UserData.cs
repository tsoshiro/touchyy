using UnityEngine;
using System.Collections;

public class UserData {


	int DEFAULT_VALUE_INT = 0;
	float DEFAULT_VALUE_FLOAT = 0.0f;
	string DEFAULT_VALUE_STRING = "";

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

	#region 
	public bool checkIfIsNewRecord (int pValue, string pKey) {
		if ( pValue > getUserDataInt(pKey)) {
			setUserData (pKey, pValue);
			return true;
		}
		return false;
	}
	#endregion

	public void saveUserData () {
		PlayerPrefs.Save ();
	}

	public void resetUserData () {
		PlayerPrefs.DeleteAll ();
	}
}