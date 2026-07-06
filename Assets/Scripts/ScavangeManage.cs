using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class ScavengeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI logText;       
    public TextMeshProUGUI fishTotalText; 
    public TextMeshProUGUI timerText;       
    public GameObject returnToMapButton;   

    [Header("Timer Configuration")]
    public float timeRemaining = 30f;       
    private bool isTimerRunning = false;

    [Header("Item Pool")]
    public List<ScrapItem> possibleScrap;

    [Header("Inventory Data")]
    public int totalScrapCollected = 0; 
    private List<ScrapItem> currentLootBag = new List<ScrapItem>();
    
    private int totalFishCoins = 0; 

    void Start()
    {
        LoadSavedPlayerData();
        UpdateFishUI();

       
        isTimerRunning = true;
        if (returnToMapButton != null)
        {
            returnToMapButton.SetActive(false);
        }

        if (logText != null)
        {
            logText.text = "SYSTEM READY.\nUSE WASD TO SEARCH FOR SCRAP...";
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; 
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimerUI();
                EndScavengeRun();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
           
            timerText.text = "Time Left: " + Mathf.CeilToInt(timeRemaining) + "s";
        }
    }

    private void EndScavengeRun()
    {
        if (logText != null)
        {
            logText.text = "TIME EXPIRED!\nSCAVENGE OVER. RETURN TO MAP NODE.";
        }

        if (returnToMapButton != null)
        {
            returnToMapButton.SetActive(true); 
        }
    }

   
    public void LoadMapScene()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void LogFoundItem(ScrapItem foundItem)
    {
       
        if (!isTimerRunning) return;

        currentLootBag.Add(foundItem);
        totalScrapCollected++; 
        
        SavePlayerData();

        if (logText != null)
        {
            logText.text = $"[SYSTEM LOG]\n> FOUND: {foundItem.itemName}!\n> TOTAL SCRAP HELD: {totalScrapCollected}";
        }
    }

    public void SellScrapForFish()
    {
        if (totalScrapCollected <= 0 || currentLootBag.Count == 0)
        {
            if (logText != null)
            {
                logText.text = "ERROR: TRANSACTION FAILED. NO SCRAP IN INVENTORY.";
            }
            return; 
        }

        int fishCoinsEarned = 0;

        foreach (ScrapItem item in currentLootBag)
        {
            fishCoinsEarned += item.valueInCredits;
        }

        totalFishCoins += fishCoinsEarned;

        currentLootBag.Clear();
        totalScrapCollected = 0;

        SavePlayerData();
        UpdateFishUI();

        if (logText != null)
        {
            logText.text = $"scrap sold, {totalFishCoins} fish collected";
        }
    }

    private void UpdateFishUI()
    {
        if (fishTotalText != null)
        {
            fishTotalText.text = "Fish: " + totalFishCoins;
        }
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("SavedScrapCount", totalScrapCollected);
        PlayerPrefs.SetInt("SavedFishCoins", totalFishCoins);
        PlayerPrefs.Save(); 
    }

    public void LoadSavedPlayerData()
    {
        totalScrapCollected = PlayerPrefs.GetInt("SavedScrapCount", 0);
        totalFishCoins = PlayerPrefs.GetInt("SavedFishCoins", 0);

        currentLootBag.Clear();
        for (int i = 0; i < totalScrapCollected; i++)
        {
            if (possibleScrap.Count > 0)
            {
                currentLootBag.Add(possibleScrap[0]); 
            }
        }
    }

    [ContextMenu("Reset Save Data")]
    public void ResetSaveData()
    {
        PlayerPrefs.DeleteKey("SavedScrapCount");
        PlayerPrefs.DeleteKey("SavedFishCoins");
        totalScrapCollected = 0;
        totalFishCoins = 0;
        currentLootBag.Clear();
        UpdateFishUI();
        Debug.Log("Scavenge save data reset to zero.");
    }
}