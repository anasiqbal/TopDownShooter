using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
	public Transform enemy_Container;
	public Enemy enemy;
	public EnemyWave [] waves;
	public int maxCorpsesOnField;

	EnemyWave currentWave;
	int currentWaveNumber;

	int enemiesRemainingToSpawn;
	int enemiesAlive;
	float nextSpawnTime;

	Level activeLevel;
	bool isInitialized;

	List<Enemy> spawnedEnemies;
	public event System.Action<int> OnNewWave;

	private void Start()
	{
		activeLevel = LevelManager.Instance.CurrentLevel;
		NextWave ();
	}

	private void Update()
	{
		if (isInitialized)
		{
			if (enemiesRemainingToSpawn > 0 && Time.time >= nextSpawnTime)
			{
				enemiesRemainingToSpawn--;
				nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

				StartCoroutine (SpawnEnemy ());
			}
		}
	}

	IEnumerator SpawnEnemy()
	{
		SpawnPoint spawnPoint = activeLevel.GetRandomSpawnPoint ();
		float xPos = Random.Range (spawnPoint.Position.x - spawnPoint.activeRadius, spawnPoint.Position.x + spawnPoint.activeRadius);
		float zPos = Random.Range (spawnPoint.Position.z - spawnPoint.activeRadius, spawnPoint.Position.z + spawnPoint.activeRadius);

		spawnPoint.FlashWarning ();
		yield return new WaitForSeconds (spawnPoint.flashWarningDuration);

		Enemy spawnedEnemy = Instantiate (enemy, new Vector3(xPos, 0, zPos), Quaternion.identity, enemy_Container);
		spawnedEnemy.OnDeath += SpawnedEnemy_OnDeath;

		spawnedEnemies.Add (spawnedEnemy);

		if(spawnedEnemies.Count > maxCorpsesOnField)
		{
			RemoveFirstDeadEnemyFromField ();
		}
	}

	public void Initialize()
	{
		activeLevel = LevelManager.Instance.CurrentLevel;
		currentWaveNumber = 0;

		if (spawnedEnemies == null)
			spawnedEnemies = new List<Enemy> ();

		if(spawnedEnemies.Count > 0)
		{
			for (int i = 0; i < spawnedEnemies.Count; i++)
				Destroy (spawnedEnemies [i].gameObject);
		}

		spawnedEnemies.Clear ();

		NextWave ();

		isInitialized = true;
	}

	public void StopSpawner()
	{
		isInitialized = false;
	}

	void SpawnedEnemy_OnDeath()
	{
		enemiesAlive--;
		if(enemiesAlive <= 0)
		{
			NextWave ();
		}
	}

	void NextWave()
	{
		Debug.Log ("Starting new wave");
		currentWaveNumber++;
		if(currentWaveNumber - 1 < waves.Length)
		{
			currentWave = waves [currentWaveNumber - 1];
			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesAlive = enemiesRemainingToSpawn;
		}

		if (OnNewWave != null)
			OnNewWave (currentWaveNumber);
	}

	void RemoveFirstDeadEnemyFromField()
	{
		if(spawnedEnemies[0].RemoveSelfFromField())
		{
			spawnedEnemies.RemoveAt (0);
		}
	}
}
