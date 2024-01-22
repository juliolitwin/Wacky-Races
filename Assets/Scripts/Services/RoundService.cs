using UnityEngine;

public class RoundService
{
    // Constant value representing the delay before spawning entities.
    private const float SpawnDelay = 3f;

    // Timer to track the delay before the next spawn.
    private float _spawnDelayTimer = 0f;

    // Flag to indicate if spawning is ready.
    private bool _isSpawnReady = true;

    // Variable to store the last countdown time.
    private int _lastCountdown;

    /// <summary>
    /// Constructor to inject the entity service dependency.
    /// </summary>
    /// <param name="entityService"></param>
    public RoundService(EntityService entityService)
    {
        EntityService = entityService;
    }

    /// <summary>
    /// Publicly accessible EntityService instance.
    /// </summary>
    public EntityService EntityService { get; }

    /// <summary>
    /// Property to keep track of the current round.
    /// </summary>
    public int Round { get; private set; } = 1;

    /// <summary>
    /// Initializes the service with spawn heights and sets up the end round callback.
    /// </summary>
    /// <param name="spawnHeights">Array of heights at which entities should be spawned.</param>
    public void Initialization(float[] spawnHeights)
    {
        EntityService?.Initialization(spawnHeights, EndRound);
    }

    /// <summary>
    /// Update method, called once per frame. Handles the update of entities and time processing.
    /// </summary>
    public void Update()
    {
        // Update the entity service and process the time-related logic.
        EntityService?.Update();
        TimeProcess();
    }

    /// <summary>
    /// Handles the time-related logic for spawning entities.
    /// </summary>
    private void TimeProcess()
    {
        // Skip if spawning is already ready.
        if (_isSpawnReady)
        {
            return;
        }

        // Increment the spawn delay timer.
        _spawnDelayTimer += Time.deltaTime;

        // Calculate remaining seconds and log countdown if necessary.
        var secondsRemaining = Mathf.CeilToInt(SpawnDelay - _spawnDelayTimer);
        if (secondsRemaining < _lastCountdown)
        {
            if (secondsRemaining > 0)
            {
                Debug.Log($"{secondsRemaining} to start the race.");
            }
            _lastCountdown = secondsRemaining;
        }

        // Check if spawn delay has been reached and start the race if so.
        if (_spawnDelayTimer >= SpawnDelay)
        {
            _spawnDelayTimer = 0f;
            _isSpawnReady = true;
            Fire();
            Debug.Log("Race is started!");
        }
    }

    /// <summary>
    /// Starts a new round, spawns entities and resets countdown.
    /// </summary>
    public void StartRound()
    {
        Debug.Log($"Round ({Round}) is started.");

        // Spawn entities based on the Fibonacci sequence of the current round.
        EntityService?.Spawn(MathHelper.Fibonacci(Round));
        _isSpawnReady = false;
        _lastCountdown = (int)SpawnDelay + 1;
    }

    /// <summary>
    /// Fires the entities. Typically called when the spawn delay has been reached.
    /// </summary>
    private void Fire()
    {
        EntityService?.Fire();
    }

    /// <summary>
    /// Ends the current round and starts a new one.
    /// </summary>
    private void EndRound()
    {
        Debug.Log($"Round ({Round}) is finished.");

        // Increment round number and start a new round.
        Round++;
        StartRound();
    }
}
