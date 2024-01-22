using VContainer;
using VContainer.Unity;

/// <summary>
/// Defines the launch lifetime scope using VContainer.
/// This class is responsible for setting up the dependencies specifically for the launch phase of the application.
/// </summary>
public class LaunchLifetimeScope : LifetimeScope
{
    /// <summary>
    /// Configures the container with dependencies required during the launch phase.
    /// </summary>
    /// <param name="builder">The container builder used to register dependencies.</param>
    protected override void Configure(IContainerBuilder builder)
    {
        // Registers the entry point for the launch phase. This is typically the main presenter or controller for this phase.
        builder.RegisterEntryPoint<LaunchPresenter>();

        // Registers the LaunchService with a Scoped lifetime. Scoped lifetime means the instance is shared within the same scope, 
        // but a new instance is created for different scopes. This is suitable for services that have state specific to a particular phase or operation.
        builder.Register<LaunchService>(Lifetime.Scoped);
    }
}
