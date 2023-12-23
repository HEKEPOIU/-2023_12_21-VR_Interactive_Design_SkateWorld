using TMPro;
using UnityEngine;

namespace Player.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textMeshPro;

        public void OnCoinCollect(int newCoinCount)
        {
            _textMeshPro.text = newCoinCount.ToString();
        }

        public void OnGameOver(int coinCount)
        {
            OnCoinCollect(coinCount);
            gameObject.SetActive(true);
        }
    }
}