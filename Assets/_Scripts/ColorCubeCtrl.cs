using UnityEngine;
using System.Collections;

public class ColorCubeCtrl : MonoBehaviour {
	#region SETTING
	public GameObject renewalObj;
	public GameObject restrictionObj;
	#endregion

	// RENEWAL OR RESTRICT
	int myType = Const.TYPE_RENEWAL;

	int colorCount;
	float validTime = 3f;

	public int getColorType () {
		return myType;
	}

	public int getRestrictColorCount () {
		return colorCount;
	}

	public float getRestrictValidTime () {
		return validTime;
	}

	public void setRestrictParameters (int pCount, float pValidTime) {
		myType = Const.TYPE_RESTRICT;
		renewalObj.SetActive (false);
		restrictionObj.SetActive (true);

		colorCount = pCount;
		validTime = pValidTime;
	}
}
