using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonStyleScript : MonoBehaviour
{
    public void Select(Button b)
    {
        if (b.IsInteractable())
        {
            if (EventSystem.current.currentSelectedGameObject != b.gameObject)
                EventSystem.current.SetSelectedGameObject(b.gameObject);
            b.GetComponentInChildren<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Underline;
        }
    }

    public void Deselect(Button b)
    {
        b.GetComponentInChildren<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Normal;
    }

}
