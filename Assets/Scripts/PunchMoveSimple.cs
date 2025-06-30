using UnityEngine;
using DG.Tweening; 

public class PunchMoveSimple : MonoBehaviour
{
    public float moveDistance = 1f;
    public float duration = 0.2f;

    public void DoPunch()
    {
        
        transform.DOPunchPosition(Vector3.right * moveDistance, duration, 10, 1f);
    }
}
