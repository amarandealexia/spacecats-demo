using TMPro;
using UnityEngine;

public class HPDisplay : MonoBehaviour
{
    public Character character;
    public TMP_Text hpText;

    void Update()
    {
        // Safety guard: only track if a target monster exists on screen!
        if (character != null && hpText != null)
        {
            hpText.text = character.currentHP + "/" + character.maxHP;
        }
    }
}