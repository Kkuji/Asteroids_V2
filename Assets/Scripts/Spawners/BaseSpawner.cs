using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

public abstract class SpawnerBase : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D spawnBordersPolygonCollider2D;

    protected const float SPAWN_TIMEOUT = 1f;
    protected MyObjectPool<IPoolable> _objectPool;
    protected WorldConfig _worldConfig;
    protected CancellationTokenSource _cancellationTokenSource;
    protected Factory _factory;

    [Inject]
    public void Initialize(ConfigService configService, MyObjectPool<IPoolable> objectPool, Factory factory)
    {
        _worldConfig = configService.gameConfig.worldConfig;
        _objectPool = objectPool;
        _factory = factory;
    }

    protected virtual void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        RegisterPools();
    }

    protected abstract void RegisterPools();

    protected void StartSpawn(Enums.SpawnType spawnType, int maxObjectsAmount)
    {
        SpawnIPoolableObjects(_cancellationTokenSource.Token, spawnType, maxObjectsAmount).Forget();
    }

    private async UniTask SpawnIPoolableObjects(CancellationToken token, Enums.SpawnType spawnType,
    int maxObjectsAmount)
    {
        while (!token.IsCancellationRequested)
        {
            if (_objectPool.InvolvedObjectsAmount(spawnType) >= maxObjectsAmount)
            {
                await UniTask.WaitForSeconds(SPAWN_TIMEOUT, cancellationToken: token);
                continue;
            }

            float randomSpawnDelay = spawnType switch
            {
                Enums.SpawnType.EnemyAsteroid => Random.Range(
                    _worldConfig.asteroidsSpawnMinTimeStep,
                    _worldConfig.asteroidsSpawnMaxTimeStep),
                Enums.SpawnType.EnemyShip => Random.Range(
                    _worldConfig.enemyShipSpawnMinTimeStep,
                    _worldConfig.enemyShipSpawnMaxTimeStep),
                _ => SPAWN_TIMEOUT
            };

            await UniTask.Delay(TimeSpan.FromSeconds(randomSpawnDelay), cancellationToken: token);
            SpawnIPoolableObject(spawnType);
        }
    }

    protected void SpawnIPoolableObject(Enums.SpawnType spawnType)
    {
        var poolable = _objectPool.Get(spawnType);
        poolable.SetStartPosition(GetRandomPointInSpawnBounds(spawnBordersPolygonCollider2D));
    }

    protected Vector3 GetRandomPointInSpawnBounds(PolygonCollider2D collider)
    {
        var randomPoint = new Vector2(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y)
        );
        return collider.ClosestPoint(new Vector3(randomPoint.x, randomPoint.y, 0));
    }
}