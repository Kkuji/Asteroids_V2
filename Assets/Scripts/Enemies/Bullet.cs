using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;
using Gameplay;

public class Bullet : MonoBehaviour, IPoolable
{
    protected bool isActive;
    private CancellationTokenSource _moveCancellationTokenSource;
    private Vector2 _velocity;

    [Inject] protected Transform PlayerTransform;
    [Inject] protected ConfigService ConfigService;
    [Inject] protected MyObjectPool<IPoolable> ObjectPool;
    [Inject] private PointsStorage pointsStorage;

    private Dictionary<Type, int> _scoreByEnemyType;

    private void Awake()
    {
        EnemyConfig enemyConfig = ConfigService.gameConfig.enemyConfig;
        _scoreByEnemyType = new Dictionary<Type, int>
        {
            { typeof(EnemyAsteroid), enemyConfig.scoreAsteroid },
            { typeof(EnemyAsteroidSmall), enemyConfig.scoreAsteroidSmall },
            { typeof(EnemyShip), enemyConfig.scoreEnemyShip }
        };
    }

    public void OnSpawn()
    {
        isActive = true;
        gameObject.SetActive(true);
        _moveCancellationTokenSource = new CancellationTokenSource();
    }

    public void OnDespawn()
    {
        isActive = false;
        gameObject.SetActive(false);
        _velocity = Vector2.zero;

        _moveCancellationTokenSource?.Cancel();
        _moveCancellationTokenSource?.Dispose();
        _moveCancellationTokenSource = null;
    }

    public async void SetStartPosition(Vector3 position)
    {
        transform.position = position;
        transform.rotation = PlayerTransform.rotation;

        Vector2 forwardDirection = PlayerTransform.up;
        _velocity = forwardDirection * ConfigService.gameConfig.worldConfig.bulletSpeed;

        await MoveForward(_moveCancellationTokenSource.Token);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out EnemyBase enemyBase))
            return;

        enemyBase.Damaged();
        var type = enemyBase.GetType();

        if (_scoreByEnemyType.TryGetValue(type, out int score))
            pointsStorage.AddPoints(score);

        ObjectPool.Release(Enums.SpawnType.Bullet, this);
    }

    private async UniTask MoveForward(CancellationToken cancellationToken)
    {
        try
        {
            while (isActive && !cancellationToken.IsCancellationRequested)
            {
                if (_velocity != Vector2.zero)
                    transform.Translate(new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime, Space.World);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void OnDestroy()
    {
        _moveCancellationTokenSource?.Cancel();
        _moveCancellationTokenSource?.Dispose();
    }
}