using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombCtrl : MonoBehaviour {
	public Const.BombType bombType;

	public void setBombType (Const.BombType pBombType) {
		bombType = pBombType;

		if (bombType == Const.BombType.CROSS) {
			return;
		} else if (bombType == Const.BombType.HORIZONTAL) {
			for (int i = 0; i < transform.childCount; i++) {
				Transform t = transform.GetChild (i);
				if (t.name == "y") {
					t.gameObject.SetActive (false);
				}
			}
		} else {
			for (int i = 0; i < transform.childCount; i++) {
				Transform t = transform.GetChild (i);
				if (t.name == "x") {
					t.gameObject.SetActive (false);
				}
			}			
		}
	}

	public Const.BombType getBombType () {
		return bombType;
	}
}
