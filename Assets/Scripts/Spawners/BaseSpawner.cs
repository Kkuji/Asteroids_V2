using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public abstract class SpawnerBase : MonoBehaviour
{
    protected const float SPAWN_TIMEOUT = 1f;
    protected CancellationTokenSource _cancellationTokenSource;
    protected Factory _factory;
    protected MyObjectPool<IPoolable> _objectPool;
    protected ISpawnPositionProvider _spawnPositionProvider;
    protected WorldConfig _worldConfig;

    protected virtual void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        RegisterPools();
    }

    [Inject]
    public void Initialize(ConfigService configService,
    MyObjectPool<IPoolable> objectPool,
    Factory factory,
    ISpawnPositionProvider spawnPositionProvider)
    {
        _worldConfig = configService.gameConfig.worldConfig;
        _objectPool = objectPool;
        _factory = factory;
        _spawnPositionProvider = spawnPositionProvider;
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
        IPoolable poolable = _objectPool.Get(spawnType);

        Vector3 spawnPosition = _spawnPositionProvider.GetRandomSpawnPositionAtBorder();
        poolable.SetStartPosition(spawnPosition);
    }
}