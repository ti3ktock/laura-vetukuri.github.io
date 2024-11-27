using UnityEngine;

public class PlanetTrigger : MonoBehaviour
{
    public string planetName;

    /// <summary>
    /// Detects when a GameObject with the tag "Ball" enters the trigger and notifies the GameManager.
    /// </summary>
    /// <param name="other">The collider of the object entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GameManager.instance.ShowPlanetPopup(planetName);
        }
    }

}
