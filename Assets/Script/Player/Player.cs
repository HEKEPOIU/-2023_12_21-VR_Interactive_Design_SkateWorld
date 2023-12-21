using System;
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

        protected void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerCollector = GetComponent<Collector>();
            PlayerState = new PlayerState();

            PlayerCollector.OnCollect += OnCoinCollect;
        }
        protected void OnDestroy()
        {
            PlayerCollector.OnCollect -= OnCoinCollect;
        }
        private void OnCoinCollect()
        {
            PlayerState.Coins++;
            _playerHUD.OnCoinCollect(PlayerState.Coins);
        }
    }
}