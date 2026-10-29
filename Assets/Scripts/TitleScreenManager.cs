using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    private int currentSkinIndex = 0;
    [SerializeField] GameObject[] playerSkins;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI playerName;

    void Start()
    {
        // Initialize the first skin and name
        playerSkins[currentSkinIndex].SetActive(true);
        playerName.text = playerSkins[currentSkinIndex].name;
    }
    public IEnumerator StartGameAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainGame");

        yield return null;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameAsync());
    }

    public void ChooseNextSkin()
    {
        playerSkins[currentSkinIndex].SetActive(false);
        currentSkinIndex = (currentSkinIndex + 1) % playerSkins.Length;
        playerSkins[currentSkinIndex].SetActive(true);

        playerName.text = playerSkins[currentSkinIndex].name;
    }

    public void ChoosePreviousSkin()
    {
        playerSkins[currentSkinIndex].SetActive(false);
        currentSkinIndex = (currentSkinIndex - 1 + playerSkins.Length) % playerSkins.Length;
        playerSkins[currentSkinIndex].SetActive(true);

        playerName.text = playerSkins[currentSkinIndex].name;
    }
}
