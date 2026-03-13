using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KwhMovement : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    [SerializeField] private float amplitude = 0.05f;
    [SerializeField] private float frequency = 1.5f;

    private Vector3 startPos;
    private bool isFloating = false;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (!isFloating) return;

        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0f, yOffset, 0f);
    }

    public void StartFloating()
    {
        startPos = transform.localPosition;
        isFloating = true;
    }

    public void StopFloating()
    {
        isFloating = false;
    }
}
