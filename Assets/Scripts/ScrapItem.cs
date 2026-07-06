using UnityEngine;

[CreateAssetMenu(fileName = "NewScrap", menuName = "Scavenge/Scrap Item")]
public class ScrapItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int valueInCredits;
    public float weight;
    [Range(0, 100)] public float spawnChance; 
}