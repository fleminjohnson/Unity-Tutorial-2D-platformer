using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuperPlatformer
{
    public class GameManager : SingletonGeneric<GameManager>
    {
        [SerializeField]
        private PlayerController playerController;
        [SerializeField]
        private UIManager uIManager;

        private int currentScore;
        private int highScore;

        public PlayerController PlayerController { get => playerController;}

        // Start is called before the first frame update
        void Start()
        {
            currentScore = 0;
            uIManager.UpdateHealthBar(1,1);
            LoadHighScore();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CheckHighScore()
        {
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
                uIManager.UpdateHighScore(highScore);
            }
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            uIManager.UpdateHighScore(highScore);
        }

        public void AddScore(int points)
        {
            currentScore += points;

            uIManager.UpdateScore(currentScore);

            CheckHighScore();
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            uIManager.UpdateHealthBar(currentHealth, maxHealth);
        }

        public void GameOver()
        {
            uIManager.OnGameOver();
            CheckHighScore();
        }

        public void SetWin()
        {
            uIManager.OnGameWin();
            CheckHighScore();
        }

        public void Restart()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(currentScene.name);
        }
    }
}
