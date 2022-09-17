using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem
{


    private static TooltipSystem INSTANCE;
    private Tooltip tooltip;


    public static TooltipSystem getInstance()
    {
        if (INSTANCE is null)
        {
            INSTANCE = new TooltipSystem();
        }

        return INSTANCE;
    }

    public TooltipSystem()
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/TooltipCanvas", typeof(GameObject))) as GameObject;
        
        tooltip = obj.transform.GetChild(0).gameObject.GetComponent<Tooltip>();
    }

    public void Show(string content, string header = "")
    {
        tooltip.SetText(content, header);
        tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}
