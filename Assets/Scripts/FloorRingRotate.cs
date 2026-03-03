using UnityEngine;

public class FloorRingRotate : MonoBehaviour
{
    [SerializeField] private Transform ringRoot;      // Drag: Generator Text
    [SerializeField] private float speed = 10f;

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
        transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.Self);
    }
}