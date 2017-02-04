using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimationManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// 演出用Cubeを30個生成し、リストに保管
	void init () {		

	}

	// 消えるCubeの色と場所を引数に
	public void playAnimation (Vector3 pPosition, GameCtrl.Colors pColor) {
		// poolから演出用Cubeをひとつ取り出す

		// 演出用Cubeの色と場所をを変更


		// Cubeをアニメーションさせる


		// アニメーションが終わったら、poolに戻す
	}
}
