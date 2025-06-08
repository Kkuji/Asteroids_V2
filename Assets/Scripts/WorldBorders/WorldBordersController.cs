using UnityEngine;

public class WorldBordersController : MonoBehaviour, ISpawnPositionProvider
{
    [SerializeField] private WorldBorder leftBorder, rightBorder, topBorder, bottomBorder;

    [SerializeField] private float borderThickness = 1f;
    [SerializeField] private float borderOffset = 1.5f;
    [SerializeField] private float addingDistanceAfterTeleport = 1f;
    [SerializeField] private float spawnDistanceFromBorder = 2f;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        UpdateBorderPositions();
    }

    private void OnEnable()
    {
        SetBorderEvents(true);
    }

    private void OnDisable()
    {
        SetBorderEvents(false);
    }

    public Vector3 GetRandomSpawnPositionAtBorder()
    {
        int randomBorder = Random.Range(0, 4);
        WorldBorder selectedBorder = null;
        Vector3 spawnPosition = Vector3.zero;

        float verticalSize = _mainCamera.orthographicSize;
        float horizontalSize = verticalSize * _mainCamera.aspect;

        switch (randomBorder)
        {
            case 0:
                spawnPosition = new Vector3(
                    -horizontalSize - borderOffset - spawnDistanceFromBorder,
                    Random.Range(-verticalSize, verticalSize),
                    0
                );
                break;
            case 1:
                spawnPosition = new Vector3(
                    horizontalSize + borderOffset + spawnDistanceFromBorder,
                    Random.Range(-verticalSize, verticalSize),
                    0
                );
                break;
            case 2:
                spawnPosition = new Vector3(
                    Random.Range(-horizontalSize, horizontalSize),
                    verticalSize + borderOffset + spawnDistanceFromBorder,
                    0
                );
                break;
            case 3:
                spawnPosition = new Vector3(
                    Random.Range(-horizontalSize, horizontalSize),
                    -verticalSize - borderOffset - spawnDistanceFromBorder,
                    0
                );
                break;
        }

        return spawnPosition;
    }

    private void SetBorderEvents(bool subscribe)
    {
        WorldBorder[] borders = { leftBorder, rightBorder, topBorder, bottomBorder };

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
            ? objectTransform.position.x < 0 ? -1 : 1
            : objectTransform.position.y < 0
                ? -1
                : 1;

        if (isHorizontalTeleport)
            newPosition.x = transformTeleportTo.position.x + sign * addingDistanceAfterTeleport;
        else
            newPosition.y = transformTeleportTo.position.y + sign * addingDistanceAfterTeleport;

        objectTransform.position = newPosition;
    }

    private void UpdateBorderPositions()
    {
        if (_mainCamera == null || !_mainCamera.orthographic)
            return;

        float verticalSize = _mainCamera.orthographicSize;
        float horizontalSize = verticalSize * _mainCamera.aspect;

        leftBorder.transform.position = new Vector3(-horizontalSize - borderOffset, 0, 0);
        leftBorder.transform.localScale =
            new Vector3(borderThickness, verticalSize * 2 + borderThickness, borderThickness);

        rightBorder.transform.position = new Vector3(horizontalSize + borderOffset, 0, 0);
        rightBorder.transform.localScale =
            new Vector3(borderThickness, verticalSize * 2 + borderThickness, borderThickness);

        topBorder.transform.position = new Vector3(0, verticalSize + borderOffset, 0);
        topBorder.transform.localScale =
            new Vector3(horizontalSize * 2 + borderThickness, borderThickness, borderThickness);

        bottomBorder.transform.position = new Vector3(0, -verticalSize - borderOffset, 0);
        bottomBorder.transform.localScale =
            new Vector3(horizontalSize * 2 + borderThickness, borderThickness, borderThickness);
    }
}