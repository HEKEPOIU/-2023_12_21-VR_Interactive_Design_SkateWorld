using System;
using AIExhibition.Mediator;
using Player.UI;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public PlayerMovement PlayerMovement { get; private set; }
        public Collector PlayerCollector { get; private set; }
        
        [SerializeField] private PlayerHUD _playerHUD;
        public PlayerHUD PlayerHUD { get => _playerHUD; private set => _playerHUD = value; }
        public PlayerState PlayerState { get; private set; }
        
        private GameState _gameState = GameState.Start;
        
        private bool _playerTend = false;
        
        private float _currentInputColdDown = 0;
        
        private float _inputColdDown = 2f;

        private void OnEnable()
        {
            PlayerCollector.OnCollect += OnCoinCollect;
            PlayerMovement.Ontend += PlayerTend;
            BaseMediator.Instance.AddListener<GameState>("OnGameStateChange", ChangePlayerState);
        }
        private void OnDisable()
        {
            PlayerCollector.OnCollect -= OnCoinCollect;
            PlayerMovement.Ontend -= PlayerTend;
            BaseMediator.Instance.RemoveListener<GameState>("OnGameStateChange", ChangePlayerState);
        }

        protected void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerCollector = GetComponent<Collector>();
            PlayerState = new PlayerState();

        }

        private void Update()
        {
            if (_gameState == GameState.GameOver)
            {
                _currentInputColdDown += Time.deltaTime;
            }
            
            if (_playerTend == true )
            {
                if (_gameState == GameState.Start)
                {
                    BaseMediator.Instance.Broadcast("StartGame");
                }
                else if(_gameState == GameState.GameOver)
                {
                    BaseMediator.Instance.Broadcast("RestartGame");
                }
                _playerTend = false;
            }
        }

        private void PlayerTend()
        {
            if (_gameState == GameState.Start)
            {
                _playerTend = true;
            }

            if (_gameState == GameState.GameOver && _currentInputColdDown >= _inputColdDown)
            {
                _currentInputColdDown = 0;
                _playerTend = true;
                
            }
        }

        private void ChangePlayerState(GameState newGameState)
        {
            _gameState = newGameState;
            switch (_gameState)
            {
                case GameState.Start:
                    PlayerMovement.SetMoveAble(false);
                    break;
                case GameState.Game:
                    PlayerMovement.SetMoveAble(true);
                    break;
                case GameState.GameOver:
                    PlayerMovement.SetMoveAble(false);
                    PlayerHUD.OnGameOver(PlayerState.Coins);
                    break;
                default:
                    break;
            }
        }
        
        private void OnCoinCollect()
        {
            PlayerState.Coins++;
            // _playerHUD.OnCoinCollect(PlayerState.Coins);
        }
    }
}