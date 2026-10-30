using UnityEngine;
using UnityEngine.InputSystem;

public class StartRound : MonoBehaviour
{
    private bool playerInside;
    [SerializeField] GameObject ui;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(true);
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            ui.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            GameManager.Instance.StartRound();
    }
}
