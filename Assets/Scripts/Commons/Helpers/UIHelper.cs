using UnityEngine;

public static class UIHelper
{
    public static GameObject GetCanvas()
    {
        return GameObject.Find("Canvas");
    }

    public static GameObject Find(string name)
    {
        return GameObject.Find(name);
    }

    public static T GetComponent<T>(string path)
    {
        var transf = GetTransform(path);
        if (transf == null)
        {
            UnityEngine.Debug.LogError($"Failed to find the component by the path: {path}.");
            return default;
        }

        return transf.GetComponent<T>();
    }

    public static T GetComponent<T>(Transform transform, string path)
    {
        return transform.Find(path).GetComponent<T>();
    }

    public static Transform GetTransform(string path)
    {
        return GetCanvas().transform.Find(path);
    }
}
