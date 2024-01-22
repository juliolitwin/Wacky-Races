using UnityEngine.SceneManagement;

/// <summary>
/// Service class responsible for the initialization logic during the launch phase of the application.
/// This typically involves scene transitions or initial setup tasks.
/// </summary>
public class LaunchService
{
    /// <summary>
    /// Initialization method for the launch service.
    /// Loads the main game scene.
    /// </summary>
    public void Initialization()
    {
        // Load the main game scene as defined in ScenesConstants.
        // This is typically called at the start of the application to transition from a launch or menu scene to the main game scene.
        SceneManager.LoadScene(ScenesConstants.Game);
    }
}