using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    [Header("Coin Production")]
    public int currentCoinsStored;

    public int maxCoinsStored = 500;
    public int incCoinsStored = 100;

    public int amtCoinProduce = 10;
    public int incAmtCoinProduce = 5;

    public float timeCoinProduce = 1f;

    float currentTime;

    protected override void Start()
    {
        base.Start();

        currentCoinsStored = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (GameManager.instance.state == GameState.isResume)
        {
            if (currentCoinsStored < maxCoinsStored)
            {
                currentTime += Time.deltaTime;

                if (currentTime > timeCoinProduce)
                {
                    currentCoinsStored += amtCoinProduce;
                    currentTime = 0;
                }
            }

            if (currentCoinsStored > maxCoinsStored)
            {
                currentCoinsStored = maxCoinsStored;
            }
        }
    }

    protected override void OnTriggerStay(Collider actor)
    {
        base.OnTriggerStay(actor);

        if (GameManager.instance.state == GameState.isResume)
        {
            if (actor.gameObject.CompareTag("Player"))
            {
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    Collect();
                }
            }
        }
    }

    protected override void UpdateBuildingInfo()
    {
        base.UpdateBuildingInfo();
        GameManager.instance.txtBuildingUpgradeCost.text = "Upgrade Cost: " + upgradeCurrentCost.ToString();
        GameManager.instance.txtBuildingProductionRate.text = "Production: " + amtCoinProduce.ToString() + " Coins per " + timeCoinProduce.ToString() + " seconds";
        GameManager.instance.txtCoinsProduced.text = "Stored Coins: " + currentCoinsStored.ToString();
    }

    protected override void Collect()
    {
        base.Collect();

        if (currentCoinsStored > 0)
        {
            GameManager.instance.invCoins += currentCoinsStored;
            GameManager.instance.totalCoins += currentCoinsStored;
            currentCoinsStored = 0;
        }
        else
        {
            QuickReaction("No coins to collect");
        }
    }

    protected override void Interact()
    {
        base.Interact();

        if (GameManager.instance.invCoins >= upgradeCurrentCost)
        {
            if (currentUpgradeIndex < maxUpgradeIndex)
            {
                Upgrade();

                GameManager.instance.invCoins -= upgradeCurrentCost;
                currentUpgradeIndex += 1;
                upgradeCurrentCost += upgradeCostInc;
            }
            else
            {
                QuickReaction("Max upgrade reached");
            }
        }
        else
        {
            QuickReaction("Not enough coins");
        }

    }

    protected override void Upgrade()
    {
        base.Upgrade();

        amtCoinProduce += incAmtCoinProduce;
        maxCoinsStored += incCoinsStored;
    }
}
