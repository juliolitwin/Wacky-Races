using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityService
{
    private Transform _entitiesTransform;

    private IDictionary<long, Monster> _monsters = new Dictionary<long, Monster>();
    private IDictionary<long, float> _monstersToRemove = new Dictionary<long, float>();
    private IList<long> _temporaryMonstersToRemove = new List<long>();

    public float[] _spawnHeights;

    private RoundFinishDelegate OnRoundFinish;

    public EntityService(PoolService poolService)
    {
        ObjectPool = poolService;
    }

    public PoolService ObjectPool { get; }

    public long LastMonsterId { get; private set; }
    public int CurrentCount { get; private set; }

    public void Initialization(float[] spawnHeights, RoundFinishDelegate onFinish)
    {
        LoadComponents();
        _spawnHeights = spawnHeights;
        OnRoundFinish = onFinish;
    }

    public void Clear()
    {
        LastMonsterId = 0;
        CurrentCount = 0;

        _monstersToRemove.Clear();

        foreach (var pair in _monsters)
        {
            var monster = pair.Value;
            RemoveMonster(monster);
        }

        _monsters.Clear();
    }

    public void Spawn(int monstersToSpawn)
    {
        CurrentCount = monstersToSpawn;
        CreateMonsters(monstersToSpawn);
    }

    public void Fire()
    {
        foreach (var pair in _monsters)
        {
            var monster = pair.Value;
            monster.ChangeState(EntityState.Run);
        }
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
        monster.transform.SetParent(_entitiesTransform, false);
        monster.transform.localScale = MonsterConstants.DefaultScale;

        var id = GenerateEntityId();

        //var movementSpeed = 500f;
        var movementSpeed = Random.Range(1f, 15f);
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
        return LastMonsterId++;
    }

    private Monster GetMonster(long monsterId)
    {
        if (_monsters.TryGetValue(monsterId, out var monster))
        {
            return monster;
        }

        return null;
    }

    private void RemoveMonster(Monster monster)
    {
        _monsters.Remove(monster.Id);
        ObjectPool.Monster.Release(monster.gameObject);
    }

    private void OnMonsterOut(Monster monster)
    {
        if (!_monstersToRemove.ContainsKey(monster.Id))
            _monstersToRemove.Add(monster.Id, 1f);

        CurrentCount--;
    }

    private void LoadComponents()
    {
        _entitiesTransform = GameObject.Find("Entities").transform;
    }

    private void MonsterRemoveProcess()
    {
        if (_monstersToRemove.Count == 0)
            return;

        _temporaryMonstersToRemove.Clear();

        var monsters = _monstersToRemove.ToList();

        for (int i = 0; i < monsters.Count; i++)
        {
            var pair = monsters[i];
            _monstersToRemove[pair.Key] -= Time.deltaTime;
            if (_monstersToRemove[pair.Key] <= 0)
            {
                _temporaryMonstersToRemove.Add(pair.Key);
            }
        }

        for (var i = 0; i < _temporaryMonstersToRemove.Count; i++)
        {
            var monsterId = _temporaryMonstersToRemove[i];
            var monster = GetMonster(monsterId);
            if (monster == null)
                continue;

            RemoveMonster(monster);
            _monstersToRemove.Remove(monsterId);
        }

        if (_monsters.Count == 0)
            OnRoundFinish();
    }
}
