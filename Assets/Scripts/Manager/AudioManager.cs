using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Data")]
    [SerializeField] private AudioData audioData;
    
    [Header("Audio Sources")] 
    private AudioSource musicSource;
    private AudioSource sfxSource;
    
    [Header("Volume Controls")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        InitializeAudioSources();
        audioData?.Initialize();
        DontDestroyOnLoad(this.gameObject);
    }

    private void InitializeAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
        
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
        sfxSource.loop = false;
    }
    public void PlaySFX(SFXType type)
    {
        AudioClip clip = audioData.GetClip(type);
        if(clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
    public void PlayMusic(AudioClip clip)
    {
        if(clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }
}
