using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public abstract class EnemyBase : MonoBehaviour, IPoolable
{
    protected bool isActive;
    protected bool canBounce = true;
    protected float speed;
    protected Vector3 motionDirection;

    [Inject] protected Transform PlayerTransform;
    [Inject] protected ConfigService ConfigService;
    [Inject] protected MyObjectPool<IPoolable> ObjectPool;

    public void OnSpawn()
    {
        isActive = true;
        gameObject.SetActive(true);
        StartMovement();
    }

    public void OnDespawn()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    protected abstract void ReleaseThisEnemy();

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
        SetMovingDirection(position);
    }

    public abstract void Damaged();
    protected abstract void SetMovingDirection(Vector3 position);
    protected abstract void StartMovement();

    private void OnCollisionEnter(Collision collision)
    {
        // if (!canBounce)
        //     return;
        //
        // if (!collision.collider.TryGetComponent(out PlayerCollisions playerCollisions))
        //     return;
        //
        // if (playerCollisions.LastCollidedObject != gameObject)
        //     return;
        //
        // motionDirection = Vector3.Reflect(motionDirection, collision.contacts[0].normal);
        // BounceCooldown().Forget();
    }

    public void CollidedWithPlayer(Collision collision)
    {
        motionDirection = Vector3.Reflect(motionDirection, collision.contacts[0].normal);
        BounceCooldown().Forget();
    }

    protected virtual async UniTaskVoid BounceCooldown()
    {
        canBounce = false;
        await UniTask.Delay(TimeSpan.FromSeconds(ConfigService.gameConfig.enemyConfig.bounceCooldown));
        canBounce = true;
    }
}