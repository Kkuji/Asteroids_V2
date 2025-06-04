using UnityEngine;
using Zenject;

public class BulletSpawner : SpawnerBase
{
    protected override void RegisterPools()
    {
        _objectPool.RegisterPool(
            Enums.SpawnType.Bullet,
            _factory.GetCreatorForType(Enums.SpawnType.Bullet, transform.position),
            _worldConfig.starBulletsAmount);
    }
}