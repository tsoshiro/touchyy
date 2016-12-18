using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class TestCubeColor {

	[Test]
	public void CheckSpawnDifferentColor()
	{
		// 消えた色と生成された新しい色が被らないようにする。
		GameCtrl.Colors befColor;

		// 全色チェック
		for (int i = 0; i < (int)GameCtrl.Colors.NUM; i++) {
			befColor = (GameCtrl.Colors)i;
			CheckEachColorTenTimes (befColor);
		}
	}

	void CheckEachColorTenTimes (GameCtrl.Colors befColor) {
		int n = 10; // 各色ごとに10回ずつ試行
		GameCtrl.Colors aftColor;
		List<GameCtrl.Colors> restrictColors = new List<GameCtrl.Colors> ();

		// デフォルト
		do {
			aftColor = GetColor (false, restrictColors, befColor);
			Assert.AreNotEqual (befColor, aftColor);
			n++;
		} while (n <= 10);

		// 色制限時

	}

	// 色取得メソッド
	GameCtrl.Colors GetColor (bool pIsRestrictionValid,
	                          List<GameCtrl.Colors> pRestrictColors,
	                          GameCtrl.Colors pBefColor) 
	{
		return (GameCtrl.Colors)GameCtrl.GetInstance ().decideColor (pIsRestrictionValid,
		                                                             pRestrictColors,
		                                                             pBefColor);
	}
}
