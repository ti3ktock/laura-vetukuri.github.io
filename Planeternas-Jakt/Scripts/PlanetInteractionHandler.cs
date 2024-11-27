using UnityEngine;

/// <summary>
/// Handles interactions when the GameObject enters a trigger collider.
/// </summary>
/// <param name="other">The collider that the GameObject interacts with.</param>
public class PlanetInteractionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            GameManager.instance.UpdatePlanetInteraction();
        }
    }
}
