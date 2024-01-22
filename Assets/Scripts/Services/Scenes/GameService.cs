using UnityEngine;

/// <summary>
/// Service class responsible for managing the game's core logic and environment.
/// It interacts with the RoundService for round management and GameView for user interface updates.
/// </summary>
public class GameService
{
    // Transforms for canvas and environment within the game scene.
    private Transform _canvas;
    private Transform _environments;

    // Renderer for the background image/sprite.
    private SpriteRenderer _backgroundRenderer;

    /// <summary>
    /// Constructor for GameService.
    /// </summary>
    /// <param name="roundService">Service for managing game rounds.</param>
    /// <param name="gameView">View component representing the game's UI.</param>
    public GameService(RoundService roundService, GameView gameView)
    {
        RoundService = roundService;
        GameView = gameView;
    }

    public RoundService RoundService { get; }

    public GameView GameView { get; }

    // Heights at which entities (e.g., monsters) will spawn.
    public float[] SpawnHeights { get; private set; }

    /// <summary>
    /// Initializes the GameService, loading necessary components and setting up the RoundService.
    /// </summary>
    public void Initialization()
    {
        LoadComponents();
        RoundService?.Initialization(SpawnHeights, EndRound);
    }

    /// <summary>
    /// Update method called each frame to update game logic.
    /// </summary>
    public void Update()
    {
        RoundService?.Update();
    }

    /// <summary>
    /// Calculates Y positions for spawning entities based on the background dimensions and a tolerance value.
    /// </summary>
    /// <param name="numberOfLines">Number of lines (spawn positions) to calculate.</param>
    /// <param name="backgroundHeight">Height of the background.</param>
    /// <param name="backgroundPosition">Position of the background.</param>
    /// <param name="tolerance">Tolerance value to adjust spawn positions.</param>
    /// <returns>Array of Y positions for entity spawning.</returns>
    private float[] GetYPositionsForLines(int numberOfLines, float backgroundHeight, Vector2 backgroundPosition, float tolerance)
    {
        var yPositions = new float[numberOfLines];
        var bottomEdge = backgroundPosition.y - backgroundHeight / 2;
        var topEdge = backgroundPosition.y + backgroundHeight / 2;

        for (var i = 0; i < numberOfLines; i++)
        {
            yPositions[i] = i == numberOfLines - 1
                ? topEdge - tolerance
                : bottomEdge + backgroundHeight / (numberOfLines + 1) * (i + 1);
        }

        return yPositions;
    }

    /// <summary>
    /// Loads essential UI components and calculates spawn heights.
    /// </summary>
    private void LoadComponents()
    {
        _canvas = UIHelper.GetCanvas().transform;
        _environments = UIHelper.Find("Environments").transform;
        _backgroundRenderer = _environments.Find("Road").GetComponent<SpriteRenderer>();

        LoadUI();

        // Calculate spawn heights based on the background dimensions and a set tolerance.
        SpawnHeights = GetYPositionsForLines(3, GetBackgroundHeight(), GetBackgroundPosition(), 0.66825f);
    }

    /// <summary>
    /// Loads and sets up the game's UI components.
    /// </summary>
    private void LoadUI()
    {
        GameView.transform.SetParent(_canvas, false);
        GameView.LoadComponents();
    }

    /// <summary>
    /// Gets the height of the background.
    /// </summary>
    /// <returns>The height of the background sprite.</returns>
    private float GetBackgroundHeight()
    {
        return _backgroundRenderer.bounds.size.y;
    }

    /// <summary>
    /// Gets the position of the background.
    /// </summary>
    /// <returns>The position of the background sprite.</returns>
    private Vector2 GetBackgroundPosition()
    {
        return _backgroundRenderer.transform.position;
    }

    /// <summary>
    /// Ends the current round and starts the next round.
    /// </summary>
    private void EndRound()
    {
        RoundService?.NextRound();
    }
}
