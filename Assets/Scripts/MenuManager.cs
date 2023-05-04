using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
        GameManager.OnGameStateChange += MenuManagerOnGameStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= MenuManagerOnGameStateChange;
    }

    private void MenuManagerOnGameStateChange(GameState state)
    {
        if (state == GameState.Exit)
            print("Вы вышли");
    }

    public void OpenSkinsPanel()
    {
        GameManager.Instance.UpdateGameState(GameState.Skins);
    }

    public void BackToMenu()
    {
        GameManager.Instance.UpdateGameState(GameState.Menu);
    }

    public void PlayGame()
    {
        GameManager.Instance.UpdateGameState(GameState.Play);
    }
}
