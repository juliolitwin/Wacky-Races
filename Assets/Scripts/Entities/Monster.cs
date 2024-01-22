using UnityEngine;

public class Monster : Entity
{
    /// <summary>
    /// Transforms for the monster's body.
    /// </summary>
    private Transform _bodyTransform;

    /// <summary>
    /// Transforms for the monster's shadow.
    /// </summary>
    private Transform _shadowTransform;

    /// <summary>
    /// Event triggered when the monster moves out of the screen.
    /// </summary>
    public event MonsterOutDelegate OutEvent;

    public override void Awake()
    {
        // Find and assign the body and shadow transforms from the children.
        _bodyTransform = transform.Find("Body");
        _shadowTransform = transform.Find("Shadow");

        // Get the sprite renderer component from the body for rendering.
        BodyRenderer = _bodyTransform.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Property to check if the monster is out of the screen.
    /// </summary>
    public bool IsOutFromScreen { get; private set; }

    /// <summary>
    /// Public property to access the body's sprite renderer.
    /// </summary>
    public SpriteRenderer BodyRenderer { get; private set; }

    /// <summary>
    /// Property to access the material of the body renderer.
    /// </summary>
    public Material Material => BodyRenderer.material;

    /// <summary>
    /// Property to get the width of the sprite.
    /// </summary>
    public float SpriteWidth => BodyRenderer?.bounds.size.x ?? 0;

    /// <summary>
    /// Initialization method for setting up monster properties.
    /// </summary>
    /// <param name="id">The id generated for the entity.</param>
    /// <param name="movementSpeed">The movement speed.</param>
    /// <param name="layer">The sort layer for the rendering.</param>
    /// <param name="bodyHue">The body hue.</param>
    /// <param name="eyeHue">The eye hue.</param>
    /// <param name="bodyShade">The body shade.</param>
    /// <param name="startPosition">The start position where will to spawn the monster.</param>
    /// <param name="isRare">Rare monster will to have a cool color effect.</param>
    public void Initialization(long id, float movementSpeed, int layer, float bodyHue, float eyeHue, float bodyShade, Vector3 startPosition, bool isRare)
    {
        // Setting basic properties like ID, movement speed, etc.
        Id = id;
        MovementSpeed = movementSpeed;
        SetColorSwap(bodyHue, eyeHue, bodyShade, isRare);

        // Setting the sorting order for rendering and initial position.
        BodyRenderer.sortingOrder = layer;
        transform.position = startPosition;
        IsOutFromScreen = false;
    }

    public override void Update()
    {
        // Skip update if the monster is already out of the screen.
        if (IsOutFromScreen)
            return;

        // Calculate screen width and movement speed.
        var screenWidth = CameraUtilities.CalculateScreenWidthInWorldUnits();
        var calculatedSpeed = screenWidth / MovementSpeed;

        // Process movement and animation based on calculated speed.
        MovementProcess(calculatedSpeed);
        AnimationProcess(calculatedSpeed);

        // Check if the monster is out of the screen and trigger the Out event.
        if (IsOutScreen())
        {
            Out();
        }
    }

    /// <summary>
    /// Method to handle the monster's movement.
    /// </summary>
    /// <param name="speed">The movement speed.</param>
    private void MovementProcess(float speed)
    {
        // Move the monster at the given speed.
        Move(speed);
    }

    /// <summary>
    /// Method to handle the monster's animations.
    /// </summary>
    /// <param name="speed">The movement speed</param>
    private void AnimationProcess(float speed)
    {
        // 1. Body rotation animation.
        _bodyTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * speed * MonsterConstants.AnimationSpeed) * MonsterConstants.AnimationAngle);

        // 2. Shadow scale animation.
        var shadowScale = (Mathf.Sin(Time.time * speed * MonsterConstants.AnimationShadowSpeed) + 1) / 2 * (MonsterConstants.AnimationShadowMaxScale - MonsterConstants.AnimationShadowMinScale) + MonsterConstants.AnimationShadowMinScale;
        _shadowTransform.localScale = new Vector3(shadowScale, shadowScale, shadowScale);
    }

    /// <summary>
    /// Method to set color swap effects based on monster rarity.
    /// </summary>
    private void SetColorSwap(float bodyHue, float eyeHue, float bodyShade, bool isRare)
    {
        // Apply special effects for rare monsters.
        if (isRare)
        {
            Material.SetFloat("_ColorChangeEffectToggle", 1f);
            Material.SetFloat("_ColorChangeSpeed", Random.Range(3f, 10f));
        }
        else
        {
            // No special effects for common monsters.
            Material.SetFloat("_ColorChangeEffectToggle", 0f);
            Material.SetFloat("_ColorChangeSpeed", 0f);
        }

        // Set the hue and shade for the monster's material.
        Material.SetVector("_HueShift", new Vector2(bodyHue, eyeHue));
        Material.SetVector("_ShadeShift", new Vector2(bodyShade, 0));
    }

    /// <summary>
    /// Method to check if the monster is out of the screen.
    /// </summary>
    /// <returns></returns>
    private bool IsOutScreen()
    {
        // Determine if the monster's position is beyond the screen's end.
        var calculatedOut = transform.position.x - SpriteWidth / 2;
        return calculatedOut > CameraUtilities.GetEndPosition().x;
    }

    /// <summary>
    /// Method called when the monster moves out of the screen.
    /// </summary>
    private void Out()
    {
        // Invoke the Out event and update the status.
        OutEvent?.Invoke(this);
        IsOutFromScreen = true;
    }
}