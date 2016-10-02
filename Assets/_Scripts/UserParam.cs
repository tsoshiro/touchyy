using UnityEngine;
using System.Collections;

public class UserParam {
	// LV設定から確率などを計算する
	public int basePoint;
	public float areaBombRate;
	public float lineBombRate;
	public float renewalBombRate;
	public float colorLockBombRate;
	public float timeBombRate;

	// データ初期化・基本UserParamからデータは取る
	public UserParam (UserData pUserData) {
		setUserParam (pUserData);
	}

	public void setUserParam (UserData pUserData) {
		basePoint 		= getBasePoint (pUserData._userParamsList[Const.PARAM_LV_BASE]);
		areaBombRate 	= getBombRate (pUserData._userParamsList[Const.PARAM_LV_AREA_BOMB]);
		lineBombRate 	= getBombRate (pUserData._userParamsList[Const.PARAM_LV_LINE_BOMB]);
		renewalBombRate = getBombRate (pUserData._userParamsList[Const.PARAM_LV_RENEWAL_BOMB]);
		colorLockBombRate = getBombRate (pUserData._userParamsList[Const.PARAM_LV_COLOR_LOCK_BOMB]);
		timeBombRate 	= getBombRate (pUserData._userParamsList[Const.PARAM_LV_TIME_BOMB]);
	}

	int getBasePoint (int pLv) {
		int value = 100;
		return value;
	}

	float START_RATE = 0.05f;
	float getBombRate (int pLv) {
		float value = 0.0f;
		if (pLv <= 0) {
			return 0;
		}
		if (pLv == 1) {
			value = START_RATE;
		} else {
			value = START_RATE + (float)pLv * START_RATE;
		}
		return value;
	}
}
