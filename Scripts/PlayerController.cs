using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    public Camera mainCamera;
    public NavMeshAgent playerAgent;

    private Vector3 targetDestination;

    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.state == GameState.isResume)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    playerAgent.SetDestination(hit.point);
                }
            }

            playerAgent.isStopped = false;
        }
        else
        {
            playerAgent.isStopped = true;
        }

        if (playerAgent.velocity.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
