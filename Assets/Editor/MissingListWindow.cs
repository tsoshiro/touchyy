using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MissingListWindow : EditorWindow {
	private static string[] extensions = {".prefab", ".mat", ".controller", ".cs", ".shader", ".mask", ".asset"};
	private static List<AssetParameterData> missingList = new List<AssetParameterData>();
	private Vector2 scrollPos;

	[MenuItem("Assets/MissingList")]
	private static void ShowMissingList() {
		// Missingがあるアセットを検索
		Search ();

		// ウィンドウを表示
		var window = GetWindow<MissingListWindow>();
		window.minSize = new Vector2(900, 300);
	}

	[MenuItem("Assets/MissingList in Scene")]
	private static void ShowMissingListInScene() {
		GetAllObjectsInScene();


//		SearchObjectInScene();
//
//		// ウィンドウを表示
//		var window = GetWindow<MissingListWindow>();
//		window.minSize = new Vector2(900, 300);
	}

	/// <summary>
	/// Missingがあるアセットを検索
	/// </summary>
	private static void Search() {
		// 全てのアセットのファイルパスを取得
		string[] allPaths = AssetDatabase.GetAllAssetPaths();
		int length = allPaths.Length;

		for (int i = 0; i < length; i++) {
			// プログレスバーを表示
//			EditorUtility.DisplayProgressBar("Search Missing", string.Format("{0}/{1}", i+1, length), (float)i / length);
			EditorUtility.DisplayProgressBar("Search Missing", (i+1)+"/"+length, (float)i / length);

			if (extensions.Contains (Path.GetExtension (allPaths [i]))) {
				SearchMissing (allPaths [i]);
			}
		}

		// プログレスバーを消す
		EditorUtility.ClearProgressBar();
	}

	/// <summary>
	/// 指定アセットにMissingのプロパティがあれば、それをmissingListに追加する
	/// </summary>
	/// <param name="path">Path.</param>
	private static void SearchMissing(string path) {
		// 指定パスのアセットを全て取得
		IEnumerable<UnityEngine.Object> assets = AssetDatabase.LoadAllAssetsAtPath(path);

		// 各アセットについて、Missingのプロパティがあるかチェック
		foreach (UnityEngine.Object obj in assets) {
			if (obj == null) {
				continue;
			}
			if (obj.name == "Deprecated EditorExtensionImpl") {
				continue;
			}

			// SerializedObjectを通してアセットのプロパティを取得する
			SerializedObject sobj = new SerializedObject(obj);
			SerializedProperty property = sobj.GetIterator();

			while (property.Next(true)) {
				// プロパティの種類がオブジェクト（アセット）への参照で、
				// その参照がnullなのにもかかわらず、参照先インスタンスIDが0でないものはMissing状態！
				if (property.propertyType == SerializedPropertyType.ObjectReference &&
				    property.objectReferenceValue == null &&
				    property.objectReferenceInstanceIDValue != 0) {

					// Missing状態のプロパティリストに追加する
					missingList.Add(new AssetParameterData() {
						obj = obj,
						path = path,
						property = property
					});
				}
			}
		}
	}
	
	/// <summary>
	/// Missingのリストを表示
	/// </summary>
	private void OnGUI() {
		// 列見出し
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Asset", GUILayout.Width(200));
		EditorGUILayout.LabelField("Property", GUILayout.Width(200));
		EditorGUILayout.LabelField("Path");
		EditorGUILayout.EndHorizontal();

		// リスト表示
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		foreach (AssetParameterData data in missingList) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.ObjectField(data.obj, data.obj.GetType (), true, GUILayout.Width(200));
			EditorGUILayout.TextField(data.property.name, GUILayout.Width(200));
			EditorGUILayout.TextField(data.path);
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView();
	}

	private static void GetAllObjectsInScene() {
		string list = "GameObjects in scene:";

		Object[] objs = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject));
		for (int i = 0; i < objs.Length; i++) {
			string path = AssetDatabase.GetAssetOrScenePath(objs[i]);
			if (path.Contains(".unity")) {
				list += "\n\t"+objs[i].name;
			}
		}
		Debug.Log(list);
	}

	private static void SearchObjectInScene() {
		Object[] objs = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject));
		int length = objs.Length;
		int count = 0;
		// Typeで指定した型の全てのオブジェクトを配列で取得し,その要素数分繰り返す.
		for (int i = 0; i < length; i++) {
			EditorUtility.DisplayProgressBar("Search Missing in Scene", (i+1)+"/"+length, (float)i / length);

			string path = AssetDatabase.GetAssetOrScenePath(objs[i]);
			bool isScene = path.Contains(".unity");
			if (isScene) {
				if (objs[i] is GameObject) {

				}
//				Debug.Log("name:"+objs[i].name);
				checkObject((GameObject)objs[i], path);
			} else {
//				Debug.Log("path:"+path+" name:"+((GameObject)objs[i]).name);
			}
			count++;
		}
//
//
//
//		foreach (GameObject obj in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)))
//		{
//			EditorUtility.DisplayProgressBar("Search Missing in Scene", (count+1)+"/"+length, (float)count / length);
//
//			// アセットからパスを取得.シーン上に存在するオブジェクトの場合,シーンファイル（.unity）のパスを取得.
//			string path = AssetDatabase.GetAssetOrScenePath(obj);
//
//			// シーン上に存在するオブジェクトかどうか文字列で判定.
//			bool isScene = path.Contains(".unity");
//			// シーン上に存在するオブジェクトならば処理.
//			if (isScene)
//			{
//				// GameObjectの名前を表示.
//				Debug.Log("path: "+path + "\nname: "+obj.name);
//				checkObject(obj, path);
//			} 
////			else {
////				Debug.Log ("NOT IN SCENE path: "+path);
////			}
//			count++;
//		}
		// プログレスバーを消す
		EditorUtility.ClearProgressBar();
	}

	private static void checkObject(UnityEngine.Object obj, string path) {
		if (obj == null) {
			return;
		}
		if (obj.name == "Deprecated EditorExtensionImpl") {
			return;
		}
		
		// SerializedObjectを通してアセットのプロパティを取得する
		SerializedObject sobj = new SerializedObject(obj);
		SerializedProperty property = sobj.GetIterator();

		string log = "log of "+obj.name;

		while (property.Next(true)) {
			log += "\n"+property.displayName+"("+property.propertyType+")";
			// プロパティの種類がオブジェクト（アセット）への参照で、
			// その参照がnullなのにもかかわらず、参照先インスタンスIDが0でないものはMissing状態！
			if (property.propertyType == SerializedPropertyType.ObjectReference &&
			    property.objectReferenceValue == null	&&
			    property.objectReferenceInstanceIDValue != 0) 
			{
				Debug.Log("MISSING!!\nobj: "+ obj.name + "\npath: "+path+"\n"
				          + "property displayname: "+property.displayName + "\n"
				          + "property name: "+property.name);
				// Missing状態のプロパティリストに追加する
				missingList.Add(new AssetParameterData() {
					obj = obj,
					path = path,
					property = property
				});
			}
		}
		Debug.Log (log);
	}
}
