using UnityEngine;

public class EmissionMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyVisualController energyController;
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float maxHeight = 3f;

    [Header("Drift")]
    [SerializeField] private float driftStrength = 0.25f;
    [SerializeField] private float driftSpeed = 1.2f;

    [Header("Swirl")]
    [SerializeField] private float swirlRadius = 0.15f;
    [SerializeField] private float swirlSpeed = 2f;

    private Vector3 startPos;
    private Vector3 swirlOffset;
    private Vector3 lastSwirlOffset;
    private float randomSeed;

    private void Start()
    {
        startPos = transform.position;

        if (energyController == null)
            energyController = FindObjectOfType<EnergyVisualController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        randomSeed = Random.Range(0f, 100f);
    }

    private void Update()
    {
        if (energyController == null) return;

        float t = Mathf.Clamp01(energyController.cleanEnergy / 100f);
        float emissionStrength = 1f - t;

        float currentSpeed = baseSpeed * Mathf.Max(emissionStrength, 0.1f);

        // Move upward
        Vector3 upwardMove = Vector3.up * currentSpeed * Time.deltaTime;

        // Gentle drift using Perlin noise
        float driftX = Mathf.PerlinNoise(Time.time * driftSpeed, randomSeed) - 0.5f;
        float driftZ = Mathf.PerlinNoise(randomSeed, Time.time * driftSpeed) - 0.5f;
        Vector3 driftMove = new Vector3(driftX, 0f, driftZ) * driftStrength * emissionStrength * Time.deltaTime;

        // Swirl motion
        float swirlAngle = Time.time * swirlSpeed + randomSeed;
        swirlOffset = new Vector3(
            Mathf.Cos(swirlAngle),
            0f,
            Mathf.Sin(swirlAngle)
        ) * (swirlRadius * emissionStrength);

        // Apply only the change in swirl offset each frame
        Vector3 swirlMove = swirlOffset - lastSwirlOffset;
        lastSwirlOffset = swirlOffset;

        transform.position += upwardMove + driftMove + swirlMove;

        // Keep text upright but facing the camera
        if (cameraTransform != null)
        {
            Vector3 dir = cameraTransform.position - transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }

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