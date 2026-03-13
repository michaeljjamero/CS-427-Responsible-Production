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
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                isOpen = !isOpen;

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

                if (fridgeDoor != null) fridgeDoor.SetActive(isOpen);

                if (isOpen && vocType != null)
                    vocType.SetActive(true);

                // Large VOC text appears once and floats upward
                if (isOpen && vocTypeLarge != null && !largeTextPlayed)
                {
                    largeTextPlayed = true;
                    StartCoroutine(FloatLargeText());
                }
            }
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
