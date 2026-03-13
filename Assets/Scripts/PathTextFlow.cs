using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathTextFlow : MonoBehaviour
{
    [Header("Path")]
    public Transform[] points;

    [Header("Text")]
    public TextMeshPro textPrefab;
    public int textCount = 3;
    public string displayText = "CLEAN ENERGY";

    [Header("Motion")]
    public float speed = 2f;
    public float spacing = 2.5f;
    public float textYOffset = 0f;

    [Header("Look")]
    public bool faceCamera = true;
    public Color colorA = new Color(0.0f, 0.6f, 1f, 1f);
    public Color colorB = new Color(0.0f, 1f, 1f, 1f);
    public float pulseSpeed = 2f;

    List<TextMeshPro> spawnedTexts = new List<TextMeshPro>();
    float totalLength;

    void Start()
    {
        if (points == null || points.Length < 2 || textPrefab == null) return;

        totalLength = GetTotalLength();

        for (int i = 0; i < textCount; i++)
        {
            TextMeshPro t = Instantiate(textPrefab, transform);
            t.text = displayText;
            spawnedTexts.Add(t);
        }
    }

    void Update()
    {
        if (points == null || points.Length < 2 || spawnedTexts.Count == 0) return;

        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        Color c = Color.Lerp(colorA, colorB, pulse);

        for (int i = 0; i < spawnedTexts.Count; i++)
        {
            float dist = (Time.time * speed + i * spacing) % totalLength;

            Vector3 pos = GetPointAtDistance(dist);
            Vector3 dir = GetDirectionAtDistance(dist);

            pos.y += textYOffset;

            spawnedTexts[i].transform.position = pos;

            if (dir != Vector3.zero)
            {
                spawnedTexts[i].transform.rotation = Quaternion.LookRotation(dir);
            }

            if (faceCamera && Camera.main != null)
            {
                Vector3 lookDir = spawnedTexts[i].transform.position - Camera.main.transform.position;
                lookDir.y = 0f;

                if (lookDir != Vector3.zero)
                    spawnedTexts[i].transform.rotation = Quaternion.LookRotation(lookDir);
            }

            spawnedTexts[i].color = c;
        }
    }

    float GetTotalLength()
    {
        float len = 0f;
        for (int i = 0; i < points.Length - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
                len += Vector3.Distance(points[i].position, points[i + 1].position);
        }
        return len;
    }

    Vector3 GetPointAtDistance(float dist)
    {
        float remaining = dist;

        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 a = points[i].position;
            Vector3 b = points[i + 1].position;
            float segLen = Vector3.Distance(a, b);

            if (remaining <= segLen)
            {
                float t = segLen == 0f ? 0f : remaining / segLen;
                return Vector3.Lerp(a, b, t);
            }

            remaining -= segLen;
        }

        return points[points.Length - 1].position;
    }

    Vector3 GetDirectionAtDistance(float dist)
    {
        float remaining = dist;

        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 a = points[i].position;
            Vector3 b = points[i + 1].position;
            float segLen = Vector3.Distance(a, b);

            if (remaining <= segLen)
            {
                return (b - a).normalized;
            }

            remaining -= segLen;
        }

        return Vector3.forward;
    }
}