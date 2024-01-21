using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        LoadViews(builder);

        builder.RegisterEntryPoint<GamePresenter>(Lifetime.Scoped);
        builder.Register<GameService>(Lifetime.Scoped);
        builder.Register<PoolService>(Lifetime.Singleton);
    }

    private void LoadViews(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(Resources.Load<GameView>(ResourcesConstants.GameView), Lifetime.Singleton);
    }
}
