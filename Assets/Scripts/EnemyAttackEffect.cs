using UnityEngine;
using DG.Tweening;

public class EnemyAttackEffect : MonoBehaviour
{
    public float punchDistance = 0.2f;
    public float punchDuration = 0.3f;

    public void PlayAttack()
    {
        transform.DOPunchPosition(Vector3.left * punchDistance, punchDuration, 10, 1f);
    }
}
