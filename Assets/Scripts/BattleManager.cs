using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public GameObject[] enemyPrefabs; 
}

public class BattleManager : MonoBehaviour
{
    public Character player;
    
    [Header("Wave Configuration")]
    public List<EnemyWave> waves; 
    public Transform[] enemySpawnPositions; 

    private int currentWaveIndex = 0;
    private List<Character> activeEnemies = new List<Character>();

    [HideInInspector]
    public bool playerTurn = true;
    public int enemyDamage = 5; 

    [Header("Energy System")]
    public int maxEnergy = 3;
    public int currentEnergy;
    public UnityEngine.UI.Image[] energyRectangles;

    [Header("Win/Loss UI")]
    public TextMeshProUGUI victoryText; // FIXED: Changed from Text to TextMeshProUGUI!

    [Header("End Game Victory UI")]
    public CanvasGroup victoryCanvasGroup;
    public string mapSceneName = "MapScene";

    [Header("Turn Management")]
    public int currentTurn = 0;
    public int maxTurns = 7;
    public Text turnText;

    private Card selectedPreparedCard = null;
    private bool waitingForTarget = false;

    void Start()
    {
        currentTurn = 0;
        currentWaveIndex = 0;

        if (victoryCanvasGroup != null)
        {
            victoryCanvasGroup.alpha = 0f;
            victoryCanvasGroup.interactable = false;
            victoryCanvasGroup.blocksRaycasts = false;
        }

        SpawnWave(currentWaveIndex);
        StartPlayerTurn();
    }

    void SpawnWave(int waveIndex)
    {
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null) Destroy(enemy.gameObject);
        }
        activeEnemies.Clear();

        if (waveIndex >= waves.Count)
        {
            ShowEndGame("CAMPAIGN VICTORY!", Color.green);
            StartCoroutine(FadeInVictoryScreen());
            return;
        }

        EnemyWave wave = waves[waveIndex];
        Debug.Log("Starting Wave: " + wave.waveName);

        for (int i = 0; i < wave.enemyPrefabs.Length; i++)
        {
            if (i >= enemySpawnPositions.Length) break; 

            GameObject spawnedEnemy = Instantiate(wave.enemyPrefabs[i], enemySpawnPositions[i]);
            
            RectTransform rect = spawnedEnemy.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero; 
                rect.localPosition = Vector3.zero;    
            }

            Character enemyComponent = spawnedEnemy.GetComponent<Character>();
            if (enemyComponent != null)
            {
                activeEnemies.Add(enemyComponent);
            }
        }
    }

    public void StartPlayerTurn()
    {
        currentTurn++;
        
        if (currentTurn > maxTurns)
        {
            GameOverByTurns();
            return;
        }

        playerTurn = true;
        currentEnergy = maxEnergy; 
        ClearTargetingState();
        
        UpdateEnergyUI();
        UpdateTurnUI("Player Turn");
    }

    public void PlayCard(Card card)
    {
        if (!playerTurn) return;
        if (activeEnemies.Count == 0) return;
        if (currentEnergy < card.data.cost) return;

        if (card.data.damage <= 0)
        {
            currentEnergy -= card.data.cost;
            UpdateEnergyUI();
            card.Play(player, null);
            CheckEnergyEndTurn();
        }
        else 
        {
            selectedPreparedCard = card;
            waitingForTarget = true;
            Debug.Log("Card armed! Click directly on an enemy to attack!");
        }
    }

    public void SelectEnemyTargetSimple()
    {
        Character clickedEnemy = GetComponent<Character>();
        
        if (clickedEnemy == null)
        {
            Debug.LogError("Click registered, but no Character script found on this object!");
            return;
        }

        SelectEnemyTarget(clickedEnemy);
    }

    public void SelectEnemyTarget(Character clickedEnemy)
    {
        if (!waitingForTarget || selectedPreparedCard == null) return;
        if (!activeEnemies.Contains(clickedEnemy)) return;

        currentEnergy -= selectedPreparedCard.data.cost;
        UpdateEnergyUI();

        selectedPreparedCard.Play(player, clickedEnemy);
        
        if (clickedEnemy.currentHP <= 0)
        {
            CharacterDied(clickedEnemy);
        }
        else
        {
            ClearTargetingState();
            CheckEnergyEndTurn();
        }
    }

    void CheckEnergyEndTurn()
    {
        if (currentEnergy <= 0 && activeEnemies.Count > 0)
        {
            EndTurn();
        }
    }

    void ClearTargetingState()
    {
        selectedPreparedCard = null;
        waitingForTarget = false;
    }

    public void EndTurn()
    {
        if (!playerTurn) return;

        playerTurn = false;
        UpdateTurnUI("Enemy Turn");
        Invoke("EnemyTurn", 1.0f); 
    }

    void EnemyTurn()
    {
        Debug.Log("Enemies are attacking!");

        foreach (Character enemy in activeEnemies)
        {
            if (enemy != null && enemy.currentHP > 0)
            {
                int finalDamage = enemy.CalculateDamage(enemyDamage);
                player.TakeDamage(finalDamage);
            }
        }

        player.block = 0;
        StartPlayerTurn();
    }

    public void CharacterDied(Character deadCharacter)
    {
        if (deadCharacter == player)
        {
            playerTurn = false;
            CancelInvoke("EnemyTurn");
            ShowEndGame("DEFEAT...", Color.red);
            return;
        }

        if (activeEnemies.Contains(deadCharacter))
        {
            activeEnemies.Remove(deadCharacter);
            Destroy(deadCharacter.gameObject, 0.1f); 
        }

        if (activeEnemies.Count == 0)
        {
            currentWaveIndex++;
            if (currentWaveIndex < waves.Count)
            {
                currentTurn = 0; 
                SpawnWave(currentWaveIndex);
                StartPlayerTurn();
            }
            else
            {
                playerTurn = false;
                CancelInvoke("EnemyTurn");
                ShowEndGame("VICTORY!", Color.green);
                StartCoroutine(FadeInVictoryScreen());
            }
        }
        else
        {
            ClearTargetingState();
            CheckEnergyEndTurn();
        }
    }

    private System.Collections.IEnumerator FadeInVictoryScreen()
    {
        if (victoryCanvasGroup != null)
        {
            yield return new WaitForSeconds(1.0f); 

            float duration = 1.5f; 
            float currentTime = 0f;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                victoryCanvasGroup.alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
                yield return null;
            }
            
            victoryCanvasGroup.alpha = 1f;
            victoryCanvasGroup.interactable = true;
            victoryCanvasGroup.blocksRaycasts = true;
        }
    }

    void GameOverByTurns()
    {
        playerTurn = false;
        ShowEndGame("OUT OF TURNS\nDEFEAT...", Color.red);
    }

    void ShowEndGame(string message, Color textColor)
    {
        if (victoryText != null)
        {
            victoryText.text = message;
            victoryText.color = textColor;
        }
        if (turnText != null)
        {
            turnText.text = "Game Over";
        }
    }

    void UpdateEnergyUI()
    {
        for (int i = 0; i < energyRectangles.Length; i++)
        {
            if (energyRectangles[i] != null)
            {
                if (i < currentEnergy)
                {
                    energyRectangles[i].enabled = true;
                    Color c = energyRectangles[i].color;
                    c.a = 1.0f; 
                    energyRectangles[i].color = c;
                }
                else 
                {
                    energyRectangles[i].enabled = true;
                    Color c = energyRectangles[i].color;
                    c.a = 0.15f; 
                    energyRectangles[i].color = c;
                }
            }
        }
    }

    void UpdateTurnUI(string activeSide)
    {
        if (turnText != null)
        {
            turnText.text = "Turn: " + currentTurn + "/" + maxTurns + " (" + activeSide + ")";
        }
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene(mapSceneName);
    }
}