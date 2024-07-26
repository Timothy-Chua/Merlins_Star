using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Building : MonoBehaviour
{
    [Header("Building Info")]
    public string buildingName;
    public GameObject areaObj;
    public Material materialActive;
    public Material materialInactive;

    [Header("Upgrade")]
    public int upgradeBaseCost;
    public int upgradeCurrentCost;
    public int upgradeCostInc;
    public int currentUpgradeIndex;
    public int maxUpgradeIndex;

    protected virtual void Start()
    {
        areaObj.GetComponent<MeshRenderer>().material = materialInactive;

        currentUpgradeIndex = 0;
        upgradeCurrentCost = upgradeBaseCost;
    }

    protected virtual void Update()
    {
        
    }

    // TRIGGER ----------------------------------------------------------
    protected virtual void OnTriggerEnter(Collider actor)
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (actor.gameObject.CompareTag("Player"))
            {
                areaObj.GetComponent<MeshRenderer>().material = materialActive;

                UpdateBuildingInfo();
                EnterReactionText();
            }
        }
    }

    protected virtual void OnTriggerStay(Collider actor)
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (actor.gameObject.CompareTag("Player"))
            {
                areaObj.GetComponent<MeshRenderer>().material = materialActive;
                UpdateBuildingInfo();

                if (Input.GetKeyUp(KeyCode.E))
                {
                    Interact();
                }
            }
        }
    }

    protected virtual void OnTriggerExit(Collider actor)
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (actor.gameObject.CompareTag("Player"))
            {
                areaObj.GetComponent<MeshRenderer>().material = materialInactive;

                GameManager.instance.txtInteract.text = "";
                ClearBuildingInfo();
            }
        }
    }

    // CORE FUNCTIONS ----------------------------------------------------
    protected virtual void Produce()
    {

    }

    protected virtual void Collect()
    {

    }

    protected virtual void Interact()
    {

    }

    protected virtual void Upgrade()
    {
        
    }

    protected virtual void EnterReactionText()
    {

    }

    protected virtual void UpdateBuildingInfo()
    {
        GameManager.instance.txtBuildingName.text = "Building Name: " + buildingName;
    }

    protected virtual void ClearBuildingInfo()
    {
        GameManager.instance.txtBuildingName.text = "";
        GameManager.instance.txtBuildingProductionRate.text = "";
        GameManager.instance.txtBuildingUpgradeCost.text = "";
        GameManager.instance.txtCoinsProduced.text = "";
    }

    protected virtual IEnumerator QuickReaction(string text)
    {
        GameManager.instance.txtInteract.text = text;
        yield return new WaitForSeconds(3f);
        GameManager.instance.txtInteract.text = "";
    }
}
