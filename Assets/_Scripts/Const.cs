using UnityEngine;
using System.Collections;

public class Const {
	public enum CubeType
	{
		NORMAL,
		BOMB,
		COLOR,
		TIME
	}

	public enum BombType
	{
		VERTICAL, // 縦一列 5コ
		HORIZONTAL, // 横一列 5コ
		CROSS, // 縦横一列
		PLUS, // 上下左右 4コ
		MULTIPLE, // 右上・左上・右下・左下 4コ
		AROUND // 周囲8コ 
	}

	// 
	public const int TYPE_VERTICAL 	= 0;
	public const int TYPE_HORIZONTAL= 1;
	public const int TYPE_CROSS		= 2;
	public const int TYPE_PLUS		= 3;
	public const int TYPE_MULTIPLE 	= 4;
	public const int TYPE_AROUND	= 5;
	public const int TYPE_RENEWAL	= 6;
	public const int TYPE_RESTRICT	= 7;
	public const int TYPE_ADD_TYME	= 8;
	public const int TYPE_NORMAL 	= 9;
}
