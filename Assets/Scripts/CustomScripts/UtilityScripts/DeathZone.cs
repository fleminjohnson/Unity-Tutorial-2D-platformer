using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().SetPlayerHurt();
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
