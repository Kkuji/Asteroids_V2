using System;
using Gameplay;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

public class HealthViewModel : IInitializable, IDisposable
{
    [Data("Int")] [ReadOnly] public ReactiveProperty<string> Health = new();

    private readonly HealthStorage HealthStorage;
    private const string PREFIX = "Health: ";

    public HealthViewModel(HealthStorage HealthStorage)
    {
        this.HealthStorage = HealthStorage;
    }

    public void Initialize()
    {
        this.OnHealthChanged(HealthStorage.Health);
        this.HealthStorage.OnHealthChanged += OnHealthChanged;
    }

    public void Dispose()
    {
        this.HealthStorage.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int Health)
    {
        this.Health.Value = PREFIX + Health;
    }
}