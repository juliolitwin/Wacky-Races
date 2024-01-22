using UnityEngine.SceneManagement;

public class LaunchService
{
    public void Initialization()
    {
        SceneManager.LoadScene(ScenesConstants.Game);
    }
}