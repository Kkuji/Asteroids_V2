using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class SpeedStorage
    {
        public event Action<float> OnSpeedChanged;

        [ReadOnly] public float Speed { get; private set; }

        [Button]
        public void SetSpeed(float position)
        {
            this.Speed = position;
            this.OnSpeedChanged?.Invoke(this.Speed);
        }
    }
}