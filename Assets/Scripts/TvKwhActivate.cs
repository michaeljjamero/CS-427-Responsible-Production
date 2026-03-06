using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvKwhActivate : MonoBehaviour
{


    ////// Update is called once per frame
    ////void Update()
    ////{

    ////}
    //[Header("Assign the parent that holds all kWh objects (or a single object)")]
    //[SerializeField] private GameObject kwhGroup;
    //[SerializeField] private GameObject tvCanvas; // Drag Canvas here



    //[Header("Optional: click settings")]
    //[SerializeField] private Camera cam; // leave empty to auto-use Camera.main
    //[SerializeField] private bool toggle = true; // true = click toggles on/off, false = only turns on

    ////private void Awake()
    ////{
    ////    if (cam == null) cam = Camera.main;

    ////    // Hide on start
    ////    if (kwhGroup != null)
    ////        kwhGroup.SetActive(false);
    ////    else
    ////        Debug.LogError("TvKwhActivate: kwhGroup is not assigned.");
    ////}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    if (cam == null) cam = Camera.main;

    //    if (kwhGroup != null)
    //        kwhGroup.SetActive(false);
    //    else
    //        Debug.LogError("TvKwhActivate: kwhGroup is not assigned.");

    //    if (tvCanvas != null)
    //        tvCanvas.SetActive(false); // TV starts blank
    //}

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0)) // left click
    //    {
    //        if (cam == null) return;

    //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out RaycastHit hit))
    //        {
    //            // Did we click THIS TV (or one of its children)?
    //            if (hit.transform == transform || hit.transform.IsChildOf(transform))
    //            {
    //                ActivateKwh();
    //            }
    //        }
    //    }
    //}

    //private void ActivateKwh()
    //{
    //    if (kwhGroup == null) return;

    //    if (toggle)
    //        kwhGroup.SetActive(!kwhGroup.activeSelf);
    //    else
    //        kwhGroup.SetActive(true);

    //    if (tvCanvas != null)
    //    {
    //        if (toggle)
    //            tvCanvas.SetActive(!tvCanvas.activeSelf);
    //        else
    //            tvCanvas.SetActive(true);
    //    }
    //}

    [Header("Assign the parent that holds all kWh objects")]
    [SerializeField] private GameObject kwhGroup;

    [SerializeField] private GameObject tvCanvas;

    [Header("Optional: click settings")]
    [SerializeField] private Camera cam;

    private bool tvIsOn = false;
    private KwhMovement bobScript;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        if (kwhGroup != null)
        {
            kwhGroup.SetActive(false); 
            bobScript = kwhGroup.GetComponent<KwhMovement>();

            if (bobScript != null)
                bobScript.StopFloating();
        }
        else
        {
            Debug.LogError("TvKwhActivate: kwhGroup is not assigned.");
        }

        if (tvCanvas != null)
            tvCanvas.SetActive(false); // TV starts blank
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cam == null) return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    ToggleTV();
                }
            }
        }
    }

    //private void ToggleTV()
    //{
    //    tvIsOn = !tvIsOn;

    //    if (tvCanvas != null)
    //        tvCanvas.SetActive(tvIsOn);

    //    if (bobScript != null)
    //    {
    //        if (tvIsOn)
    //            bobScript.StartFloating();
    //        else
    //            bobScript.StopFloating();
    //    }
    //}
    private void ToggleTV()
    {
        tvIsOn = !tvIsOn;

        // Show / hide the TV picture
        if (tvCanvas != null)
            tvCanvas.SetActive(tvIsOn);

        if (bobScript != null)
        {
            if (tvIsOn)
            {
                kwhGroup.SetActive(true);   // SHOW kWh when TV turns on
                bobScript.StartFloating();
            }
            else
            {
                bobScript.StopFloating();   // stop movement but keep visible
            }
        }
    }
}
