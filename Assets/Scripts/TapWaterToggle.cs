using UnityEngine;
using System.Collections;

public class TapWaterToggle : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject waterGroup;
    [SerializeField] private GameObject h2oType;
    [SerializeField] private GameObject h2oTypeLarge;

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

        if (waterGroup != null) waterGroup.SetActive(false);
        if (h2oType != null) h2oType.SetActive(false);
        if (h2oTypeLarge != null) h2oTypeLarge.SetActive(false);
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

                // Water toggles normally
                if (waterGroup != null)
                    waterGroup.SetActive(isOn);

                // Small H2O text appears and stays
                if (isOn && h2oType != null)
                    h2oType.SetActive(true);

                // Large H2O text floats up once
                if (isOn && h2oTypeLarge != null && !largeTextPlayed)
                {
                    largeTextPlayed = true;
                    StartCoroutine(FloatLargeText());
                }
            }
        }
    }

    IEnumerator FloatLargeText()
    {
        h2oTypeLarge.SetActive(true);

        Vector3 startPos = h2oTypeLarge.transform.position;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / duration);

            h2oTypeLarge.transform.position = Vector3.Lerp(startPos, endPos, p);

            yield return null;
        }

        h2oTypeLarge.SetActive(false);
        h2oTypeLarge.transform.position = startPos;
    }
}
