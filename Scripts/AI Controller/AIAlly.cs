using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAlly : MonoBehaviour
{
    public Animator animator;

    NavMeshAgent agentAlly;
    GameObject target;

    public float spdAlly = 3f;
    public float lifePeriod = 15f;

    public float timer;

    void Start()
    {
        agentAlly = GetComponent<NavMeshAgent>();
        agentAlly.speed = spdAlly;

        animator = GetComponent<Animator>();

        timer = 0;
    }

    void Update()
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (gameObject.activeSelf)
            {
                timer += Time.deltaTime;

                if (timer >= lifePeriod)
                {
                    timer = 0;
                    this.gameObject.SetActive(false);
                }
            }

            agentAlly.isStopped = false;

            target = GetClosestEnemy();
            if (target == null) { }

            agentAlly.SetDestination(target.transform.position);
        }
        else
        {
            agentAlly.isStopped = true;
        }

        if (agentAlly.velocity.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void OnCollisionEnter(Collision actor)
    {
        if (actor.gameObject.CompareTag("Enemy"))
        {
            this.gameObject.SetActive(false);
        }
    }

    private GameObject GetClosestEnemy()
    {
        List<GameObject> list = PoolManager.instancePool.poolEnemy;
        
        float dist;
        float minDist = Mathf.Infinity;
        GameObject closestEnemy = null;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].gameObject.activeInHierarchy)
            {
                dist = Vector3.Distance(list[i].gameObject.transform.position, this.gameObject.transform.position);

                if (dist < minDist)
                {
                    closestEnemy = list[i].gameObject;
                    minDist = dist;
                }
            }
        }

        return closestEnemy;
    }
}
