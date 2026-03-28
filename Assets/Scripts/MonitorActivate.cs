using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorActivate : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private GameObject monitorCanvas;
    [SerializeField] private GameObject bigKwh;
    [SerializeField] private GameObject kwhGroup;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    [Header("Big kWh Animation")]
    [SerializeField] private float bigKwhDelay = 0.2f;
    [SerializeField] private float dissipateDuration = 1.5f;
    [SerializeField] private float floatUpAmount = 0.5f;
    [SerializeField] private float groupDropDuration = 0.8f;

    private bool isOn = false;
    private bool isAnimating = false;
    private Coroutine monitorRoutine;

    private Vector3 bigKwhStartPos;
    private Vector3 kwhGroupStartPos;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (monitorCanvas != null) monitorCanvas.SetActive(false);

        if (kwhGroup != null)
        {
            kwhGroup.SetActive(false);
            kwhGroupStartPos = kwhGroup.transform.localPosition;
        }

        if (bigKwh != null)
        {
            bigKwh.SetActive(false);
            bigKwhStartPos = bigKwh.transform.localPosition;
        }
    }

    void Update()
    {
        if (!(Input.GetMouseButtonDown(0) || CAVE2.GetButtonDown(CAVE2.Button.Button5)))
            return;

        if (cam == null) cam = Camera.main;
        if (cam == null) return;
        if (isAnimating) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                isOn = !isOn;

                if (audioSource != null && clickSound != null)
                    audioSource.PlayOneShot(clickSound);

                if (monitorRoutine != null)
                    StopCoroutine(monitorRoutine);

                if (isOn)
                    monitorRoutine = StartCoroutine(TurnOnSequence());
                else
                    monitorRoutine = StartCoroutine(TurnOffSequence());
            }
        }
    }

    IEnumerator TurnOnSequence()
    {
        isAnimating = true;

        if (monitorCanvas != null)
            monitorCanvas.SetActive(true);

        if (kwhGroup != null)
            kwhGroup.SetActive(false);

        if (bigKwh != null)
        {
            ResetBigKwh();
            bigKwh.SetActive(true);
        }

        yield return new WaitForSeconds(bigKwhDelay);

        Vector3 bigKwhEndPos = bigKwhStartPos + Vector3.up * floatUpAmount;

        // Big kWh rises first
        if (bigKwh != null)
        {
            float elapsed = 0f;
            Vector3 startPos = bigKwhStartPos;
            Vector3 endPos = bigKwhEndPos;

            while (elapsed < dissipateDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / dissipateDuration);

                float driftX = Mathf.Sin(t * 8f) * 0.03f;
                float driftZ = Mathf.Cos(t * 6f) * 0.015f;

                Vector3 basePos = Vector3.Lerp(startPos, endPos, t);
                bigKwh.transform.localPosition = basePos + new Vector3(driftX, 0f, driftZ);

                yield return null;
            }

            bigKwh.SetActive(false);
        }

        // Small group drops down from where big one disappeared
        if (kwhGroup != null)
        {
            kwhGroup.transform.localPosition = bigKwhEndPos;
            kwhGroup.SetActive(true);

            float elapsed = 0f;
            Vector3 dropStart = bigKwhEndPos;
            Vector3 dropEnd = kwhGroupStartPos;

            while (elapsed < groupDropDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / groupDropDuration);

                float smoothT = Mathf.SmoothStep(0f, 1f, t);
                kwhGroup.transform.localPosition = Vector3.Lerp(dropStart, dropEnd, smoothT);

                yield return null;
            }

            kwhGroup.transform.localPosition = kwhGroupStartPos;
        }

        isAnimating = false;
    }

    IEnumerator TurnOffSequence()
    {
        //isAnimating = true;

        //if (monitorCanvas != null)
        //    monitorCanvas.SetActive(false);

        //if (kwhGroup != null)
        //{
        //    kwhGroup.transform.localPosition = kwhGroupStartPos;
        //    kwhGroup.SetActive(false);
        //}

        //if (bigKwh != null)
        //{
        //    ResetBigKwh();
        //    bigKwh.SetActive(false);
        //}

        //yield return null;
        //isAnimating = false;
        isAnimating = true;

        if (monitorCanvas != null)
            monitorCanvas.SetActive(false);

        // keep kwhGroup visible after monitor turns off
        if (bigKwh != null)
        {
            ResetBigKwh();
            bigKwh.SetActive(false);
        }

        yield return null;
        isAnimating = false;
    }

    void ResetBigKwh()
    {
        if (bigKwh != null)
            bigKwh.transform.localPosition = bigKwhStartPos;
    }


    public void TriggerFromZone()
    {
        if (isOn || isAnimating) return;

        isOn = true;

        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        if (monitorRoutine != null)
            StopCoroutine(monitorRoutine);

        monitorRoutine = StartCoroutine(TurnOnSequence());
    }
}
