using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class MasterTableBase<T> where T : MasterBase, new() {
	protected List<T> masters;
	public List<T> All { get { return masters; } }

	public void Load(string filePath) 
	{
		var text = ((TextAsset)Resources.Load (filePath, typeof(TextAsset))).text;
		text = text.Trim ().Replace ("\r", "") + "\n";
		var lines = text.Split ('\n').ToList ();

		// header
		var headerElements = lines[0].Split(',');
		lines.RemoveAt (0); // header

		// body
		masters = new List<T>();
		foreach (var line in lines)
			ParseLine (line, headerElements);
	}

	private void ParseLine(string line, string[] headerElements)
	{
		var elements = line.Split (',');
		if (elements.Length == 1)
			return;
		if (elements.Length != headerElements.Length) { // 何かがおかしい
			Debug.LogWarning (string.Format ("can't load: {0}", line));
			return;
		}

		var param = new Dictionary<string, string> ();
		for (int i = 0; i < elements.Length; i++) {
			var element = elements [i];

			if (element.Contains("<comma>")) {
				element = element.Replace ("<comma>", ",");
			}

			    param.Add (headerElements [i], element);
		}
		var master = new T ();
		master.Load (param);
		masters.Add (master);
	}

	// TODO カンマ入りstringの処理をいずれ考えたい
	void ParseWierdLine (string[] elements) {
		
	}

	// MasterTableという文言を省いたファイルを取得
	// 例: UserMasterTable → User
	public string convertClassToFilePath(string className) {
		className = className.Replace ("MasterTable", "");
		className = className.LowerCamelToSnake ();
		Debug.Log (className);
		return className;
	}
}

public class MasterBase
{
	public void Load(Dictionary<string, string> param) 
	{
		foreach (string key in param.Keys)
			SetField (key, param [key]);
	}

	private void SetField(string key, string value) 
	{
		//Debug.Log ("key:" + key + " value:" + value);
		PropertyInfo propertyInfo = this.GetType ().GetProperty (key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		if (propertyInfo.PropertyType == typeof(int))
			propertyInfo.SetValue (this, int.Parse (value), null);
		else if (propertyInfo.PropertyType == typeof(string))
			propertyInfo.SetValue (this, value, null);
		else if (propertyInfo.PropertyType == typeof(double))
			propertyInfo.SetValue (this, double.Parse (value), null);
		else if (propertyInfo.PropertyType == typeof(float))
			propertyInfo.SetValue (this, float.Parse (value), null);
		else if (propertyInfo.PropertyType == typeof (PBClass.BigInteger))
			propertyInfo.SetValue (this, new PBClass.BigInteger (value), null);
	}

}