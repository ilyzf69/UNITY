using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float timeRemaining = 60f;
    public Text timeText; 
    public GameObject winPanel;
    public GameObject losePanel; 
    private int totalCollectibles;
    private int collectedCount = 0; 
    public bool isGameActive = true;

    void Start()
    {
        
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        totalCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    void Update()
    {
        if (isGameActive)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                LoseGame();
            }
        }
    }

    void UpdateTimerText()
    {
        if (timeText != null)
        {
            timeText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
        }
        else
        {
            Debug.LogError("timeText n'est pas assigné dans l'Inspector !");
        }
    }

    public void CollectItem()
    {
        collectedCount++;
        if (collectedCount >= totalCollectibles)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        isGameActive = false;
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        isGameActive = false;
        losePanel.SetActive(true);
    }
}
