using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombCtrl : MonoBehaviour {
	public Const.BombType bombType;

	#region SETTING
	public GameObject x;
	public GameObject y;
	public GameObject plus;
	public GameObject multiple;
	#endregion

	public void setBombType (Const.BombType pType) {
		bombType = pType;

		switch (pType) {
		case Const.BombType.HORIZONTAL:
			x.SetActive (true);
			break;
		case Const.BombType.VERTICAL:
			y.SetActive (true);
			break;
		case Const.BombType.CROSS:
			x.SetActive (true);
			y.SetActive (true);
			break;
		case Const.BombType.PLUS:
			plus.SetActive (true);
			break;
		case Const.BombType.MULTIPLE:
			multiple.SetActive (true);
			break;
		case Const.BombType.AROUND:
			plus.SetActive (true);
			multiple.SetActive (true);
			break;
		default:
			break;
		}
	}

	public Const.BombType getBombType () {
		return bombType;
	}
}
