using System;
using UnityEngine;
using Zenject;

public class WorldBorder : MonoBehaviour
{
    [SerializeField] private Transform transformTeleportTo;
    [SerializeField] private bool isHorizontalTeleport;
    [Inject] protected MyObjectPool<IPoolable> ObjectPool;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManagerSystem playerInputSystem)
            || collision.gameObject.TryGetComponent(out EnemyBase enemyBase))
            objectToTeleportEncounteredAction?.Invoke(collision.transform, transformTeleportTo, isHorizontalTeleport);

        if (collision.gameObject.TryGetComponent(out Bullet bullet))
            ObjectPool.Release(Enums.SpawnType.Bullet, bullet);
    }

    public event Action<Transform, Transform, bool> objectToTeleportEncounteredAction;
}