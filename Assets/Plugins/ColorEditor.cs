using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ColorEditor : MonoBehaviour {
	// αを設定する
	public static void setFade(GameObject pObj, float pAlpha) {
		if (pObj.transform.childCount > 0) { // 子オブジェクトがある場合は再帰処理
			foreach (Transform child in pObj.transform) {
				setFade (child.gameObject, pAlpha);
			}
		}

		Color aColor = getObjectColor (pObj);
		aColor = getAlphaColor (aColor, pAlpha);
		setColor(pObj, aColor);
	}

	public static Color getObjectColor(GameObject pObj) {
		Color aColor = new Color();
		Transform child = pObj.transform;
		if (child.GetComponent<SpriteRenderer> ()) {			
			SpriteRenderer sr = child.GetComponent<SpriteRenderer> ();
			aColor = sr.color;
		} else if (child.GetComponent<TextMesh> ()) {
			TextMesh tm = child.GetComponent<TextMesh> ();
			aColor = tm.color;
		} else if (child.GetComponent<Material> ()) {
			Material mt = child.GetComponent<Material> ();
			aColor = mt.color;
		}
		return aColor;
	}


	public static void paintColor(GameObject pObj, string pColorCode) {
		// 色情報を取得し、塗る
		Color aColor;
		if (ColorUtility.TryParseHtmlString("#"+pColorCode, out aColor)) {
			pObj.GetComponent<SpriteRenderer> ().color = aColor;
		}
	}
		
	static Color getAlphaColor (Color pColor, float pAlpha) {
		Color color = pColor;
		color.a = pAlpha;
		return color;
	}

	static void setColor(GameObject pObj, Color pColor) {
		Transform child = pObj.transform;
		if (child.GetComponent<SpriteRenderer> ()) {
			SpriteRenderer sr = child.GetComponent<SpriteRenderer> ();
			sr.color = pColor;
		}
		if (child.GetComponent<TextMesh> ()) {
			TextMesh tm = child.GetComponent<TextMesh> ();
			tm.color = pColor;
		}
		if (child.GetComponent<Material> ()) {
			Material mt = child.GetComponent<Material> ();
			mt.color = pColor;
		}
	}
}
