using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopCtrl : MonoBehaviour
{
	List<ItemCtrl> itemCtrlList = new List<ItemCtrl> ();
	public GameObject _lvCardsGroup;
	public GameCtrl _gameCtrl;
	public GameObject _cardPrefab;
	public TextMesh _coin;

	float X_POS = 1.2f;
	float Y_POS = -2;

	public void Start () {
		int coin = _gameCtrl._userData.getUserDataInt (Const.PREF_COIN);
		_coin.text = "" + coin;
	}

	public void initUserItems () {
		if (itemCtrlList.Count > 0) {
			return;
		}
		UserData data = _gameCtrl._userData;
		for (int i = 1; i < data._userParamsList.Count; i++) {
			Item aItem = new Item (i, data.getUserDataInt (data._userParamsList [i]));

			float xPos = (i % 2 != 0) ? X_POS : -X_POS;
			float yPos = (float)((i - 1) / 2) * Y_POS;

			Vector3 pos = new Vector3 (xPos, yPos, 0);

			GameObject go = Instantiate (_cardPrefab,
										 pos,
			                             _cardPrefab.transform.rotation) as GameObject;
			go.GetComponent<ItemCtrl> ().init (aItem);
			go.transform.parent = _lvCardsGroup.transform;
			itemCtrlList.Add (go.GetComponent<ItemCtrl>());
		}
	}

	void reloadUserItems () {
		UserData data = _gameCtrl._userData;
		for (int i = 1; i < data._userParamsList.Count; i++) {
			Item aItem = new Item (i, data.getUserDataInt (data._userParamsList [i]));
			itemCtrlList [i].init (aItem);
		}
	}

	// PURCHASE
	void actionLvCard (Item item)
	{
		int cost = item.cost;
		int coin = _gameCtrl._userData.getUserDataInt (Const.PREF_COIN);
		if (coin >= cost) {
			purchase (item);
		} else {
			Debug.Log ("CAN'T BUY");
		}
	}

	void purchase (Item pItem) {
		string key = _gameCtrl._userData._userParamsList [pItem.id];
		_gameCtrl._userData.addTotalRecords (key, 1);
		_gameCtrl._userData.saveUserData ();
		reloadUserItems ();
	}
}