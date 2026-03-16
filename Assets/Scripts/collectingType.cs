using UnityEngine;

public class collectingType : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CH4") ||
            other.CompareTag("VOC") ||
            other.CompareTag("H2O") ||
            other.CompareTag("CO2") ||
            other.CompareTag("KWH"))
        {
            countingTypes.instance.AddElement(other.tag);

            Debug.Log("Collected: " + other.tag);

            // Completely remove it so it can't block raycasts
            Destroy(other.gameObject);
        }
    }
}
