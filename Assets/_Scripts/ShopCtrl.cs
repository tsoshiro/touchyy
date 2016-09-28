using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopCtrl : MonoBehaviour
{
	List<Item> itemList = new List<Item> ();

	// Use this for initialization
	void Start ()
	{
		initUserItems ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void initUserItems () {
		
	}

	void levelUp (int pId)
	{

	}
}

public class Item {
	int id;
	int lv;
	int cost;

	public Item (int pId, int pLv) {
		id = pId;
		lv = pLv;
	}

	int getCostFromId (int pId, int pLv) {
		int value = 0;
		if (pLv >= Const.CUSTOM_LV_MAX) {
			value = Const.COST_BASE + (300 * pLv);
		} else {
			
		}

		return value;
	}

	bool isReleased () {
		if (lv <= 0) {
			return false;
		}
		return true;
	}
}