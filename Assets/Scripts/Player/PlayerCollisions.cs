using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private PlayerManagerSystem playerManagerSystem;
    private int _health;

    private PlayerConfig _playerConfig;
    private SignalBus _signalBus;

    public bool IsCollided { get; private set; }

    public GameObject LastCollidedObject { get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsCollided)
            return;

        if (!collision.collider.TryGetComponent(out EnemyBase enemy))
            return;

        enemy.CollidedWithPlayer(collision);
        playerManagerSystem.DecreaseHealth();
        LastCollidedObject = collision.collider.gameObject;
        Vector3 normal3D = collision.contacts[0].normal;
        Vector2 normal = new(normal3D.x, normal3D.y);
        playerManagerSystem.ShipHit(normal);

        _signalBus.Fire<PlayerHitSignal>();

        HitCooldownAsync().Forget();
    }

    [Inject]
    public void Initialize(ConfigService configService, SignalBus signalBus)
    {
        _playerConfig = configService.gameConfig.playerConfig;
        _signalBus = signalBus;
    }

    private async UniTaskVoid HitCooldownAsync()
    {
        IsCollided = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_playerConfig.invulnerableDuration));
        playerManagerSystem.ShipMovable();
        await UniTask.Delay(TimeSpan.FromSeconds(_playerConfig.moveAfterHitDuration));
        IsCollided = false;
    }
}