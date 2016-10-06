using UnityEngine;
using System.Collections;

public class LocalizationMasterTable : MasterTableBase<LocalizationMaster>
{
	public void Load ()
	{
		Load (convertClassToFilePath (this.GetType ().Name));
	}
}

public class LocalizationMaster : MasterBase
{
	public string MESSAGE_CODE { get; private set; }
	public string JP { get; private set; }
	public string EN { get; private set; }
}

