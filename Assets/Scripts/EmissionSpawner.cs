using System.Collections.Generic;
using UnityEngine;

public class EmissionSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyVisualController energyController;
    [SerializeField] private GameObject emissionPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField] private float baseSpawnInterval = 0.5f;
    [SerializeField] private int maxActiveEmissions = 10;

    private float timer = 0f;
    private List<GameObject> activeEmissions = new List<GameObject>();

    private void Update()
    {
        if (energyController == null || emissionPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
            return;

        float t = Mathf.Clamp01(energyController.cleanEnergy / 4f);
        float emissionStrength = 1f - t;

        activeEmissions.RemoveAll(e => e == null);

        if (emissionStrength <= 0.01f)
            return;

        int allowedActive = Mathf.RoundToInt(maxActiveEmissions * emissionStrength);
        allowedActive = Mathf.Max(1, allowedActive);

        float currentSpawnInterval = baseSpawnInterval / Mathf.Max(emissionStrength, 0.1f);

        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval && activeEmissions.Count < allowedActive)
        {
            timer = 0f;

            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Vector3 randomOffset = new Vector3(
                Random.Range(-0.2f, 0.2f),
                0f,
                Random.Range(-0.2f, 0.2f)
            );

            GameObject newEmission = Instantiate(
                emissionPrefab,
                randomPoint.position + randomOffset,
                randomPoint.rotation
            );

            EmissionMovement movement = newEmission.GetComponent<EmissionMovement>();
            if (movement != null)
            {
                movement.SetEnergyController(energyController);
            }

            activeEmissions.Add(newEmission);
        }
    }
}