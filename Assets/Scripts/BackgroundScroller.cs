using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollDistance = 5f;
    public float scrollDuration = 1.0f;

    public void ScrollOneStep()
    {
        StartCoroutine(ScrollSmoothly());
    }

    private IEnumerator ScrollSmoothly()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.left * scrollDistance;

        float elapsed = 0f;
        while (elapsed < scrollDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / scrollDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }
}
