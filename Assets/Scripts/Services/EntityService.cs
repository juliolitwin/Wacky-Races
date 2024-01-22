using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class EntityService
{
    private Transform _entities;

    private IDictionary<long, Monster> _monsters = new Dictionary<long, Monster>();
    private IDictionary<Monster, float> _monstersToRemove = new ConcurrentDictionary<Monster, float>();

    private long _lastMonsterId = 0;
    private int _currentCount = 0;

    public float[] _spawnHeights;

    public EntityService(PoolService poolService)
    {
        ObjectPool = poolService;
    }

    public PoolService ObjectPool { get; }

    public void Initialization(float[] spawnHeights)
    {
        LoadComponents();
        _spawnHeights = spawnHeights;
    }

    public void Spawn(int monstersToSpawn)
    {
        _currentCount = monstersToSpawn;
        CreateMonsters(monstersToSpawn);
    }

    public void CreateMonsters(int monstersToSpawn)
    {
        if (monstersToSpawn <= 0)
            return;

        CreateMonster();

        // Recursive function.
        CreateMonsters(monstersToSpawn - 1);
    }

    public void Update()
    {
        MonsterRemoveProcess();
    }

    public void CreateMonster()
    {
        var monsterObject = ObjectPool.Monster.Get();
        var monster = monsterObject.GetComponent<Monster>();
        monster.transform.SetParent(_entities, false);
        monster.transform.localScale = MonsterConstants.DefaultScale;

        var id = GenerateEntityId();

        //var movementSpeed = 500f;
        var movementSpeed = Random.Range(2f, 10f);
        var bodyHue = Random.Range(0f, 1f);
        var eyeHue = Random.Range(0f, 1f);
        var bodyShade = Random.Range(0f, 1f);
        var isRare = Random.Range(0, 100) >= 90;

        var startTransform = CameraUtilities.GetStartPosition().x + (monster.SpriteWidth / 2);

        var rndLine = Random.Range(0, _spawnHeights.Length);
        var currentLine = _spawnHeights[rndLine];

        var spawnPosition = new Vector3(startTransform, currentLine, 0);

        var sortLayer = (_spawnHeights.Length - 1) - rndLine;

        monster.Initialization(id, movementSpeed, sortLayer, bodyHue, eyeHue, bodyShade, spawnPosition, isRare);
        monster.OutEvent += OnMonsterOut;

        _monsters.Add(id, monster);
    }

    private long GenerateEntityId()
    {
        return _lastMonsterId++;
    }

    private void RemoveMonster(Monster monster)
    {
        _monsters.Remove(monster.Id);
        ObjectPool.Monster.Release(monster.gameObject);
    }

    private void OnMonsterOut(Monster monster)
    {
        _monstersToRemove.Add(monster, 1f);

        _currentCount--;
        if (_currentCount <= 0)
        {
            //EndRound();
        }
    }

    private void LoadComponents()
    {
        _entities = GameObject.Find("Entities").transform;
    }

    private void MonsterRemoveProcess()
    {
        if (_monstersToRemove.Count == 0)
            return;

        var monstersToRemove = new List<Monster>();

        foreach (var pair in _monstersToRemove)
        {
            _monstersToRemove[pair.Key] -= Time.deltaTime;
            if (_monstersToRemove[pair.Key] <= 0)
            {
                monstersToRemove.Add(pair.Key);
            }
        }

        foreach (var monster in monstersToRemove)
        {
            RemoveMonster(monster);
            _monstersToRemove.Remove(monster);
        }
    }
}
