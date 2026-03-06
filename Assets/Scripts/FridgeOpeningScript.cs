using UnityEngine;

public class FridgeOpeningScript : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject fridgeDoor;
    [SerializeField] private GameObject vocType;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    private bool isOpen = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (fridgeDoor != null) fridgeDoor.SetActive(false);
        if (vocType != null) vocType.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                isOpen = !isOpen;

                // door toggles
                if (fridgeDoor != null) fridgeDoor.SetActive(isOpen);

                // VOC appears once and stays
                if (isOpen && vocType != null)
                {
                    vocType.SetActive(true);
                }
            }
        }
    }
}