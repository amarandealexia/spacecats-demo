using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    [Header("Combat Stats")]
    public string characterName = "Enemy";
    public int maxHP = 50; // Setting this to 50 defaults your rats to match your layout design!
    public int currentHP;
    public int strength = 0;
    public int block = 0;

    [Header("Prefab Text Link")]
    public TMP_Text hpText; 

    private BattleManager battleManager;

   void Awake()
    {
        currentHP = maxHP;
        battleManager = FindFirstObjectByType<BattleManager>(); 
        UpdateHPDisplay();
    }

    void Update()
    {
        UpdateHPDisplay();
    }

    public void TakeDamage(int damageAmount)
    {
        int remainingDamage = damageAmount - block;
        if (remainingDamage > 0)
        {
            block = 0;
            currentHP -= remainingDamage;
        }
        else
        {
            block -= damageAmount;
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            UpdateHPDisplay();
            if (battleManager != null)
            {
                battleManager.CharacterDied(this);
            }
        }
    }

    public void GainBlock(int amount)
    {
        block += amount;
    }

    public int CalculateDamage(int baseDamage)
    {
        return baseDamage + strength;
    }

    public void UpdateHPDisplay()
    {
        if (hpText != null)
        {
            hpText.text = currentHP + " / " + maxHP;
        }
    }

    // This handles the click selection for targeting!
    public void OnPointerDown()
    {
        if (battleManager != null)
        {
            battleManager.SelectEnemyTarget(this);
        }
    }
}