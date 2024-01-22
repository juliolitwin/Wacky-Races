using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        LoadViews(builder);

        builder.RegisterEntryPoint<GamePresenter>(Lifetime.Scoped);
        builder.Register<GameService>(Lifetime.Singleton);
        builder.Register<EntityService>(Lifetime.Singleton);
        builder.Register<PoolService>(Lifetime.Singleton);
        builder.Register<RoundService>(Lifetime.Singleton);
    }

    private void LoadViews(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(Resources.Load<GameView>(ResourcesConstants.GameView), Lifetime.Singleton);
    }
}
