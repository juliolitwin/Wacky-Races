using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class GameService
{
    private Transform _canvas;
    private Transform _objects;

    private IDictionary<long, Monster> _monsters = new Dictionary<long, Monster>();
    private IDictionary<Monster, float> _monstersToRemove = new ConcurrentDictionary<Monster, float>();
    private long _lastMonsterId = 0;

    public GameService(PoolService poolService, GameView gameView)
    {
        ObjectPool = poolService;
        GameView = gameView;
    }

    public PoolService ObjectPool { get; }

    public GameView GameView { get; }

    public void Initialization()
    {
        LoadComponents();

        for (var i = 0; i < 20; i++)
        {
            CreateMonster();
        }
    }

    public void Update()
    {
        MonsterRemoveProcess();
    }

    public void CreateMonster()
    {
        var monsterObject = ObjectPool.Monster.Get();
        var monster = monsterObject.GetComponent<Monster>();
        monster.transform.SetParent(_objects, false);
        monster.transform.localScale = MonsterConstants.DefaultScale;

        var id = GenerateId();

        var startTransform = CameraUtilities.GetStartPosition().x + ((monster.SpriteWidth + 2) / 2);

        var movementSpeed = Random.Range(2f, 10f);
        var bodyHue = Random.Range(0f, 1f);
        var eyeHue = Random.Range(0f, 1f);
        var bodyShade = Random.Range(0f, 1f);
        var startPosition = new Vector3(startTransform, 0f, 0f);
        var isRare = Random.Range(0, 100) >= 90;
        monster.Initialization(id, movementSpeed, bodyHue, eyeHue, bodyShade, startPosition, isRare);
        monster.OutEvent += OnMonsterOut;

        _monsters.Add(id, monster);
    }

    public bool IsOut(Monster monster)
    {
        var calculatedOut = monster.transform.position.x - monster.SpriteWidth / 2;
        return calculatedOut > CameraUtilities.GetEndPosition().x;
    }

    private void MonsterRemoveProcess()
    {
        if (_monstersToRemove.Count == 0)
            return;

        var monstersToRemove = new List<Monster>();

        // Primeiro, itere e identifique quais monstros precisam ser removidos
        foreach (var pair in _monstersToRemove)
        {
            _monstersToRemove[pair.Key] -= Time.deltaTime;
            if (_monstersToRemove[pair.Key] <= 0)
            {
                monstersToRemove.Add(pair.Key);
            }
        }

        // Em seguida, remova os monstros fora do loop de iteração
        foreach (var monster in monstersToRemove)
        {
            RemoveMonster(monster);
            _monstersToRemove.Remove(monster);
        }
    }

    private long GenerateId()
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
    }

    private void LoadComponents()
    {
        _canvas = GameObject.Find("Canvas").transform;
        _objects = GameObject.Find("Objects").transform;
        GameView.transform.SetParent(_canvas.transform);
    }
}
