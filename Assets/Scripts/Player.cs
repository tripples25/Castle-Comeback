using System;
using UnityEngine;

public class Player : Entity
{
    public bool isPathCreated;
    public bool isPathCompleted;

    private void Awake()
    {
        GameManager.Instance.players.Add(this);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GetComponent<PathMover>().StopAllCoroutines();
        GameManager.Instance.UpdateGameState(GameState.Lose);
    }
}
