using UnityEngine;
using System.Collections;

public class TimeCubeCtrl : MonoBehaviour {
	float addTime;

	public void setAddTime (float pTime) {
		addTime = pTime;
	}

	public float getAddTime () {
		return addTime;
	}
}
