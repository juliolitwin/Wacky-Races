using VContainer.Unity;

/// <summary>
/// Presenter class for the game. This class is responsible for coordinating the game's logic (handled by GameService) 
/// and its presentation (handled by GameView).
/// It implements IStartable for initialization and ITickable for frame-to-frame updates.
/// </summary>
public class GamePresenter : IStartable, ITickable
{
    /// <summary>
    /// Constructor for the GamePresenter.
    /// Injects the GameService and GameView dependencies.
    /// </summary>
    /// <param name="gameService">Instance of GameService containing the game logic.</param>
    /// <param name="gameView">Instance of GameView representing the user interface.</param>
    public GamePresenter(GameService gameService, GameView gameView)
    {
        GameService = gameService;
        GameView = gameView;
    }

    // Reference to the GameService that contains the game logic.
    private GameService GameService { get; }

    // Reference to the GameView that represents the user interface.
    private GameView GameView { get; }

    /// <summary>
    /// Called by VContainer at application start. Initializes the game service and binds events.
    /// </summary>
    void IStartable.Start()
    {
        // Initialize the game service and setup event bindings.
        GameService.Initialization();
        EventBinding();
    }

    /// <summary>
    /// Called by VContainer on each frame update. Delegates to the GameService's Update method.
    /// </summary>
    public void Tick()
    {
        // Call the update method of the game service for frame-to-frame logic.
        GameService.Update();
    }

    /// <summary>
    /// Sets up event bindings between the service and the view.
    /// </summary>
    private void EventBinding()
    {
        // Bind events or actions between GameService and GameView.
    }
}
