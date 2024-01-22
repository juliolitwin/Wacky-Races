using UnityEngine;

/// <summary>
/// Base class for entities in the game, providing basic properties and methods.
/// </summary>
public class Entity : MonoBehaviour
{
    // Unique identifier for the entity.
    public long Id { get; protected set; } = 0;

    // Movement speed of the entity.
    public float MovementSpeed { get; protected set; }

    // Current state of the entity.
    public EntityState EntityState { get; private set; }

    // Awake is called when the script instance is being loaded.
    public virtual void Awake() { }

    // Start is called before the first frame update.
    public virtual void Start() { }

    // Update is called once per frame.
    public virtual void Update() { }

    // LateUpdate is called after all Update methods have been called.
    public virtual void LateUpdate() { }

    /// <summary>
    /// Moves the entity at a specified speed.
    /// </summary>
    /// <param name="speed">The speed at which to move the entity.</param>
    public virtual void Move(float speed)
    {
        // Translate the entity in the right direction based on the given speed and deltaTime.
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

    /// <summary>
    /// Calculates a random speed variation for the entity.
    /// </summary>
    /// <returns>A float representing the varied speed.</returns>
    public virtual float CalculateSpeedVariation()
    {
        // Returns a random speed between 1.5 times slower and 1.5 times faster than the base speed.
        return Random.Range(MovementSpeed / 1.5f, MovementSpeed * 1.5f);
    }

    /// <summary>
    /// Changes the state of the entity.
    /// </summary>
    /// <param name="state">The new state to be set for the entity.</param>
    public virtual void ChangeState(EntityState state)
    {
        // Update the entity's state.
        EntityState = state;
    }
}
