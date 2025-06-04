using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Factory : MonoBehaviour
{
    [SerializeField] private List<Enums.SpawnType> typeKeys;
    [SerializeField] private List<GameObject> prefabValues;

    [Inject] private DiContainer _container;

    private Dictionary<Enums.SpawnType, GameObject> _prefabsByType;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _prefabsByType = new Dictionary<Enums.SpawnType, GameObject>();

        for (int i = 0; i < Mathf.Min(typeKeys.Count, prefabValues.Count); i++)
            _prefabsByType[typeKeys[i]] = prefabValues[i];
    }

    public Func<IPoolable> GetCreatorForType(Enums.SpawnType spawnType, Vector3 position)
    {
        return () =>
        {
            if (!_prefabsByType.TryGetValue(spawnType, out GameObject prefab))
            {
                Debug.LogError($"No prefab for {spawnType}");
                return null;
            }

            GameObject createdObject = _container.InstantiatePrefab(
                prefab,
                position,
                Quaternion.identity,
                transform
            );

            return createdObject.GetComponent<IPoolable>();
        };
    }
}