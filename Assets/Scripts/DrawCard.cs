using UnityEngine;
using UnityEngine.UI;

public class DrawCard : MonoBehaviour
{
    public GameObject cardPrefab;          
    public Transform cardHolder;           

    public void DrawCards()
    {
       
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
