using UnityEngine;

public class WaterFalling : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float resetHeight = 3f;
    [SerializeField] private float bottomLimit = -2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < bottomLimit)
        {
            transform.position = new Vector3(
                startPos.x,
                resetHeight,
                startPos.z
            );
        }
    }
}