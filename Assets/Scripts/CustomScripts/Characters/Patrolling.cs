using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{

    public class Patrolling : IEnemyState
    {
        private EnemyController enemy;
        private List<Vector2> wayPoints = new List<Vector2>();
        private int currentWayPointIndex;

        void IEnemyState.Enter(EnemyController enemyController)
        {
            enemy = enemyController;
            wayPoints.Add(enemy.transform.position);
            wayPoints.Add(enemy.WayPoint.position);
        }

        public void Execute()
        {
            Patrol();
            if (enemy.IsPlayerDead) return;
            if(Vector2.Distance(enemy.transform.position, enemy.Player.transform.position) < enemy.EnemyDetectionRange)
            {
                enemy.ChangeState(new Following());
            }
        }

        public void Exit()
        {
            enemy = null;
            wayPoints.Clear();
        }

        private void Patrol()
        {
            Vector2 targetWayPoint = wayPoints[currentWayPointIndex];

            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetWayPoint, enemy.PatrolSpeed * Time.deltaTime);

            if(Vector2.Distance(enemy.transform.position,targetWayPoint) < 0.3f)
            {
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Count;
            }

            //Debug.Log("Patrolling");
        }
    }
}
