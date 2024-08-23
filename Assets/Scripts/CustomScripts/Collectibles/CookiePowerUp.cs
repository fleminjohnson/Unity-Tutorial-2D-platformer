using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class CookiePowerUp : MonoBehaviour
    {
        [SerializeField]
        private float duration = 5.0f;
        [SerializeField]
        private int score = 5;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if(playerController != null)
                {
                    playerController.ActivateCookiePowerUp();
                    GameManager.Instance.AddScore(score);
                    Destroy(gameObject);
                }
            }
        }
    }
}
