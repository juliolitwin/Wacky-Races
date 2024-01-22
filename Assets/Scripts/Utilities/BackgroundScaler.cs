using UnityEngine;

[ExecuteInEditMode]
public class BackgroundScaler : MonoBehaviour
{
    // Reference to the sprite renderer of the background.
    private SpriteRenderer _spriteRenderer;

    // Configuration for the road lines.
    private float _lineHeight = 0.02f;    // Height of each road line.
    private float _lineSpacing = 1.8f;    // Spacing between road lines.
    private float _startHeight = -3.6f;   // Starting vertical position for the first road line.

    [SerializeField] private SpriteRenderer[] roads; // Array of sprite renderers for the road lines.

    void Start()
    {
        // Get the sprite renderer attached to the background object.
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Exit early if there's no main camera or sprite renderer.
        if (Camera.main == null || _spriteRenderer == null)
            return;

        // Scale and position the background sprite to fit the camera view.
        ScaleAndPositionSprite();

        // Create and position the road lines on the background.
        CreateRoadLines();
    }

    private void ScaleAndPositionSprite()
    {
        // Calculate the camera's dimensions.
        var cameraHeight = Camera.main.orthographicSize * 2;
        var cameraWidth = cameraHeight * Camera.main.aspect;

        // Scale the sprite to match the camera's width.
        var spriteWidth = _spriteRenderer.sprite.bounds.size.x;
        var scaleX = cameraWidth / spriteWidth;

        // Scale the sprite to cover half of the camera's height.
        var spriteHeight = _spriteRenderer.sprite.bounds.size.y;
        var scaleY = (cameraHeight / 2) / spriteHeight;

        // Apply the scaling to the transform.
        transform.localScale = new Vector3(scaleX, scaleY, 1);

        // Adjust the sprite's position to align with the bottom-center of the screen.
        var newYPosition = -Camera.main.orthographicSize / 2;
        transform.position = new Vector3(0, newYPosition, transform.position.z);
    }

    private void CreateRoadLines()
    {
        // Calculate the width of the camera view.
        var cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        // Iterate through each road line and set its scale and position.
        for (var i = 0; i < 2; i++)
        {
            var line = roads[i];
            line.transform.localScale = new Vector3(cameraWidth, _lineHeight, 1);
            var yPosition = _startHeight + (i * (_lineHeight + _lineSpacing));
            line.transform.position = new Vector3(0, yPosition, 0);
        }
    }
}
