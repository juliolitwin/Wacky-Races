using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// Defines the game's lifetime scope using the VContainer dependency injection framework.
/// This class is responsible for configuring the dependencies that will be used throughout the game's lifetime.
/// </summary>
public class GameLifetimeScope : LifetimeScope
{
    /// <summary>
    /// Configures the container with the required dependencies for the game.
    /// </summary>
    /// <param name="builder">The container builder used to register dependencies.</param>
    protected override void Configure(IContainerBuilder builder)
    {
        // Load and register views (UI and other visual components).
        LoadViews(builder);

        // Register game-related services and presenters with their respective lifetimes.
        builder.RegisterEntryPoint<GamePresenter>(Lifetime.Scoped);
        builder.Register<GameService>(Lifetime.Singleton);
        builder.Register<EntityService>(Lifetime.Singleton);
        builder.Register<PoolService>(Lifetime.Singleton);
        builder.Register<RoundService>(Lifetime.Singleton);
    }

    /// <summary>
    /// Loads and registers views with the container.
    /// </summary>
    /// <param name="builder">The container builder used to register views.</param>
    private void LoadViews(IContainerBuilder builder)
    {
        // Registers the GameView component, loaded from resources, as a singleton.
        builder.RegisterComponentInNewPrefab(Resources.Load<GameView>(ResourcesConstants.GameView), Lifetime.Singleton);
    }
}
