using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class SpeedPowerUp : MonoBehaviour
    {
        [SerializeField]
        private float speedMultiplier = 2f;
        [SerializeField]
        private int score = 5;
        [SerializeField]
        private float duration = 5.0f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ActivateSpeedPowerUp(duration, speedMultiplier);
                    GameManager.Instance.AddScore(score);
                    Destroy(gameObject);
                }
            }
        }
    }
}
