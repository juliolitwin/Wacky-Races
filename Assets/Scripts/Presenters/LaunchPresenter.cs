using VContainer.Unity;

public class LaunchPresenter : IStartable
{
    private readonly LaunchService _launchService;

    public LaunchPresenter(LaunchService launchService)
    {
        _launchService = launchService;
    }

    void IStartable.Start()
    {
        _launchService.Initialization();
    }
}