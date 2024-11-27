using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private GameObject player;

    /// <summary>
    /// Initializes the block and finds the player GameObject.
    /// </summary>
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    /// <summary>
    /// Checks the position of the player relative to the block and destroys the block if it's far behind the player.
    /// </summary>
    private void Update()
    {
        if (!player) return;
        float playerPosZ = player.transform.position.z;
        float currentZ = transform.position.z;
        if (playerPosZ - currentZ > 15f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unsubscribes from the combo animation update event when the block is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        GameManager.instance.updateComboAnimation -= UpdateComboAnimation;
    }

    /// <summary>
    /// Subscribes the block to the combo animation update event.
    /// </summary>
    public void SubscribeToMethod()
    {
        GameManager.instance.updateComboAnimation += UpdateComboAnimation;
    }

    /// <summary>
    /// Updates the combo animation behavior.
    /// </summary>
    /// <param name="isCombo">Indicates whether the combo is active.</param>
    void UpdateComboAnimation(bool isCombo)
    {
        // Update combo behavior if needed
    }
    
}
