using VContainer.Unity;

public class GamePresenter : IStartable, ITickable
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

    public void Tick()
    {
        GameService.Update();
    }

    private void EventBinding()
    {

    }
}
