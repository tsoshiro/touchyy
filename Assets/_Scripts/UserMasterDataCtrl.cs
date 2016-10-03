using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserMasterDataCtrl : MonoBehaviour {
	List<UserMaster> _userMasterList = new List<UserMaster> ();

	// Use this for initialization
	void Start () {
		initMasterData ();

		printData ();
	}

	void initMasterData ()
	{
		var entityMasterTable = new UserMasterTable ();
		entityMasterTable.Load ();
		foreach (var entityMaster in entityMasterTable.All) {
			_userMasterList.Add (entityMaster);
		}
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
