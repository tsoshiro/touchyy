using UnityEngine;
using System.Collections;
using System;

public class IntValueConverter {
	enum ValueType {
		UNDER_MILLION,
		MILLION,
		BILLION,
		TRILLION
	}

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
		ValueType valueType = ValueType.UNDER_MILLION; // 2: Billion, 1:Million, 0:None
		if (c.Length >= 13) {
			valueType = ValueType.TRILLION;
		} else if (c.Length >= 10) { // Billion
			valueType = ValueType.BILLION;
		} else if (c.Length >= 7) { // Million
			valueType = ValueType.MILLION;
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
	string getStringValue (int pKeta, int pLastKeta, int pC, string pValue, ValueType pValueType) {
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

	string getTBM (ValueType pValueType) {
		if (pValueType == ValueType.TRILLION) {
			return "T";
		} else if (pValueType == ValueType.BILLION) {
			return "B";
		} else if (pValueType == ValueType.MILLION) {
			return "M";
		} else {
			return "";
		}
	}

	// 
	int getUnderShosutenKeta (ValueType pValueType) {
		return ((int)pValueType + 1) * 3;
	}

	// SimpleRound
	public int SimpleRound (int pLeftNum, int pRightNum) {
		if (pRightNum >= 5) {
			return pLeftNum++;
		}
		return pLeftNum;
	}
}
