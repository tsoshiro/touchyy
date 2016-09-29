using UnityEngine;
using System.Collections;

public class ItemCtrl : MonoBehaviour {
	public TextMesh cardText;
	Item _MyItem;

	public void init (Item pItem)
	{
		_MyItem = pItem;

		cardText.text = _MyItem.name;
		cardText.text += "\nLV." + _MyItem.lv;
		cardText.text += "\n" + _MyItem.cost;

		if (_MyItem.releaseFlg) {
			gameObject.GetComponentInChildren<SpriteRenderer> ().color = Color.white;
		} else { 
			gameObject.GetComponentInChildren<SpriteRenderer> ().color = Color.gray;
		}
	}

	public Item getMyItem () {
		return _MyItem;
	}
}