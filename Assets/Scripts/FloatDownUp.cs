using UnityEngine;

public class FloatDownUp : MonoBehaviour
{
    [SerializeField] private float height = 0.5f;   // how high it moves
    [SerializeField] private float speed = 2f;      // how fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * height;

        transform.position = new Vector3(
            startPos.x,
            startPos.y + yOffset,
            startPos.z
        );
    }
}