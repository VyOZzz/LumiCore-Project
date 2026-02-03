using System;
using System.Collections.Generic;
using UnityEngine;


public enum UpgradeType
{
    Speed,
    Damage,
    MaxHealth,
    AttackSpeed,
    Shield,
    JumpHigh,
}
[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game Data/Upgrade Data")]

public class UpgradeData : ScriptableObject
{
    [Header("Display Info")]
    public string upgradeName;
    public string description;
    public Sprite icon;
    
    [Header("Stats")]
    public UpgradeType upgradeType;
    [Tooltip("Giá trị cộng thêm. VD: Speed = 1, Damage = 5")]
    public float value;
}
