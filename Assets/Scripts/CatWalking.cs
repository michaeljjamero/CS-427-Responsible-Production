using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWalking : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float stopDistance = 0.05f;

    [Header("Rotation Settings")]
    [SerializeField] private float turnSpeed = 180f; // degrees per second

    private Transform target;
    private bool isTurning = false;

    void Start()
    {
        target = pointB;
        FaceTargetInstant();
    }

    void Update()
    {
        if (target == null || isTurning) return;

        // Move forward
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Reached target → turn
        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            target = (target == pointB) ? pointA : pointB;
            StartCoroutine(TurnAround());
        }
    }

    IEnumerator TurnAround()
    {
        isTurning = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 180, 0);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * (turnSpeed / 180f);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.rotation = endRot;

        isTurning = false;
    }

    void FaceTargetInstant()
    {
        Vector3 direction = target.position - transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
