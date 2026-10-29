using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Parametres : MonoBehaviour
{
    private const string MusicParamName = "musicParam";
    private const string SfxParamName = "SFXParam";
    private const float MinVolumeDb = -80f;

    // L'audio mixer utilisé pour gérer le volume du son
    [SerializeField] private AudioMixer mixer;
    // Le slider de la musique
    [SerializeField] private Slider musicSlider;
    // Le slider des SFX
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        if (musicSlider != null)
        {
            mixer.GetFloat(MusicParamName, out float musicDb);
            musicSlider.value = Mathf.Pow(10f, musicDb / 20f);
            //Quand la valeur du slider est changer on appelle SetMusicVolume avec la nouvelle valeur comme parametre
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            mixer.GetFloat(SfxParamName, out float musicDb);
            sfxSlider.value = Mathf.Pow(10f, musicDb / 20f);
            //Quand la valeur du slider est changer on appelle SetSFXVolume avec la nouvelle valeur comme parametre
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        quitButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float value)
    {
        if (mixer == null)
        {
            return;
        }

        mixer.SetFloat(MusicParamName, SliderValueToDb(value));
    }

    public void SetSFXVolume(float value)
    {
        if (mixer == null)
        {
            return;
        }

        mixer.SetFloat(SfxParamName, SliderValueToDb(value));
    }

    private float SliderValueToDb(float value)
    {
        value = Mathf.Clamp01(value);

        if (value <= 0f)
        {
            return MinVolumeDb;
        }

        return Mathf.Log10(value) * 20f;
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
