using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyAsteroid : EnemyBase
{
    private float _deflection;
    private const int ASTEROIDS_AMOUNT = 3;

    protected override void ReleaseThisEnemy()
    {
        ObjectPool.Release(Enums.SpawnType.EnemyAsteroid, this);
    }

    public override void Damaged()
    {
        for (int i = 0; i < ASTEROIDS_AMOUNT; i++)
        {
            IPoolable smallAsteroid = ObjectPool.Get(Enums.SpawnType.EnemySmallAsteroid);
            smallAsteroid.SetStartPosition(transform.position);
            smallAsteroid.OnSpawn();
        }

        ReleaseThisEnemy();
    }

    protected override void SetMovingDirection(Vector3 position)
    {
        speed = Random.Range(
            ConfigService.gameConfig.enemyConfig.asteroidMinSpeed,
            ConfigService.gameConfig.enemyConfig.asteroidMaxSpeed);

        _deflection = ConfigService.gameConfig.enemyConfig.asteroidDeflectionAngle;
        var baseDir = (PlayerTransform.position - position).normalized;
        motionDirection = (baseDir + Random.insideUnitSphere * _deflection).normalized;
        motionDirection = new Vector3(motionDirection.x, motionDirection.y,
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