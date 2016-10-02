using UnityEngine;
using System.Collections;

public class ItemCtrl : MonoBehaviour {
	public TextMesh cardTextLevel;
	public TextMesh cardTextCost;
	public GameObject[] cardImageObjects;

	Item _MyItem;


	public void init (Item pItem)
	{
		_MyItem = pItem;

		// 表示設定
		setImage(_MyItem.id);

		cardTextLevel.text = ""+_MyItem.lv;
		cardTextCost.text = ""+_MyItem.cost;

		if (_MyItem.releaseFlg) {
			ColorEditor.setFade (this.gameObject, 1.0f);
		} else { 
			ColorEditor.setFade (this.gameObject, 0.6f);
		}
	}
	// IDが1以上ならCardImageObjectsの中から選んで表示する。(0はBASE)
	int SPECIAL_ID_IS_FROM = 1;
	void setImage(int pId) {
		if (_MyItem.id >= SPECIAL_ID_IS_FROM) {
			for (int i = 0; i < cardImageObjects.Length; i++) {
				if (i == (_MyItem.id - SPECIAL_ID_IS_FROM)) {
					cardImageObjects [i].SetActive (true);
					Vector3 pos = cardImageObjects [i].transform.localPosition;
					pos.z = -0.1f;
					cardImageObjects [i].transform.localPosition = pos;
				} else {
					cardImageObjects [i].SetActive (false);
				}
			}
		}
	}


	public Item getMyItem () {
		return _MyItem;
	}
}