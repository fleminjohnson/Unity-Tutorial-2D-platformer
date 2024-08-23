using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip walkAudio;
        [SerializeField]
        private AudioClip jumpAudio;
        [SerializeField]
        private Transform playerStartingPosition;
        [SerializeField]
        private float speed = 5.0f;
        [SerializeField]
        private float jumpTakeOffSpeed = 7.0f;
        [SerializeField]
        private float secondJumpTakeOffSpeed = 2.0f;
        [SerializeField]
        private float attackForce = 7.0f;
        [SerializeField]
        private float maxHeightDuration = 1.0f;
        [SerializeField]
        private int maxHealth = 100;
        [SerializeField]
        private int lives = 3;
        [SerializeField]
        private int obstacleDamage = 10;

        private Rigidbody2D rb;
        private float moveInput;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private bool canJump;
        private bool isGrounded = false;
        private bool isInvincible = false;

        private float currentJumpTakeoffSpeed;
        private int currentHealth;
        private Coroutine delayCoroutine = null;

        private enum PlayerState { Grounded, PrepareToJump, Jumping, ReadyForAttack ,InFlight, Landing}

        [SerializeField]
        PlayerState playerState = PlayerState.Grounded;
        private bool isReadyForAttack;
        private bool isAttacking;

        public bool IsAttacking { get => isAttacking; }
        public bool IsInvincible { get => isInvincible;}

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            moveInput = Input.GetAxis("Horizontal");

            if(isGrounded & Mathf.Abs(moveInput) > 0.1f)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = walkAudio;
                    audioSource.Play();
                }
            }

            if(moveInput > 0.01f)
            {
                spriteRenderer.flipX = false;
            }

            else if(moveInput < -0.01f)
            {
                spriteRenderer.flipX = true;
            }

            if (isGrounded & Input.GetButtonDown("Jump"))
            {
                playerState = PlayerState.PrepareToJump;
            }
            if(!isGrounded & Input.GetKeyDown(KeyCode.B))
            {
                playerState = PlayerState.ReadyForAttack;
            }
            UpdatePlayerState();
        }

        private void FixedUpdate()
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
                animator.SetFloat("Speed", Mathf.Abs(moveInput));
            }

            animator.SetBool("isJumping", !isGrounded);

            if (canJump & Input.GetButton("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, currentJumpTakeoffSpeed);
                canJump = false;
                isGrounded = false;
            }
            if (isReadyForAttack)
            {
                rb.velocity = new Vector2(rb.velocity.x, -attackForce);
                isReadyForAttack = false;
                isAttacking = true;
            }
        }

        private void UpdatePlayerState()
        {
            switch (playerState)
            {
                case PlayerState.PrepareToJump:
                    playerState = PlayerState.Jumping;
                    canJump = true;
                    currentJumpTakeoffSpeed = jumpTakeOffSpeed;
                    PlayJumpAudio();
                    break;
                case PlayerState.Jumping:
                    if(delayCoroutine == null)
                    {
                        delayCoroutine = StartCoroutine(DelayedStateChange(maxHeightDuration));
                    }
                    break;
                case PlayerState.ReadyForAttack:
                    isReadyForAttack = true;
                    playerState = PlayerState.Landing;
                    break;
                case PlayerState.InFlight:
                    canJump = true;
                    isGrounded = false;
                    currentJumpTakeoffSpeed = secondJumpTakeOffSpeed;
                    playerState = PlayerState.Landing;
                    break;

                case PlayerState.Landing:
                    playerState = PlayerState.Grounded;
                    break;
            }
        }

        IEnumerator DelayedStateChange(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!isGrounded)
            {
                playerState = PlayerState.InFlight;
            }
            delayCoroutine = null;
        }

        private void PlayJumpAudio()
        {
            Debug.Log("Playing jump audio");
            if(audioSource & jumpAudio)
            {
                audioSource.PlayOneShot(jumpAudio);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if(collision.collider.CompareTag("Platform"))
            {
                isGrounded = true;
            }
            if (collision.collider.CompareTag("Obstacle"))
            {
                if (!IsInvincible)
                {
                    SetPlayerHurt(obstacleDamage);
                }
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                isAttacking = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Ontrigger Enter");
            if (collision.CompareTag("Finish"))
            {
                GameManager.Instance.SetWin();
            }
        }

        private void SetPlayerDead()
        {
            animator.SetBool("isDead", true);
        }

        public bool SetPlayerHurt(int damage = -1)
        {
            if (damage < 0) damage = maxHealth;

            currentHealth -= damage;
            GameManager.Instance.UpdateHealth(currentHealth, maxHealth);

            if(currentHealth <= 0)
            {
                lives--;
                SetPlayerDead();
                if (lives > 0)
                {
                    Respawn();
                }
                else
                {
                    GameManager.Instance.GameOver();
                }
                return true;
            }
            animator.SetTrigger("isHurt");
            return false;
        }

        private void Respawn()
        {
            StartCoroutine(Delay(2.0f, () =>
            {
                transform.position = playerStartingPosition.position;
                animator.SetBool("isRespawn", true);
                animator.SetBool("isDead", false);
                currentHealth = maxHealth;
                GameManager.Instance.UpdateHealth(currentHealth, maxHealth);
                EventManager.Instance.TriggerPlayerRespawn();
            }));
        }

        public void ActivateCookiePowerUp(float duration = 5.0f)
        {
            float scaleFactor = 2;
            isInvincible = true;
            transform.localScale *= scaleFactor;
            StartCoroutine(Delay(duration, ()=>
            {
                transform.localScale /= scaleFactor;
                isInvincible = false;
            }));
        }

        public void ActivateSpeedPowerUp(float duration, float speedMultiplier)
        {
            speed *= speedMultiplier;
            isInvincible = true;
            StartCoroutine(Delay(duration,()=>
            {
                speed /= speedMultiplier;
                isInvincible = false;
            }));
        }

        IEnumerator Delay(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback.Invoke();
        }
    }
}
