using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int manaCost;
    public int damage;
    public int healAmount; // Heilt den Spieler
    public int bonusManaNextTurn; // Gibt Bonusmana für nächste Runde
}
