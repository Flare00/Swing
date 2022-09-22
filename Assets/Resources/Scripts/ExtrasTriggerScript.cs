
using System;
using UnityEngine;

using UnityEngine.EventSystems;

public class ExtrasTriggerScript : MonoBehaviour, IPointerClickHandler
{
    public int x = 0, y = 0;
    public ExtrasScript mainScript = null;

    public ExtrasTriggerScript()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (mainScript != null)
            this.mainScript.moveCursor(x, y);
    }

}
