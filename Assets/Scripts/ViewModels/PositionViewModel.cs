using System;
using Gameplay;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using MVVM;

namespace UI.ViewModels
{
    public class PositionViewModel : IInitializable, IDisposable
    {
        [Data("Vector3")] [ReadOnly] public ReactiveProperty<string> Position = new();

        private readonly PositionStorage positionStorage;
        private const string PREFIX = "Position: ";

        public PositionViewModel(PositionStorage positionStorage)
        {
            this.positionStorage = positionStorage;
        }

        public void Initialize()
        {
            this.OnPositionChanged(positionStorage.Position);
            this.positionStorage.OnPositionChanged += OnPositionChanged;
        }

        public void Dispose()
        {
            this.positionStorage.OnPositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged(Vector3 position)
        {
            this.Position.Value = PREFIX + position.ToString();
        }
    }
}