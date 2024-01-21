using UnityEngine;

public class GameService
{
    private readonly GameView _gameView;
    private Transform _canvas;

    public GameService(GameView gameView)
    {
        _gameView = gameView;
    }

    public void Initialization()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        _canvas = GameObject.Find("Canvas").transform;
        _gameView.transform.SetParent(_canvas.transform);
    }
}
