using UnityEngine;
using System.Collections;

public class StoveOnScript : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject pan;
    [SerializeField] private GameObject ch4Type;
    [SerializeField] private GameObject ch4TypeLarge;

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

        if (fire != null) fire.SetActive(false);
        if (pan != null) pan.SetActive(false);
        if (ch4Type != null) ch4Type.SetActive(false);
        if (ch4TypeLarge != null) ch4TypeLarge.SetActive(false);
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

                if (fire != null) fire.SetActive(isOn);
                if (pan != null) pan.SetActive(isOn);

                if (isOn && ch4Type != null)
                    ch4Type.SetActive(true);

                // Only play the large floating text once
                if (isOn && ch4TypeLarge != null && !largeTextPlayed)
                {
                    largeTextPlayed = true;
                    StartCoroutine(FloatLargeText());
                }
            }
        }
    }

    IEnumerator FloatLargeText()
    {
        ch4TypeLarge.SetActive(true);

        Vector3 startPos = ch4TypeLarge.transform.position;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / duration);

            ch4TypeLarge.transform.position = Vector3.Lerp(startPos, endPos, p);

            yield return null;
        }

        ch4TypeLarge.SetActive(false);
        ch4TypeLarge.transform.position = startPos;
    }
}
