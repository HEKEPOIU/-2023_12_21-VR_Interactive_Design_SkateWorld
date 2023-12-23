using System;
using AIExhibition.Mediator;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Start,
    Game,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    
    private GameState _gameState = GameState.Start;
    [SerializeField] private Timer _gameTimer;
    

    protected override void Awake()
    {
        base.Awake();
        _gameTimer.OnTimerEnd += GameEnd;
        BaseMediator.Instance.AddListener("StartGame", StartGame);
        BaseMediator.Instance.AddListener("RestartGame", RestartGame);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _gameTimer.OnTimerEnd -= GameEnd;
        BaseMediator.Instance.RemoveListener("StartGame", StartGame);
        BaseMediator.Instance.RemoveListener("RestartGame", RestartGame);
    }

    private void Start()
    {
        BaseMediator.Instance.Broadcast("OnGameStateChange", _gameState);
    }

    private void GameEnd()
    {
        ChangeState(GameState.GameOver);
    }

    public void ChangeState(GameState state)
    {
        _gameState = state;
        BaseMediator.Instance.Broadcast("OnGameStateChange", _gameState);
    }
    
    
    private void StartGame()
    {
        ChangeState(GameState.Game);
        _gameTimer.StartTimer();
    }
    
    
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
