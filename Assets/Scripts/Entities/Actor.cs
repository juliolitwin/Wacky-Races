using UnityEngine;

public class Actor : MonoBehaviour
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
}
