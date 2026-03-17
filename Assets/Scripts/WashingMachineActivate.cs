using System.Collections;
using UnityEngine;

public class WashingMachineActivate : MonoBehaviour
{
    [Header("Toggle Targets")]
    [SerializeField] private GameObject towel;
    [SerializeField] private GameObject basket;

    [Header("CO2 Type Animation")]
    [SerializeField] private GameObject bigCo2Type;
    [SerializeField] private GameObject co2Type;

    [Header("Machine To Wobble")]
    [SerializeField] private Transform machineBody;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("Wobble Settings")]
    [SerializeField] private float wobbleAngle = 3f;
    [SerializeField] private float wobbleSpeed = 6f;

    [Header("Big Type Animation Settings")]
    [SerializeField] private float bigTypeDelay = 0.2f;
    [SerializeField] private float dissipateDuration = 1.5f;
    [SerializeField] private float floatUpAmount = 0.5f;
    [SerializeField] private float groupDropDuration = 0.8f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip washingLoop;

    private bool isOn = false;
    private bool isAnimating = false;

    private Quaternion originalRotation;
    private Coroutine wobbleRoutine;
    private Coroutine machineRoutine;

    private Vector3 bigCo2StartPos;
    private Vector3 co2GroupStartPos;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (machineBody == null)
            machineBody = transform;

        originalRotation = machineBody.localRotation;

        if (towel != null) towel.SetActive(false);
        if (basket != null) basket.SetActive(false);

        if (co2Type != null)
        {
            co2Type.SetActive(false);
            co2GroupStartPos = co2Type.transform.localPosition;
        }

        if (bigCo2Type != null)
        {
            bigCo2Type.SetActive(false);
            bigCo2StartPos = bigCo2Type.transform.localPosition;
        }
    }

    void Update()
    {
        if (!(Input.GetMouseButtonDown(0) || CAVE2.GetButtonDown(CAVE2.Button.Button5)))
            return;
        if (cam == null) return;
        if (isAnimating) return;

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

        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        if (machineRoutine != null)
            StopCoroutine(machineRoutine);

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

            machineRoutine = StartCoroutine(TurnOnSequence());
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

            machineRoutine = StartCoroutine(TurnOffSequence());
        }
    }

    IEnumerator TurnOnSequence()
    {
        isAnimating = true;

        if (towel != null) towel.SetActive(true);
        if (basket != null) basket.SetActive(true);

        if (co2Type != null)
            co2Type.SetActive(false);

        if (bigCo2Type != null)
        {
            ResetBigType();
            bigCo2Type.SetActive(true);
        }

        yield return new WaitForSeconds(bigTypeDelay);

        Vector3 bigTypeEndPos = bigCo2StartPos + Vector3.up * floatUpAmount;

        // Big CO2 rises first
        if (bigCo2Type != null)
        {
            float elapsed = 0f;
            Vector3 startPos = bigCo2StartPos;
            Vector3 endPos = bigTypeEndPos;

            while (elapsed < dissipateDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / dissipateDuration);

                float driftX = Mathf.Sin(t * 8f) * 0.03f;
                float driftZ = Mathf.Cos(t * 6f) * 0.015f;

                Vector3 basePos = Vector3.Lerp(startPos, endPos, t);
                bigCo2Type.transform.localPosition = basePos + new Vector3(driftX, 0f, driftZ);

                yield return null;
            }

            bigCo2Type.SetActive(false);
        }

        // Small CO2 group drops down from where big one disappeared
        if (co2Type != null)
        {
            co2Type.transform.localPosition = bigTypeEndPos;
            co2Type.SetActive(true);

            float elapsed = 0f;
            Vector3 dropStart = bigTypeEndPos;
            Vector3 dropEnd = co2GroupStartPos;

            while (elapsed < groupDropDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / groupDropDuration);

                float smoothT = Mathf.SmoothStep(0f, 1f, t);
                co2Type.transform.localPosition = Vector3.Lerp(dropStart, dropEnd, smoothT);

                yield return null;
            }

            co2Type.transform.localPosition = co2GroupStartPos;
        }

        isAnimating = false;
    }

    IEnumerator TurnOffSequence()
    {
        isAnimating = true;

        if (towel != null) towel.SetActive(false);
        if (basket != null) basket.SetActive(false);

        // keep kwhGroup visible after TV turns off
        if (bigCo2Type != null)
        {
            ResetBigType();
            bigCo2Type.SetActive(false);
        }

        yield return null;
        isAnimating = false;
        //isAnimating = true;

        //if (towel != null) towel.SetActive(false);
        //if (basket != null) basket.SetActive(false);

        //if (co2Type != null)
        //{
        //    co2Type.transform.localPosition = co2GroupStartPos;
        //    co2Type.SetActive(false);
        //}

        //if (bigCo2Type != null)
        //{
        //    ResetBigType();
        //    bigCo2Type.SetActive(false);
        //}

        //yield return null;
        //isAnimating = false;
    }

    void ResetBigType()
    {
        if (bigCo2Type != null)
            bigCo2Type.transform.localPosition = bigCo2StartPos;
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


    //[Header("Toggle Targets")]
    //[SerializeField] private GameObject towel;
    //[SerializeField] private GameObject basket;
    //[SerializeField] private GameObject co2Type;

    //[Header("Machine To Wobble")]
    //[SerializeField] private Transform machineBody;

    //[Header("Click Settings")]
    //[SerializeField] private Camera cam;

    //[Header("Wobble Settings")]
    //[SerializeField] private float wobbleAngle = 3f;
    //[SerializeField] private float wobbleSpeed = 6f;

    //[Header("Audio")]
    //[SerializeField] private AudioSource audioSource;
    //[SerializeField] private AudioClip clickSound;
    //[SerializeField] private AudioClip washingLoop;

    //private bool isOn = false;
    //private Quaternion originalRotation;
    //private Coroutine wobbleRoutine;

    //void Start()
    //{
    //    if (cam == null) cam = Camera.main;

    //    if (machineBody == null)
    //        machineBody = transform;

    //    originalRotation = machineBody.localRotation;

    //    if (towel != null) towel.SetActive(false);
    //    if (basket != null) basket.SetActive(false);
    //    if (co2Type != null) co2Type.SetActive(false);
    //}

    //void Update()
    //{
    //    if (!Input.GetMouseButtonDown(0)) return;
    //    if (cam == null) return;

    //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        if (hit.transform == transform || hit.transform.IsChildOf(transform))
    //        {
    //            ToggleMachine();
    //        }
    //    }
    //}

    //void ToggleMachine()
    //{
    //    isOn = !isOn;

    //    // play click
    //    if (audioSource != null && clickSound != null)
    //        audioSource.PlayOneShot(clickSound);

    //    if (towel != null) towel.SetActive(isOn);
    //    if (basket != null) basket.SetActive(isOn);

    //    if (isOn && co2Type != null)
    //        co2Type.SetActive(true);

    //    if (isOn)
    //    {
    //        if (audioSource != null && washingLoop != null)
    //        {
    //            audioSource.clip = washingLoop;
    //            audioSource.loop = true;
    //            audioSource.Play();
    //        }

    //        if (wobbleRoutine == null)
    //            wobbleRoutine = StartCoroutine(WobbleMachine());
    //    }
    //    else
    //    {
    //        if (audioSource != null)
    //            audioSource.Stop();

    //        if (wobbleRoutine != null)
    //        {
    //            StopCoroutine(wobbleRoutine);
    //            wobbleRoutine = null;
    //        }

    //        machineBody.localRotation = originalRotation;
    //    }
    //}

    //IEnumerator WobbleMachine()
    //{
    //    while (true)
    //    {
    //        float zAngle = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAngle;
    //        machineBody.localRotation = originalRotation * Quaternion.Euler(0f, 0f, zAngle);
    //        yield return null;
    //    }
    //}
}