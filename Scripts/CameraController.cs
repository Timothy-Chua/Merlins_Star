using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;

    private Vector3 lastPlayerPosition;
    private float distanceToMoveX;
    private float distanceToMoveZ;

    void Start()
    {
        lastPlayerPosition = player.transform.position;
    }

    void Update()
    {
        distanceToMoveX = player.transform.position.x - lastPlayerPosition.x;
        distanceToMoveZ = player.transform.position.z - lastPlayerPosition.z;

        this.transform.position = new Vector3(this.transform.position.x + distanceToMoveX, this.transform.position.y, this.transform.position.z + distanceToMoveZ);
        lastPlayerPosition = player.transform.position;
    }
}
