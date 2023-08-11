using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySaveData
{
	public List<EnemyCategoryData> enemyManagerDataList;

	public EnemySaveData()
	{
		enemyManagerDataList = new List<EnemyCategoryData>();
	}
}

[System.Serializable]
public class EnemyCategoryData
{
	public int id;
	public string name;
	public List<Vector3> enemyPositions;

	public EnemyCategoryData()
	{
		id = 0;
		name = "";
		enemyPositions = new List<Vector3>();
	}
}
