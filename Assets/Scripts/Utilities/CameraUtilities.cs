using UnityEngine;

public static class CameraUtilities
{
    public static float CalculateScreenHeightInWorldUnits()
    {
        return 2f * Camera.main.orthographicSize;
    }

    public static float CalculateScreenWidthInWorldUnits()
    {
        float height = CalculateScreenHeightInWorldUnits();
        return height * Camera.main.aspect;
    }

    public static Vector3 GetStartPosition()
    {
        float screenWidth = CalculateScreenWidthInWorldUnits();
        return new Vector3(Camera.main.transform.position.x - screenWidth / 2, 0, 0);
    }

    public static Vector3 GetEndPosition()
    {
        float screenWidth = CalculateScreenWidthInWorldUnits();
        return new Vector3(Camera.main.transform.position.x + screenWidth / 2, 0, 0);
    }
}
