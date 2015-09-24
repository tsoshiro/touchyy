using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombCtrl : MonoBehaviour {
	public List<CubeCtrl> explodeTargets;
	CubeCtrl _cubeCtrl;

	int idBk;

	void Start() {
		_cubeCtrl = GetComponent<CubeCtrl>();
		idBk = _cubeCtrl.getId();
	}

	public void explode(int id) {
		List<int> pIds = new List<int>();

		int pId = id;
		// JUDGE
		if (isTop(pId)) {
			if (isLeft(pId)) {
				pIds.Add (pId + 5);
				pIds.Add (pId + 4);
				pIds.Add (pId - 1);
			} else if (isRight(pId)) {
				pIds.Add (pId - 5);
				pIds.Add (pId - 6);
				pIds.Add (pId - 1);
			}
		} else if (isBottom(pId)) {
			if (isLeft(pId)) {
				pIds.Add (pId + 5);
				pIds.Add (pId + 6);
				pIds.Add (pId + 1);
			} else if (isRight(pId)) {
				pIds.Add (pId - 5);
				pIds.Add (pId - 4);
				pIds.Add (pId + 1);
			}
		} else if (isLeft(pId)) {
			pIds.Add (pId + 4);
			pIds.Add (pId + 5);
			pIds.Add (pId + 6);
			pIds.Add (pId - 1);
			pIds.Add (pId + 1);
		} else if (isRight(pId)) {
			pIds.Add (pId - 4);
			pIds.Add (pId - 5);
			pIds.Add (pId - 6);
			pIds.Add (pId - 1);
			pIds.Add (pId + 1);
		} else {
			pIds.Add (pId + 4);
			pIds.Add (pId + 5);
			pIds.Add (pId + 6);
			pIds.Add (pId - 4);
			pIds.Add (pId - 5);
			pIds.Add (pId - 6);
			pIds.Add (pId - 1);
			pIds.Add (pId + 1);
		}

		// GET CUBES
		setTargets(pIds);

		// CALL THEM
		for (int i = 0; i < explodeTargets.Count; i++){
			if (explodeTargets[i] != null) {
				explodeTargets[i].vanish();
			}
		};
	}

	void setTargets(List<int> pIdList) {
		Debug.Log ("id :" + idBk);

		GameCtrl gameCtrl = _cubeCtrl.getGameCtrl();

		explodeTargets = new List<CubeCtrl>();
		for (int i = 0; i < pIdList.Count; i++) {
			explodeTargets.Add(gameCtrl.cubes[pIdList[i]-1]);
		}

	}

//	enum GET_ID_OF { UPPER, UPPER_RIGHT, RIGHT, BOTTOM_RIGHT, BOTTOM, BOTTOM_LEFT, LEFT, UPPER_LEFT};
//	GET_ID_OF targetDir;
//
//	int getId(GET_ID_OF pDir) {
//		switch (pDir) {
//		case GET_ID_OF.UPPER:
//
//		}
//	}

	bool isTop(int pId) {
		if (pId % 5 == 0) {
			return true;
		}
		return false;
	}

	bool isBottom(int pId) {
		if (pId % 5 == 1) {
			return true;
		}
		return false;
	}

	bool isLeft(int pId) {
		if (pId <= 5) {
			return true;
		}
		return false;
	}

	bool isRight(int pId) {
		if (pId >= 21) {
			return true;
		}
		return false;
	}
}
