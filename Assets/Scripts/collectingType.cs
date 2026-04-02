using UnityEngine;

public class collectingType : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // drag from inspector

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

            //  PLAY SOUND
            audioSource.Play();

            Destroy(other.gameObject);
        }
    }
}