﻿using UnityEngine;
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

			float xPos = (i % 2 == 0) ? X_POS : -X_POS;
			float yPos = (float)((i - 1) / 2) * Y_POS;

			Vector3 pos = new Vector3 (xPos, yPos, -5);

			GameObject go = Instantiate (_cardPrefab,
			                             _cardPrefab.transform.position,
			                             _cardPrefab.transform.rotation) as GameObject;
			go.name = _cardPrefab.name;
			go.GetComponent<ItemCtrl> ().init (aItem);
			go.GetComponent<ButtonCtrl> ().parentObj = this.gameObject;
			go.transform.parent = _lvCardsGroup.transform;
			go.transform.localPosition = pos;

			itemCtrlList.Add (go.GetComponent<ItemCtrl>());
		}
	}

	void reloadUserItems () {
		UserData data = _gameCtrl._userData;
		for (int i = 0; i < data._userParamsList.Count - 1; i++) {
			Item aItem = new Item (i + 1, data.getUserDataInt (data._userParamsList [i + 1]));
			itemCtrlList [i].init (aItem);
		}

		_coin.text = "" + data.getUserDataInt (Const.PREF_COIN);
	}

	// PURCHASE
	void actionLvCard (GameObject obj)
	{
		Item item = obj.GetComponent<ItemCtrl> ().getMyItem ();
		int cost = item.cost;
		int coin = _gameCtrl._userData.getUserDataInt (Const.PREF_COIN);
		if (coin >= cost) {
			_gameCtrl._userData.setUserData (Const.PREF_COIN, (coin - cost));
			purchase (item);
			_gameCtrl._audioMgr.play (Const.SE_UP);
		} else {
			Debug.Log ("CAN'T BUY");
			_gameCtrl._audioMgr.play (Const.SE_BAD);
		}
	}

	void purchase (Item pItem) {
		string key = _gameCtrl._userData._userParamsList [pItem.id];
		_gameCtrl._userData.addTotalRecords (key, 1);
		_gameCtrl._userData.saveUserData ();

		reloadUserItems ();
	}

	// GO BACK
	void actionBackBtn () {
		_gameCtrl.backFromShop ();
	}
}