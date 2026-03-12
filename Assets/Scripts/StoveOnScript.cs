using UnityEngine;

public class StoveOnScript : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject pan;
    [SerializeField] private GameObject ch4Type;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    private bool isOn = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (fire != null) fire.SetActive(false);
        if (pan != null) pan.SetActive(false);
        if (ch4Type != null) ch4Type.SetActive(false);
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

                // Fire and pan toggle normally
                if (fire != null) fire.SetActive(isOn);
                if (pan != null) pan.SetActive(isOn);

                // CH4 appears once and stays
                if (isOn && ch4Type != null)
                {
                    ch4Type.SetActive(true);
                }
            }
        }
    }
}