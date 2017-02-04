using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimationManager : MonoBehaviour {
	public GameObject _cubeFx; // 演出用Cubeのプレハブ
	public GameObject _cubeFxPoolContainer; // CubeFxPoolのGameObjectを入れておく

	int cubeFxCount = 30;
	CubeColorChanger[] cubeFxPool; // Pool
	int counter = 0; // 選出用カウンタ
	Vector3 cubeFxPos;
	Vector3 cubeFxOriginalScale;
	float scaleRate = 1.2f;
	float scaleTime = 0.3f;
	float fadeTime = 0.3f;

	// 演出用Cubeを30個生成し、リストに保管
	public void init () {
		cubeFxOriginalScale = _cubeFx.transform.localScale;
		cubeFxPos = _cubeFxPoolContainer.transform.position;

		cubeFxPool = new CubeColorChanger[cubeFxCount];
		for (int i = 0; i < cubeFxCount; i++) {
			GameObject go = (GameObject)Instantiate (_cubeFx, cubeFxPos, _cubeFx.transform.rotation);
			go.transform.parent = _cubeFxPoolContainer.transform;
			go.name = "FX_" + i.ToString ("D2");
			cubeFxPool [i] = go.GetComponent<CubeColorChanger>();
		}
	}

	// 消えるCubeの色と場所を引数に
	public void playAnimation (Vector3 pPosition, GameCtrl.Colors pColor) {
		// poolから演出用Cubeをひとつ取り出す
		CubeColorChanger cubeFx = cubeFxPool [counter];
		GameObject cubeFxObj = cubeFx.gameObject;

		// 演出用Cubeの色と場所を変更
		cubeFx.setColor ((int)pColor);

		Vector3 pos = pPosition;
		pos.z -= 0.5f;
		cubeFx.transform.position = pos;

		// Cubeをアニメーションさせる
		iTween.ScaleTo (cubeFxObj, iTween.Hash ("scale", cubeFxOriginalScale * scaleRate, "time", scaleTime));
		iTween.FadeTo (cubeFxObj, iTween.Hash ("alpha", 0, "time", fadeTime,
		                                    // 演出が終わったらPoolに戻す
		                                    "oncomplete", "returnToPool",
		                                    "oncompletetarget", this.gameObject,
		                                    "oncompleteparams", counter));

		// カウンタを増やす
		counter++;

		// カウンタがcubeFxPoolの最後の要素以上になったらリセット
		if (counter >= cubeFxPool.Length) { 
			counter = 0;
		};
	}

	void returnToPool (int pCounter) {
		GameObject cubeFxObj = cubeFxPool [pCounter].gameObject;

		// 位置を戻す
		cubeFxObj.transform.position = cubeFxPos;

		// サイズを戻す
		cubeFxObj.transform.localScale = cubeFxOriginalScale;

		// 色を戻す
		ColorEditor.setFade (cubeFxObj, 1.0f, true);
	}
}