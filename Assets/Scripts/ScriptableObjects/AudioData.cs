using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Shoot,
    Hit,
    EnemyDeath,
    LevelUp,
    ButtonClick,
    PlayerDeath
}

[System.Serializable]
public class SFXEntry
{
    public SFXType type;
    public AudioClip clip;
}
[CreateAssetMenu(fileName = "AudioDate", menuName = "ScriptableObjects/AudioData")]
public class AudioData : ScriptableObject
{
    [Header("SFX Clips")]
    [SerializeField] private List<SFXEntry> sfxEntries = new List<SFXEntry>();
    
    private Dictionary<SFXType, AudioClip> sfxDictionary;

    public void Initialize()
    {
        sfxDictionary = new Dictionary<SFXType, AudioClip>();
        foreach (var entry in sfxEntries)
        {
            if (!sfxDictionary.ContainsKey(entry.type))
            {
                sfxDictionary.Add(entry.type, entry.clip);
            }
        }
    }

    public AudioClip GetClip(SFXType type)
    {
        if (sfxDictionary == null || sfxDictionary.Count == 0)
        {
            Initialize();
        }
        return sfxDictionary.TryGetValue(type, out AudioClip clip) ? clip : null;
    }
    
}
