using VContainer;
using VContainer.Unity;

public class LaunchLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<LaunchPresenter>();

        builder.Register<LaunchService>(Lifetime.Scoped);
    }
}