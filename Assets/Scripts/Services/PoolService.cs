using UnityEngine;
using UnityEngine.Pool;

public class PoolService
{
    // Configuration constants for the object pool.
    private const bool CollectionCheck = false; // Disables collection checks for performance optimization.
    private const int DefaultCapacity = 1000;  // Default capacity of the pool.
    private const int MaxSize = 20000;         // Maximum size of the pool.

    // Reference to the prefab used for creating new monster instances.
    private GameObject _monsterObject;

    public PoolService()
    {
        // Initialize the Monster pool with specified create, get, return, and destroy actions.
        Monster = new ObjectPool<GameObject>(
            CreateMonsterPooled, OnMonsterTakeFromPool, OnMonsterReturnedToPool, OnMonsterDestroyPoolObject,
            CollectionCheck, DefaultCapacity, MaxSize);

        // Load the monster prefab from resources.
        _monsterObject = Resources.Load<GameObject>(ResourcesConstants.Monster);
    }

    // Publicly accessible object pool for Monsters.
    public IObjectPool<GameObject> Monster { get; }

    /// <summary>
    /// Creates a new monster instance for the pool.
    /// </summary>
    /// <returns>A new monster GameObject.</returns>
    private GameObject CreateMonsterPooled()
    {
        // Instantiate a new monster object from the prefab.
        return GameObject.Instantiate(_monsterObject);
    }

    /// <summary>
    /// Action to perform when a monster is taken from the pool.
    /// </summary>
    /// <param name="pooledGameObject">The pooled monster GameObject.</param>
    private void OnMonsterTakeFromPool(GameObject pooledGameObject)
    {
        // Activate the monster when it is taken from the pool.
        pooledGameObject.SetActive(true);
    }

    /// <summary>
    /// Action to perform when a monster is returned to the pool.
    /// </summary>
    /// <param name="pooledGameObject">The pooled monster GameObject.</param>
    private void OnMonsterReturnedToPool(GameObject pooledGameObject)
    {
        // Deactivate the monster when it is returned to the pool.
        pooledGameObject.SetActive(false);
    }

    /// <summary>
    /// Action to perform when a monster is permanently removed from the pool.
    /// </summary>
    /// <param name="pooledGameObject">The pooled monster GameObject.</param>
    private void OnMonsterDestroyPoolObject(GameObject pooledGameObject)
    {
        // Destroy the monster GameObject.
        GameObject.Destroy(pooledGameObject);
    }
}
