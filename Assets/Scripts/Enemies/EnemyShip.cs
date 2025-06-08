using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShip : EnemyBase
{
    private const float MINIMUM_DISTANCE_TO_PLAYER_SHIP = 1f;
    private Vector3 _targetPosition;

    protected override void ReleaseThisEnemy()
    {
        ObjectPool.Release(Enums.SpawnType.EnemyShip, this);
    }

    public override void Damaged()
    {
        ReleaseThisEnemy();
    }

    protected override void SetMovingDirection(Vector3 position)
    {
        _targetPosition = PlayerTransform.position;
        speed = Random.Range(
            ConfigService.gameConfig.enemyConfig.enemyShipMinSpeed,
            ConfigService.gameConfig.enemyConfig.enemyShipMaxSpeed);
        motionDirection = (_targetPosition - position).normalized;
    }

    protected override async void StartMovement()
    {
        while (isActive)
        {
            if (canBounce)
            {
                float distance = Vector3.Distance(transform.position, _targetPosition);
                if (distance < MINIMUM_DISTANCE_TO_PLAYER_SHIP)
                {
                    _targetPosition = PlayerTransform.position;
                    motionDirection = (_targetPosition - transform.position).normalized;
                }
            }

            transform.Translate(motionDirection * speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }

    protected override async UniTaskVoid BounceCooldown()
    {
        canBounce = false;
        await UniTask.Delay(TimeSpan.FromSeconds(ConfigService.gameConfig.enemyConfig.bounceCooldown));
        canBounce = true;
        UpdateUserPosition();
    }

    private void UpdateUserPosition()
    {
        _targetPosition = PlayerTransform.position;
        motionDirection = (_targetPosition - transform.position).normalized;
    }
}