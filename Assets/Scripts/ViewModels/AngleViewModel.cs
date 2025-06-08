using System;
using Gameplay;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

public class AngleViewModel : IInitializable, IDisposable
{
    [Data("Vector3")] [ReadOnly] public ReactiveProperty<string> Angle = new();

    private readonly AngleStorage angleStorage;
    private const string PREFIX = "Angle: ";

    public AngleViewModel(AngleStorage angleStorage)
    {
        this.angleStorage = angleStorage;
    }

    public void Initialize()
    {
        this.OnAngleChanged(angleStorage.Angle);
        this.angleStorage.OnAngleChanged += OnAngleChanged;
    }

    public void Dispose()
    {
        this.angleStorage.OnAngleChanged -= OnAngleChanged;
    }

    private void OnAngleChanged(Vector3 angle)
    {
        this.Angle.Value = PREFIX + angle.ToString();
    }
}