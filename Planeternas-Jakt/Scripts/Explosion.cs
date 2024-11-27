using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem planetParticleEffect;

    /// <summary>
    /// Plays the particle effect and schedules the destruction of the GameObject.
    /// </summary>
    private void Start()
    {
        if (planetParticleEffect != null)
        {
            planetParticleEffect.Play();
        }

        Destroy(gameObject, 5f);
    }
}
