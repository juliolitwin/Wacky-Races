using UnityEngine;

public class CloudEffect : MonoBehaviour
{
    // Tolerance distance for resetting cloud position.
    private float _tolerance = 5f;

    // Speed at which clouds move.
    public float _speed = 2;

    // Array of cloud GameObjects.
    public GameObject[] _clouds;

    private void Start()
    {
        // Initialize the position of each cloud at the start of the game.
        for (var i = 0; i < _clouds.Length; i++)
        {
            // Calculate half the camera's height to determine the vertical range for cloud positioning.
            var halfHeight = Camera.main.orthographicSize;
            var yPosition = Random.Range(halfHeight / 2, halfHeight);

            // Set the initial position of each cloud off-screen to the left at a random height.
            _clouds[i].transform.position = new Vector3(
                Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - Random.Range(0, 4) * _tolerance,
                yPosition,
                0);
        }
    }

    private void Update()
    {
        // Update the position of each cloud every frame.
        for (var i = 0; i < _clouds.Length; i++)
        {
            var cloud = _clouds[i];

            // Move the cloud to the right based on the speed and time passed.
            cloud.transform.Translate(_speed * Time.deltaTime, 0, 0);

            // Check if the cloud has moved past the right edge of the screen.
            if (cloud.transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + _tolerance)
            {
                // Calculate a new random vertical position within the upper half of the camera's view.
                var halfHeight = Camera.main.orthographicSize;
                var yPosition = Random.Range(halfHeight / 2, halfHeight);

                // Reset the cloud's position to off-screen on the left at the new height.
                cloud.transform.position = new Vector3(
                    Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - Random.Range(0, 2) * (_tolerance / 2),
                    yPosition,
                    0);
            }
        }
    }
}
