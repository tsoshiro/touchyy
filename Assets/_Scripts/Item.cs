using UnityEngine;
using System.Collections;

public class Item {
	public int id;
	public string name;
	public int lv;
	public PBClass.BigInteger cost;
	public bool releaseFlg;
	public string [] names = {
		"BASE",
		"AREA",
		"LINE",
		"RENEW",
		"LOCK",
		"TIME"
	};
	public Item (int pId, int pLv) {
		id = pId;
		lv = pLv;

		name = names[id];
		cost = getCostFromId (id, lv);
		releaseFlg = isReleased ();
	}

	// コストの算出
	PBClass.BigInteger getCostFromId (int pId, int pLv)
	{
		Debug.Log ("getCostFromId(" + pId + "," + pLv + ")");
		PBClass.BigInteger value = 0;

		if (pLv > Const.CUSTOM_LV_MAX) {
			PBClass.BigInteger lastValue = getCostFromId (pId, Const.CUSTOM_LV_MAX);
			int addTimes = pLv - Const.CUSTOM_LV_MAX;
			PBClass.BigInteger unitValue = lastValue * Const.OVER_LV_COST_ADD_RATE;
			value = lastValue + (unitValue / addTimes);
		} else {
			value = getCostFromIdAndCSV (pId, pLv);
		}
		Debug.Log ("value:" + value);
		return value;
	}

	PBClass.BigInteger getCostFromIdAndCSV (int pId, int pLv) {
		PBClass.BigInteger value = 0;
		UserParam up = GameCtrl.GetInstance ()._userParam; 
		switch (pId) {
		case Const.PARAM_LV_BASE:
			value = up._userMasterDataCtrl.getCostBase (pLv);
			break;
		case Const.PARAM_LV_AREA_BOMB:
		case Const.PARAM_LV_LINE_BOMB:
			value = up._userMasterDataCtrl.getCostBomb (pLv);
			break;
		case Const.PARAM_LV_RENEWAL_BOMB:
		case Const.PARAM_LV_COLOR_LOCK_BOMB:
			value = up._userMasterDataCtrl.getCostColor (pLv);
			break;
		case Const.PARAM_LV_TIME_BOMB:
			value = up._userMasterDataCtrl.getCostTime (pLv);
			break;
		}
		return value;
	}

	bool isReleased ()
	{
		if (lv <= 0) {
			return false;
		}
		return true;
	}
}