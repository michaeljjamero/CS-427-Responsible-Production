using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvKwhActivate : MonoBehaviour
{
    [Header("Assign the parent that holds all kWh objects (or a single object)")]
    [SerializeField] private GameObject kwhGroup;
    [SerializeField] private GameObject tvCanvas; // Drag Canvas here

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    private bool isOn = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (tvCanvas != null) tvCanvas.SetActive(false);
        if (kwhGroup != null) kwhGroup.SetActive(false);
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

                // Play click sound
                if (audioSource != null && clickSound != null)
                {
                    audioSource.PlayOneShot(clickSound);
                }

                // Water toggles normally
                if (tvCanvas != null)
                    tvCanvas.SetActive(isOn);

                // Text appears once and stays
                if (isOn && kwhGroup != null)
                    kwhGroup.SetActive(true);
            }
        }
    }

    
}
