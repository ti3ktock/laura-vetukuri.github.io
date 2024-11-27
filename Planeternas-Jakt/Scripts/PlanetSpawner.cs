using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] planetPrefabs;
    private GameObject currentPlanet;
    private GameObject lastSpawnedPlanet;
    private static int blockCount = 0;
    private static Dictionary<string, bool> planetStatus = new Dictionary<string, bool>(); // Tracks collected status

    /// <summary>
    /// Initializes the planet spawner and determines whether to spawn a planet.
    /// </summary>
    private void Start()
    {
        blockCount++;

        // Spawn a planet only every 4 blocks
        if (blockCount % 4 != 0)
        {
            return;
        }

        // Skip spawning on the first block if it's marked in the GameManager
        if (GameManager.instance != null && GameManager.instance.IsFirstBlock)
        {
            GameManager.instance.IsFirstBlock = false;
            return;
        }

        SpawnPlanet();
    }

    /// <summary>
    /// Spawns a planet from the available pool of planets.
    /// </summary>
    private void SpawnPlanet()
    {
        List<GameObject> availablePlanets = new List<GameObject>();

        // Filter planets that are not collected and not recently spawned
        foreach (GameObject planetPrefab in planetPrefabs)
        {
            string planetName = planetPrefab.name;

            if (!GameManager.instance.IsPlanetCollected(planetName)
                && (lastSpawnedPlanet == null || lastSpawnedPlanet.name != planetName))
            {
                availablePlanets.Add(planetPrefab);
            }
        }

        if (availablePlanets.Count == 0)
        {
            return;
        }

        // Randomly select a planet to spawn
        int index = Random.Range(0, availablePlanets.Count);
        GameObject selectedPlanet = availablePlanets[index];
        string selectedPlanetName = selectedPlanet.name;

        // Instantiate and configure the new planet
        currentPlanet = Instantiate(selectedPlanet, transform);
        currentPlanet.transform.localPosition = new Vector3(0, 1, 0);
        currentPlanet.tag = "Planet";

        lastSpawnedPlanet = currentPlanet;

        // Update planet status if it is not already tracked
        if (!planetStatus.ContainsKey(selectedPlanetName))
        {
            planetStatus.Add(selectedPlanetName, false);
        }
    }

    /// <summary>
    /// Destroys the currently spawned planet and updates its status.
    /// </summary>
    public void DestroyPlanet()
    {
        if (currentPlanet != null)
        {
            string planetName = currentPlanet.name;

            Destroy(currentPlanet);
            currentPlanet = null;

            if (GameManager.instance != null)
            {
                GameManager.instance.OnPlanetDestroyed();
            }

            if (planetStatus.ContainsKey(planetName))
            {
                planetStatus[planetName] = false;
            }
        }
    }

    /// <summary>
    /// Ensures any spawned planet is destroyed when the spawner is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (currentPlanet != null)
        {
            Destroy(currentPlanet);
        }
    }
}
