using UnityEngine;

public class generatorTrigger : MonoBehaviour
{
    [Header("Optional path systems")]
    [SerializeField] private ManualEnergyPath[] energyPaths;
    [SerializeField] private PathTextFlow[] pathTextFlows;

    [Header("Objects to turn on")]
    [SerializeField] private GameObject[] objectsToEnable;

    [Header("Settings")]
    [SerializeField] private bool destroyChestAfter = true;
    [SerializeField] private string chestTag = "Chest";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(chestTag))
            return;

       

        for (int i = 0; i < objectsToEnable.Length; i++)
        {
            if (objectsToEnable[i] != null)
                objectsToEnable[i].SetActive(true);
        }

        if (destroyChestAfter)
            Destroy(other.transform.root.gameObject);
    }
}