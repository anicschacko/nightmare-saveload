using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerMovement player;
	[SerializeField] private EnemyManager enemyManager;
	[SerializeField] private SaveGameManager saveManager;

	private PlayerHealth playerHealth;

	private const string PLAYER_DATA_FILE = "PlayerFile";
	private const string ENEMY_DATA_FILE = "EnemyFile";

	private void Awake()
	{
		playerHealth = player.GetComponent<PlayerHealth>();
	}

	private void Start()
	{
		SetPlayerData();
		SetEnemyData();
	}

	private void SetPlayerData()
	{
		if (saveManager.TryGetSavedGameState(PLAYER_DATA_FILE, out PlayerSaveData data))
		{
			player.transform.position = data.playerPosition;
			playerHealth.SetSavedHealth(data.playerHealth); 
			ScoreManager.score = data.score;
			player.gameObject.SetActive(true);
		}
		else
		{
			player.gameObject.SetActive(true);
			playerHealth.SetSavedHealth(100);
		}
	}

	private void SetEnemyData()
	{
		if (saveManager.TryGetSavedGameState(ENEMY_DATA_FILE, out EnemySaveData data))
			enemyManager.LoadEnemies(data);
		else
			StartCoroutine(enemyManager.StartEnemySpawn());
	}

	private void OnApplicationQuit()
	{
		SaveGameState();
	}

	public void SaveGameState()
	{
		SavePlayerData();
		SaveEnemyData();

		void SavePlayerData()
		{
			PlayerSaveData data = new();

			data.playerPosition = player.transform.position;
			data.playerHealth = playerHealth.CurrentHealth;
			data.score = ScoreManager.score;
			
			saveManager.SaveData(data, PLAYER_DATA_FILE);
		}

		void SaveEnemyData()
		{
			EnemySaveData data = new();
			
			foreach (var enemyInstantiateData in enemyManager.EnemiesData)
			{
				EnemyCategoryData categoryData = new EnemyCategoryData();

				categoryData.id = enemyManager.EnemiesData.FindIndex(x => x == enemyInstantiateData);
				categoryData.name = enemyInstantiateData.enemy.name;
				foreach (var activeEnemy in enemyInstantiateData.activeEnemies)
				{
					categoryData.enemyPositions.Add(activeEnemy.transform.position);
				}

				data.enemyManagerDataList.Add(categoryData);
			}
			
			saveManager.SaveData(data, ENEMY_DATA_FILE);
		}
	}
}
