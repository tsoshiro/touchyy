using UnityEngine;
using System.Collections;

public class Const {
	public enum BombType
	{
		VERTICAL, // 縦一列 5コ
		HORIZONTAL, // 横一列 5コ
		CROSS, // 縦横一列
		PLUS, // 上下左右 4コ
		MULTIPLE, // 右上・左上・右下・左下 4コ
		AROUND // 周囲8コ 
	}
}
