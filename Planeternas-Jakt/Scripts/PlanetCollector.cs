using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class PlanetCollector : MonoBehaviour
{
    /// <summary>
    /// References to the planet entry GameObjects for each planet.
    /// </summary>
    public GameObject earthEntry;
    public GameObject jupiterEntry;
    public GameObject marsEntry;
    public GameObject mercuryEntry;
    public GameObject neptuneEntry;
    public GameObject saturnEntry;
    public GameObject uranusEntry;
    public GameObject venusEntry;
    private List<GameObject> collectedPlanetEntries = new List<GameObject>();

    /// <summary>
    /// Dictionary defining the order of planets based on their distance from the sun.
    /// </summary>
    private Dictionary<string, int> planetOrder = new Dictionary<string, int>
    {
        { "Mercury", 0 },
        { "Venus", 1 },
        { "Earth", 2 },
        { "Mars", 3 },
        { "Jupiter", 4 },
        { "Saturn", 5 },
        { "Uranus", 6 },
        { "Neptune", 7 }
    };

    /// <summary>
    /// Activates the corresponding planet entry, applies effects, updates GameManager state, 
    /// and removes existing instances of the collected planet in the scene.
    /// </summary>
    /// <param name="planetName">The name of the planet to collect.</param>
    public void CollectPlanet(string planetName)
    {
        GameObject planetEntry = null;

        switch (planetName)
        {
            case "Earth":
                planetEntry = earthEntry;
                break;
            case "Jupiter":
                planetEntry = jupiterEntry;
                break;
            case "Mars":
                planetEntry = marsEntry;
                break;
            case "Mercury":
                planetEntry = mercuryEntry;
                break;
            case "Neptune":
                planetEntry = neptuneEntry;
                break;
            case "Saturn":
                planetEntry = saturnEntry;
                break;
            case "Uranus":
                planetEntry = uranusEntry;
                break;
            case "Venus":
                planetEntry = venusEntry;
                break;
            default:
                Debug.LogWarning("Unknown planet: " + planetName);
                return;
        }

        if (planetEntry.activeSelf)
        {
            Debug.Log($"{planetName} has already been collected.");
            return;
        }

        planetEntry.SetActive(true);
        StartCoroutine(ScaleEffect(planetEntry));

        collectedPlanetEntries.Add(planetEntry);
        collectedPlanetEntries.Sort((a, b) =>
        {
            int orderA = planetOrder[a.name.Replace("Entry", "")];
            int orderB = planetOrder[b.name.Replace("Entry", "")];
            return orderA.CompareTo(orderB);
        });

        for (int i = 0; i < collectedPlanetEntries.Count; i++)
        {
            collectedPlanetEntries[i].transform.SetSiblingIndex(i);
        }

        GameManager.instance.MarkPlanetAsCollected(planetName);
        GameManager.instance.UpdatePlanetInteraction();
        GameManager.instance.UpdateCollectedPlanetCount();
        GameObject[] spawnedPlanets = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in spawnedPlanets)
        {
            if (planet.name.Contains(planetName) && planet.transform.parent == null)
            {
                Destroy(planet);
            }
        }
    }

    /// <summary>
    /// Coroutine to apply a scaling animation effect to the collected planet entry.
    /// </summary>
    /// <param name="target">The GameObject to apply the scaling effect to.</param>
    /// <returns>An enumerator for the coroutine.</returns>
    private IEnumerator ScaleEffect(GameObject target)
    {
        Vector3 originalScale = target.transform.localScale;
        Vector3 enlargedScale = originalScale * 3f;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(originalScale, enlargedScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, elapsed / duration);
            yield return null;
        }
        target.transform.localScale = originalScale;
    }
}
