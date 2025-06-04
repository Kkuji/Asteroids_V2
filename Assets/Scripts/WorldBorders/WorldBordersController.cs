using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WorldBordersController : MonoBehaviour
{
    [SerializeField] private WorldBorder leftWorldBorder, rightWorldBorder, topWorldBorder, bottomWorldBorder;
    [SerializeField] private Transform shipTransform;
    [SerializeField] private float addingDistanceAfterTeleport = 1f;

    private void OnEnable()
    {
        SetBorderEvents(true);
    }

    private void OnDisable()
    {
        SetBorderEvents(false);
    }

    private void SetBorderEvents(bool subscribe)
    {
        WorldBorder[] borders = { leftWorldBorder, rightWorldBorder, topWorldBorder, bottomWorldBorder };

        foreach (WorldBorder border in borders)
        {
            if (subscribe)
                border.objectToTeleportEncounteredAction += TeleportObject;
            else
                border.objectToTeleportEncounteredAction -= TeleportObject;
        }
    }

    private void TeleportObject(
    Transform objectTransform,
    Transform transformTeleportTo,
    bool isHorizontalTeleport)
    {
        Vector3 newPosition = objectTransform.position;

        float sign = isHorizontalTeleport
            ? (objectTransform.position.x < 0 ? -1 : 1)
            : (objectTransform.position.y < 0 ? -1 : 1);

        if (isHorizontalTeleport)
            newPosition.x = transformTeleportTo.position.x + sign * addingDistanceAfterTeleport;
        else
            newPosition.y = transformTeleportTo.position.y + sign * addingDistanceAfterTeleport;

        objectTransform.position = newPosition;
    }
}