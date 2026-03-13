using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualEnergyPath : MonoBehaviour
{
        [Header("Path Points")]
    public Transform[] points;

    [Header("Line")]
    public LineRenderer line;
    public float yOffset = 0.03f;

    [Header("Pulse")]
    public Color colorA = new Color(0.1f, 0.7f, 1f, 1f);
    public Color colorB = new Color(0.6f, 1f, 1f, 1f);
    public float pulseSpeed = 2f;

    [Header("Width")]
    public float minWidth = 0.06f;
    public float maxWidth = 0.12f;
    public float widthPulseSpeed = 2f;

    [Header("Flow")]
    public bool animateTexture = true;
    public float textureScrollSpeed = 1f;

    void Awake()
    {
        if (!line) line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!line || points == null || points.Length < 2) return;
        Debug.Log("ManualEnergyPath running");
        DrawLine();
        AnimateLine();
    }

    void DrawLine()
    {
        line.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null) continue;

            Vector3 p = points[i].position;
            p.y += yOffset;
            line.SetPosition(i, p);
        }
    }

    void AnimateLine()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        Color c = Color.Lerp(colorA, colorB, t);

        line.startColor = c;
        line.endColor = c;

        float w = Mathf.Lerp(minWidth, maxWidth, (Mathf.Sin(Time.time * widthPulseSpeed) + 1f) * 0.5f);
        line.startWidth = w;
        line.endWidth = w;

        if (animateTexture && line.material != null)
        {
            Vector2 offset = line.material.mainTextureOffset;
            offset.x -= Time.deltaTime * textureScrollSpeed;
            line.material.mainTextureOffset = offset;
        }
    }
}
