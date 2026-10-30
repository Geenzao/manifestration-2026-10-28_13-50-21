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
    [SerializeField] GameObject settingsPanel;



    void Start()
    {
        // Initialize the first skin and name
        playerSkins[currentSkinIndex].SetActive(true);
        playerName.text = playerSkins[currentSkinIndex].name;
    }
    public IEnumerator StartGameAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Cour & Map");

        yield return null;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameAsync());
    }

    public void Settings()
    {
        if (settingsPanel.activeInHierarchy)
            settingsPanel.SetActive(false);
        else
            settingsPanel.SetActive(true);
    }

    public void ChooseNextSkin()
    {
        playerSkins[currentSkinIndex].SetActive(false);
        currentSkinIndex = (currentSkinIndex + 1) % playerSkins.Length;
        playerSkins[currentSkinIndex].SetActive(true);

        playerName.text = playerSkins[currentSkinIndex].name;

        ChangeAvatar();
    }

    public void ChoosePreviousSkin()
    {
        playerSkins[currentSkinIndex].SetActive(false);
        currentSkinIndex = (currentSkinIndex - 1 + playerSkins.Length) % playerSkins.Length;
        playerSkins[currentSkinIndex].SetActive(true);

        playerName.text = playerSkins[currentSkinIndex].name;

        ChangeAvatar();
    }

    public void ChangeAvatar()
    {
        // Get current skin
        // Get avatar from its animator
        // Give avatar to parent (Player) animator
        GameObject currentSkin = playerSkins[currentSkinIndex];
        Animator currentSkinAnimator = currentSkin.GetComponent<Animator>();

        if (currentSkinAnimator != null)
        {
            // Get avatar from the current skin's animator
            Avatar avatar = currentSkinAnimator.avatar;

            // Get parent
            GameObject player = currentSkin.transform.parent.gameObject;

            // Get the player's animator
            Animator playerAnimator = player.GetComponent<Animator>();

            // Assign the correct avatar to the parent animator
            if (playerAnimator != null)
                playerAnimator.avatar = avatar;
        }
    }

}
