using System;
using Gameplay;
using MVVM;
using Sirenix.OdinInspector;
using UniRx;
using Zenject;

public class LaserAvailableShotsViewModel : IInitializable, IDisposable
{
    private const string PREFIX = "Laser available shots: ";

    private readonly LaserAvailableShotsStorage LaserAvailableShotsStorage;
    [Data("Int")] [ReadOnly] public ReactiveProperty<string> LaserAvailableShots = new();

    public LaserAvailableShotsViewModel(LaserAvailableShotsStorage laserAvailableShotsStorage)
    {
        LaserAvailableShotsStorage = laserAvailableShotsStorage;
    }

    public void Dispose()
    {
        LaserAvailableShotsStorage.OnLaserAvailableShotsChanged -= OnLaserAvailableShotsChanged;
    }

    public void Initialize()
    {
        OnLaserAvailableShotsChanged(LaserAvailableShotsStorage.LaserAvailableShots);
        LaserAvailableShotsStorage.OnLaserAvailableShotsChanged += OnLaserAvailableShotsChanged;
    }

    private void OnLaserAvailableShotsChanged(int LaserAvailableShots)
    {
        this.LaserAvailableShots.Value = PREFIX + LaserAvailableShots;
    }
}