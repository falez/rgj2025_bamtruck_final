using UnityEngine;
using UnityEngine.InputSystem;

public class MenuEntry : MonoBehaviour
{
    [SerializeField] private ScreenBase startingScreen;
    [SerializeField] private bool instant = true;

    private void Start()
    {
        ScreenManager.Reset();
        ScreenManager.Show(startingScreen, instant);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
#if UNITY_EDITOR
            bool result = ScreenManager.BackRequested();
            Debug.Log($"Back result: {result}");
#else
            ScreenManager.BackRequested();
#endif
        }
    }
}