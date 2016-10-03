using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Result {
	public PBClass.BigInteger score;
	public int maxCombo;
	public int comboCount;
	public int deleteCount;
	public int killAllCount;
	public int missCount;
	public List<int> itemCountList;

	public Result ()
	{
		score = 0;
		maxCombo = 0;
		deleteCount = 0;
		killAllCount = 0;
		missCount = 0;

		itemCountList = new List<int> ();
		initItemCount ();
	}

	// ItemCountListの初期化
	void initItemCount () {
		int itemCount = Const.TYPE_NORMAL + 1;
		for (int i = 0; i < itemCount; i++) {
			itemCountList.Add (0);
		}
	}

	// UserDataを出力
	public void showResultData () {
		string str = "score:"+score+ "\n"
			+"maxCombo:"+maxCombo+"\n"
			+"comboCount:"+comboCount+"\n"
			+ "deleteCount:"+deleteCount+"\n"
			+ "killAllCout:"+killAllCount+"\n"
			+ "missCount:"+missCount;
		Debug.Log (str);
	}
}