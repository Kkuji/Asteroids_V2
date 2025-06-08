using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;
using Zenject;

public class PlayerManagerSystem : MonoBehaviour
{
    [SerializeField] private LaserController laserController;

    private PositionStorage _positionStorage;
    private AngleStorage _angleStorage;
    private SpeedStorage _speedStorage;
    private HealthStorage _healthStorage;

    private bool _isMovingByKey;
    private PlayerConfig _playerConfig;
    private IInputService _inputService;
    private CancellationTokenSource _inertiaTokenSource;
    private Vector2 _velocity = Vector2.zero;

    private bool _isHit;
    private int _currentHealth;
    private MyObjectPool<IPoolable> _objectPool;

    public Vector2 Velocity => _velocity;

    private void Awake()
    {
        _healthStorage.SetHealth(_currentHealth);
    }

    public void DecreaseHealth()
    {
        _currentHealth--;
        _healthStorage.SetHealth(_currentHealth);

        if (_currentHealth < 1)
            gameObject.SetActive(false);
    }

    [Inject]
    public void Initialize(
    ConfigService configService,
    IInputService inputService,
    MyObjectPool<IPoolable> objectPool,
    PositionStorage positionStorage,
    AngleStorage angleStorage,
    SpeedStorage speedStorage,
    HealthStorage healthStorage)
    {
        _playerConfig = configService.gameConfig.playerConfig;
        _currentHealth = _playerConfig.maxHealth;
        _inputService = inputService;
        _objectPool = objectPool;
        _positionStorage = positionStorage;
        _angleStorage = angleStorage;
        _speedStorage = speedStorage;
        _healthStorage = healthStorage;
    }

    public void ShipHit(Vector2 normal)
    {
        float currentSpeed = _velocity.magnitude;

        if (currentSpeed < 0.1f)
            _velocity = normal * _playerConfig.minHitPushForce;
        else
            _velocity = Vector2.Reflect(_velocity, normal) * _playerConfig.hitSpeedDecrease;

        if (_velocity.magnitude > _playerConfig.maxSpeed)
            _velocity = _velocity.normalized * _playerConfig.maxSpeed;

        _isHit = true;
    }

    public void ShipMovable() => _isHit = false;

    private void Update()
    {
        _positionStorage.SetPosition(transform.position);
        _angleStorage.SetAngle(transform.rotation.eulerAngles);
        _speedStorage.SetSpeed(_velocity.magnitude);

        if (_isHit)
        {
            if (_velocity != Vector2.zero)
                transform.Translate(new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime, Space.World);
            return;
        }

        if (_inputService.IsFireBulletPressed())
        {
            IPoolable bullet = _objectPool.Get(Enums.SpawnType.Bullet);
            bullet.SetStartPosition(transform.position);
        }

        if (_inputService.IsFireLaserPressed() && laserController.CanFire)
        {
            laserController.FireAsync().Forget();
        }

        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        float rotationInput = _inputService.GetRotationInput();
        transform.Rotate(0, 0, rotationInput * _playerConfig.rotationSpeed * Time.deltaTime);
    }

    private void HandleMovement()
    {
        float verticalInput = _inputService.GetForwardInput();

        if (verticalInput > 0.1f)
        {
            Vector2 forwardDirection = transform.up;
            _velocity += forwardDirection * (verticalInput * _playerConfig.speed * Time.deltaTime);

            if (_velocity.magnitude > _playerConfig.maxSpeed)
                _velocity = _velocity.normalized * _playerConfig.maxSpeed;

            _isMovingByKey = true;
            _inertiaTokenSource?.Cancel();
            _inertiaTokenSource = new CancellationTokenSource();
        }
        else if (_isMovingByKey)
        {
            _isMovingByKey = false;
            ApplyInertiaAsync().Forget();
        }

        if (_velocity != Vector2.zero)
        {
            transform.Translate(new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime, Space.World);
        }
    }

    private async UniTaskVoid ApplyInertiaAsync()
    {
        _inertiaTokenSource?.Dispose();
        _inertiaTokenSource = new CancellationTokenSource();
        CancellationToken token = _inertiaTokenSource.Token;

        try
        {
            float elapsedTime = 0f;
            while (elapsedTime < _playerConfig.inertiaTime && _velocity.magnitude > 0.01f)
            {
                token.ThrowIfCancellationRequested();
                _velocity *= _playerConfig.inertiaDragFactor;
                elapsedTime += Time.deltaTime;
                await UniTask.WaitForSeconds(_playerConfig.inertiaDecreaseSpeed, cancellationToken: token);
            }

            if (!_isMovingByKey)
                _velocity = Vector2.zero;
        }
        catch (Exception)
        {
        }
    }

    private void OnDestroy()
    {
        _inertiaTokenSource?.Cancel();
        _inertiaTokenSource?.Dispose();
    }
}