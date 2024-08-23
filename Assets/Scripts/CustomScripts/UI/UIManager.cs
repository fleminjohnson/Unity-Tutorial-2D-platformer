using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SuperPlatformer
{

    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject MainMenu;
        [SerializeField]
        private TMP_Text scoreText;
        [SerializeField]
        private Image healthBarFill;
        [SerializeField]
        private TMP_Text highScoreText;
        [SerializeField]
        private GameObject gameOverPanel;
        [SerializeField]
        private GameObject gameWinPanel;
        


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //enable the Main Menu
                MainMenu.SetActive(true);
            }
        }

        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }

        public void UpdateHighScore(int highScore)
        {
            highScoreText.text = highScore.ToString();
        }

        public void OnGameOver()
        {
            gameOverPanel.SetActive(true);
        }

        public void OnGameWin()
        {
            gameWinPanel.SetActive(true);
        }
    }
}
