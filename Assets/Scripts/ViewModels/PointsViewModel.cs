using System;
using Gameplay;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

public class PointsViewModel : IInitializable, IDisposable
{
    [Data("Int")] [ReadOnly] public ReactiveProperty<string> Points = new();

    private readonly PointsStorage pointsStorage;
    private const string PREFIX = "Points: ";

    public PointsViewModel(PointsStorage pointsStorage)
    {
        this.pointsStorage = pointsStorage;
    }

    public void Initialize()
    {
        this.OnPointsChanged(pointsStorage.Points);
        this.pointsStorage.OnPointsChanged += OnPointsChanged;
    }

    public void Dispose()
    {
        this.pointsStorage.OnPointsChanged -= OnPointsChanged;
    }

    private void OnPointsChanged(int points)
    {
        this.Points.Value = PREFIX + points;
    }
}