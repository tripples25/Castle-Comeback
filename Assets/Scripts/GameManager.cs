using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChange;
    private int currentLevel = 1;
    public List<Player> players = new();
    public event Action OnAllPathsCreated;
    public event Action OnAllPathsCompleted;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (state == GameState.Play)
        {
            if (players.Count != 0 && players.All(x => x.isPathCreated))
            {
                UpdateGameState(GameState.Walking);
            }
        }

        if (state == GameState.Walking && players.All(x => x.isPathCompleted))
        {
            OnAllPathsCompleted?.Invoke();
            UpdateGameState(GameState.Win);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Win:
                currentLevel++;
                players.Clear();
                break;
            case GameState.Lose:
                players.Clear();
                break;
            case GameState.Walking:
                OnAllPathsCreated?.Invoke();
                break;
            case GameState.Pause:
                break;
            case GameState.Menu:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameState.Skins:
                SceneManager.LoadScene("Skins");
                break;
            case GameState.Play:
                SceneManager.LoadScene("Level" + currentLevel);
                break;
            case GameState.Exit:
                Application.Quit();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }
}
