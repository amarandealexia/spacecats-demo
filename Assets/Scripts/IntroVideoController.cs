using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    [Header("Next Destination")]
    public string gameplaySceneName = "GameScene";

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("No VideoPlayer component found on this object! Transitioning immediately.");
            LoadNextScene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Intro cinematic finished. Loading game scene now...");
        LoadNextScene();
    }

   
    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            Debug.Log("Intro skipped by player.");
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
       
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }
}