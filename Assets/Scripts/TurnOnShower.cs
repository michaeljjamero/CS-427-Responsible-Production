using UnityEngine;

public class TurnOnShower : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject showerWater;   // parent object holding all cylinders
    [SerializeField] private GameObject showerType;    // text object

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    private bool isOn = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (showerWater != null) showerWater.SetActive(false);
        if (showerType != null) showerType.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                isOn = !isOn;

                // water toggles
                if (showerWater != null)
                    showerWater.SetActive(isOn);

                // type appears once and stays
                if (isOn && showerType != null)
                    showerType.SetActive(true);
            }
        }
    }
}