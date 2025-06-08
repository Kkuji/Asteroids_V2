using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class LaserCooldownStorage
    {
        public event Action<float> OnCooldownChanged;

        [ReadOnly] public float Cooldown { get; private set; }

        [Button]
        public void SetCooldown(float cooldown)
        {
            this.Cooldown = cooldown;
            this.OnCooldownChanged?.Invoke(this.Cooldown);
        }
    }
}