using UnityEngine;

public class FloorRingRotate : MonoBehaviour
{
    [SerializeField] private Transform ringRoot;      // Drag: Generator Text
    [SerializeField] private EnergyVisualController energyController;

    [Header("Rotation")]
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float maxSpeed = 100f;

    void Start()
    {
        if (ringRoot == null) return;

        // Compute center of all renderers under the ring
        var renderers = ringRoot.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            b.Encapsulate(renderers[i].bounds);

        Vector3 center = b.center;

        // Move pivot to center, keep ring visually in place
        Vector3 offset = ringRoot.position - center;
        transform.position = center;
        ringRoot.position = center + offset;

        // Make ring a child of pivot if it isn't already
        if (ringRoot.parent != transform)
            ringRoot.SetParent(transform, true);
    }

    void Update()
    {
        float currentSpeed = minSpeed;

        if (energyController != null)
        {
            float cleanEnergyPercent = energyController.cleanEnergy / 100f;
            cleanEnergyPercent = Mathf.Clamp01(cleanEnergyPercent);
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, cleanEnergyPercent);
        }

        transform.Rotate(0f, -currentSpeed * Time.deltaTime, 0f, Space.Self);
    }
}