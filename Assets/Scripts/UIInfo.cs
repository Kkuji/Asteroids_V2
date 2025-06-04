using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.SpaceFighter;

public class UIInfo : MonoBehaviour
{
    private const string START_POINTS = "0";
    private int _currentPoints;

    [SerializeField] private TMP_Text playerPositionText;
    [SerializeField] private TMP_Text playerAngleText;
    [SerializeField] private TMP_Text playerSpeedText;
    [SerializeField] private TMP_Text laserAvailableShotsAmountText;
    [SerializeField] private TMP_Text laserCooldownText;
    [SerializeField] private TMP_Text userPointsText;
    [SerializeField] private TMP_Text userHealthText;

    private PlayerManagerSystem _playerManagerSystem;
    private LaserController _laserController;

    [Inject]
    private void Init(PlayerManagerSystem playerManagerSystem, LaserController laserController)
    {
        _playerManagerSystem = playerManagerSystem;
        _laserController = laserController;
        _playerManagerSystem.HealthPointsChangedAction += SetHealthPoints;
    }

    private void Awake()
    {
        userPointsText.text = "Points: " + START_POINTS;
    }

    private void FixedUpdate()
    {
        playerPositionText.text = "Player position: " + _playerManagerSystem.gameObject.transform.position;
        playerAngleText.text = "Player angle: " + _playerManagerSystem.gameObject.transform.rotation.eulerAngles.z;
        playerSpeedText.text = "Player speed: " + _playerManagerSystem.Velocity.magnitude;
        laserAvailableShotsAmountText.text = "Laser shots available: " + _laserController.CurrentShots;
        laserCooldownText.text = "Next laser shot in: " + _laserController.TimeUntilNextShot.ToString("F1") + "s";
    }

    public void AddPoints(int value)
    {
        _currentPoints += value;
        userPointsText.text = "Points: " + _currentPoints;
    }

    public void SetHealthPoints(int value)
    {
        userHealthText.text = "Health: " + value;
    }

    private void OnDestroy()
    {
        _playerManagerSystem.HealthPointsChangedAction -= SetHealthPoints;
    }
}