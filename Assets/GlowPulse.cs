using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private Color glowColor = Color.cyan;

    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 3f;
    [SerializeField] private float speed = 2f;

    private Material mat;

    void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();

        mat = rend.material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // sharper pulse using sine
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        // exaggerate contrast (makes flashing obvious)
        t = Mathf.Pow(t, 3f);

        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        Color finalColor = glowColor * intensity;
        mat.SetColor("_EmissionColor", finalColor);
    }
}