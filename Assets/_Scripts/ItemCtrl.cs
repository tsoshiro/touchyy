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
	}
}