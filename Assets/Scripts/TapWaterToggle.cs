using UnityEngine;

public class TapWaterToggle : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject waterGroup;   // Drag: Kitchen Sink H2O
    [SerializeField] private GameObject h2oType;      // Drag: your H2O text/typography object

    [Header("Click Settings")]
    [SerializeField] private Camera cam;              // Optional (leave empty to use MainCamera)

    private bool isOn = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (waterGroup != null) waterGroup.SetActive(false);
        if (h2oType != null) h2oType.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            // Clicked tap or any child mesh of tap
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                isOn = !isOn;

                if (waterGroup != null) waterGroup.SetActive(isOn);
                if (h2oType != null) h2oType.SetActive(isOn);
            }
        }
    }
}