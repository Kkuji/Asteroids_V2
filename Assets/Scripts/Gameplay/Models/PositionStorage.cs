using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class PositionStorage
    {
        public event Action<Vector3> OnPositionChanged;

        [ReadOnly] public UnityEngine.Vector3 Position { get; private set; }

        [Button]
        public void SetPosition(Vector3 position)
        {
            this.Position = position;
            this.OnPositionChanged?.Invoke(this.Position);
        }
    }
}