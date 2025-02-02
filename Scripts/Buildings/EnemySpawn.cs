using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform spawnEnemyLoc;
    public bool isSpawning = false;
    public int enemiesToSpawn;
    public float spawnDelay = 3f;

    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        spawnDelay = WaveManager.instanceWM.enemySpawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameState.isResume)
        {

            if (isSpawning)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= spawnDelay)
                {
                    if (enemiesToSpawn > 0)
                    {
                        ProduceEnemy();
                        enemiesToSpawn -= 1;
                        currentTime = 0;
                    }
                    else
                    {
                        isSpawning = false;
                    }
                }
            }
            else
            {
                currentTime = 0;
            }

        }
    }

    public void ProduceEnemy()
    {
        GameObject enemy = PoolManager.instancePool.GetPooledObject(PoolManager.instancePool.poolEnemy);

        if (enemy != null)
        {
            enemy.transform.position = spawnEnemyLoc.position;
            enemy.transform.rotation = spawnEnemyLoc.rotation;
            enemy.SetActive(true);
        }
        else { }
    }
}
