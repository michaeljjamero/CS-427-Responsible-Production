using System.Collections;
using UnityEngine;

public class WashingMachineActivate : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject towel;
    [SerializeField] private GameObject basket;
    [SerializeField] private GameObject co2Type;

    [Header("Machine To Wobble")]
    [SerializeField] private Transform machineBody;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("Wobble Settings")]
    [SerializeField] private float wobbleAngle = 3f;
    [SerializeField] private float wobbleSpeed = 6f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip washingLoop;

    private bool isOn = false;
    private Quaternion originalRotation;
    private Coroutine wobbleRoutine;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (machineBody == null)
            machineBody = transform;

        originalRotation = machineBody.localRotation;

        if (towel != null) towel.SetActive(false);
        if (basket != null) basket.SetActive(false);
        if (co2Type != null) co2Type.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                ToggleMachine();
            }
        }
    }

    void ToggleMachine()
    {
        isOn = !isOn;

        // play click
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        if (towel != null) towel.SetActive(isOn);
        if (basket != null) basket.SetActive(isOn);

        if (isOn && co2Type != null)
            co2Type.SetActive(true);

        if (isOn)
        {
            if (audioSource != null && washingLoop != null)
            {
                audioSource.clip = washingLoop;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (wobbleRoutine == null)
                wobbleRoutine = StartCoroutine(WobbleMachine());
        }
        else
        {
            if (audioSource != null)
                audioSource.Stop();

            if (wobbleRoutine != null)
            {
                StopCoroutine(wobbleRoutine);
                wobbleRoutine = null;
            }

            machineBody.localRotation = originalRotation;
        }
    }

    IEnumerator WobbleMachine()
    {
        while (true)
        {
            float zAngle = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAngle;
            machineBody.localRotation = originalRotation * Quaternion.Euler(0f, 0f, zAngle);
            yield return null;
        }
    }
}