using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class GameService
{
    private Transform _canvas;
    private Transform _environments;
    private Transform _objects;

    private IDictionary<long, Monster> _monsters = new Dictionary<long, Monster>();
    private IDictionary<Monster, float> _monstersToRemove = new ConcurrentDictionary<Monster, float>();
    private long _lastMonsterId = 0;
    private SpriteRenderer _backgroundRenderer;

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

        var lines = GetYPositionsForLines(3, GetBackgroundHeight(), GetBackgroundPosition());

        for (var i = 0; i < 20; i++)
        {
            CreateMonster(lines);
        }
    }

    public void Update()
    {
        MonsterRemoveProcess();
    }

    public void CreateMonster(float[] lines)
    {
        var monsterObject = ObjectPool.Monster.Get();
        var monster = monsterObject.GetComponent<Monster>();
        monster.transform.SetParent(_objects, false);
        monster.transform.localScale = MonsterConstants.DefaultScale;

        var id = GenerateId();

        var movementSpeed = 100f;
        //var movementSpeed = Random.Range(2f, 10f);
        var bodyHue = Random.Range(0f, 1f);
        var eyeHue = Random.Range(0f, 1f);
        var bodyShade = Random.Range(0f, 1f);
        var isRare = Random.Range(0, 100) >= 90;

        var startTransform = CameraUtilities.GetStartPosition().x + ((monster.SpriteWidth + 2) / 2);
        var spawnPosition = new Vector3(startTransform, lines[Random.Range(0, lines.Length)], 0);
        monster.Initialization(id, movementSpeed, bodyHue, eyeHue, bodyShade, spawnPosition, isRare);
        monster.OutEvent += OnMonsterOut;

        _monsters.Add(id, monster);
    }

    public bool IsOut(Monster monster)
    {
        var calculatedOut = monster.transform.position.x - monster.SpriteWidth / 2;
        return calculatedOut > CameraUtilities.GetEndPosition().x;
    }

    private float[] GetYPositionsForLines(int numberOfLines)
    {
        var screenHeight = CameraUtilities.CalculateScreenHeightInWorldUnits();
        var yPositions = new float[numberOfLines];

        for (var i = 0; i < numberOfLines; i++)
        {
            yPositions[i] = -screenHeight / 2 + screenHeight / (numberOfLines + 1) * (i + 1);
        }

        return yPositions;
    }

    public float[] GetYPositionsForLines(int numberOfLines, float backgroundHeight, Vector2 backgroundPosition)
    {
        var yPositions = new float[numberOfLines];
        var bottomEdge = backgroundPosition.y - backgroundHeight / 2;
        var topEdge = backgroundPosition.y + backgroundHeight / 2;

        for (int i = 0; i < numberOfLines; i++)
        {
            yPositions[i] = bottomEdge + backgroundHeight / (numberOfLines + 1) * (i + 1);
        }

        return yPositions;
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
        _environments = GameObject.Find("Environments").transform;
        _objects = GameObject.Find("Objects").transform;
        GameView.transform.SetParent(_canvas.transform);

        _backgroundRenderer = _environments.transform.Find("Road").GetComponent<SpriteRenderer>();
    }

    private float GetBackgroundHeight()
    {
        UnityEngine.Debug.Log($"{_backgroundRenderer.bounds.size.y}");
        return _backgroundRenderer.bounds.size.y;
    }

    private Vector2 GetBackgroundPosition()
    {
        return new Vector2(_backgroundRenderer.transform.position.x, _backgroundRenderer.transform.position.y);
    }
}
