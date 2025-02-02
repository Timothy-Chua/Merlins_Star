using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    public Animator animator;
    NavMeshAgent agentEnemy;

    public float speedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        agentEnemy = GetComponent<NavMeshAgent>();
        agentEnemy.speed = speedEnemy;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            agentEnemy.isStopped = false;
            agentEnemy.SetDestination(GameManager.instance.locTower.position);
        }
        else
        {
            agentEnemy.isStopped = true;
        }

        if (agentEnemy.velocity.magnitude > 0)
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
        if (actor.gameObject.CompareTag("Ally"))
        {
            WaveManager.instanceWM.enemiesDefeatedTotal += 1;
            WaveManager.instanceWM.waveEnemiesRemaining -= 1;
            this.gameObject.SetActive(false);
        }

        if (actor.gameObject.CompareTag("Tower"))
        {
            GameManager.instance.towerCurrentHP -= WaveManager.instanceWM.enemyDMG;
            WaveManager.instanceWM.waveEnemiesRemaining -= 1;
            this.gameObject.SetActive(false);
        }
    }
}
