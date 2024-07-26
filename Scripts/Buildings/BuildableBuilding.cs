using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableBuilding : Building
{
    [Header("Build Settings")]
    public GameObject buildingMesh;
    public int buildCost;
    public bool isBuilt = false;

    protected override void Start()
    {
        base.Start();

        isBuilt = false;
        buildingMesh.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (isBuilt)
        {
            buildingMesh.SetActive(true);
        }
        else
        {
            buildingMesh.SetActive(false);
        }
    }

    protected override void Interact()
    {
        base.Interact();

        if (isBuilt)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (GameManager.instance.invCoins >= upgradeCurrentCost)
                {
                    if (currentUpgradeIndex < maxUpgradeIndex)
                    {
                        Upgrade();

                        upgradeCurrentCost += upgradeCostInc;
                        currentUpgradeIndex += 1;
                        GameManager.instance.invCoins -= upgradeCurrentCost;
                    }
                    else
                    {
                        QuickReaction("Max upgrade reached");
                    }
                }
                else
                {
                    StartCoroutine(QuickReaction("Not enough coins to upgrade"));
                }
            }
        }
        else
        {
            if (GameManager.instance.invCoins >= buildCost)
            {
                GameManager.instance.invCoins -= buildCost;
                isBuilt = true;
            }
            else
            {
                StartCoroutine(QuickReaction("Not enough coins to build"));
            }
        }
    }

    protected override void UpdateBuildingInfo()
    {
        base.UpdateBuildingInfo();

        if (!isBuilt)
        {
            GameManager.instance.txtBuildingUpgradeCost.text = "Build Cost: " + buildCost.ToString();
        }
        else
        {
            if (currentUpgradeIndex >= maxUpgradeIndex)
            {
                GameManager.instance.txtBuildingUpgradeCost.text = "Maxed Upgrade";
            }
            else
            {
                GameManager.instance.txtBuildingUpgradeCost.text = "Upgrade Cost: " + upgradeCurrentCost.ToString();
            }
        }
    }

    protected override void EnterReactionText()
    {
        base.EnterReactionText();

        if (!isBuilt)
        {
            GameManager.instance.txtInteract.text = "Press [E] to build";
        }
        else
        {
            GameManager.instance.txtInteract.text = "Press [E] to upgrade";
        }
    }
}
