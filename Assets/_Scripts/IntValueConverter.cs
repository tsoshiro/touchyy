using UnityEngine;
using System.Collections;
using System;

public class IntValueConverter {
	enum ValueType {
		UNDER_MILLION,
		MILLION,
		BILLION,
		TRILLION,
		QUARDRILLION,
		QUINTILLION,
		COUNT
	}

	public void test ()
	{
		Debug.Log ("===============================TEST START!!!!===============================");
		for (int i = 1; i < 20; i++) {
			string str = "";
			int count = 0;
			for (int j = 1; j <= i; j++) {
				count++;
				if (count >= 10) {
					count = 0;
				}
				str += count.ToString();
			}
			showSample (new PBClass.BigInteger (str));
		}
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
		ValueType valueType = (ValueType)getValueType (c.Length);

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
		//Debug.Log (pKeta + " " + pLastKeta + " " + pC + " " + pValue + " " + pValueType);

		if ((int)pValueType > 0) {
			int us = getUnderShosutenKeta (pValueType); // UnderShosuten
			//Debug.Log ("us:" + us);
			if (pKeta < us) {
				str = pValue;
			} else if (pKeta == us) {
				//Round
				str = "" + SimpleRound (pC, pLastKeta) + getTBM (pValueType) + pValue;
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

	// valueTypeの境界を桁数から取得
	int getValueType (int pKeta) { 
		// 0	1234
		// 1	1234567
		// 2	1234567890
		// 3	1234567890123

		int vt = ((pKeta - 1) / 3) - 1;
		return vt;
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
		if (pValueType == ValueType.QUARDRILLION) {
			return "QD";
		} else if (pValueType == ValueType.QUINTILLION) {
			return "QT";
		} else if (pValueType == ValueType.TRILLION) {
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
		// Million
		// 1.2M 1234567
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
