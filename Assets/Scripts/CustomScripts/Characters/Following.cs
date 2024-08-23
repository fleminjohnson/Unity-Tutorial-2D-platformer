using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperPlatformer
{
    public class Following : IEnemyState
    {
        private EnemyController enemy;

        void IEnemyState.Enter(EnemyController enemyController)
        {
            enemy = enemyController;
        }

        public void Execute()
        {
            if (!enemy.IsPlayerDead)
            {
                Follow();
            }
            else
            {
                enemy.ChangeState(new Patrolling());
            }

            if(Vector2.Distance(enemy.transform.position,enemy.Player.transform.position) < enemy.AttackRange & !enemy.IsPlayerDead)
            {
                enemy.ChangeState(new Attacking());
            }
            else if(Vector2.Distance(enemy.transform.position, enemy.Player.transform.position) > enemy.EnemyDetectionRange)
            {
                enemy.ChangeState(new Patrolling());
            }
        }

        public void Exit()
        {
            enemy = null;
        }

        private void Follow()
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.Player.transform.position, enemy.FollowSpeed * Time.deltaTime);

            //Debug.Log("Following");

        }
    }
}

