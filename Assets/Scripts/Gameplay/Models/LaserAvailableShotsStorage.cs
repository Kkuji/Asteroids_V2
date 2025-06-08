using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class LaserAvailableShotsStorage
    {
        public event Action<int> OnLaserAvailableShotsChanged;

        [ReadOnly] public int LaserAvailableShots { get; private set; }

        [Button]
        public void SetLaserAvailableShots(int laserAvailableShots)
        {
            this.LaserAvailableShots = laserAvailableShots;
            this.OnLaserAvailableShotsChanged?.Invoke(this.LaserAvailableShots);
        }
    }
}