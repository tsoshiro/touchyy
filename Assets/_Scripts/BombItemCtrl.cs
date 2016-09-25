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
		case Const.BombType.PLUS:
			getPlusCubes (pId);
			break;
		case Const.BombType.MULTIPLE:
			getMultipleCubes (pId);
			break;
		case Const.BombType.AROUND:
			getPlusCubes (pId);
			getMultipleCubes (pId);
			break;
		default:
			break;
		}
		omitDuplicatedCubes ();
		deleteCubes ();
		resetDeleteCubes ();
	}

	// HORIZONTAL
	void getHorizontalCubes (int pId) 
	{
		int startValue = pId % _gameCtrl.HEIGHT;
		if (startValue == 0) {
			startValue = _gameCtrl.HEIGHT;
		}

		for (int i = 0; i < _gameCtrl.WIDTH; i++) {
			int deletingId = startValue + (i * _gameCtrl.HEIGHT);
			deletingCubes.Add (_gameCtrl.cubes[deletingId - 1]);
		}
	}

	// VERTICAL
	void getVerticalCubes (int pId) 
	{
		int value = (pId - 1) / _gameCtrl.HEIGHT;
		int startId = ( value * _gameCtrl.HEIGHT ) + 1;
		for (int i = startId; i < startId + _gameCtrl.HEIGHT; i++) {
			deletingCubes.Add (_gameCtrl.cubes [i - 1]);
		}
	}

	void getPlusCubes (int pId)
	{
		int u = pId + 1;
		int d = pId - 1;
		int r = pId + _gameCtrl.HEIGHT;
		int l = pId - _gameCtrl.HEIGHT;

		int colNum = (pId - 1) / _gameCtrl.HEIGHT + 1;
		int rowNum = (pId % _gameCtrl.HEIGHT == 0) ? _gameCtrl.HEIGHT : pId % _gameCtrl.HEIGHT;

		deletingCubes.Add (_gameCtrl.cubes [pId - 1]);

		if (colNum != 1) {
			deletingCubes.Add (_gameCtrl.cubes [l - 1]);
		}
		if (colNum != _gameCtrl.WIDTH) {
			deletingCubes.Add (_gameCtrl.cubes [r - 1]);
		}
		if (rowNum != 1) {
			deletingCubes.Add (_gameCtrl.cubes [d - 1]);
		}
		if (rowNum != _gameCtrl.HEIGHT) {
			deletingCubes.Add (_gameCtrl.cubes [u - 1]);
		}
	}

	void getMultipleCubes (int pId)
	{
		int ul = pId + 1 - _gameCtrl.HEIGHT;
		int dl = pId - 1 - _gameCtrl.HEIGHT;
		int ur = pId + 1 + _gameCtrl.HEIGHT;
		int dr = pId - 1 + _gameCtrl.HEIGHT;

		int colNum = (pId - 1) / _gameCtrl.HEIGHT + 1;
		int rowNum = (pId % _gameCtrl.HEIGHT == 0) ? _gameCtrl.HEIGHT : pId % _gameCtrl.HEIGHT;

		deletingCubes.Add (_gameCtrl.cubes [pId - 1]);

		if (colNum != 1) {
			if (rowNum != 1) {
				deletingCubes.Add (_gameCtrl.cubes [dl - 1]);
			}
			if (rowNum != _gameCtrl.HEIGHT) {
				deletingCubes.Add (_gameCtrl.cubes [ul - 1]);
			}
		}
		if (colNum != _gameCtrl.WIDTH) {
			if (rowNum != 1) {
				deletingCubes.Add (_gameCtrl.cubes [dr - 1]);
			}
			if (rowNum != _gameCtrl.HEIGHT) {
				deletingCubes.Add (_gameCtrl.cubes [ur - 1]);
			}
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
