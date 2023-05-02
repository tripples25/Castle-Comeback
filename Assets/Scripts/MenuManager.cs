using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject skinsPanel; 
    
    private void Awake()
    {
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
        skinsPanel.SetActive(state == GameState.Skins);
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
