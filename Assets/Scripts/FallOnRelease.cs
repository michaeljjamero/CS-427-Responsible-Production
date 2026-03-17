using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOnRelease : MonoBehaviour
{
    private Rigidbody rb;

    private bool wasMoving = false;
    private bool hasFallen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on " + gameObject.name);
            return;
        }

        // keep it floating until released
        rb.useGravity = false;
    }

    void Update()
    {
        if (hasFallen) return;

        // detect if the object is being moved by the grab system
        bool isMoving = rb.velocity.magnitude > 0.1f;

        if (isMoving)
            wasMoving = true;

        // if it was moving (grabbed) and now mouse released
        if (wasMoving && Input.GetMouseButtonUp(0))
        {
            FallNow();
        }
    }

    void FallNow()
    {
        rb.useGravity = true;
        rb.drag = 0;
        rb.angularDrag = 0.05f;

        hasFallen = true;
    }
}
