using UnityEngine;

public class TextSwaponEnergy : MonoBehaviour
{
    [Header("Text Objects")]
    [SerializeField] private GameObject originalText;
    [SerializeField] private GameObject collectedText;
    [SerializeField] private GameObject instructionText;

    [Header("Settings")]
    [SerializeField] private int triggerEnergy = 4;

    private bool triggered = false;

    void Start()
    {
        if (collectedText != null) collectedText.SetActive(false);
        if (instructionText != null) instructionText.SetActive(false);

        countingTypes.instance.OnEnergyChanged += CheckEnergy;
    }

    void CheckEnergy(int energy)
    {
        if (triggered) return;

        if (energy >= triggerEnergy)
        {
            triggered = true;

            if (originalText != null)
                originalText.SetActive(false);

            if (collectedText != null)
                collectedText.SetActive(true);

            if (instructionText != null)
                instructionText.SetActive(true);
        }
    }
}