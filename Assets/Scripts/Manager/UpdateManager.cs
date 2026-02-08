using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    // public static UpdateManager Instance { get; private set; }
    [Header("Settings")]
    [SerializeField] private List<UpgradeData> allUpgrades;
    [SerializeField] private int upgradeChoicesCount = 3;
    
    [Header("UI References")]
    [SerializeField] private Transform upgradeUIContainer;
    [SerializeField] private GameObject upgradeUIPrefab;

    private void OnEnable()
    {
        LevelManger.OnPlayerLevelUp += ShowUpgradeOptions;
    }

    private void OnDisable()
    {
        LevelManger.OnPlayerLevelUp -= ShowUpgradeOptions;
    }

    private void ShowUpgradeOptions()
    {
        Debug.Log("Showing upgrade options");
        // hiện  panel level up
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OpenLevelUpPanel();
        }
        
        //xoas hết các UI nâng cấp cũ
        foreach (Transform child in upgradeUIContainer)
        {
            Destroy(child.gameObject);
        }
        //chọn ngẫu nhiên các nâng cấp
        List<UpgradeData> randomUpgradese = GetRandomUpgrades(upgradeChoicesCount);
        //tạo UI cho từng nâng cấp
        foreach (var upgrade in randomUpgradese)
        {
            GameObject upgradeUI = Instantiate(upgradeUIPrefab, upgradeUIContainer);
            UpgradeUI uiComponent = upgradeUI.GetComponent<UpgradeUI>();
            uiComponent.SetData(upgrade, OnUpgradeSelected);
        }
        
        
    }

    private void OnUpgradeSelected(UpgradeData obj)
    {
        Debug.Log($"Người chơi đã chọn nâng cấp: {obj.upgradeName} - {obj.description}");
        //ap dung nâng cấp cho người chơi ở đây
        ApplyUpgrade(obj);
        //ẩn UI nâng cấp sau khi chọn
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CloseLevelUpPanel();
        }
        
        
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        Player player = Player.Instance;
        if (player == null) return;
        switch (upgrade.upgradeType)
        {
            case UpgradeType.Speed:
                // tang toc
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if(playerMovement != null) playerMovement.IncreaseSpeed((int)upgrade.value);
                break;
            case UpgradeType.Damage:
                // tang sat thuong
                Projectile projectile = player.GetComponentInChildren<Projectile>();
                if (projectile != null)
                {
                    projectile.IncreaseDamage((int)upgrade.value);
                }
                break;
            case UpgradeType.MaxHealth:
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.IncreaseMaxHealth((int)upgrade.value);
                }
                break;
            case UpgradeType.AttackSpeed:
                Projectile projectile2 = player.GetComponentInChildren<Projectile>();
                if (projectile2 != null)
                {
                    projectile2.IncreaseSpeedAttack((int)upgrade.value);
                }
                break;
        }
    }
    private List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> selected = new List<UpgradeData>();
        List<UpgradeData> available = new List<UpgradeData>(allUpgrades);
        count = Mathf.Min(count, available.Count);
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, available.Count);
            selected.Add(available[randomIndex]);
            available.RemoveAt(randomIndex);
        }
        return selected;
    }
}
