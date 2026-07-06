using UnityEngine;

public enum CardType
{
    Attack,
    Skill,
    Power
}

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public string cardName;

    [TextArea]
    public string description;

    public Sprite artwork;

    public int cost;

    public CardType cardType;

    // stats
    public int damage;
    public int block;
}