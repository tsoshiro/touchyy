using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageCtrl : MonoBehaviour {
	public enum LanguageSetting
	{
		JP,
		EN
	};
	public LanguageSetting _lang;
	public bool isLanguageSettingDebug = false;

	List<LocalizationMaster> _localizationMasterList = new List<LocalizationMaster> ();

	public void initLocalization () 
	{
		if (isLanguageSettingDebug) {
			//
		} else {
			if (Application.systemLanguage == SystemLanguage.Japanese) {
				_lang = LanguageSetting.JP;
			} else {
				_lang = LanguageSetting.EN;
			}			
		}
		initMasterData ();
	}

	public void initMasterData ()
	{
		var entityMasterTable = new LocalizationMasterTable ();
		entityMasterTable.Load ();
		foreach (var entityMaster in entityMasterTable.All) {
			_localizationMasterList.Add (entityMaster);
		}
	}

	public string getMessageFromCode (string pCode) {
		string str = "";
		for (int i = 0; i < _localizationMasterList.Count; i++) {
			LocalizationMaster lMaster = _localizationMasterList [i];
			if (lMaster.MESSAGE_CODE == pCode) {
				str = (_lang == LanguageSetting.JP)	? lMaster.JP : lMaster.EN;
				break;
			}
		}
		return str;
	}
}
