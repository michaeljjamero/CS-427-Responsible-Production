using UnityEngine;
using System.Collections;

public class FridgeOpeningScript : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject fridgeDoor;
    [SerializeField] private GameObject vocType;
    [SerializeField] private GameObject vocTypeLarge;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("CAVE2 Settings")]
    [SerializeField] private Transform controller; // assign wand here
    [SerializeField] private float rayDistance = 100f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private float openVolume = 1.0f;
    [SerializeField] private float closeVolume = 1.0f;

    [Header("Large Text Animation")]
    [SerializeField] private float floatDistance = 2f;
    [SerializeField] private float duration = 4f;

    private bool isOpen = false;
    private bool largeTextPlayed = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (fridgeDoor != null) fridgeDoor.SetActive(false);
        if (vocType != null) vocType.SetActive(false);
        if (vocTypeLarge != null) vocTypeLarge.SetActive(false);
    }

    void Update()
    {
        // Dual input support
        bool mouseClick = Input.GetMouseButtonDown(0);
        bool caveClick = CAVE2.GetButtonDown(CAVE2.Button.Button7);

        if (!(mouseClick || caveClick)) return;

        Ray ray;

        if (caveClick && controller != null)
        {
            ray = new Ray(controller.position, controller.forward);
            Debug.DrawRay(controller.position, controller.forward * rayDistance, Color.green, 1f);
        }
        else
        {
            if (cam == null) cam = Camera.main;
            if (cam == null) return;

            ray = cam.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                ToggleFridge();
            }
        }
    }

    void ToggleFridge()
    {
        isOpen = !isOpen;

        // Audio
        if (isOpen)
        {
            if (audioSource != null && openSound != null)
                audioSource.PlayOneShot(openSound, openVolume);
        }
        else
        {
            if (audioSource != null && closeSound != null)
                audioSource.PlayOneShot(closeSound, closeVolume);
        }

        // Door toggle
        if (fridgeDoor != null)
            fridgeDoor.SetActive(isOpen);

        // Small VOC text stays
        if (isOpen && vocType != null)
            vocType.SetActive(true);

        // Large VOC text once
        if (isOpen && vocTypeLarge != null && !largeTextPlayed)
        {
            largeTextPlayed = true;
            StartCoroutine(FloatLargeText());
        }
    }

    IEnumerator FloatLargeText()
    {
        vocTypeLarge.SetActive(true);

        Vector3 startPos = vocTypeLarge.transform.position;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / duration);

            vocTypeLarge.transform.position = Vector3.Lerp(startPos, endPos, p);

            yield return null;
        }

        vocTypeLarge.SetActive(false);
        vocTypeLarge.transform.position = startPos;
    }
}