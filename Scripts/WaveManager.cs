using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instanceWM;

    [Header("Enemy Settings")]
    public EnemySpawn[] spawns;
    public float enemySpawnDelay = 3f;
    public int enemyDMG = 10;
    public int enemiesDefeatedTotal;

    [Header("Waves")]
    public bool isWaveStart;
    public bool isNextWave;
    public int waveNum = 1;
    public int waveEnemiesToSpawn = 2;
    public int waveEnemiesInc = 1;
    public int waveEnemiesRemaining;

    public float timeStartWave = 10f;
    public float timeBetweenWaves = 6f;

    private void Awake()
    {
        if (instanceWM == null)
        {
            instanceWM = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        enemiesDefeatedTotal = 0;
        waveNum = 1;
        isNextWave = true;
    }

    private void Update()
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (GameManager.instance.timeSurvived >= timeStartWave)
            {
                EnemyWaves();
                UpdateWaveUI();
            }
        }
    }

    private void EnemyWaves()
    {
        switch (waveNum % 7)
        {
            case 0:
                AllSpawn();
                break;
            case 1:
                SingleSpawn(0);
                break;
            case 2:
                SingleSpawn(1);
                break;
            case 3:
                SingleSpawn(2);
                break;
            case 4:
                SingleSpawn(3);
                break;
            case 5:
                DoubleSpawn(0, 1);
                break;
            case 6:
                DoubleSpawn(2, 3);
                break;
        }
    }

    private void SingleSpawn(int index)
    {
        if (!isWaveStart)
        {
            waveEnemiesRemaining = waveEnemiesToSpawn;

            spawns[index].enemiesToSpawn = waveEnemiesToSpawn;
            spawns[index].isSpawning = true;

            StartCoroutine(QuickAnnouncement("Wave " + waveNum + " has started."));

            isWaveStart = true;
        }
        else
        {
            if (waveEnemiesRemaining == 0)
            {
                waveEnemiesToSpawn += waveEnemiesInc;
                waveNum += 1;
                isWaveStart = false;
            }
        }
    }

    private void DoubleSpawn(int index1, int index2)
    {
        if (!isWaveStart)
        {
            waveEnemiesRemaining = waveEnemiesToSpawn;

            int enemiesEach = waveEnemiesToSpawn / 2;
            int remainingEnemies = waveEnemiesToSpawn % 2;

            spawns[index1].enemiesToSpawn = enemiesEach + remainingEnemies;
            spawns[index2].enemiesToSpawn = enemiesEach;

            spawns[index1].isSpawning = true;
            spawns[index2].isSpawning = true;

            StartCoroutine(QuickAnnouncement("Wave " + waveNum + " has started."));

            isWaveStart = true;
        }
        else
        {
            if (waveEnemiesRemaining == 0)
            {
                waveEnemiesToSpawn += waveEnemiesInc;
                waveNum += 1;
                isWaveStart = false;
            }
        }
    }

    private void AllSpawn()
    {
        if (!isWaveStart)
        {
            waveEnemiesRemaining = waveEnemiesToSpawn;

            int enemiesEach = waveEnemiesToSpawn / 4;
            int remainingEnemies = waveEnemiesToSpawn % 4;

            foreach (EnemySpawn spawn in spawns)
            {
                spawn.enemiesToSpawn = enemiesEach;
            }

            int index = 0;
            for (int i = 0; i < remainingEnemies; i++)
            {
                if (index > 3)
                {
                    index = 0;
                }

                spawns[index].enemiesToSpawn += 1;
                index++;
                i++;
            }

            foreach (EnemySpawn spawn in spawns)
            {
                spawn.isSpawning = true;
            }

            StartCoroutine(QuickAnnouncement("Wave " + waveNum + " has started."));

            isWaveStart = true;
        }
        else
        {
            if (waveEnemiesRemaining == 0)
            {
                waveEnemiesToSpawn += waveEnemiesInc;
                waveNum += 1;
                isWaveStart = false;
            }
        }
    }

    private IEnumerator DelayWave()
    {
        StartCoroutine(QuickAnnouncement("Wave " + waveNum + " completed."));

        yield return new WaitForSeconds(timeBetweenWaves);

        isWaveStart = false;
        waveEnemiesToSpawn += waveEnemiesInc;
        waveEnemiesRemaining = waveEnemiesToSpawn;
        waveNum += 1;
    }

    private void UpdateWaveUI()
    {
        GameManager.instance.txtWaveEnemiesRemaining.text = "Enemies Remaining: " + waveEnemiesRemaining.ToString();
        GameManager.instance.txtWaveCount.text = "Wave: " + waveNum.ToString();
    }

    private IEnumerator QuickAnnouncement(string text)
    {
        GameManager.instance.txtAnnouncement.text = text;
        yield return new WaitForSeconds(3f);
        GameManager.instance.txtAnnouncement.text = "";
    }
}
