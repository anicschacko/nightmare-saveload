using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private List<EnemyInstantiateData> enemiesData;

    public List<EnemyInstantiateData> EnemiesData => enemiesData;
    
    public IEnumerator StartEnemySpawn(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        foreach (var enemyInstantiateData in enemiesData)
        {
            StartCoroutine(Spawn(enemyInstantiateData));
        }
    }

    IEnumerator Spawn(EnemyInstantiateData data)
    {
        yield return new WaitForSeconds(data.spawnTime);
        
        if(playerHealth.CurrentHealth <= 0f)
        {
            yield break;
        }

        int spawnPointIndex = Random.Range (0, data.spawnPoints.Length);
        
        var go = Instantiate (data.enemy, data.spawnPoints[spawnPointIndex].position, data.spawnPoints[spawnPointIndex].rotation);
        data.EnemyInstantiated(go);
        StartCoroutine(Spawn(data));
    }

    public void LoadEnemies(EnemySaveData data)
    {
        for(int i = 0; i< data.enemyManagerDataList.Count;i++)
        {
            SpawnEnemiesToLoad(enemiesData[i], data.enemyManagerDataList[i]);
        }
        StartCoroutine(StartEnemySpawn(3f));
    }

    void SpawnEnemiesToLoad(EnemyInstantiateData instantiateData, EnemyCategoryData data)
    {
        foreach (var position in data.enemyPositions)
        {
            var go = Instantiate (instantiateData.enemy, position, Quaternion.identity);
            instantiateData.EnemyInstantiated(go);
        }
    }
}

[System.Serializable]
public class EnemyInstantiateData
{
    public EnemyHealth enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    
    [HideInInspector]
    public List<EnemyHealth> activeEnemies;

    private Transform parent;
    
    public void EnemyInstantiated(EnemyHealth health)
    {
        health.OnDeath += EnemyDead;
        
        parent ??= new GameObject().transform;
        parent.name = $"{enemy.name}Parent";
        health.transform.SetParent(parent);
        
        activeEnemies ??= new List<EnemyHealth>();
        activeEnemies.Add(health);
    }
    
    public void EnemyDead(EnemyHealth health)
    {
        activeEnemies.Remove(health);
    }
}
