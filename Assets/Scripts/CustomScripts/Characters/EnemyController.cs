using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public interface IEnemyState
    {
        void Enter(EnemyController enemyController);
        void Execute();
        void Exit();
    }

    public class EnemyController : MonoBehaviour
    {
        private Transform player;
        [SerializeField]
        private Transform wayPoint;
        [SerializeField]
        private int score = 1;
        [SerializeField]
        private int playerDamage = 20;
        [SerializeField]
        private float patrolSpeed = 2.0f;
        [SerializeField]
        private float followSpeed = 3.0f;
        [SerializeField]
        private float detectionRange = 5.0f;
        [SerializeField]
        private float attackRange = 1.0f;
        [SerializeField]
        private float attackCoolDown = 1.0f;
        [SerializeField]
        private float jumpForce = 5.0f;
        [SerializeField]
        private float chargeForce = 20.0f;
        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private bool isGrounded = true;
        [SerializeField]
        private Animator animator;

        private IEnemyState currentState;
        private bool isPlayerDead = false;
        private bool isEnemyDead = false;

        public Transform Player { get => player; }
        public float PatrolSpeed { get => patrolSpeed;}
        public float FollowSpeed { get => followSpeed; }
        public float EnemyDetectionRange { get => detectionRange;}
        public float AttackRange { get => attackRange;}
        public float AttackCoolDown { get => attackCoolDown;}
        public IEnemyState CurrenState { get => currentState;}
        public Transform WayPoint { get => wayPoint;}
        public float JumpForce { get => jumpForce; }
        public float ChargeForce { get => chargeForce; }
        public Rigidbody2D Rb { get => rb;}
        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public bool IsPlayerDead { get => isPlayerDead; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GameManager.Instance.PlayerController.transform;
            currentState = new Patrolling();
            currentState.Enter(this);
            EventManager.Instance.PlayerRespawn += OnPlayerRespawn;
        }

        // Update is called once per frame
        void Update()
        {
            currentState.Execute();
        }

        private void OnDisable()
        {
            EventManager.Instance.PlayerRespawn -= OnPlayerRespawn;
        }

        public void ChangeState(IEnemyState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Platform"))
            {
                isGrounded = true;
            }

            if (collision.collider.CompareTag("Player"))
            {
                PlayerController playerController = player.GetComponent<PlayerController>();

                if (playerController.IsAttacking | playerController.IsInvincible)
                {
                    EnemyDead();
                }
                else if(!isEnemyDead)
                {
                    isPlayerDead = playerController.SetPlayerHurt(playerDamage);
                    ChangeState(new Patrolling());
                }
            }
        }

        private void EnemyDead()
        {
            animator.SetBool("EnemyDead", true);
            isEnemyDead = true;
            GameManager.Instance.AddScore(score);
            StartCoroutine(DelayedDeath(2.0f));
        }

        IEnumerator DelayedDeath(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

        public void OnPlayerRespawn()
        {
            isPlayerDead = false;
        }
    }
}
