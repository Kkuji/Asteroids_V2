using UnityEngine;
using UnityEngine.UI;

public class MobileInputService : MonoBehaviour, IInputService
{
    [SerializeField] private DirectionJoystick joystick;
    [SerializeField] private ForwardButton forwardButton;
    [SerializeField] private Button bulletButton;
    [SerializeField] private Button laserButton;

    public float GetRotationInput() => joystick.Horizontal;
    public float GetForwardInput() => forwardButton.IsPressed ? 1f : 0f;
    public bool IsFireBulletPressed() => bulletButton.onClick != null;
    public bool IsFireLaserPressed() => laserButton.onClick != null;
}