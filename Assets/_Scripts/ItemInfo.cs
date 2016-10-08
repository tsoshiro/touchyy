using UnityEngine;
using System.Collections;

public class ItemInfo : MonoBehaviour {
	public TextMesh explainText;
	public GameObject [] itemImages;

	public void setText (string pMessage) {
		explainText.text = pMessage;
	}

	// IDが1以上ならCardImageObjectsの中から選んで表示する。(0はBASE)
	int SPECIAL_ID_IS_FROM = 1;
	public void setImage (int pId)
	{
		if (pId >= SPECIAL_ID_IS_FROM) {
			for (int i = 0; i < itemImages.Length; i++) {
				if (i == (pId - SPECIAL_ID_IS_FROM)) {
					itemImages [i].SetActive (true);
					Vector3 pos = itemImages [i].transform.localPosition;
					pos.z = -0.1f;
					itemImages [i].transform.localPosition = pos;
				} else {
					itemImages [i].SetActive (false);
				}
			}
		}
	}
}
