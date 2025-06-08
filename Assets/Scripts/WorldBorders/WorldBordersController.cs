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
        float verticalSize = _mainCamera.orthographicSize;
        float horizontalSize = verticalSize * _mainCamera.aspect;

        return randomBorder switch
        {
            0 => new Vector3(
                -horizontalSize - borderOffset - spawnDistanceFromBorder,
                Random.Range(-verticalSize, verticalSize),
                0
            ),
            1 => new Vector3(
                horizontalSize + borderOffset + spawnDistanceFromBorder,
                Random.Range(-verticalSize, verticalSize),
                0
            ),
            2 => new Vector3(
                Random.Range(-horizontalSize, horizontalSize),
                verticalSize + borderOffset + spawnDistanceFromBorder,
                0
            ),
            3 => new Vector3(
                Random.Range(-horizontalSize, horizontalSize),
                -verticalSize - borderOffset - spawnDistanceFromBorder,
                0
            ),
            _ => Vector3.zero
        };
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
        float verticalSize = _mainCamera.orthographicSize;
        float horizontalSize = verticalSize * _mainCamera.aspect;

        SetBorderPosition(leftBorder, new Vector3(-horizontalSize - borderOffset, 0, 0),
            new Vector3(borderThickness, verticalSize * 2 + borderThickness, borderThickness));

        SetBorderPosition(rightBorder, new Vector3(horizontalSize + borderOffset, 0, 0),
            new Vector3(borderThickness, verticalSize * 2 + borderThickness, borderThickness));

        SetBorderPosition(topBorder, new Vector3(0, verticalSize + borderOffset, 0),
            new Vector3(horizontalSize * 2 + borderThickness, borderThickness, borderThickness));

        SetBorderPosition(bottomBorder, new Vector3(0, -verticalSize - borderOffset, 0),
            new Vector3(horizontalSize * 2 + borderThickness, borderThickness, borderThickness));
    }

    private void SetBorderPosition(WorldBorder border, Vector3 position, Vector3 scale)
    {
        border.transform.position = position;
        border.transform.localScale = scale;
    }
}