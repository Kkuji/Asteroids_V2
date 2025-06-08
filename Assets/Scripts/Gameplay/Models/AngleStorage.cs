using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class AngleStorage
    {
        public event Action<Vector3> OnAngleChanged;

        [ReadOnly] public UnityEngine.Vector3 Angle { get; private set; }

        [Button]
        public void SetAngle(Vector3 position)
        {
            this.Angle = position;
            this.OnAngleChanged?.Invoke(this.Angle);
        }
    }
}