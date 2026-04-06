using UnityEngine;
using System.Collections.Generic;

public class countingTypes : MonoBehaviour
{
    public static countingTypes instance;

    // Per-type counts
    public Dictionary<string, int> counts = new Dictionary<string, int>();

 
    public System.Action<int> OnEnergyChanged;

    // Global energy
    public int totalEnergy = 0;

    [Header("Chest Unlock Settings")]
    [SerializeField] private string chestTag = "Chest";
    [SerializeField] private int unlockThreshold = 4;

    private List<GrabbableObject> chestGrabs = new List<GrabbableObject>();
    private bool unlocked = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Find all chests by tag
        GameObject[] chests = GameObject.FindGameObjectsWithTag(chestTag);

        foreach (GameObject obj in chests)
        {
            GrabbableObject grab = obj.GetComponent<GrabbableObject>();

            if (grab != null)
            {
                grab.enabled = false;
                chestGrabs.Add(grab);
            }
            else
            {
                Debug.LogWarning(obj.name + " is tagged as Chest but has no GrabbableObject!");
            }
        }

        Debug.Log("Found " + chestGrabs.Count + " chests.");
    }

    public void AddElement(string tag)
    {
        // Update per-type counts
        if (!counts.ContainsKey(tag))
            counts[tag] = 0;

        counts[tag]++;

        // Update total energy
        totalEnergy++;

        
        OnEnergyChanged?.Invoke(totalEnergy);

        Debug.Log("Collected: " + tag);
        Debug.Log("Total Energy: " + totalEnergy);

        // Unlock check
        if (!unlocked && totalEnergy >= unlockThreshold)
        {
            UnlockAllChests();
        }
    }

    void UnlockAllChests()
    {
        unlocked = true;

        foreach (var grab in chestGrabs)
        {
            if (grab != null)
                grab.enabled = true;
        }

        Debug.Log("All chests unlocked!");
    }
}