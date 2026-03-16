using UnityEngine;
using System.Collections;

public class EmissionMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyVisualController energyController;

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 2.0f;
    [SerializeField] private float maxHeight = 3.0f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (energyController == null) return;

        float t = Mathf.Clamp01(energyController.cleanEnergy / 100f);
        float emissionStrength = 1f - t;

        float currentSpeed = baseSpeed * Mathf.Max(emissionStrength, 0.1f);
        transform.position += Vector3.up * currentSpeed * Time.deltaTime;

        if (transform.position.y >= startPos.y + maxHeight)
        {
            Destroy(gameObject);
        }
    }

    public void SetEnergyController(EnergyVisualController controller)
    {
        energyController = controller;
    }
}