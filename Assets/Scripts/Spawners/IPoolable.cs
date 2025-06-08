using UnityEngine;

public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
    void SetStartPosition(Vector3 position);
}