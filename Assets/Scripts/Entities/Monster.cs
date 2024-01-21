using UnityEngine;

public class Monster : Actor
{
    [SerializeField] private float _bodyHueValue = 0.0F;
    [SerializeField] private float _eyeHueValue = 0.0F;
    [SerializeField] private float _bodyShadeValue = 0.0F;

    [SerializeField] private int _movementSpeed = 5;

    private Material _material;
    private float _spriteWidth;

    private Transform _bodyTransform;
    private Transform _shadowTransform;

    public override void Awake()
    {
        _bodyTransform = transform.Find("Body");
        _shadowTransform = transform.Find("Shadow");

        _material = _bodyTransform.GetComponent<Renderer>().material;
        _spriteWidth = _bodyTransform.GetComponent<SpriteRenderer>().bounds.size.x;

        var startPosition = CameraUtilities.GetStartPosition();
        var startTransform = startPosition.x + ((_spriteWidth + 2) / 2);
        this.transform.position = new Vector3(startTransform, 0f, 0f);
    }

    public override void Update()
    {
        var speed = MovementProcess();
        AnimationProcess(speed);

        if (IsOut())
        {
            Out();
        }
    }

    private float MovementProcess()
    {
        var screenWidth = CameraUtilities.CalculateScreenWidthInWorldUnits();
        var calculatedSpeed = screenWidth / _movementSpeed;

        transform.Translate(calculatedSpeed * Time.deltaTime * Vector3.right);
        return calculatedSpeed;
    }

    private void AnimationProcess(float ms)
    {
        // 1. Body rotation animation.
        _bodyTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * ms * MonsterConstants.AnimationSpeed) * MonsterConstants.AnimationAngle);

        // 2. Shadow scale animation.
        var shadowScale = (Mathf.Sin(Time.time * ms * MonsterConstants.AnimationShadowSpeed) + 1) / 2 * (MonsterConstants.AnimationShadowMaxScale - MonsterConstants.AnimationShadowMinScale) + MonsterConstants.AnimationShadowMinScale;
        _shadowTransform.localScale = new Vector3(shadowScale, shadowScale, shadowScale);
    }

    private bool IsOut()
    {
        var calculatedOut = transform.position.x - _spriteWidth / 2;
        return calculatedOut > CameraUtilities.GetEndPosition().x;
    }

    private void Out()
    {
        Destroy(gameObject);
    }

    public override void LateUpdate()
    {
        SetColorSwap(_bodyHueValue, _eyeHueValue, _bodyShadeValue);
    }

    private void SetColorSwap(float bodyHue, float eyeHue, float bodyShade)
    {
        _material.SetVector("_HueShift", new Vector2(bodyHue, eyeHue));
        _material.SetVector("_ShadeShift", new Vector2(bodyShade, 0));
    }
}