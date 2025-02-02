using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : BuildableBuilding
{
    [Header("Spawn Settings")]
    public Transform spawnLoc;
    float currentTime;

    public int produceQuantity = 1;
    public float produceTime = 10f;
    public float productionRateInc = 1f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        currentTime = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (GameManager.instance.state == GameState.isResume)
        {
            if (isBuilt)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= produceTime)
                {
                    Produce();
                    currentTime = 0;
                }
            }
        }
    }

    protected override void Upgrade()
    {
        base.Upgrade();

        produceTime -= productionRateInc;
    }

    protected override void UpdateBuildingInfo()
    {
        base.UpdateBuildingInfo();

        GameManager.instance.txtBuildingProductionRate.text = "Production Rate: " + produceQuantity.ToString() + " units per " + produceTime.ToString() + " second";
    }

    protected override void Produce()
    {
        base.Produce();

        GameObject ally = PoolManager.instancePool.GetPooledObject(PoolManager.instancePool.poolAlly);

        if (ally != null)
        {
            ally.transform.position = spawnLoc.position;
            ally.transform.rotation = spawnLoc.rotation;
            ally.SetActive(true);
        }
    }
}
