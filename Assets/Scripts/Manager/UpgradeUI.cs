using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    
    [Header("Color Tint Components")]
    [SerializeField] private Image frameImage;      // Khung viền (Sprite phải màu TRẮNG)
    [SerializeField] private Image glowImage;       // Hiệu ứng phát sáng (Sprite phải màu TRẮNG)
    [SerializeField] private Image bgTint;          // Nền mờ

    private UpgradeData _data;

    public void SetData(UpgradeData data, System.Action<UpgradeData> onSelected)
    {
        _data = data;

        // 1. Hiển thị thông tin
        if(iconImage) iconImage.sprite = data.icon;
        if(nameText) nameText.text = data.upgradeName;
        if(descText) descText.text = data.description;

        // 2. 👉 TỰ ĐỘNG CHỌN MÀU THEO LOẠI (TYPE)
        Color typeColor = GetColorByType(data.upgradeType);

        // Áp dụng màu
        if (frameImage) frameImage.color = typeColor;
        if (glowImage) glowImage.color = typeColor;
        // Nền thì cho mờ đi một tí nhìn cho sang
        if (bgTint) bgTint.color = new Color(typeColor.r, typeColor.g, typeColor.b, 0.3f); 
        // Tên skill cùng màu luôn
        if (nameText) nameText.color = typeColor;

        // 3. Gắn sự kiện nút bấm (Giữ nguyên)
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => onSelected?.Invoke(_data));
    }

    // --- BẢNG MÀU QUY ĐỊNH Ở ĐÂY ---
    private Color GetColorByType(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                return new Color(1f, 0.3f, 0.3f); // Đỏ (Red) - Sát thương
            
            case UpgradeType.Speed:
                return new Color(0f, 0.8f, 1f);   // Xanh dương sáng (Cyan) - Tốc độ
            
            case UpgradeType.MaxHealth:
                return new Color(0.3f, 1f, 0.4f); // Xanh lá (Green) - Hồi máu
            case UpgradeType.AttackSpeed:
                return new Color(1f, 0.4f, 1f);
            case UpgradeType.JumpHigh:
                return new Color(1f, 0.4f, 1f);
            case UpgradeType.Shield:
                return new Color(1f, 0.4f, 1f);
            default:
                return Color.white; // Mặc định màu trắng
        }
    }
}