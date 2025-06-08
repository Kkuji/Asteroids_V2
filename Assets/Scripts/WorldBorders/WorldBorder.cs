using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class WorldBorder : MonoBehaviour
{
    public event Action<Transform, Transform, bool> objectToTeleportEncounteredAction;

    [SerializeField] private Transform transformTeleportTo;
    [SerializeField] private bool isHorizontalTeleport;
    [SerializeField] private BoxCollider boxCollider;
    [Inject] protected MyObjectPool<IPoolable> ObjectPool;

    public BoxCollider BoxCollider => boxCollider;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManagerSystem playerInputSystem)
            || collision.gameObject.TryGetComponent(out EnemyBase enemyBase))
            objectToTeleportEncounteredAction?.Invoke(collision.transform, transformTeleportTo, isHorizontalTeleport);

        if (collision.gameObject.TryGetComponent(out Bullet bullet))
            ObjectPool.Release(Enums.SpawnType.Bullet, bullet);
    }
}