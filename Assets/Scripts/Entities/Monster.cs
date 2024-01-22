using UnityEngine;

public class Monster : Entity
{
    private Transform _bodyTransform;
    private Transform _shadowTransform;

    public event MonsterOutEvent OutEvent;

    public override void Awake()
    {
        _bodyTransform = transform.Find("Body");
        _shadowTransform = transform.Find("Shadow");

        BodyRenderer = _bodyTransform.GetComponent<SpriteRenderer>();
    }

    public bool IsOutFromScreen { get; private set; }

    public SpriteRenderer BodyRenderer { get; private set; }
    public Material Material => BodyRenderer.material;

    public float SpriteWidth => BodyRenderer?.bounds.size.x ?? 0;

    public void Initialization(long id, float movementSpeed, int layer, float bodyHue, float eyeHue, float bodyShade, Vector3 startPosition, bool isRare)
    {
        Id = id;
        MovementSpeed = movementSpeed;
        SetColorSwap(bodyHue, eyeHue, bodyShade, isRare);

        BodyRenderer.sortingOrder = layer;
        transform.position = startPosition;
        IsOutFromScreen = false;
    }

    public override void Update()
    {
        if (IsOutFromScreen)
            return;

        var screenWidth = CameraUtilities.CalculateScreenWidthInWorldUnits();
        var calculatedSpeed = screenWidth / MovementSpeed;

        MovementProcess(calculatedSpeed);
        AnimationProcess(calculatedSpeed);

        if (IsOutScreen())
        {
            Out();
        }
    }

    private void MovementProcess(float speed)
    {
        Move(speed);
    }

    private void AnimationProcess(float ms)
    {
        // 1. Body rotation animation.
        _bodyTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * ms * MonsterConstants.AnimationSpeed) * MonsterConstants.AnimationAngle);

        // 2. Shadow scale animation.
        var shadowScale = (Mathf.Sin(Time.time * ms * MonsterConstants.AnimationShadowSpeed) + 1) / 2 * (MonsterConstants.AnimationShadowMaxScale - MonsterConstants.AnimationShadowMinScale) + MonsterConstants.AnimationShadowMinScale;
        _shadowTransform.localScale = new Vector3(shadowScale, shadowScale, shadowScale);
    }

    private void SetColorSwap(float bodyHue, float eyeHue, float bodyShade, bool isRare)
    {
        if (isRare)
        {
            Material.SetFloat("_ColorChangeEffectToggle", 1f);
            Material.SetFloat("_ColorChangeSpeed", Random.Range(3f, 10f));
        }
        else
        {
            Material.SetFloat("_ColorChangeEffectToggle", 0f);
            Material.SetFloat("_ColorChangeSpeed", 0f);
        }

        Material.SetVector("_HueShift", new Vector2(bodyHue, eyeHue));
        Material.SetVector("_ShadeShift", new Vector2(bodyShade, 0));
    }

    private bool IsOutScreen()
    {
        var calculatedOut = transform.position.x - SpriteWidth / 2;
        return calculatedOut > CameraUtilities.GetEndPosition().x;
    }

    private void Out()
    {
        OutEvent?.Invoke(this);
        IsOutFromScreen = true;
    }
}