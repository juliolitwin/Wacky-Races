using VContainer.Unity;

public class GamePresenter : IStartable
{
    public GamePresenter(GameService gameService, GameView gameView)
    {
        GameService = gameService;
        GameView = gameView;
    }

    private GameService GameService { get; }
    private GameView GameView { get; }

    void IStartable.Start()
    {
        GameService.Initialization();
        EventBinding();
    }

    private void EventBinding()
    {

    }
}
