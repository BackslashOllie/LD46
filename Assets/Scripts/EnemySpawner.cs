using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int enemyAmount = 5;
    public List<Enemy> enemies = new List<Enemy>();

    public float spawnRadius = 5;
    public bool drawGizmos;

    private SphereCollider collider;
    public bool playerInArea, dragonInArea;
    private bool playerOrDragonInArea { get { return playerInArea || dragonInArea; } }

    private bool dragonFound;
    float curTime = 0;
    public float tickRate = .2f;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = spawnRadius;
    }

    public void Start()
    {
        for (int i = 0; i < enemyAmount; i++)
        {

            GameObject go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);

            Enemy e = go.GetComponent<Enemy>();
            SpawnEnemy(e);
            enemies.Add(e);
            e.UpdateTarget();
            print("Spawned at " + gameObject.transform.parent.name);
        }
    }

    void Update()
    {
        if (!Dragon.Instance.Collected)
        {
            if (!dragonFound)
            {
                dragonFound = true;
                dragonInArea = Vector3.Distance(Dragon.Instance.transform.position, transform.position) < spawnRadius;
                
            }
        }
        else
        {
            dragonFound = false;
            dragonInArea = playerInArea;
        }
            
        curTime -= Time.deltaTime;

        if (curTime <= 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (playerOrDragonInArea)
                    enemies[i].UpdateTarget();
                else
                    enemies[i].UpdateTarget(true);
            }

            curTime = tickRate;
        }
    }

    public void SpawnEnemy(Enemy e)
    {
        e.mySpawner = this;
        e.mySpawnSpot = transform.position + (Random.insideUnitSphere * spawnRadius);
        e.transform.position = e.mySpawnSpot;
        e.InitAgent();
    }

    public void OnDrawGizmos()
    {
        if(drawGizmos)
        {
            Gizmos.DrawWireSphere(transform.position , spawnRadius);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("PlayerInArea");
            playerInArea = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("PlayerOutArea");
            playerInArea = false;
        }
    }
}
