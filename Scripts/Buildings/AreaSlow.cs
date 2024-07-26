using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AreaSlow : BuildableBuilding
{
    [Header("Area Settings")]
    public float spdDecrease = .75f;
    public Collider buyArea;
    public Collider slowArea;

    protected override void Start()
    {
        base.Start();

        slowArea.enabled = false;
        buyArea.enabled = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isBuilt)
        {
            slowArea.enabled = true;
            buyArea.enabled = false;
            areaObj.gameObject.SetActive(false);
        }
        else
        {
            slowArea.enabled = false;
            buyArea.enabled = true;
            areaObj.gameObject.SetActive(true);
        }
    }
    

    protected override void OnTriggerEnter(Collider actor)
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (isBuilt)
            {
                if (actor.gameObject.CompareTag("Enemy"))
                {
                    actor.GetComponent<AIEnemy>().speedEnemy -= spdDecrease;
                }
            }
            else
            {
                if (actor.gameObject.CompareTag("Player"))
                {
                    areaObj.GetComponent<MeshRenderer>().material = materialActive;

                    UpdateBuildingInfo();
                    EnterReactionText();
                }
            }
        }
    }

    protected override void OnTriggerStay(Collider actor)
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (!isBuilt)
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
    }

    protected override void OnTriggerExit(Collider actor)
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (isBuilt)
            {
                if (actor.gameObject.CompareTag("Enemy"))
                {
                    actor.GetComponent<AIEnemy>().speedEnemy += spdDecrease;
                }
            }
            else
            {
                if (actor.gameObject.CompareTag("Player"))
                {
                    areaObj.GetComponent<MeshRenderer>().material = materialInactive;

                    GameManager.instance.txtInteract.text = "";
                    ClearBuildingInfo();
                }
            }
        }
    }
}
