using UnityEngine;
using UnityEngine.UI;

public class MobileInputService : MonoBehaviour, IInputService
{
    [SerializeField] private DirectionJoystick joystick;
    [SerializeField] private ForwardButton forwardButton;
    [SerializeField] private Button bulletButton;
    [SerializeField] private Button laserButton;

    private bool _bulletFired = false;
    private bool _laserFired = false;

    private void Start()
    {
        bulletButton.onClick.AddListener(OnBulletButtonClicked);
        laserButton.onClick.AddListener(OnLaserButtonClicked);
    }

    private void OnDestroy()
    {
        bulletButton.onClick.RemoveListener(OnBulletButtonClicked);
        laserButton.onClick.RemoveListener(OnLaserButtonClicked);
    }

    public float GetRotationInput()
    {
        return joystick.Horizontal;
    }

    public float GetForwardInput()
    {
        return forwardButton.IsPressed ? 1f : 0f;
    }

    public bool IsFireBulletPressed()
    {
        if (_bulletFired)
        {
            _bulletFired = false;
            return true;
        }

        return false;
    }

    public bool IsFireLaserPressed()
    {
        if (_laserFired)
        {
            _laserFired = false;
            return true;
        }

        return false;
    }

    private void OnBulletButtonClicked()
    {
        _bulletFired = true;
    }

    private void OnLaserButtonClicked()
    {
        _laserFired = true;
    }
}