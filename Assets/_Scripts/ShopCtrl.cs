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

	float X_POS = 1.34f;
	float Y_POS = -2;
	//	-1.34
	// -0.1

	// 1.2f
	// -2

	public InfoCtrl _infoCtrl;

	public void Start () {
		_coin.text = new IntValueConverter ().FixBigInteger ( _gameCtrl._userData.coin);
	}

	public void initUserItems () {
		_coin.text = new IntValueConverter ().FixBigInteger (_gameCtrl._userData.coin);
		if (itemCtrlList.Count > 0) {
			return;
		}

		UserData data = _gameCtrl._userData;

		// UserParamのリストでItemListを作成する
		for (int i = 0; i < data._userParamsList.Count; i++) {
			Item aItem = new Item (i, data._userParamsList[i]);

			float xPos = (i % 2 != 0) ? X_POS : -X_POS;
			float yPos = (float)(i / 2) * Y_POS;

			Vector3 pos = new Vector3 (xPos, yPos, -5);

			GameObject go = Instantiate (_cardPrefab,
			                             _cardPrefab.transform.position,
			                             _cardPrefab.transform.rotation) as GameObject;
			go.name = _cardPrefab.name;
			go.GetComponent<ItemCtrl> ().init (aItem);
			go.GetComponent<ButtonCtrl> ().parentObj = this.gameObject;
			go.transform.parent = _lvCardsGroup.transform;
			go.transform.localPosition = pos;

			// Animation用Componentを追加
			go.AddComponent<AppealAnimation> ();

			itemCtrlList.Add (go.GetComponent<ItemCtrl>());
		}
	}

	void reloadUserItems () {
		UserData data = _gameCtrl._userData;

		for (int i = 0; i < data._userParamsList.Count; i++) {
			Item aItem = new Item (i, data._userParamsList [i]);
			itemCtrlList [i].init (aItem);
		}

		_coin.text = new IntValueConverter ().FixBigInteger (_gameCtrl._userData.coin);
	}

	// PURCHASE
	void actionLvCard (GameObject obj)
	{
		Item item = obj.GetComponent<ItemCtrl> ().getMyItem ();
		PBClass.BigInteger cost = item.cost;
		PBClass.BigInteger coin = _gameCtrl._userData.coin; ;
		if (coin >= cost) {
			_gameCtrl.spendCoin (cost);
			levelUp (item);

			// 演出
			obj.GetComponent<AppealAnimation> ().playOnce ();
			_gameCtrl._audioMgr.play (Const.SE_UP);
		} else {
			Debug.Log ("CAN'T BUY");
			_gameCtrl._audioMgr.play (Const.SE_NO);
		}
	}

	void levelUp (Item pItem) {
		new AnalyticsManager ().SendShop (pItem);

		_gameCtrl._userData._userParamsList [pItem.id]++;

		_gameCtrl._userData.save ();

		_gameCtrl.reloadUserData ();
		reloadUserItems ();
	}

	// GO BACK
	void actionBackBtn () {
		_gameCtrl._audioMgr.play (Const.SE_BUTTON);
		_gameCtrl.backFromShop ();
	}

	void actionInfoBtn () {
		_gameCtrl._audioMgr.play (Const.SE_BUTTON);

		_infoCtrl.gameObject.SetActive (true);
		//_infoCtrl.gameObject.i
		iTween.FadeTo (_infoCtrl.gameObject,
		               iTween.Hash ("a", 1.0f, "time", 0.2f));
	}

	void actionResetDataBtn ()
	{
		_gameCtrl._userData.reset ();

		reloadUserItems ();

		_gameCtrl.reloadUserData ();
	}

	public bool checkIsAffordSomething () {
		for (int i = 0; i < itemCtrlList.Count; i++) {
			if (itemCtrlList [i].getMyItem ().cost <= _gameCtrl._userData.coin ) {
				return true;
			}
		}
		return false;
	}
}