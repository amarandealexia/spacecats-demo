using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransitionController : MonoBehaviour
{
    public void GoToMap()
    {
        SceneManager.LoadScene("MapScene");
    }
}