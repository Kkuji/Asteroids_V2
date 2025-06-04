using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class MyObjectPool<T> where T : IPoolable
{
    private Dictionary<Enum, Queue<T>> _pools = new();
    private Dictionary<Enum, Func<T>> _createFuncs = new();
    private Dictionary<Enum, int> _involvedObjectsAmount = new();

    public int InvolvedObjectsAmount(Enums.SpawnType spawnType) => _involvedObjectsAmount[spawnType];

    public void RegisterPool(
    Enum objectType,
    Func<T> createFunc,
    int startAmount)
    {
        if (_pools.ContainsKey(objectType))
            return;

        _pools[objectType] = new Queue<T>();
        _createFuncs[objectType] = createFunc;
        _involvedObjectsAmount[objectType] = 0;

        for (int i = 0; i < startAmount; i++)
        {
            T obj = createFunc();
            obj.OnDespawn();
            _pools[objectType].Enqueue(obj);
        }
    }

    public T Get(Enums.SpawnType objectType)
    {
        T obj;
        obj = _pools[objectType].Count == 0 ? _createFuncs[objectType]() : _pools[objectType].Dequeue();
        obj.OnSpawn();
        _involvedObjectsAmount[objectType]++;

        return obj;
    }

    public void Release(Enums.SpawnType objectType, T obj)
    {
        obj.OnDespawn();
        _pools[objectType].Enqueue(obj);
        _involvedObjectsAmount[objectType]--;
    }
}