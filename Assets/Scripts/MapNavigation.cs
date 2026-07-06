using UnityEngine;
using UnityEngine.SceneManagement;

public class MapNavigation : MonoBehaviour
{
    public void LoadScavengeScene()
    {
        SceneManager.LoadScene("Scavenge");
    }

    public void LoadShopScene()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}