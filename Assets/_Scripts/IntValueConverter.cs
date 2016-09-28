using UnityEngine;
using System.Collections;
using System;

public class IntValueConverter {
	long k = 1000;
	long m, b, t, q;

	void setValues () {
		k = 1000;
		m = (long)Math.Pow (k, 2);
		b = (long)Math.Pow (k, 3);
		t = (long)Math.Pow (k, 4);
		q = (long)Math.Pow (k, 5);
	}

	public void test () {
		setValues ();

		long sampleK = UnityEngine.Random.Range ((int)k, (int)m);
		long sampleM = UnityEngine.Random.Range ((int)m, (int)b);
		long sampleB = UnityEngine.Random.Range ((int)b, (int)t);
		long sampleT = UnityEngine.Random.Range ((int)t, (int)q);

		Debug.Log ("sampleK :" + sampleK + " convert to :" + convertIntToString (sampleK));

		Debug.Log ("sampleM :" + sampleM + " convert to :" + convertIntToString (sampleM));

		Debug.Log ("sampleB :" + sampleB + " convert to :" + convertIntToString (sampleB));
	}

	public string convertIntToString (long pValue)
	{
		Debug.Log ("value:"+pValue);
		string str = "";
		string strBase = pValue.ToString();
		int ketasu = strBase.Length;
		Debug.Log ("strBase:" + strBase + "strBase.Length():"+strBase.Length);

		string strSeisu = "";
		string strShosu = "";

		int subValue = 3;

		if (pValue >= (int)k &&
			pValue < (int)m) {
			strSeisu = strBase.Substring (0, ketasu - subValue);
			strShosu = strBase.Substring (ketasu - 3, ketasu - (subValue+2));
			str = strSeisu + "." + strShosu + "K";
		} else if (pValue >= (int)m &&
				   pValue < (int)b) {
			strSeisu = strBase.Substring (0, ketasu - (subValue+3));
			strShosu = strBase.Substring (ketasu - (subValue + 3), ketasu - (subValue + 5));
			str = strSeisu + "." + strShosu + "M";
		} else if (pValue >= (int)b &&
				   pValue < (int)t) {
			strSeisu = strBase.Substring (0, ketasu - (subValue+6));
			strShosu = strBase.Substring (ketasu - (subValue+6), ketasu - (subValue +8));
			str = strSeisu + "." + strShosu + "B";
		} else if (pValue >= (int)t) {
			strSeisu = strBase.Substring (0, ketasu - (subValue + 9));
			strShosu = strBase.Substring (ketasu - (subValue + 9), ketasu - (subValue + 11));
			str = strSeisu + "." + strShosu + "Q";
		}
		return str;
	}
}
