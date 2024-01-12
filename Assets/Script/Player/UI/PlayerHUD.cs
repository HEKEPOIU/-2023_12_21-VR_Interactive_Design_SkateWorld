using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinText;
        private int currentTutorialPanelIndex = 0;
        [SerializeField] private GameObject[] _tutorialPanels;
        [SerializeField] private GameObject _gameOverPanel;

        private void OnCoinCollect(int newCoinCount)
        {
            _coinText.text = newCoinCount.ToString();
        }

        public void OnGameOver(int coinCount)
        {
            OnCoinCollect(coinCount);
            _gameOverPanel.SetActive(true);
        }
        
        public void SwitchTutorialPanel()
        {
            _tutorialPanels[currentTutorialPanelIndex].SetActive(false);
            currentTutorialPanelIndex++;
            if (currentTutorialPanelIndex >= _tutorialPanels.Length)
            {
                currentTutorialPanelIndex = 0;
                GameManager.Instance.ChangeState(GameState.Start);
                return;
            }
            _tutorialPanels[currentTutorialPanelIndex].SetActive(true);
        }
    }
}