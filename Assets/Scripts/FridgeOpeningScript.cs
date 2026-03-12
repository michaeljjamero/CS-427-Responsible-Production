using UnityEngine;

public class FridgeOpeningScript : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject fridgeDoor;
    [SerializeField] private GameObject vocType;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private float openVolume = 1.0f;
    [SerializeField] private float closeVolume = 1.0f;

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

                if (isOpen)
                {
                    // play open sound
                    if (audioSource != null && openSound != null)
                        audioSource.PlayOneShot(openSound);
                }
                else
                {
                    // play close sound
                    if (audioSource != null && closeSound != null)
                        audioSource.PlayOneShot(closeSound);
                }
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