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
		Dictionary<string, object> obj = new Dictionary<string, object> {
			{ "score", 		pResult.score.ToString() },
			{ "maxCombo", 	pResult.maxCombo },
			{ "comboCount", pResult.comboCount },
			{ "deleteCount",pResult.deleteCount},
			{ "killAllCount",pResult.killAllCount},
			{ "missCount", pResult.missCount}
		};

		SendCustomEvent ("GameResult", obj);
	}

	public void SendShop (Item pPurchasedItem) {
		Dictionary<string, object> obj = new Dictionary<string, object> {
			{"id", pPurchasedItem.id},
			{"name",pPurchasedItem.name},
			{"lv", pPurchasedItem.lv},
			{"cost",pPurchasedItem.cost.ToString()}
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
