using VContainer.Unity;

public class GamePresenter : IStartable
{
    private readonly GameService _gameService;
    private readonly GameView _gameView;

    public GamePresenter(GameService gameService, GameView gameView)
    {
        _gameService = gameService;
        _gameView = gameView;
    }

    void IStartable.Start()
    {
        _gameService.Initialization();
        EventBinding();
    }

    private void EventBinding()
    {

    }
}
