using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Configuration")]
    public string introSceneName = "IntroScene"; 
    public void StartGame()
    {
        
        SceneManager.LoadScene(introSceneName);
    }

    public void OpenOptions()
    {
        
        Debug.Log("Options Menu Opened");
    }

    public void ExitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit(); 
    }
}