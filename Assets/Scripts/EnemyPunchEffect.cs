using UnityEngine;
using DG.Tweening;

public class EnemyPunchEffect : MonoBehaviour
{
    public float punchDistance = 0.2f;
    public float punchDuration = 0.2f;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void PlayPunch()
    {
        // DOTween zurücksetzen und Punch Animation abspielen
        transform.DOKill(); // Storniert ggf. laufende Tweens
        transform.localPosition = originalPosition; // Reset

        transform.DOPunchPosition(Vector3.right * punchDistance, punchDuration, 10, 1f)

                 .SetEase(Ease.OutQuad);
    }
}
