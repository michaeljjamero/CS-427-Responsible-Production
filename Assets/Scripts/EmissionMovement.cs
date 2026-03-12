using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyVisualController energyController;

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 0.75f;
    [SerializeField] private float maxHeight = 1.5f;

    [Header("Optional")]
    [SerializeField] private bool hideAtMaxCleanEnergy = true;
    [SerializeField] private float minVisibleStrength = 0.05f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (energyController == null) return;

        float t = Mathf.Clamp01(energyController.cleanEnergy / 100f);

        // full emissions at low clean energy, less emissions at high clean energy
        float emissionStrength = 1f - t;

        // hide completely when clean energy is high
        if (hideAtMaxCleanEnergy && emissionStrength <= minVisibleStrength)
        {
            gameObject.SetActive(false);
            return;
        }
        else if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        float currentSpeed = baseSpeed * emissionStrength;

        transform.localPosition += Vector3.up * currentSpeed * Time.deltaTime;

        if (transform.localPosition.y >= startPos.y + maxHeight)
        {
            transform.localPosition = startPos;
        }
    }
}
