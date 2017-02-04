using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class DebugLogger {
	[Conditional ("UNITY_EDITOR")]
	public static void Log (object o) {
		UnityEngine.Debug.Log (o);
	}
}
