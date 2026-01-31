using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManger : MonoBehaviour
{
    public static LevelManger Instance { get; private set; }

    [Header("UI References")] 
    [SerializeField] private Slider expSlider;
    [FormerlySerializedAs("expText")] [SerializeField] private TextMeshProUGUI levelText;
    
    [Header("Stats")]
    [SerializeField] private int baseExp = 100;   
    
    public int currentLevel { get; private set; } = 1;
    public int currentExp { get; private set; } = 0;
    
    public int ExpRequired => Mathf.RoundToInt(baseExp * (currentLevel  * 1.2f));
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        UpdateUI();
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"Nhận được {amount} EXP. Tổng EXP hiện tại: {currentExp}/{ExpRequired}");
        while (currentExp >= ExpRequired)
        {
            currentExp -= ExpRequired;
            LevelUp();
        }
        UpdateUI();
    }

    private void LevelUp()
    {
        currentLevel++;
        Debug.Log($"Chúc mừng! Bạn đã lên cấp {currentLevel}!");
        
        // TODO: Pause Game & Show Upgrade Panel
    }

    private void UpdateUI()
    {
        if (expSlider != null)
        {
            float progress = (float)currentExp / ExpRequired;
            expSlider.value = progress;
        }
        if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }
    }
}
