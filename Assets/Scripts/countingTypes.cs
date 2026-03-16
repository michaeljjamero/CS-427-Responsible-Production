using UnityEngine;
using System.Collections.Generic;

public class countingTypes : MonoBehaviour
{
    public static countingTypes instance;

    public Dictionary<string, int> counts = new Dictionary<string, int>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddElement(string tag)
    {
        if (!counts.ContainsKey(tag))
            counts[tag] = 0;

        counts[tag]++;

        Debug.Log(tag + " count: " + counts[tag]);

       

        // Optional: log the entire inventory
        foreach (var item in counts)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }
}
