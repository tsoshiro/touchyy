using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserMasterDataCtrl : MonoBehaviour {
	List<UserMaster> _userMasterList = new List<UserMaster> ();

	public void initMasterData ()
	{
		var entityMasterTable = new UserMasterTable ();
		entityMasterTable.Load ();
		foreach (var entityMaster in entityMasterTable.All) {
			_userMasterList.Add (entityMaster);
		}
	}

	public PBClass.BigInteger getBaseValue (int pLv) {
		UserMaster um = _userMasterList [pLv - 1];
		return um.VALUE;
	}

	public PBClass.BigInteger getCostBase (int pLv)
	{
		UserMaster um = _userMasterList [pLv - 1];
		return um.COST_BASE;
	}

	public PBClass.BigInteger getCostBomb (int pLv) {
		// Base以外については、未開放(=LV.0とLV.1は同じ値)
		pLv = (pLv == 0) ? 1 : pLv;
		UserMaster um = _userMasterList [pLv - 1];
		return um.COST_BASE;
	}

	public PBClass.BigInteger getCostColor (int pLv) {
		// Base以外については、未開放(=LV.0とLV.1は同じ値)
		pLv = (pLv == 0) ? 1 : pLv;
		UserMaster um = _userMasterList [pLv - 1];
		return um.COST_COLOR;
	}

	public PBClass.BigInteger getCostTime (int pLv)
	{
		// Base以外については、未開放(=LV.0とLV.1は同じ値)
		pLv = (pLv == 0) ? 1 : pLv;
		UserMaster um = _userMasterList [pLv - 1];
		return um.COST_TIME;
	}

	public PBClass.BigInteger getFree (int pLv) {
		UserMaster um = _userMasterList [pLv - 1];
		return um.FREE;
	}

	public PBClass.BigInteger getReward (int pLv)
	{
		UserMaster um = _userMasterList [pLv - 1];
		return um.REWARD;
	}

	void printData () {
		for (int i = 0; i < _userMasterList.Count; i++) {
			UserMaster um = _userMasterList [i];
			Debug.Log ("LV:" + um.LV
					  + " COST_BASE:" + um.COST_BASE
					  + " FREE: " + um.FREE
					  + " REWARD: " + um.REWARD
					  + " COST_BOMB: " + um.COST_BOMB
					  + " COST_COLOR: " + um.COST_COLOR
					  + " COST_TIME: " + um.COST_TIME);
		}
	}
}