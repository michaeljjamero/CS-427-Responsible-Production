using UnityEngine;
using System.Collections;
using TMPro;


public class DisinegrateText : MonoBehaviour
{
    public float floatDistance = 2f;
    public float duration = 2f;

    private Vector3 startPos;
    private Renderer rend;
    private Color startColor;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(FloatAndFade());
    }

    IEnumerator FloatAndFade()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            // move upward
            transform.position = startPos + Vector3.up * (floatDistance * progress);

            // fade out
            Color c = startColor;
            c.a = 1 - progress;
            rend.material.color = c;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
