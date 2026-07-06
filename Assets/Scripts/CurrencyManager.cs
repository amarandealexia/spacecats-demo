using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CurrencyManager : MonoBehaviour
{
    [Header("Currency Totals")]
    public int currentScrap = 10; 
    public int currentFish = 0;

    [Header("UI Displays")]
    public Text fishTotalText;    
    public Text terminalText;     

    [Header("Shop Settings")]
    public int scrapSellValue = 100; 

    void Start()
    {
        LoadCurrencyData();
        UpdateUI();
        
        if (terminalText != null)
        {
            terminalText.text = "SYSTEM ONLINE. AWAITING SCRAP TRANSACTIONS...";
        }
    }

    public void SellScrap()
    {
        if (currentScrap > 0)
        {
            currentScrap--;       
            currentFish += scrapSellValue; 

            string message = $"scrap sold, {currentFish} fish collected";
            
            Debug.Log(message);

            if (terminalText != null)
            {
                terminalText.text = message.ToUpper(); 
            }

            SaveCurrencyData();
            UpdateUI();
        }
        else
        {
            if (terminalText != null)
            {
                terminalText.text = "ERROR: TRANSACTION FAILED. NO SCRAP IN INVENTORY.";
            }
        }
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void UpdateUI()
    {
        if (fishTotalText != null)
        {
            fishTotalText.text = "Fish: " + currentFish;
        }
    }

    public void SaveCurrencyData()
    {
        PlayerPrefs.SetInt("SavedScrap", currentScrap);
        PlayerPrefs.SetInt("SavedFish", currentFish);
        PlayerPrefs.Save();
    }

    public void LoadCurrencyData()
    {
        currentScrap = PlayerPrefs.GetInt("SavedScrap", 10); 
        currentFish = PlayerPrefs.GetInt("SavedFish", 0);
    }
}