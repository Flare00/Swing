using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossSceneData
{
    private static bool _transitionMainMenu = false;
    public static bool Multijoueur { get; set; }
    public static bool Mission { get; set; }
    public static bool TransitionMainMenu { get => _transitionMainMenu;  set => _transitionMainMenu = value; }
}
