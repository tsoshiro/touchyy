using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombItemCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;

	// Use this for initialization
	void Start () {
		_gameCtrl = this.gameObject.GetComponent<GameCtrl> ();
	}

	public List<CubeCtrl> deletingCubes = new List<CubeCtrl> ();

	public void tapBomb (int pId, Const.BombType pType) {
		Debug.Log ("pId:" + pId + " pType:" + pType);
		switch (pType) {
		case Const.BombType.HORIZONTAL:
			getHorizontalCubes (pId);
			break;
		case Const.BombType.VERTICAL:
			getVerticalCubes (pId);
			break;
		case Const.BombType.CROSS:
			getHorizontalCubes (pId);
			getVerticalCubes (pId);
			break;
		case Const.BombType.SIDE:
			break;
		case Const.BombType.UPDOWN:
			break;
		case Const.BombType.AROUND:
			break;
		default:
			break;
		}
		omitDuplicatedCubes ();
		deleteCubes ();
		resetDeleteCubes ();
	}

	void getHorizontalCubes (int pId) {
		int startValue = pId % _gameCtrl.HEIGHT;
		if (startValue == 0) {
			startValue = _gameCtrl.HEIGHT;
		}

		for (int i = 0; i < _gameCtrl.WIDTH; i++) {
			int deletingId = startValue + (i * _gameCtrl.HEIGHT);
			deletingCubes.Add (_gameCtrl.cubes[deletingId - 1]);
		}
	}

	void getVerticalCubes (int pId) {
		int value = (pId - 1) / _gameCtrl.HEIGHT;
		int startId = ( value * _gameCtrl.HEIGHT ) + 1;
		for (int i = startId; i < startId + _gameCtrl.HEIGHT; i++) {
			deletingCubes.Add (_gameCtrl.cubes [i - 1]);
		}
	}

	void omitDuplicatedCubes () {
		List<CubeCtrl> tmpCubeList = new List<CubeCtrl> ();
		for (int i = 0; i < deletingCubes.Count; i++) {
			if (!tmpCubeList.Contains (deletingCubes [i])) {
				tmpCubeList.Add (deletingCubes [i]);
			}
		}

		deletingCubes = tmpCubeList;
		for (int i = 0; i < deletingCubes.Count; i++) {
			Debug.Log ("omitDuplicateCubes\nDELETING CUBES[" + i + "]:" + deletingCubes [i].getId());
		}
	}

	void deleteCubes () {
		for (int i = 0; i < deletingCubes.Count; i++) {
			deletingCubes [i].vanish ();
		}
	}

	void resetDeleteCubes () {
		deletingCubes.Clear ();
	}

}
