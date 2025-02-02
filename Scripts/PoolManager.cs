using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instancePool;

    [Header("Enemies")]
    public List<GameObject> poolEnemy = new List<GameObject>();
    public GameObject objEnemy;
    public int amountEnemy;

    [Header("Allies")]
    public List<GameObject> poolAlly = new List<GameObject>();
    public GameObject objAlly;
    public int amountAlly;

    // Start is called before the first frame update
    void Start()
    {
        if (instancePool == null)
        {
            instancePool = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        poolEnemy = new List<GameObject>();
        GameObject tmpEnemy;
        for (int i = 0; i < amountEnemy; i++)
        {
            tmpEnemy = Instantiate(objEnemy);
            tmpEnemy.SetActive(false);
            poolEnemy.Add(tmpEnemy);
        }

        poolAlly = new List<GameObject>();
        GameObject tmpAlly;
        for (int i = 0; i < amountAlly; i++)
        {
            tmpAlly = Instantiate(objAlly);
            tmpAlly.SetActive(false);
            poolAlly.Add(tmpAlly);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in poolAlly)
        {
            if (!obj.activeInHierarchy)
            {
                obj.GetComponent<AIAlly>().timer = 0;
            }
        }
    }

    public GameObject GetPooledObject(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].gameObject.activeInHierarchy)
            {
                return list[i].gameObject;
            }
        }

        GameObject newUnit = null;
        if (list == poolAlly) 
        { 
            newUnit = Instantiate(objAlly);
            poolAlly.Add(newUnit);
        }
        else if (list == poolEnemy)
        {  
            newUnit = Instantiate(objEnemy);
            poolEnemy.Add(newUnit);
        }

        return newUnit;
    }
}
