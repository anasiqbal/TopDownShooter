
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public string levelName;
	public List<SpawnPoint> spawnPoints;

	public SpawnPoint GetRandomSpawnPoint()
	{
		int randIndex = Random.Range (0, spawnPoints.Count);
		return spawnPoints [randIndex];
	}
}
