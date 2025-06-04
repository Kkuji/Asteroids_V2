using UnityEngine;

namespace Managment
{
    public class KeyboardInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const string Bullet = "Bullet";
        private const string Laser = "Laser";

        public float GetRotationInput() => Input.GetAxis(Horizontal);
        public float GetForwardInput() => Input.GetAxis(Vertical);
        public bool IsFireBulletPressed() => Input.GetButtonDown(Bullet);
        public bool IsFireLaserPressed() => Input.GetButtonDown(Laser);
    }
}