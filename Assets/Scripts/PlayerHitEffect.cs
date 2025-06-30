
using UnityEngine;
using DG.Tweening;

public class PlayerHitEffect : MonoBehaviour
{
    public float moveDistance = 0.5f;
    public float duration = 0.2f;

    public void PlayHit()
    {
        // Bewege das GameObject nach rechts und dann zurück (Punch-Effekt)
        transform.DOPunchPosition(Vector3.left * moveDistance, duration, 10, 1f);
    }
}
