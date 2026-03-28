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

        if (fire != null) fire.SetActive(false);
        if (pan != null) pan.SetActive(false);
        if (ch4Type != null) ch4Type.SetActive(false);
        if (ch4TypeLarge != null) ch4TypeLarge.SetActive(false);
    }

    void Update()
    {
        // Support BOTH desktop + CAVE2
        bool mouseClick = Input.GetMouseButtonDown(0);
        bool caveClick = CAVE2.GetButtonDown(CAVE2.Button.Button7); // L2 trigger

        if (!(mouseClick || caveClick)) return;

        Ray ray;

        if (caveClick && controller != null)
        {
            // CAVE2: shoot ray from controller forward
            ray = new Ray(controller.position, controller.forward);

            // Debug ray (helps visualize in editor)
            Debug.DrawRay(controller.position, controller.forward * rayDistance, Color.red, 1f);
        }
        else
        {
            // Desktop: use mouse
            ray = cam.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                ToggleStove();
            }
        }
    }

    void ToggleStove()
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
