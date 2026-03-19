using UnityEngine;
using UnityEngine.UI;

public class CleanEnergyUI : MonoBehaviour
{
    public Text label;
    public Slider slider;
    public EnergyVisualController energyController;

    void Start()
    {
        if (energyController == null)
            energyController = FindObjectOfType<EnergyVisualController>();

        if (slider == null)
            slider = GetComponent<Slider>();

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.wholeNumbers = true;
        }

        if (energyController != null && slider != null)
        {
            slider.value = energyController.cleanEnergy;
            UpdateLabel(slider.value);
        }
        else
        {
            Debug.LogWarning("CleanEnergyUI: Missing slider or EnergyVisualController reference.");
        }
    }

    public void UpdateCleanEnergy()
    {
        if (energyController == null || slider == null)
            return;

        energyController.cleanEnergy = slider.value;
        UpdateLabel(slider.value);
    }

    void UpdateLabel(float energy)
    {
        if (label != null)
            label.text = "Clean Energy: " + Mathf.RoundToInt(energy) + "%";
    }
}