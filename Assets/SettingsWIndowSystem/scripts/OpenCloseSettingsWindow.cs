using System;
using UnityEngine;

public class OpenCloseSettingsWindow : MonoBehaviour
{
    public static event Action<bool> OnOpenCloseSettings;
    public static Func<bool> OnEscPressed;
    private static bool isOpen;

    public static void SetOpen(bool open)
    {
        isOpen = open;
        OnOpenCloseSettings?.Invoke(isOpen);
    }
}
