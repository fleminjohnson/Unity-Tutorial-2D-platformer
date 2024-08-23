using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class Asteroid : MonoBehaviour
    {
        private Transform player;
        [SerializeField]
        private float minimumPlayerDistance = 5.0f;
        [SerializeField]
        private float deployForce = 1.0f;

        private Rigidbody2D rb;
        private bool deploy;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameManager.Instance.PlayerController.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector2.Distance(player.position, transform.position) < minimumPlayerDistance)
            {
                deploy = true;
            }
        }

        private void FixedUpdate()
        {
            if (deploy)
            {
                rb.AddForce(new Vector2(-deployForce,-1), ForceMode2D.Impulse);
                enabled = false;
                rb.gravityScale = 1;
            }
        }
    }
}
