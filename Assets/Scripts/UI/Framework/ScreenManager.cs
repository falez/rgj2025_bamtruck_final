using System;
using System.Collections.Generic;
using UnityEngine;

public interface IScreen
{
    void Show(bool instant = false);

    // force hide
    void Hide(bool instant = false);

    void Focus();

    void Unfocus();

    // attempts to hide, may fail if concrete class disallows.
    bool TryHide();

    public delegate void ScreenCallback(IScreen context);

    event ScreenCallback OnShowStart;

    event ScreenCallback OnShowComplete;

    event ScreenCallback OnHideStart;

    event ScreenCallback OnHideComplete;
}

public class ScreenManager
{
    private static Stack<IScreen> screenStack = new();

    private static IScreen currentScreenHiding;

    public static void Reset()
    {
        screenStack.Clear();
    }

    public static void ShowOverlay(IScreen screen, bool instant = false)
    {
        screen.OnShowStart += OnShowOverlayStarted;
        screen.OnShowComplete += OnShowOverlayCompleted;
        screen.OnHideStart += OnHideOverlayStarted;
        screen.OnHideComplete += OnHideOverlayCompleted;

        if (screenStack.TryPeek(out IScreen currentScreen))
        {
            currentScreen.Unfocus();
        }

        screen.Show(instant);
    }

    public static void Show(IScreen screen, bool instant = false)
    {
        screen.OnShowStart += OnShowStarted;
        screen.OnShowComplete += OnShowCompleted;
        screen.OnHideStart += OnHideStarted;
        screen.OnHideComplete += OnHideCompleted;

        if (screenStack.TryPeek(out IScreen currentScreen))
        {
            currentScreen.Unfocus();
        }

        screenStack.Push(screen);
        screen.Show(instant);
    }

    // No Hide method, responsibility is on the Screen which needs to
    // invoke OnHideStart and OnHideComplete

    public static bool BackRequested()
    {
        if (screenStack.TryPeek(out IScreen currentScreen))
        {
            return currentScreen.TryHide();
        }
        return false;
    }

    private static void OnShowStarted(IScreen context)
    {
        Debug.Assert(context != null, "context cannot be null");
    }

    private static void OnShowCompleted(IScreen context)
    {
        Debug.Assert(context != null, "context cannot be null");
    }

    private static void OnHideStarted(IScreen context)
    {
        Debug.Assert(context != null, "context cannot be null");
        currentScreenHiding = context;
    }

    private static void OnHideCompleted(IScreen context)
    {
        Debug.Assert(context != null, "context cannot be null");
        Debug.Assert(currentScreenHiding != null, "OnHideStart was not called on Screen that was hidden!");

        context.OnShowStart -= OnShowStarted;
        context.OnShowComplete -= OnShowCompleted;
        context.OnHideStart -= OnHideStarted;
        context.OnHideComplete -= OnHideCompleted;

        if (currentScreenHiding.Equals(context))
        {
            screenStack.Pop();
        }

        if (screenStack.TryPeek(out IScreen currentScreen))
        {
            currentScreen.Focus();
        }
    }

    private static void OnShowOverlayStarted(IScreen context)
    {
        Debug.Assert(context != null, "context cannot be null");
    }

    private static void OnShowOverlayCompleted(IScreen context)
    {
        Debug.Assert(context != null, "context cannot be null");
    }

    private static void OnHideOverlayStarted(IScreen context)
    {
        Debug.Assert(context != null, "context cannot be null");
    }

    private static void OnHideOverlayCompleted(IScreen context)
    {
        context.OnShowStart -= OnShowOverlayStarted;
        context.OnShowComplete -= OnShowOverlayCompleted;
        context.OnHideStart -= OnHideOverlayStarted;
        context.OnHideComplete -= OnHideOverlayCompleted;

        if (screenStack.TryPeek(out IScreen currentScreen))
        {
            currentScreen.Focus();
        }
    }
}