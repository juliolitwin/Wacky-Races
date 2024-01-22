using UnityEngine;

/// <summary>
/// Static class containing utility methods for camera-related calculations.
/// This includes converting screen dimensions to world units and getting specific positions relative to the camera's view.
/// </summary>
public static class CameraUtilities
{
    /// <summary>
    /// Calculates the height of the screen in world units based on the main camera's orthographic size.
    /// </summary>
    /// <returns>Height of the screen in world units.</returns>
    public static float CalculateScreenHeightInWorldUnits()
    {
        // Orthographic size is half the height of the camera's view.
        // Multiply by 2 to get the full height in world units.
        return 2f * Camera.main.orthographicSize;
    }

    /// <summary>
    /// Calculates the width of the screen in world units based on the main camera's orthographic size and aspect ratio.
    /// </summary>
    /// <returns>Width of the screen in world units.</returns>
    public static float CalculateScreenWidthInWorldUnits()
    {
        // Get the screen height in world units.
        var height = CalculateScreenHeightInWorldUnits();

        // Multiply the height by the camera's aspect ratio to get the width.
        return height * Camera.main.aspect;
    }

    /// <summary>
    /// Calculates the start position of the screen relative to the main camera's current position.
    /// This is typically the left edge of the camera's view in world space.
    /// </summary>
    /// <returns>Vector3 representing the start position.</returns>
    public static Vector3 GetStartPosition()
    {
        // Calculate screen width in world units.
        var screenWidth = CalculateScreenWidthInWorldUnits();

        // Determine the start position (left edge) based on the camera's position and screen width.
        return new Vector3(Camera.main.transform.position.x - screenWidth / 2, 0, 0);
    }

    /// <summary>
    /// Calculates the end position of the screen relative to the main camera's current position.
    /// This is typically the right edge of the camera's view in world space.
    /// </summary>
    /// <returns>Vector3 representing the end position.</returns>
    public static Vector3 GetEndPosition()
    {
        // Calculate screen width in world units.
        var screenWidth = CalculateScreenWidthInWorldUnits();

        // Determine the end position (right edge) based on the camera's position and screen width.
        return new Vector3(Camera.main.transform.position.x + screenWidth / 2, 0, 0);
    }
}
