using VContainer.Unity;

/// <summary>
/// Presenter class for the launch phase of the application.
/// This class serves as a bridge between the launch-related logic (handled by LaunchService) and the application's entry point.
/// </summary>
public class LaunchPresenter : IStartable
{
    // Reference to the LaunchService that contains the launch logic.
    private readonly LaunchService _launchService;

    /// <summary>
    /// Constructor for the LaunchPresenter.
    /// Injects the LaunchService dependency.
    /// </summary>
    /// <param name="launchService">The LaunchService instance provided by VContainer.</param>
    public LaunchPresenter(LaunchService launchService)
    {
        _launchService = launchService;
    }

    /// <summary>
    /// Part of the IStartable interface. Called by VContainer after all injections are done and the application starts.
    /// </summary>
    void IStartable.Start()
    {
        // Call the initialization method of the LaunchService to start launch-related processes.
        _launchService.Initialization();
    }
}
