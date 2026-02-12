using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelUpPanel;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;

    }
    private void HandlePlayerDeath()
    {
        Debug.Log("Game Over Logic Triggered!");

        // 1. Dừng thời gian
        Time.timeScale = 0f;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    public void RestartGame()
    {
        // Phát âm thanh khi bấm nút Restart
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseLevelUpPanel()
    {
        // Phát âm thanh khi đóng panel
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        
        if(levelUpPanel != null) levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OpenLevelUpPanel()
    {
        if(levelUpPanel != null) levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
