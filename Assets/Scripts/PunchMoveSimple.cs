using UnityEngine;
using DG.Tweening; // Wichtig!

public class PunchMoveSimple : MonoBehaviour
{
    public float moveDistance = 1f;
    public float duration = 0.2f;

    public void DoPunch()
    {
        // Bewege das GameObject nach rechts und dann zurück (Punch-Effekt)
        transform.DOPunchPosition(Vector3.right * moveDistance, duration, 10, 1f);
    }
}
