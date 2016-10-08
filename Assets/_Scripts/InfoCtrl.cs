using UnityEngine;
using System.Collections;

public class InfoCtrl : MonoBehaviour {
	public GameObject itemInfoGroup;
	public GameObject _itemInfoPrefab;

	float X_POS = -0.3f;
	float Y_POS = -1.2f;

	// Use this for initialization
	void Start () {
		initInformation ();
	}

	void initInformation () {
		// UserParamのリストでItemListを作成する
		for (int i = 0; i < 6; i++) {
			float xPos = X_POS;
			float yPos = (float)i * Y_POS;

			Vector3 pos = new Vector3 (xPos, yPos, -5);

			GameObject go = Instantiate (_itemInfoPrefab,
										 _itemInfoPrefab.transform.position,
										 _itemInfoPrefab.transform.rotation) as GameObject;
			go.name = _itemInfoPrefab.name;
			ItemInfo itemInfo = go.GetComponent<ItemInfo> ();
			go.transform.parent = itemInfoGroup.transform;
			go.transform.localPosition = pos;

			string msgCode = "item_info_" + (i + 1).ToString ("D2");
			LanguageCtrl _lngCtrl = GameCtrl.GetInstance ()._languageCtrl;
			itemInfo.setText (_lngCtrl.getMessageFromCode (msgCode));
			itemInfo.setImage (i);
		}
	}

	void actionBackBg () {
		this.gameObject.SetActive (false);
	}
}
