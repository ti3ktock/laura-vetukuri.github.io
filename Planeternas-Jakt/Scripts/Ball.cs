using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float distance, time;

    [SerializeField]
    private GameObject explosionPrefab; 

    [SerializeField]
    private GameObject planetExplosionPrefab; 

    [SerializeField]
    private AudioClip bounceSound; 

    private float speed, startSpeed, acceleration;

    public bool hasGameStarted;

    private AudioSource audioSource; 


    /// <summary>
    /// Initializes the ball properties and components.
    /// </summary>
    private void Start()
    {
        hasGameStarted = false;

        startSpeed = 2 * distance / time;
        acceleration = -0.995f * startSpeed / time;
        speed = startSpeed;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; 
    }

    /// <summary>
    /// Updates the ball's physics and behavior at fixed intervals.
    /// </summary>
    private void FixedUpdate()
    {
        if (!hasGameStarted) return;

        speed += acceleration * Time.fixedDeltaTime;
        Vector3 temp = new Vector3(0, speed * Time.fixedDeltaTime, 0);
        transform.localPosition += temp;
        temp = transform.localPosition;

        if (temp.y < 0f)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 4f))
            {
                GameManager.instance.GameOver();
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(transform.parent.gameObject);
            }
            else
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, Vector3.down, out hit, 4f);
                GameObject tempBlock = hit.transform.gameObject;

                PlayBounceSound();

                GameManager.instance.UpdateScore(tempBlock);
            }

            speed = startSpeed;
            GameManager.instance.SpawnBlock();
        }
    }

    /// <summary>
    /// Detects when the ball enters a trigger zone.
    /// </summary>
    /// <param name="other">The collider the ball enters.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            CollectPlanet(other);
        }
    }

    /// <summary>
    /// Handles the logic when the ball collects a planet.
    /// </summary>
    /// <param name="planet">The collider of the collected planet.</param>
    private void CollectPlanet(Collider planet)
    {
        // Retrieve the planet name from the PlanetTrigger script
        PlanetTrigger planetTrigger = planet.GetComponent<PlanetTrigger>();
        if (planetTrigger != null)
        {
            string planetName = planetTrigger.planetName;
            Debug.Log("Ball collided with: " + planetName);
            GameManager.instance.ShowPlanetPopup(planetName);
        }
        else
        {
            Debug.LogWarning("PlanetTrigger component not found on the collected planet.");
        }

        // Destroy the planet when collected
        Destroy(planet.gameObject);

        // Update planet interaction
        GameManager.instance.UpdatePlanetInteraction();

        // Instantiate planet-specific explosion effects
        if (planetExplosionPrefab != null)
        {
            Instantiate(planetExplosionPrefab, planet.transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Plays the bounce sound when the ball bounces.
    /// </summary>
    private void PlayBounceSound()
    {
        if (audioSource != null && bounceSound != null)
        {
            audioSource.clip = bounceSound;
            audioSource.volume = 0.3f;
            audioSource.Play(); 
        }
    }
}
