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
    private List<PathDrawer> players = new();
    public event Action OnAllPathsCreated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (state == GameState.Play)
        {
            if (players.Count == 0)
                players = FindObjectsOfType<PathDrawer>().ToList();
            else if (players.All(x => x.isPathCreated))
            {
                OnAllPathsCreated();
                UpdateGameState(GameState.Win);
            }
        }
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Win:
                print("you win");
                currentLevel++;
                break;
            case GameState.Lose:
                print("you lose");
                break;
            case GameState.Pause:
                break;
            case GameState.Menu:
                break;
            case GameState.Skins:
                break;
            case GameState.Play:
                SceneManager.LoadScene("Level" + currentLevel);
                break;
            case GameState.Exit:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }
}
