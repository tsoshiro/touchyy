using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColorChanger : MonoBehaviour {
	// 色の変更のみを行うクラス

	public void setColor (int pColor)
	{
		Transform colorSprite = this.transform.FindChild ("colorSprite");
		ColorEditor.setColorFromColorCode (colorSprite.gameObject, GameCtrl.GetInstance().colorCodes [pColor]);
	}
}
