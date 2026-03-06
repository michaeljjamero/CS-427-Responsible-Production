using UnityEngine;

public class textMovementCH4 : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.05f;   // how high it moves
    [SerializeField] private float speed = 2f;          // how fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}