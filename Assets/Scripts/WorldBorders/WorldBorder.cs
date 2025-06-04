using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class WorldBorder : MonoBehaviour
{
    public event Action<Transform, Transform, bool> objectToTeleportEncounteredAction;

    [SerializeField] private Transform transformTeleportTo;
    [SerializeField] private bool isHorizontalTeleport;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManagerSystem playerInputSystem)
            || collision.gameObject.TryGetComponent(out EnemyBase enemyBase))
            objectToTeleportEncounteredAction?.Invoke(collision.transform, transformTeleportTo, isHorizontalTeleport);
    }
}