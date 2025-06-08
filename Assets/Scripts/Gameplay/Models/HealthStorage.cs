using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class HealthStorage
    {
        public event Action<int> OnHealthChanged;

        [ReadOnly] public int Health { get; private set; }

        [Button]
        public void SetHealth(int health)
        {
            this.Health = health;
            this.OnHealthChanged?.Invoke(this.Health);
        }
    }
}