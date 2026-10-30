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

    public void Close()
    {
        gameObject.SetActive(false);
        PlayerUIManager.instance.isUIOpen = false;
    }
}
