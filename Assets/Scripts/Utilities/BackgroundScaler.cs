using UnityEngine;

[ExecuteInEditMode]
public class BackgroundScaler : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private float _lineHeight = 0.02f;
    private float _lineSpacing = 1.8f;
    private float _startHeight = -3.6f;

    [SerializeField] private SpriteRenderer[] roads;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Camera.main == null || _spriteRenderer == null) 
            return;

        ScaleAndPositionSprite();
        CreateRoadLines();
    }

    private void ScaleAndPositionSprite()
    {
        var cameraHeight = Camera.main.orthographicSize * 2;
        var cameraWidth = cameraHeight * Camera.main.aspect;

        var spriteWidth = _spriteRenderer.sprite.bounds.size.x;
        var scaleX = cameraWidth / spriteWidth;

        var spriteHeight = _spriteRenderer.sprite.bounds.size.y;
        var scaleY = (cameraHeight / 2) / spriteHeight;

        transform.localScale = new Vector3(scaleX, scaleY, 1);

        // Adjust position to bottom-center of the screen
        var newYPosition = -Camera.main.orthographicSize / 2;
        transform.position = new Vector3(0, newYPosition, transform.position.z);
    }

    private void CreateRoadLines()
    {
        var cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        for (var i = 0; i < 2; i++)
        {
            var line = roads[i];
            line.transform.localScale = new Vector3(cameraWidth, _lineHeight, 1);
            var yPosition = _startHeight + (i * (_lineHeight + _lineSpacing));
            line.transform.position = new Vector3(0, yPosition, 0);
        }
    }
}