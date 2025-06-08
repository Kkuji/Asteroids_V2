using System;
using Gameplay;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

public class LaserCooldownViewModel : IInitializable, IDisposable
{
    [Data("Float")] [ReadOnly] public ReactiveProperty<string> Cooldown = new();

    private readonly LaserCooldownStorage laserCooldownStorage;
    private const string PREFIX = "Next laser shot in: ";

    public LaserCooldownViewModel(LaserCooldownStorage laserCooldownStorage)
    {
        this.laserCooldownStorage = laserCooldownStorage;
    }

    public void Initialize()
    {
        this.OnLaserCooldownChanged(laserCooldownStorage.Cooldown);
        this.laserCooldownStorage.OnCooldownChanged += OnLaserCooldownChanged;
    }

    public void Dispose()
    {
        this.laserCooldownStorage.OnCooldownChanged -= OnLaserCooldownChanged;
    }

    private void OnLaserCooldownChanged(float cooldown)
    {
        this.Cooldown.Value = PREFIX + cooldown.ToString("F2");
    }
}