using TMPro;
using UnityEngine;

public class InteractNotif : MonoBehaviour
{
    [SerializeField] private GameObject notifUI;
    [SerializeField] private TextMeshProUGUI notifText;
    public void EnableNotif(InteractType type)
    {
        notifUI.SetActive(true);
        switch (type)
        {
            case InteractType.Interact:
                notifText.text = "E - Interagir";
                break;
            case InteractType.PickItem:
                notifText.text = "E - Prendre";
                break;
            case InteractType.ThrowItem:
                notifText.text = "E - Lancer";
                break;
        }

    }
    public void DisableNotif()
    {
        notifUI.SetActive(false);
    }
}
