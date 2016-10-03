using UnityEngine;
using System.Collections;

public class UserMasterTable : MasterTableBase<UserMaster>
{
	public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class UserMaster : MasterBase
{
	public int LV { get; private set; }
	public PBClass.BigInteger COST_BASE { get; private set; }
	public PBClass.BigInteger VALUE { get; private set; }
	public PBClass.BigInteger FREE { get; private set; }
	public PBClass.BigInteger REWARD { get; private set;}
	public PBClass.BigInteger COST_BOMB {get; private set;}
	public PBClass.BigInteger COST_COLOR { get; private set; }
	public PBClass.BigInteger COST_TIME { get; private set; }
}
