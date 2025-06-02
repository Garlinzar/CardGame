using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [Header("Lebensdauer des Popups in Sekunden")]
    public float destroyAfterSeconds = 1.0f;

    void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
}
