using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData data;

    public void Play(Character player, Character enemy)
    {
        
        if (data.damage > 0)
        {
            int finalDamage =
                player.CalculateDamage(data.damage);

            enemy.TakeDamage(finalDamage);
        }

    
        if (data.block > 0)
        {
            player.GainBlock(data.block);
        }

        Debug.Log("Played " + data.cardName);
    }
}