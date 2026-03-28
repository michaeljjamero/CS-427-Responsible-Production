using UnityEngine;

public class FollowHead : MonoBehaviour
{
    [SerializeField] private Transform head;

    void Update()
    {
        if (head == null) return;

        // follow position (only XZ so it stays grounded)
        transform.position = new Vector3(
            head.position.x,
            transform.position.y,
            head.position.z
        );
    }
}
