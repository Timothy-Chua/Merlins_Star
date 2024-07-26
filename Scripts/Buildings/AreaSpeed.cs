using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpeed : BuildableBuilding
{
    [Header("Area Settings")]
    public float spdIncrease = 0.75f;
    public Collider buyArea;
    public Collider buffArea;

    protected override void Start()
    {
        base.Start();

        buffArea.enabled = false;
        buyArea.enabled = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isBuilt)
        {
            buffArea.enabled = true;
            buyArea.enabled = false;
            areaObj.gameObject.SetActive(false);
        }
        else
        {
            buffArea.enabled = false;
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
                if (actor.gameObject.CompareTag("Ally"))
                {
                    actor.GetComponent<AIEnemy>().speedEnemy += spdIncrease;
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
                if (actor.gameObject.CompareTag("Ally"))
                {
                    actor.GetComponent<AIEnemy>().speedEnemy -= spdIncrease;
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
