using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Overall")]
    public Camera mainCamera;
    public PlayerController playerController;
    public GameState state;
    public Transform locTower;

    [Header("Game Settings")]
    public int towerBaseHP;
    public int towerCurrentHP;
    public float timeSurvived;

    [Header("Inventory")]
    public int invCoins;
    public int totalCoins;

    [Header("UI References")]
    public GameObject panelInGame;
    public GameObject panelMenu;
    public GameObject panelGameOver;

    [Header("UI - Basic Info")]
    public TMP_Text txtTime;
    public TMP_Text txtCoins;
    public TMP_Text txtTowerHP;
    public TMP_Text txtInteract;

    [Header("UI - Wave UI")]
    public TMP_Text txtWaveCount;
    public TMP_Text txtAnnouncement;
    public TMP_Text txtWaveEnemiesRemaining;

    [Header("UI - Building Info")]
    public TMP_Text txtBuildingName;
    public TMP_Text txtBuildingUpgradeCost;
    public TMP_Text txtBuildingProductionRate;
    public TMP_Text txtCoinsProduced;

    [Header("UI - Game Over")]
    public TMP_Text txtFinalWave;
    public TMP_Text txtFinalTime;
    public TMP_Text txtBestTime;
    public TMP_Text txtTotalCoins;
    public TMP_Text txtTotalEnemiesDefeated;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Refresh values
        timeSurvived = 0;
        invCoins = 0;
        totalCoins = 0;

        state = GameState.isResume;
        txtInteract.text = "";
        txtAnnouncement.text = "";

        towerCurrentHP = towerBaseHP;
    }

    void Update()
    {
        switch (state)
        {
            case GameState.isResume:
                panelInGame.SetActive(true);
                panelMenu.SetActive(false);
                panelGameOver.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    QuickMenu();
                }

                UpdateTime();
                UpdateCoins();
                UpdateTowerHP();

                CheckGameOver();

                break;
            case GameState.isPaused:
                panelInGame.SetActive(false);
                panelMenu.SetActive(true);
                panelGameOver.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    QuickMenu();
                }

                break;
            case GameState.isGameOver:
                panelInGame.SetActive(false);
                panelMenu.SetActive(false);
                panelGameOver.SetActive(true);

                Results();

                break;
        }
    }

    // FUNCTIONS -------------------------------------------------
    private string TimeToString(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}:{1:00}", minutes, seconds);
        return currentTime;
    }

    private void QuickMenu()
    {
        switch (state)
        {
            case GameState.isPaused:
                state = GameState.isResume;
                break;
            case GameState.isResume:
                state = GameState.isPaused;
                break;
        }
    }


    private void UpdateTime()
    {
        timeSurvived += Time.deltaTime;
        txtTime.text = TimeToString(timeSurvived);
    }

    private void UpdateCoins()
    {
        txtCoins.text = "Coins: " + invCoins.ToString();
    }

    private void UpdateTowerHP()
    {
        txtTowerHP.text = "Tower HP: " + towerCurrentHP.ToString();
    }

    private void CheckGameOver()
    {
        if (towerCurrentHP <= 0)
        {
            towerCurrentHP = 0;
            state = GameState.isGameOver;
        }
    }

    private void Results()
    {
        if (PlayerPrefs.GetFloat("p_BestTime") <= timeSurvived || PlayerPrefs.GetFloat("p_BestTime") == 0)
        {
            PlayerPrefs.SetFloat("p_BestTime", timeSurvived);
        }

        float bestTime = PlayerPrefs.GetFloat("p_BestTime");

        txtFinalWave.text = "You Survived " + WaveManager.instanceWM.waveNum.ToString() + " waves";
        txtFinalTime.text = TimeToString(timeSurvived);
        txtBestTime.text = TimeToString(bestTime);
        txtTotalCoins.text = totalCoins.ToString();
        txtTotalEnemiesDefeated.text = WaveManager.instanceWM.enemiesDefeatedTotal.ToString();
    }

    // BUTTONS ---------------------------------------------------
    public void OnBtnResume()
    {
        state = GameState.isResume;
    }

    public void OnBtnPause()
    {
        state = GameState.isPaused;
    }

    public void OnBtnRestart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void OnBtnQuit()
    {
        Application.Quit();
    }
}

public enum GameState
{
    isResume,
    isPaused,
    isGameOver
}
