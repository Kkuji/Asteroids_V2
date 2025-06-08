using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private PlayerManagerSystem playerManagerSystem;
    private GameObject _lastCollidedObject;

    private bool _isCollided;
    private int _health;
    private PlayerConfig _playerConfig;
    private SignalBus _signalBus;

    public bool IsCollided => _isCollided;
    public GameObject LastCollidedObject => _lastCollidedObject;

    [Inject]
    public void Initialize(ConfigService configService, SignalBus signalBus)
    {
        _playerConfig = configService.gameConfig.playerConfig;
        _signalBus = signalBus;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsCollided)
            return;

        if (!collision.collider.TryGetComponent<EnemyBase>(out EnemyBase enemy))
            return;

        enemy.CollidedWithPlayer(collision);
        playerManagerSystem.DecreaseHealth();
        _lastCollidedObject = collision.collider.gameObject;
        Vector3 normal3D = collision.contacts[0].normal;
        Vector2 normal = new(normal3D.x, normal3D.y);
        playerManagerSystem.ShipHit(normal);

        // Отправляем сигнал вместо вызова события
        _signalBus.Fire<PlayerHitSignal>();

        HitCooldownAsync().Forget();
    }

    private async UniTaskVoid HitCooldownAsync()
    {
        _isCollided = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_playerConfig.invulnerableDuration));
        playerManagerSystem.ShipMovable();
        await UniTask.Delay(TimeSpan.FromSeconds(_playerConfig.moveAfterHitDuration));
        _isCollided = false;
    }
}