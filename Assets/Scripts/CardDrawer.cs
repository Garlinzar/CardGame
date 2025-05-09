using UnityEngine;
using UnityEngine.UI;

public class CardDrawer : MonoBehaviour
{
    public GameObject cardPrefab;          // Dein Karten-Image-Prefab
    public Transform cardHolder;           // Der Container im UI für die Karten

    public void DrawCards()
    {
        // Vorherige Karten entfernen (optional)
        foreach (Transform child in cardHolder)
        {
            Destroy(child.gameObject);
        }

        // 4 Karten erzeugen
        for (int i = 0; i < 4; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardHolder);
        }
    }
}
