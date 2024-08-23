using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class Token : MonoBehaviour
    {
        [SerializeField]
        private int points = 1;
        [SerializeField]
        private AudioClip audioClip;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.AddScore(points);
                AudioManager.Instance.PlayAudio(audioClip);
                Destroy(gameObject);
            }
        }
    }
}
