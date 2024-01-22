using TMPro;
using UnityEngine;

public class GameService
{
    private Transform _canvas;
    private Transform _environments;

    private SpriteRenderer _backgroundRenderer;

    public GameService(RoundService roundService, GameView gameView)
    {
        RoundService = roundService;
        GameView = gameView;
    }

    public RoundService RoundService { get; }

    public GameView GameView { get; }

    public float[] SpawnHeights { get; private set; }

    public void Initialization()
    {
        LoadComponents();

        RoundService?.Initialization(SpawnHeights);
    }

    public void Update()
    {
        RoundService?.Update();
    }

    private float[] GetYPositionsForLines(int numberOfLines, float backgroundHeight, Vector2 backgroundPosition, float tolerance)
    {
        var yPositions = new float[numberOfLines];
        var bottomEdge = backgroundPosition.y - backgroundHeight / 2;
        var topEdge = backgroundPosition.y + backgroundHeight / 2;

        for (var i = 0; i < numberOfLines; i++)
        {
            if (i == numberOfLines - 1)
            {
                yPositions[i] = topEdge - tolerance;
            }
            else
            {
                yPositions[i] = bottomEdge + backgroundHeight / (numberOfLines + 1) * (i + 1);
            }
        }

        return yPositions;
    }

    private void LoadComponents()
    {
        _canvas = UIHelper.GetCanvas().transform;
        _environments = UIHelper.Find("Environments").transform;
        _backgroundRenderer = _environments.transform.Find("Road").GetComponent<SpriteRenderer>();

        LoadUI();

        // @Notes - calculated the tolerante with current logic:
        // BodyRenderer.bounds.size.y / 4 = 0.66825f
        SpawnHeights = GetYPositionsForLines(3, GetBackgroundHeight(), GetBackgroundPosition(), 0.66825f);
    }

    private void LoadUI()
    {
        GameView.transform.SetParent(_canvas.transform, false);
        GameView.LoadComponents();
    }

    private float GetBackgroundHeight()
    {
        return _backgroundRenderer.bounds.size.y;
    }

    private Vector2 GetBackgroundPosition()
    {
        return new Vector2(_backgroundRenderer.transform.position.x, _backgroundRenderer.transform.position.y);
    }
}
