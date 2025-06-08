using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public abstract class EnemyBase : MonoBehaviour, IPoolable
{
    protected bool canBounce = true;
    [Inject] protected ConfigService ConfigService;
    protected bool isActive;
    protected Vector3 motionDirection;
    [Inject] protected MyObjectPool<IPoolable> ObjectPool;

    [Inject] protected Transform PlayerTransform;
    protected float speed;

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

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
        SetMovingDirection(position);
    }

    protected abstract void ReleaseThisEnemy();

    public abstract void Damaged();
    protected abstract void SetMovingDirection(Vector3 position);
    protected abstract void StartMovement();

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