using UnityEngine;

public class TriggerOffice : MonoBehaviour
{
    [Header("Objects to activate")]
    [SerializeField] private MonitorActivate monitorScript;
    [SerializeField] private LampActivate lampScript;

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;

        if (monitorScript != null)
            monitorScript.TriggerFromZone();

        if (lampScript != null)
            lampScript.TriggerFromZone();

     
    }
}