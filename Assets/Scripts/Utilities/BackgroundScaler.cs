using UnityEngine;

[ExecuteInEditMode]
public class BackgroundScaler : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (mainCamera == null || spriteRenderer == null) return;

        ScaleAndPositionSprite();
    }

    private void ScaleAndPositionSprite()
    {
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float scaleX = cameraWidth / spriteWidth;

        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float scaleY = (cameraHeight / 2) / spriteHeight;

        transform.localScale = new Vector3(scaleX, scaleY, 1);

        // Adjust position to bottom-center of the screen
        float newYPosition = -mainCamera.orthographicSize / 2;
        transform.position = new Vector3(0, newYPosition, transform.position.z);
    }
}