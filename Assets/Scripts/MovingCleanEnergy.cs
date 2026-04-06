using UnityEngine;

public class MovingCleanEnergy : MonoBehaviour
{
    public Transform[] points;   // drag your points here in Inspector
    public float speed = 2f;

    private int currentIndex = 0;

    void Update()
    {
        if (points.Length == 0) return;

        Transform target = points[currentIndex];

        // Move toward target
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Check if reached
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;

            if (currentIndex >= points.Length)
            {
                currentIndex = 0; // loop
            }
        }
    }
}