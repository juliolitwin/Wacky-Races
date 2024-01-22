using UnityEngine;

public class Entity : MonoBehaviour
{
    public long Id { get; protected set; } = 0;
    public float MovementSpeed { get; protected set; }

    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }

    public virtual void Move(float speed)
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

    public virtual float CalculateSpeedVariation()
    {
        return Random.Range(MovementSpeed / 1.5f, MovementSpeed * 1.5f);
    }
}
