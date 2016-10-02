using UnityEngine;
using System.Collections;

public class Item {
	public int id;
	public string name;
	public int lv;
	public int cost;
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

	int getCostFromId (int pId, int pLv)
	{
		int value = 0;
		if (pLv >= Const.CUSTOM_LV_MAX) {
			value = Const.COST_BASE + (300 * pLv);
		} else {

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