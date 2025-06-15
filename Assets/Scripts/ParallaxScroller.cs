using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public float scrollSpeed = 0.05f;
    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(
            startPos.x + Mathf.Repeat(Time.time * scrollSpeed, 100),
            transform.position.y,
            transform.position.z
        );
    }
}
