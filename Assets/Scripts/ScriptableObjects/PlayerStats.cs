using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "LumiCore/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement Settings")]
    public float moveSpeed = 150f;
    public float rotateSpeed = 720f;
    
    [Header("Combat Settings")]
    public int maxHealth = 100;
}
