using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    
    public Transform playerTransform;

    public int enemyAmount;

    public int enemiesInLevel;

    public float tickRate = .2f;
    public List<Enemy> enemies = new List<Enemy>();

    public EnemySpawner[] enemySpawners;

    public void Start()
    {


        for (int i = 0; i < enemyAmount; i++)
        {

            GameObject go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);

            EnemySpawner spawner = enemySpawners[Random.Range(0, enemySpawners.Length)];
            Enemy e = go.GetComponent<Enemy>();
            spawner.SpawnEnemy(e);
            enemies.Add(e);
            e.UpdateTarget();

            enemiesInLevel++;
        }
    }

    float curTime = 0;

    void Update()
    {
        curTime -= Time.deltaTime;

        if(curTime <= 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].UpdateTarget();
            }

            curTime = tickRate;
        }
    }
}
