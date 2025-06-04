using UnityEngine;
using Zenject;

public class EnemySpawner : SpawnerBase
{
    private new void Start()
    {
        base.Start();
        StartSpawn(Enums.SpawnType.EnemyAsteroid, _worldConfig.maxAsteroidsAmount);
        StartSpawn(Enums.SpawnType.EnemyShip, _worldConfig.maxEnemyShipsAmount);
    }

    protected override void RegisterPools()
    {
        _objectPool.RegisterPool(
            Enums.SpawnType.EnemyShip,
            _factory.GetCreatorForType(Enums.SpawnType.EnemyShip, transform.position),
            _worldConfig.startEnemyShipsAmount);

        _objectPool.RegisterPool(
            Enums.SpawnType.EnemyAsteroid,
            _factory.GetCreatorForType(Enums.SpawnType.EnemyAsteroid, transform.position),
            _worldConfig.startAsteroidsAmount);

        _objectPool.RegisterPool(
            Enums.SpawnType.EnemySmallAsteroid,
            _factory.GetCreatorForType(Enums.SpawnType.EnemySmallAsteroid, transform.position),
            _worldConfig.startAsteroidsAmount);
    }
}