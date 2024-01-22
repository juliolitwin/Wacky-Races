
using UnityEngine;

public class RoundService
{
    private const float SpawnDelay = 3f;

    private float _spawnDelayTimer = 0f;
    private bool _isSpawnReady = true;
    private int _lastCountdown;

    public RoundService(EntityService entityService)
    {
        EntityService = entityService;
    }

    public EntityService EntityService { get; }

    public int Round { get; private set; } = 4;

    public void Initialization(float[] spawnHeights)
    {
        EntityService?.Initialization(spawnHeights, EndRound);
    }

    public void Update()
    {
        EntityService?.Update();
        TimeProcess();
    }

    private void TimeProcess()
    {
        if (_isSpawnReady)
        {
            return;
        }

        _spawnDelayTimer += Time.deltaTime;

        var secondsRemaining = Mathf.CeilToInt(SpawnDelay - _spawnDelayTimer);
        if (secondsRemaining < _lastCountdown)
        {
            if (secondsRemaining > 0)
            {
                Debug.Log($"{secondsRemaining} to start the race.");
            }
            _lastCountdown = secondsRemaining;
        }

        if (_spawnDelayTimer >= SpawnDelay)
        {
            _spawnDelayTimer = 0f;
            _isSpawnReady = true;

            Fire();
            Debug.Log("Race is started!");
        }
    }

    public void StartRound()
    {
        EntityService?.Spawn(MathHelper.Fibonacci(Round));
        _isSpawnReady = false;
        _lastCountdown = (int)SpawnDelay + 1;
    }

    private void Fire()
    {
        EntityService?.Fire();
    }

    private void EndRound()
    {
        Debug.Log($"Round ({Round}) is finished.");

        Round++;
        StartRound();
    }
}
