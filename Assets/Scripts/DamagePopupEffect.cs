using UnityEngine;
using TMPro;

public class DamagePopupEffect : MonoBehaviour
{
    [Header("Effekteinstellungen")]
    public float moveUpSpeed = 0.5f;            // Geschwindigkeit des Hochbewegens
    public float scaleFactor = 1.2f;            // Maximaler Skalierungsfaktor (Pulsieren)
    public float scaleSpeed = 5f;               // Geschwindigkeit der Skalierung
    public float fadeOutDuration = 0.5f;        // Zeit, bis das Popup ausgeblendet wird
    public float lifetime = 1.0f;               // Wie lange das Popup insgesamt sichtbar ist

    private TextMeshProUGUI text;
    private float timeElapsed = 0f;
    private Color originalColor;

    private Vector3 originalScale;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalScale = transform.localScale;

        if (text != null)
        {
            originalColor = text.color;
        }
        else
        {
            Debug.LogWarning("⚠️ Kein TextMeshProUGUI gefunden.");
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // 1. Nach oben bewegen
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // 2. Pulsieren (Skalieren)
        float pulse = 1 + Mathf.Sin(timeElapsed * scaleSpeed) * 0.1f; // leichtes Pulsieren
        transform.localScale = originalScale * pulse;

        // 3. Ausblenden am Ende
        if (timeElapsed > lifetime - fadeOutDuration && text != null)
        {
            float fade = Mathf.Clamp01((lifetime - timeElapsed) / fadeOutDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, fade);
        }

        // 4. Zerstören nach Lebenszeit
        if (timeElapsed > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
