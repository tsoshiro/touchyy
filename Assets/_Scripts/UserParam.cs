using UnityEngine;
using System.Collections;

public class UserParam {
	public int basePoint;
	public float areaBombRate;
	public float lineBombRate;
	public float renewalBombRate;
	public float colorLockBombRate;
	public float timeBombRate;

	public UserParam (UserData pUserData) {
		basePoint	 = getBasePoint (pUserData.getUserDataInt (Const.PREF_LV_BASE));
		areaBombRate 		= getBombRate (pUserData.getUserDataInt (Const.PREF_LV_AREA_BOMB));
		lineBombRate 		= getBombRate (pUserData.getUserDataInt (Const.PREF_LV_LINE_BOMB));
		renewalBombRate 	= getBombRate (pUserData.getUserDataInt (Const.PREF_LV_RENEWAL_BOMB));
		colorLockBombRate 	= getBombRate (pUserData.getUserDataInt (Const.PREF_LV_COLOR_LOCK_BOMB));
		timeBombRate 		= getBombRate (pUserData.getUserDataInt (Const.PREF_LV_TIME_BOMB));
	}

	int getBasePoint (int pLv) {
		int value = 100;
		return value;
	}

	float START_RATE = 0.01f;
	float getBombRate (int pLv) {
		float value = 0.0f;
		if (pLv == 1) {
			value = START_RATE;
		} else {
			value = START_RATE + (float)pLv * START_RATE;
		}
		return value;
	}
}
