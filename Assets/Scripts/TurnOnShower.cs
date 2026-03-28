using UnityEngine;
using System.Collections;

public class TurnOnShower : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject showerWater;
    [SerializeField] private GameObject showerType;
    [SerializeField] private GameObject showerTypeLarge;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("CAVE2 Settings")]
    [SerializeField] private Transform controller; // assign wand here
    [SerializeField] private float rayDistance = 100f;

    [Header("Large Text Animation")]
    [SerializeField] private float floatDistance = 2f;
    [SerializeField] private float duration = 4f;

    private bool isOn = false;
    private bool largeTextPlayed = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (showerWater != null) showerWater.SetActive(false);
        if (showerType != null) showerType.SetActive(false);
        if (showerTypeLarge != null) showerTypeLarge.SetActive(false);
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
            Debug.DrawRay(controller.position, controller.forward * rayDistance, Color.cyan, 1f);
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
                ToggleShower();
            }
        }
    }

    void ToggleShower()
    {
        isOn = !isOn;

        // Water toggle
        if (showerWater != null)
            showerWater.SetActive(isOn);

        // Small text stays
        if (isOn && showerType != null)
            showerType.SetActive(true);

        // Large text once
        if (isOn && showerTypeLarge != null && !largeTextPlayed)
        {
            largeTextPlayed = true;
            StartCoroutine(FloatLargeText());
        }
    }

    IEnumerator FloatLargeText()
    {
        showerTypeLarge.SetActive(true);

        Vector3 startPos = showerTypeLarge.transform.position;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / duration);

            showerTypeLarge.transform.position = Vector3.Lerp(startPos, endPos, p);

            yield return null;
        }

        showerTypeLarge.SetActive(false);
        showerTypeLarge.transform.position = startPos;
    }
}