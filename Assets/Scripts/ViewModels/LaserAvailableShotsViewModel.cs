using System;
using Gameplay;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

public class LaserAvailableShotsViewModel : IInitializable, IDisposable
{
    [Data("Int")] [ReadOnly] public ReactiveProperty<string> LaserAvailableShots = new();

    private readonly LaserAvailableShotsStorage LaserAvailableShotsStorage;
    private const string PREFIX = "LaserAvailableShots: ";

    public LaserAvailableShotsViewModel(LaserAvailableShotsStorage laserAvailableShotsStorage)
    {
        this.LaserAvailableShotsStorage = laserAvailableShotsStorage;
    }

    public void Initialize()
    {
        this.OnLaserAvailableShotsChanged(LaserAvailableShotsStorage.LaserAvailableShots);
        this.LaserAvailableShotsStorage.OnLaserAvailableShotsChanged += OnLaserAvailableShotsChanged;
    }

    public void Dispose()
    {
        this.LaserAvailableShotsStorage.OnLaserAvailableShotsChanged -= OnLaserAvailableShotsChanged;
    }

    private void OnLaserAvailableShotsChanged(int LaserAvailableShots)
    {
        this.LaserAvailableShots.Value = PREFIX + LaserAvailableShots;
    }
}