using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LaserController : MonoBehaviour
{
    [SerializeField] private GameObject laserObject;

    private float _laserAppearDuration;
    private int _maxShots;
    private float _shotRechargeTime;

    private int _currentShots;
    private bool _isFiring;
    private CancellationTokenSource _fireTokenSource;
    private bool _isRecharging;
    private float _nextRechargeEndTime;
    public float TimeUntilNextShot => _currentShots < _maxShots ? Mathf.Max(0f, _nextRechargeEndTime - Time.time) : 0f;
    private Dictionary<Type, int> _scoreByEnemyType;
    private UIInfo _uiInfo;
    private EnemyConfig _enemyConfig;

    [Inject]
    public void Initialize(ConfigService configService, UIInfo uiInfo)
    {
        var worldConfig = configService.gameConfig.worldConfig;
        _enemyConfig = configService.gameConfig.enemyConfig;
        _uiInfo = uiInfo;
        _laserAppearDuration = worldConfig.laserAppearDuration;
        _maxShots = worldConfig.maxShots;
        _shotRechargeTime = worldConfig.shotRechargeTime;
        _currentShots = _maxShots;
    }

    public bool CanFire => _currentShots > 0 && !_isFiring;
    public int CurrentShots => _currentShots;

    private void Awake()
    {
        _scoreByEnemyType = new Dictionary<Type, int>
        {
            { typeof(EnemyAsteroid), _enemyConfig.scoreAsteroid },
            { typeof(EnemyAsteroidSmall), _enemyConfig.scoreAsteroidSmall },
            { typeof(EnemyShip), _enemyConfig.scoreEnemyShip }
        };
    }

    public async UniTaskVoid FireAsync()
    {
        if (!CanFire)
            return;

        _currentShots--;
        StartRecharge();

        _fireTokenSource?.Cancel();
        _fireTokenSource?.Dispose();
        _fireTokenSource = new CancellationTokenSource();
        CancellationToken token = _fireTokenSource.Token;

        try
        {
            _isFiring = true;
            laserObject.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(_laserAppearDuration), cancellationToken: token);

            laserObject.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            laserObject.SetActive(false);
        }
        finally
        {
            _isFiring = false;
        }
    }

    private void StartRecharge()
    {
        if (_isRecharging || _currentShots >= _maxShots)
            return;

        _isRecharging = true;
        UniTask.Void(async () =>
        {
            try
            {
                while (_currentShots < _maxShots)
                {
                    _nextRechargeEndTime = Time.time + _shotRechargeTime;
                    await UniTask.Delay(TimeSpan.FromSeconds(_shotRechargeTime));
                    _currentShots = Math.Min(_currentShots + 1, _maxShots);
                }
            }
            finally
            {
                _isRecharging = false;
            }
        });
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out EnemyBase enemyBase))
        {
            enemyBase.Damaged();
            var type = enemyBase.GetType();

            if (_scoreByEnemyType.TryGetValue(type, out int score))
                _uiInfo.AddPoints(score);
        }
    }

    private void OnDestroy()
    {
        _fireTokenSource?.Cancel();
        _fireTokenSource?.Dispose();
    }
}