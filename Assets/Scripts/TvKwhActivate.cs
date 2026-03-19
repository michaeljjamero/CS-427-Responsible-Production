using System.Collections;
using UnityEngine;

public class TvKwhActivate : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private GameObject tvCanvas;
    [SerializeField] private GameObject bigKwh;
    [SerializeField] private GameObject kwhGroup;

    [Header("Click Settings")]
    [SerializeField] private Camera cam;

    [Header("CAVE2 Settings")]
    [SerializeField] private Transform controller; // drag Wand here
    [SerializeField] private float rayDistance = 1000f;

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
    private Coroutine tvRoutine;

    private Vector3 bigKwhStartPos;
    private Material bigKwhMaterialInstance;
    private Color bigKwhStartColor;
    private Vector3 kwhGroupStartPos;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (tvCanvas != null) tvCanvas.SetActive(false);

        if (kwhGroup != null)
        {
            kwhGroup.SetActive(false);
            kwhGroupStartPos = kwhGroup.transform.localPosition;
        }

        if (bigKwh != null) bigKwh.SetActive(false);

        if (bigKwh != null)
        {
            bigKwhStartPos = bigKwh.transform.localPosition;

            MeshRenderer mr = bigKwh.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                bigKwhMaterialInstance = mr.material;
                bigKwhStartColor = bigKwhMaterialInstance.color;
            }
        }
    }

    void Update()
    {
        bool mouseClick = Input.GetMouseButtonDown(0);
        bool caveClick = CAVE2.GetButtonDown(CAVE2.Button.Button7); // trigger (recommended)

        if (!(mouseClick || caveClick))
            return;

        if (cam == null) cam = Camera.main;
        if (cam == null) return;
        if (isAnimating) return;

        Ray ray;

        if (caveClick && controller != null)
        {
            // CAVE2 → wand ray
            ray = new Ray(controller.position, controller.forward);

            // Debug ray (helps visualize direction)
            Debug.DrawRay(controller.position, controller.forward * rayDistance, Color.red, 1f);
        }
        else
        {
            // Desktop → mouse ray
            ray = cam.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                isOn = !isOn;

                if (audioSource != null && clickSound != null)
                    audioSource.PlayOneShot(clickSound);

                if (tvRoutine != null)
                    StopCoroutine(tvRoutine);

                if (isOn)
                    tvRoutine = StartCoroutine(TurnOnSequence());
                else
                    tvRoutine = StartCoroutine(TurnOffSequence());
            }
        }
    }

    IEnumerator TurnOnSequence()
    {
        isAnimating = true;

        if (tvCanvas != null)
            tvCanvas.SetActive(true);

        if (kwhGroup != null)
            kwhGroup.SetActive(false);

        if (bigKwh != null)
        {
            ResetBigKwh();
            bigKwh.SetActive(true);
        }

        yield return new WaitForSeconds(bigKwhDelay);

        Vector3 bigKwhEndPos = bigKwhStartPos + Vector3.up * floatUpAmount;

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
        isAnimating = true;

        if (tvCanvas != null)
            tvCanvas.SetActive(false);

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
        bigKwh.transform.localPosition = bigKwhStartPos;

        if (bigKwhMaterialInstance != null)
        {
            Color c = bigKwhStartColor;
            c.a = 1f;
            bigKwhMaterialInstance.color = c;
        }
    }
}








//using System.Collections;
//using UnityEngine;

//public class TvKwhActivate : MonoBehaviour
//{
//    [Header("Scene References")]
//    [SerializeField] private GameObject tvCanvas;
//    [SerializeField] private GameObject bigKwh;
//    [SerializeField] private GameObject kwhGroup;

//    [Header("Click Settings")]
//    [SerializeField] private Camera cam;

//    [Header("Audio")]
//    [SerializeField] private AudioSource audioSource;
//    [SerializeField] private AudioClip clickSound;

//    [Header("Big kWh Animation")]
//    [SerializeField] private float bigKwhDelay = 0.2f;
//    [SerializeField] private float dissipateDuration = 1.5f;
//    [SerializeField] private float floatUpAmount = 0.5f;
//    [SerializeField] private float groupDropDuration = 0.8f;

//    private bool isOn = false;
//    private bool isAnimating = false;
//    private Coroutine tvRoutine;

//    private Vector3 bigKwhStartPos;
//    private Material bigKwhMaterialInstance;
//    private Color bigKwhStartColor;
//    private Vector3 kwhGroupStartPos;



//    void Start()
//    {
//        if (cam == null) cam = Camera.main;

//        if (tvCanvas != null) tvCanvas.SetActive(false);
//        if (kwhGroup != null)
//        {
//            kwhGroup.SetActive(false);
//            kwhGroupStartPos = kwhGroup.transform.localPosition;
//        }

//        if (bigKwh != null) bigKwh.SetActive(false);

//        if (bigKwh != null)
//        {
//            bigKwhStartPos = bigKwh.transform.localPosition;

//            MeshRenderer mr = bigKwh.GetComponent<MeshRenderer>();
//            if (mr != null)
//            {
//                bigKwhMaterialInstance = mr.material;
//                bigKwhStartColor = bigKwhMaterialInstance.color;
//            }
//        }
//    }

//    void Update()
//    {
//        if (!(Input.GetMouseButtonDown(0) || CAVE2.GetButtonDown(CAVE2.Button.Button5)))
//            return;

//        if (cam == null) cam = Camera.main;
//        if (cam == null) return;
//        if (isAnimating) return;

//        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

//        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
//        {
//            if (hit.transform == transform || hit.transform.IsChildOf(transform))
//            {
//                isOn = !isOn;

//                if (audioSource != null && clickSound != null)
//                    audioSource.PlayOneShot(clickSound);

//                if (tvRoutine != null)
//                    StopCoroutine(tvRoutine);

//                if (isOn)
//                    tvRoutine = StartCoroutine(TurnOnSequence());
//                else
//                    tvRoutine = StartCoroutine(TurnOffSequence());
//            }
//        }
//    }

//    IEnumerator TurnOnSequence()
//    {
//        isAnimating = true;

//        if (tvCanvas != null)
//            tvCanvas.SetActive(true);

//        if (kwhGroup != null)
//            kwhGroup.SetActive(false);

//        if (bigKwh != null)
//        {
//            ResetBigKwh();
//            bigKwh.SetActive(true);
//        }

//        yield return new WaitForSeconds(bigKwhDelay);

//        Vector3 bigKwhEndPos = bigKwhStartPos + Vector3.up * floatUpAmount;

//        // Big kWh rises first
//        if (bigKwh != null)
//        {
//            float elapsed = 0f;
//            Vector3 startPos = bigKwhStartPos;
//            Vector3 endPos = bigKwhEndPos;

//            while (elapsed < dissipateDuration)
//            {
//                elapsed += Time.deltaTime;
//                float t = Mathf.Clamp01(elapsed / dissipateDuration);

//                float driftX = Mathf.Sin(t * 8f) * 0.03f;
//                float driftZ = Mathf.Cos(t * 6f) * 0.015f;

//                Vector3 basePos = Vector3.Lerp(startPos, endPos, t);
//                bigKwh.transform.localPosition = basePos + new Vector3(driftX, 0f, driftZ);

//                yield return null;
//            }

//            bigKwh.SetActive(false);
//        }

//        // Small group drops down from where the big one disappeared
//        if (kwhGroup != null)
//        {
//            kwhGroup.transform.localPosition = bigKwhEndPos;
//            kwhGroup.SetActive(true);

//            float elapsed = 0f;
//            Vector3 dropStart = bigKwhEndPos;
//            Vector3 dropEnd = kwhGroupStartPos;

//            while (elapsed < groupDropDuration)
//            {
//                elapsed += Time.deltaTime;
//                float t = Mathf.Clamp01(elapsed / groupDropDuration);

//                float smoothT = Mathf.SmoothStep(0f, 1f, t);
//                kwhGroup.transform.localPosition = Vector3.Lerp(dropStart, dropEnd, smoothT);

//                yield return null;
//            }

//            kwhGroup.transform.localPosition = kwhGroupStartPos;
//        }

//        isAnimating = false;
//    }


//    IEnumerator TurnOffSequence()
//    {

//        isAnimating = true;

//        if (tvCanvas != null)
//            tvCanvas.SetActive(false);

//        // keep kwhGroup visible after TV turns off
//        if (bigKwh != null)
//        {
//            ResetBigKwh();
//            bigKwh.SetActive(false);
//        }

//        yield return null;
//        isAnimating = false;

//        //isAnimating = true;

//        //if (tvCanvas != null)
//        //    tvCanvas.SetActive(false);

//        //if (kwhGroup != null)
//        //{
//        //    kwhGroup.transform.localPosition = kwhGroupStartPos;
//        //    kwhGroup.SetActive(false);
//        //}

//        //if (bigKwh != null)
//        //{
//        //    ResetBigKwh();
//        //    bigKwh.SetActive(false);
//        //}

//        //yield return null;
//        //isAnimating = false;
//    }

//    void ResetBigKwh()
//    {
//        bigKwh.transform.localPosition = bigKwhStartPos;

//        if (bigKwhMaterialInstance != null)
//        {
//            Color c = bigKwhStartColor;
//            c.a = 1f;
//            bigKwhMaterialInstance.color = c;
//        }
//    }
//}

