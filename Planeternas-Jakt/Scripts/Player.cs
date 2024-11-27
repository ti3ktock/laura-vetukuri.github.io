using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float distance, time, xPos;
    private float speed;
    public bool hasGameStarted;

    /// <summary>
    /// Initializes the player's state and calculates the speed based on distance and time.
    /// </summary>
    private void Start()
    {
        hasGameStarted = false;
        speed = distance / time;
    }

    /// <summary>
    /// Updates the player's position based on input, ensuring it stays within bounds.
    /// </summary>
    private void FixedUpdate()
    {
        if (!hasGameStarted) return;

        float horizontalInput = Input.GetAxis("Horizontal");

        if (DancePadInputManager.instance != null)
        {
            if (DancePadInputManager.instance.moveLeft)
                horizontalInput = -1;
            else if (DancePadInputManager.instance.moveRight)
                horizontalInput = 1;
        }

        Vector3 temp = new Vector3(horizontalInput * speed * 2f, 0, speed) * Time.fixedDeltaTime;
        transform.Translate(temp);

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -xPos, xPos);
        transform.position = position;
    }

    /// <summary>
    /// Handles interactions when the player collides with a trigger collider.
    /// </summary>
    /// <param name="other">The collider the player interacts with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            Debug.Log("Player has hit the planet!");
            GameManager.instance.UpdatePlanetInteraction();
        }
    }
}
