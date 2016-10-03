using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T GetInstance ()
	{
		if (sInstance == null) {
			sInstance = (T)FindObjectOfType (typeof (T)) as T;
		}
		return sInstance;
	}

	protected virtual void Initialize () { }

	private void Awake ()
	{
		if (sInstance == null) {
			sInstance = GetInstance ();
			DontDestroyOnLoad (this.gameObject);

			if (sInstance == null) {
				Debug.LogError (gameObject.name);
			}
		}
		if (sInstance != this) {
			Destroy (this.gameObject);
			return;
		}
		Initialize ();
	}

	protected virtual void OnDestroy ()
	{
		if (sInstance == this) {
			sInstance = null;
		}
	}

	protected static T sInstance;
}