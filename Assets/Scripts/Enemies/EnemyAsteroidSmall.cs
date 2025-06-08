using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAsteroidSmall : EnemyBase
{
    private float _deflection;
    private const float SPEED_MULTIPLIER = 1.5f;

    protected override void ReleaseThisEnemy()
    {
        ObjectPool.Release(Enums.SpawnType.EnemySmallAsteroid, this);
    }

    public override void Damaged()
    {
        ReleaseThisEnemy();
    }

    protected override void SetMovingDirection(Vector3 position)
    {
        speed = Random.Range(
                    ConfigService.gameConfig.enemyConfig.asteroidMinSpeed,
                    ConfigService.gameConfig.enemyConfig.asteroidMaxSpeed)
                * SPEED_MULTIPLIER;

        Vector2 randomDirection2D = Random.insideUnitCircle.normalized;
        motionDirection = new Vector3(randomDirection2D.x, randomDirection2D.y,
            ConfigService.gameConfig.enemyConfig.defaultEnemiesPositionZ);
    }

    protected override async void StartMovement()
    {
        while (isActive)
        {
            transform.Translate(motionDirection * speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
}