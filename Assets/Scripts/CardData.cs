using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int manaCost;
    public int damage;
}
