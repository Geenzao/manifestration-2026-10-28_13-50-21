using UnityEngine;

public class SFXManager : MonoBehaviour
{   
    public static SFXManager instance;
    [SerializeField] AudioSource mainThemeOneShotPlayer;
    [SerializeField] AudioSource mainThemeLoopPlayer;
    [SerializeField] AudioSource sfxPlayer;
    [SerializeField] AudioClip mainThemeOneShot;
    [SerializeField] AudioClip mainThemeLoop;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        PlayMainTheme();
    }
    void PlayMainTheme()
    {
        mainThemeOneShotPlayer.clip = mainThemeOneShot;
        mainThemeOneShotPlayer.loop = false;
        mainThemeOneShotPlayer.Play();

        mainThemeLoopPlayer.clip = mainThemeLoop;
        mainThemeLoopPlayer.loop = true;
        mainThemeLoopPlayer.PlayDelayed(mainThemeOneShotPlayer.clip.length);
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxPlayer.PlayOneShot(sfx);
    }
}
