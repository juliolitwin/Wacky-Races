using UnityEngine;
using UnityEngine.Pool;

public class GameService
{
    private Transform _canvas;

    public GameService(PoolService poolService, GameView gameView)
    {
        ObjectPool = poolService;
        GameView = gameView;
    }

    public PoolService ObjectPool { get; }

    public GameView GameView { get; }

    public void Initialization()
    {
        LoadComponents();

        ObjectPool.MonsterPool.Get();
    }

    private void LoadComponents()
    {
        _canvas = GameObject.Find("Canvas").transform;
        GameView.transform.SetParent(_canvas.transform);
    }
}
