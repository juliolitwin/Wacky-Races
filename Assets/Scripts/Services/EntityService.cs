using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityService
{
    // Transform to hold all entity game objects.
    private Transform _entitiesTransform;

    // Dictionary to keep track of all active monsters.
    private IDictionary<long, Monster> _monsters = new Dictionary<long, Monster>();

    // Dictionary to track monsters that are marked for removal.
    private IDictionary<long, float> _monstersToRemove = new Dictionary<long, float>();

    // Temporary list to store monsters to remove in the current update cycle.
    private IList<long> _temporaryMonstersToRemove = new List<long>();

    // Array of heights where monsters can spawn.
    public float[] _spawnHeights;

    // Delegate for handling the end of a finish.
    private RoundFinishDelegate OnRoundFinish;

    // Delegate for handling the end of a end.
    private RoundEndDelegate OnRoundEnd;

    // Reference to the PoolService for managing object pooling.
    public EntityService(PoolService poolService, GameView gameView)
    {
        ObjectPool = poolService;
        GameView = gameView;
    }

    // Publicly accessible PoolService instance.
    public PoolService ObjectPool { get; }

    // Publicly accessible GameView instance.
    public GameView GameView { get; }

    // Property to track the ID of the last spawned monster.
    public long LastMonsterId { get; private set; }

    // Property to track the current number of monsters.
    public int Count { get; private set; }

    // Property to track the current number of active monsters.
    public int CurrentCount { get; private set; }

    /// <summary>
    /// Initializes the service with spawn heights and sets up the end round callback.
    /// </summary>
    /// <param name="spawnHeights">Array of heights for spawning entities.</param>
    /// <param name="onFinish">Delegate to call at the finish of a round.</param>
    /// <param name="onEnd">Delegate to call at the end of a round.</param>
    public void Initialization(float[] spawnHeights, RoundFinishDelegate onFinish, RoundEndDelegate onEnd)
    {
        LoadComponents();
        _spawnHeights = spawnHeights;

        // Set the delegates.
        OnRoundFinish = onFinish;
        OnRoundEnd = onEnd;
    }

    /// <summary>
    /// Clears all active monsters and resets relevant properties.
    /// </summary>
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

    /// <summary>
    /// Spawns a specified number of monsters.
    /// </summary>
    /// <param name="monstersToSpawn">The number of monsters to spawn.</param>
    public void Spawn(int monstersToSpawn)
    {
        Count = monstersToSpawn;
        CurrentCount = monstersToSpawn;
        CreateMonsters(monstersToSpawn);

        GameView.SetMonsters(monstersToSpawn, monstersToSpawn);
    }

    /// <summary>
    /// Triggers the run state for all spawned monsters.
    /// </summary>
    public void Fire()
    {
        foreach (var pair in _monsters)
        {
            var monster = pair.Value;
            monster.ChangeState(EntityState.Run);
        }
    }

    /// <summary>
    /// Recursively creates the specified number of monsters.
    /// </summary>
    /// <param name="monstersToSpawn">The number of monsters to spawn.</param>
    public void CreateMonsters(int monstersToSpawn)
    {
        if (monstersToSpawn <= 0)
            return;

        CreateMonster();

        // Recursive call to create the remaining monsters.
        CreateMonsters(monstersToSpawn - 1);
    }

    /// <summary>
    /// Updates the service, primarily handling monster removal.
    /// </summary>
    public void Update()
    {
        MonsterRemoveProcess();
    }

    /// <summary>
    /// Creates a single monster and adds it to the active monster list.
    /// </summary>
    public void CreateMonster()
    {
        var monsterObject = ObjectPool.Monster.Get();
        var monster = monsterObject.GetComponent<Monster>();
        monster.transform.SetParent(_entitiesTransform, false);
        monster.transform.localScale = MonsterConstants.DefaultScale;

        // Generate an unique id for the monster.
        var id = GenerateEntityId();

        // Random a movement speed for the monster.
        var movementSpeed = Random.Range(1f, 15f);

        // Random hue and shade color for the monster.
        var bodyHue = Random.Range(0f, 1f);
        var eyeHue = Random.Range(0f, 1f);
        var bodyShade = Random.Range(0f, 1f);

        // Special effect for the monster.
        var isRare = Random.Range(0, 100) >= 90;

        var startTransform = CameraUtilities.GetStartPosition().x + (monster.SpriteWidth / 2);
        var rndLine = Random.Range(0, _spawnHeights.Length);
        var currentLine = _spawnHeights[rndLine];
        var spawnPosition = new Vector3(startTransform, currentLine, 0);

        // Calcualte the sort layer for rendering in the road.
        var sortLayer = (_spawnHeights.Length - 1) - rndLine;

        // Init the monster with the values.
        monster.Initialization(id, movementSpeed, sortLayer, bodyHue, eyeHue, bodyShade, spawnPosition, isRare);
        monster.OutEvent += OnMonsterOut;

        _monsters.Add(id, monster);
    }

    /// <summary>
    /// Generates a unique ID for each entity.
    /// </summary>
    /// <returns>A long representing the entity's ID.</returns>
    private long GenerateEntityId()
    {
        return LastMonsterId++;
    }

    /// <summary>
    /// Retrieves a monster by its ID.
    /// </summary>
    /// <param name="monsterId">The ID of the monster.</param>
    /// <returns>The Monster instance if found, null otherwise.</returns>
    private Monster GetMonster(long monsterId)
    {
        if (_monsters.TryGetValue(monsterId, out var monster))
        {
            return monster;
        }

        return null;
    }

    /// <summary>
    /// Removes a monster from the active list and returns it to the pool.
    /// </summary>
    /// <param name="monster">The Monster instance to remove.</param>
    private void RemoveMonster(Monster monster)
    {
        _monsters.Remove(monster.Id);
        ObjectPool.Monster.Release(monster.gameObject);
    }

    /// <summary>
    /// Handles the event when a monster goes out of the screen.
    /// </summary>
    /// <param name="monster">The Monster instance that went out.</param>
    private void OnMonsterOut(Monster monster)
    {
        if (_monstersToRemove.ContainsKey(monster.Id))
        {
            return;
        }

        // Add the current monster for be removed in 1 second.
        _monstersToRemove.Add(monster.Id, 1f);

        // Decrease the current monster count.
        CurrentCount = Mathf.Max(0, CurrentCount - 1);

        // Update the ui with current count and spawned count.
        GameView.SetMonsters(CurrentCount, Count);

        if (CurrentCount == 0)
        {
            OnRoundFinish();
        }
    }

    /// <summary>
    /// Loads required components for the entity service.
    /// </summary>
    private void LoadComponents()
    {
        _entitiesTransform = GameObject.Find("Entities").transform;
    }

    /// <summary>
    /// Processes the removal of monsters marked for deletion.
    /// </summary>
    private void MonsterRemoveProcess()
    {
        if (_monstersToRemove.Count == 0)
            return;

        _temporaryMonstersToRemove.Clear();

        var monsters = _monstersToRemove.ToList();

        // Iterate all monsters need to be removed.
        for (int i = 0; i < monsters.Count; i++)
        {
            var pair = monsters[i];

            // Decrease the time requested to remove with delta time.
            _monstersToRemove[pair.Key] -= Time.deltaTime;

            // If got the time necessary to remove, we'll to add in temporary list to remove the monster.
            if (_monstersToRemove[pair.Key] <= 0)
            {
                _temporaryMonstersToRemove.Add(pair.Key);
            }
        }

        // Iterate the monsters need to be removed after 1 second.
        for (var i = 0; i < _temporaryMonstersToRemove.Count; i++)
        {
            var monsterId = _temporaryMonstersToRemove[i];
            var monster = GetMonster(monsterId);
            if (monster == null)
                continue;

            RemoveMonster(monster);
            _monstersToRemove.Remove(monsterId);
        }

        // Triggers the round finish delegate when all monsters are removed.
        if (_monsters.Count == 0)
            OnRoundEnd();
    }
}
