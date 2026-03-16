using UnityEngine;
using System.Collections;

public class TurnOnShower : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject showerWater;   // parent object holding all cylinders
    [SerializeField] private GameObject showerType;    // small text object
    [SerializeField] private GameObject showerTypeLarge; // large floating text

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

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

                // small text appears once and stays
                if (isOn && showerType != null)
                    showerType.SetActive(true);

                // large text floats upward once
                if (isOn && showerTypeLarge != null && !largeTextPlayed)
                {
                    largeTextPlayed = true;
                    StartCoroutine(FloatLargeText());
                }
            }
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
