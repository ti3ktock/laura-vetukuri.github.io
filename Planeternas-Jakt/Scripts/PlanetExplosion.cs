using UnityEngine;

public class PlanetExplosion : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem planetParticleEffect; 

    [SerializeField]
    private float destructionDelay = 3f; // Time before the explosion effect is destroyed

    /// <summary>
    /// Starts the particle effect and schedules the destruction of the GameObject.
    /// </summary>
    private void Start()
    {
        if (planetParticleEffect != null)
        {
            planetParticleEffect.Play();
        }
        Destroy(gameObject, destructionDelay);
    }

    /// <summary>
    /// Manually triggers the explosion effect at the specified position.
    /// </summary>
    /// <param name="position">The position to trigger the explosion effect.</param>
    public void TriggerExplosion(Vector3 position)
    {
        transform.position = position;
        if (planetParticleEffect != null)
        {
            planetParticleEffect.Play();
        }
        Destroy(gameObject, destructionDelay);
    }
}
