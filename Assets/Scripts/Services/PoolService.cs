using UnityEngine;
using UnityEngine.Pool;

public class PoolService
{
    private const bool CollectionCheck = false;
    private const int DefaultCapacity = 10;
    private const int MaxSize = 10000;

    private GameObject _monsterObject;

    public PoolService()
    {
        Monster = new ObjectPool<GameObject>(
            CreateMonsterPooled, OnMonsterTakeFromPool, OnMonsterReturnedToPool, OnMonsterDestroyPoolObject,
            CollectionCheck, DefaultCapacity, MaxSize);

        _monsterObject = Resources.Load<GameObject>(ResourcesConstants.Monster);
    }

    public IObjectPool<GameObject> Monster { get; }

    private GameObject CreateMonsterPooled()
    {
        return GameObject.Instantiate(_monsterObject);
    }

    private void OnMonsterTakeFromPool(GameObject pooledGameObject)
    {
        pooledGameObject.SetActive(true);
    }

    private void OnMonsterReturnedToPool(GameObject pooledGameObject)
    {
        pooledGameObject.SetActive(false);
    }

    private void OnMonsterDestroyPoolObject(GameObject pooledGameObject)
    {
        GameObject.Destroy(pooledGameObject);
    }
}