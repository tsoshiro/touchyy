using UnityEngine;
using System.Collections;
using System;

public class IntValueConverter {
	PBClass.BigInteger k = 1000;
	long m, b, t, q;

	public void test ()
	{
		Debug.Log ("===============================TEST START!!!!===============================");
		showSample (new PBClass.BigInteger (12345678));
		showSample (new PBClass.BigInteger (12345678901));
		showSample (new PBClass.BigInteger (1234567890123));
		Debug.Log ("===============================TEST END!!!!===============================");
	}

	void showSample (PBClass.BigInteger pBig) {
		Debug.Log ("[SAMPLE]:" + pBig + " to " + FixBigInteger (pBig));
	}

	public string _FixBigInteger (PBClass.BigInteger pBigInteger) {
		string value = "" + pBigInteger;
		char[] c = value.ToCharArray ();
		value = "";
		int counts = 0;
		for (int i = c.Length - 1; i >= 0; i--) {
			value = c [i] + value;
			counts++;
			if (counts == 3 && i != 0) {
				value = "," + value;
				counts = 0;
			}
		}
		return value;
	}

	public string FixBigInteger(PBClass.BigInteger pBigInteger) {
		string value = ""+pBigInteger;
		char[] c = value.ToCharArray ();

		value = "";
		int valueType = 0; // 2: Billion, 1:Million, 0:None
		if (c.Length >= 13) {
			valueType = 3;
		} else if (c.Length >= 10) { // Billion
			valueType = 2;
		} else if (c.Length >= 7) { // Million
			valueType = 1;
		}

		int ketaCount = 1;
		int lastKetaNumber = 0;
		for (int i = c.Length - 1; i >= 0; i--) {
			value = getStringValue (ketaCount, lastKetaNumber, int.Parse (c[i].ToString ()), value, valueType);
			lastKetaNumber = c [i];
			ketaCount++;
		}
		return value;
	}

	//
	string getStringValue (int pKeta, int pLastKeta, int pC, string pValue, int pValueType) {
		string str = "";

		if (pValueType > 0) {
			int us = getUnderShosutenKeta (pValueType); // UnderShosuten
			if (pKeta < us) {
				str = pValue;
			} else if (pKeta == us) {
				//Round
				str = SimpleRound (pC, pLastKeta) + getTBM (pValueType) + pValue;
			} else if (pKeta == us + 1) {
				str = pC + "." + pValue;
			} else {
				str = pC + pValue;
			}
		} else {
			str = getStringUnderMillion (pKeta, pC, pValue);
		}
		return str;
	}

	string getStringUnderMillion (int pKeta, int pC, string pValue) {
		string str = "";
		if (((pKeta - 1) % 3) == 0 &&
		    pKeta != 1) 
		{
			str = pC + "," + pValue;
		} else {
			str = pC + pValue;
		}
		return str;
	}

	string getTBM (int pValueType) {
		if (pValueType == 3) {
			return "T";
		} else if (pValueType == 2) {
			return "B";
		} else if (pValueType == 1) {
			return "M";
		} else {
			return "";
		}
	}

	// 
	int getUnderShosutenKeta (int pValueType) {
		return (pValueType + 1) * 3;
	}

	// SimpleRound
	public int SimpleRound (int pLeftNum, int pRightNum) {
		if (pRightNum >= 5) {
			return pLeftNum++;
		}
		return pLeftNum;
	}
}
