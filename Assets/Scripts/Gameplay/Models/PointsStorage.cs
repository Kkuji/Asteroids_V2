using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class PointsStorage
    {
        public event Action<int> OnPointsChanged;

        [ReadOnly] public int Points { get; private set; }

        [Button]
        public void AddPoints(int points)
        {
            this.Points += points;
            this.OnPointsChanged?.Invoke(this.Points);
        }
    }
}