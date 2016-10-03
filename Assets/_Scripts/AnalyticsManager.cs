using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class AnalyticsManager {
	void SendCustomEvent (string pCustomEventName, Dictionary<string, object> pObject)
	{
		Analytics.CustomEvent (pCustomEventName, pObject);
	}

	public void SendGameResult (Result pResult) {
		//public int score;
		//public int maxCombo;
		//public int comboCount;
		//public int deleteCount;
		//public int killAllCount;
		//public int missCount;
		//public List<int> itemCountList;
		Dictionary<string, object> obj = new Dictionary<string, object> {
			{ "score", 		pResult.score },
			{ "maxCombo", 	pResult.maxCombo },
			{ "comboCount", pResult.comboCount },
			{ "deleteCount",pResult.deleteCount},
			{ "killAllCount",pResult.killAllCount},
			{ "missCount", pResult.missCount}
		};

		SendCustomEvent ("GameResult", obj);
	}

	public void SendShop (Item pPurchasedItem) {
		//public int id;
		//public string name;
		//public int lv;
		//public int cost;

		Dictionary<string, object> obj = new Dictionary<string, object> {
			{"id", pPurchasedItem.id},
			{"name",pPurchasedItem.name},
			{"lv", pPurchasedItem.lv},
			{"cost",pPurchasedItem.cost}
		};

		SendCustomEvent ("PurchaseRecord", obj);
	}

	public void SendCounterEvent (string pEventName, int pValue) {
		Dictionary<string, object> obj = new Dictionary<string, object> {
			{"counter", pValue}
		};

		SendCustomEvent (pEventName, obj);
	}
}
