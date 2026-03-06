using UnityEngine;

public class KitchenSinkh20 : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject waterGroup;
    [SerializeField] private GameObject h2oType;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

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
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                isOn = !isOn;

                // Water toggles normally
                if (waterGroup != null)
                    waterGroup.SetActive(isOn);

                // Text appears once and stays
                if (isOn && h2oType != null)
                    h2oType.SetActive(true);
            }
        }
    }
}