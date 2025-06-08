using System;
using Gameplay;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

public class SpeedViewModel : IInitializable, IDisposable
{
    [Data("Float")] [ReadOnly] public ReactiveProperty<string> Speed = new();

    private readonly SpeedStorage speedStorage;
    private const string PREFIX = "Speed: ";

    public SpeedViewModel(SpeedStorage speedStorage)
    {
        this.speedStorage = speedStorage;
    }

    public void Initialize()
    {
        this.OnSpeedChanged(speedStorage.Speed);
        this.speedStorage.OnSpeedChanged += OnSpeedChanged;
    }

    public void Dispose()
    {
        this.speedStorage.OnSpeedChanged -= OnSpeedChanged;
    }

    private void OnSpeedChanged(float speed)
    {
        this.Speed.Value = PREFIX + speed.ToString("F2");
    }
}