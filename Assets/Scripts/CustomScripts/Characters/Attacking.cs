using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperPlatformer
{
    public class Attacking : IEnemyState
    {
        EnemyController enemy;
        private float lastAttackTime = 0;
        private bool isReadyForHorizontalDash;

        void IEnemyState.Enter(EnemyController enemyController)
        {
            enemy = enemyController;
        }

        public void Execute()
        {
            Attack();

            if (Vector2.Distance(enemy.transform.position, enemy.Player.position) > enemy.AttackRange && !isReadyForHorizontalDash)
            {
                enemy.ChangeState(new Following());
            }
        }

        public void Exit()
        {
            //enemy = null;
        }

        private void Attack()
        {
            if (Time.time - lastAttackTime > enemy.AttackCoolDown)
            {
                lastAttackTime = Time.time;
                //Attack
                PlayerAttackRoutineFunction();
            }

            //Debug.Log("Attack");
        }

        //private async Task Delay(float delay, Action playerAttackRoutineFunction)
        //{
        //    await Task.Delay((int)(delay * 1000)); // Convert delay from seconds to milliseconds
        //    playerAttackRoutineFunction.Invoke();
        //}

        private void PlayerAttackRoutineFunction()
        {
            Debug.Log("Player Attack Routine");
            if (enemy.IsGrounded)
            {
                if (!isReadyForHorizontalDash)
                {
                    Jump();
                    enemy.IsGrounded = false;
                    isReadyForHorizontalDash = true;
                }
                else
                {
                    HorizontalDash();
                    isReadyForHorizontalDash = false;
                }
            }
        }

        private void Jump()
        {
            Debug.Log("Jump");
            enemy.Rb.AddForce(Vector2.up * enemy.JumpForce, ForceMode2D.Impulse);
        }

        private void HorizontalDash()
        {
            Debug.Log("Horizontal dash");
            Vector2 direction = (enemy.Player.position - enemy.transform.position).normalized;

            enemy.Rb.AddForce(direction * enemy.ChargeForce, ForceMode2D.Impulse);
        }
    }
}
