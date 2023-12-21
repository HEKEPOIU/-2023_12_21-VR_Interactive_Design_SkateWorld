using TMPro;
using UnityEngine;

namespace Player.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private TMP_Text _textMeshPro;

        public void OnCoinCollect(int newCoinCount)
        {
            _textMeshPro.text = newCoinCount.ToString();
        }
    }
}