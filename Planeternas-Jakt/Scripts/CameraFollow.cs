using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 offset;
    public bool canFollow;

    /// <summary>
    /// Initializes the camera follow behavior and calculates the initial offset.
    /// </summary>
    void Start()
    {
        canFollow = true;
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
        else
        {
            Debug.LogWarning("Player object is missing!");  
        }
    }

    /// <summary>
    /// Updates the camera's position to follow the player at fixed intervals.
    /// </summary>
    private void FixedUpdate()
    {
        if (!canFollow || player == null) return;

        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);
    }
}
