﻿using UnityEngine;
using System.Collections;

public class UserParam {
	// LV設定から確率などを計算する
	public PBClass.BigInteger basePoint;
	public float areaBombRate;
	public float lineBombRate;
	public float renewalBombRate;
	public float colorLockBombRate;
	public float timeBombRate;

	public UserMasterDataCtrl _userMasterDataCtrl;

	// データ初期化・基本UserParamからデータは取る
	public UserParam (UserData pUserData, UserMasterDataCtrl pUserMaseterDataCtrl) {
		_userMasterDataCtrl = pUserMaseterDataCtrl;
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

	// BaseValueの算出
	PBClass.BigInteger getBasePoint (int pLv) {
		PBClass.BigInteger value = 0;
		if (pLv > Const.CUSTOM_LV_MAX) { // LV.100以下ならMasterデータから取得
			PBClass.BigInteger lastValue = _userMasterDataCtrl.getBaseValue (Const.CUSTOM_LV_MAX);
			int addTimes = pLv - Const.CUSTOM_LV_MAX;
			PBClass.BigInteger unitValue = lastValue / Const.OVER_LV_VALUE_ADD_RATE;
			value = lastValue + (unitValue * addTimes);
		} else {
			value = _userMasterDataCtrl.getBaseValue (pLv);
		}
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
