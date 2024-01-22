using System.Diagnostics;
using UnityEngine;

public class RoundService
{
    // Constant value representing the delay before spawning entities.
    private const float SpawnDelay = 3f;

    // Timer to track the delay before the next spawn.
    private float _spawnDelayTimer = 0f;

    // Variable to store the last countdown time.
    private int _lastCountdown;

    // Variable to store the elapsed time.
    private Stopwatch _stopWatch = new Stopwatch();

    // Delegate for handling the end of a round.
    private RoundFinishDelegate OnRoundFinish;

    /// <summary>
    /// Constructor to inject the entity service dependency.
    /// </summary>
    public RoundService(EntityService entityService, GameView gameView)
    {
        EntityService = entityService;
        GameView = gameView;
    }

    /// <summary>
    /// Publicly accessible EntityService instance.
    /// </summary>
    public EntityService EntityService { get; }

    /// <summary>
    /// Publicly accessible GameView instance.
    /// </summary>
    public GameView GameView { get; }

    /// <summary>
    /// Property to keep track of the current round.
    /// </summary>
    public int Round { get; private set; } = 1;

    /// <summary>
    /// Property to keep track of the current state.
    /// </summary>
    public RoundState State { get; private set; } = RoundState.Idle;

    /// <summary>
    /// Initializes the service with spawn heights and sets up the end round callback.
    /// </summary>
    /// <param name="spawnHeights">Array of heights at which entities should be spawned.</param>
    public void Initialization(float[] spawnHeights, RoundFinishDelegate onFinish)
    {
        EntityService?.Initialization(spawnHeights, FinishRound, EndRound);
        OnRoundFinish = onFinish;
    }

    /// <summary>
    /// Update method, called once per frame. Handles the update of entities and time processing.
    /// </summary>
    public void Update()
    {
        // Update the entity service and process the time-related logic.
        EntityService?.Update();
        StateProcess();
    }

    /// <summary>
    /// Prepare the next round.
    /// </summary>
    public void NextRound()
    {
        // Increment round number and start a new round.
        Round++;
        StartRound();
    }

    /// <summary>
    /// Handles the time-related logic for spawning entities.
    /// </summary>
    private void StateProcess()
    {
        switch (State)
        {
            case RoundState.Waiting:
                {
                    // Increment the spawn delay timer.
                    _spawnDelayTimer += Time.deltaTime;

                    // Calculate remaining seconds and log countdown if necessary.
                    var secondsRemaining = Mathf.CeilToInt(SpawnDelay - _spawnDelayTimer);
                    if (secondsRemaining < _lastCountdown)
                    {
                        if (secondsRemaining > 0)
                        {
                            GameView.SetCountdown($"{secondsRemaining}");
                            UnityEngine.Debug.Log($"{secondsRemaining} to start the race.");
                        }

                        _lastCountdown = secondsRemaining;
                    }

                    // Check if spawn delay has been reached and start the race if so.
                    if (_spawnDelayTimer >= SpawnDelay)
                    {
                        _spawnDelayTimer = 0f;
                        State = RoundState.Running;

                        Fire();

                        UnityEngine.Debug.Log("Race is started!");
                    }
                }
                break;
            case RoundState.Running:
                {
                    UpdateElapsedTime();
                }
                break;
        }
    }

    /// <summary>
    /// Starts a new round, spawns entities and resets countdown.
    /// </summary>
    public void StartRound()
    {
        GameView.SetRound(Round);
        UnityEngine.Debug.Log($"Round ({Round}) is started.");

        // Spawn entities based on the Fibonacci sequence of the current round.
        EntityService?.Spawn(MathHelper.Fibonacci(Round));
        State = RoundState.Waiting;
        _lastCountdown = (int)SpawnDelay + 1;

        // Restart the stop watch for calculate the elapsed time.
        _stopWatch.Reset();

        UpdateElapsedTime();
    }

    /// <summary>
    /// Updates the elapsed time in the UI.
    /// </summary>
    private void UpdateElapsedTime()
    {
        var elapsed = _stopWatch.Elapsed;
        if (elapsed.Minutes > 0)
        {
            GameView.SetElapsedTime(string.Format("{0:D2}:{1:D2}:{2:D2}", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10));
        }
        else
        {
            GameView.SetElapsedTime(string.Format("{0:D2}:{1:D2}", elapsed.Seconds, elapsed.Milliseconds / 10));
        }

    }

    /// <summary>
    /// Fires the entities. Typically called when the spawn delay has been reached.
    /// </summary>
    private void Fire()
    {
        // Set the countdown (3, 2, 1..)
        GameView.SetStartCountdown();

        // Start the stop watch for calculate the elapsed time.
        _stopWatch.Start();

        // Fire the entities to run.
        EntityService?.Fire();
    }

    /// <summary>
    /// Finishes the current round.
    /// </summary>
    private void FinishRound()
    {
        _stopWatch.Stop();
    }

    /// <summary>
    /// Ends the current round.
    /// </summary>
    private void EndRound()
    {
        UnityEngine.Debug.Log($"Round ({Round}) is end.");
        OnRoundFinish();
    }
}
