using UnityEngine;

public delegate void OutEvent(Monster sender);

public class Monster : Actor
{
    private Material _material;

    private Transform _bodyTransform;
    private Transform _shadowTransform;

    private bool _isOut = false;

    public event OutEvent OutEvent;

    public override void Awake()
    {
        _bodyTransform = transform.Find("Body");
        _shadowTransform = transform.Find("Shadow");

        _material = _bodyTransform.GetComponent<Renderer>().material;
        BodyRenderer = _bodyTransform.GetComponent<SpriteRenderer>();
    }

    public SpriteRenderer BodyRenderer { get; private set; }

    public float SpriteWidth => BodyRenderer?.bounds.size.x ?? 0;

    public void Initialization(long id, float movementSpeed, float bodyHue, float eyeHue, float bodyShade, Vector3 startPosition, bool isRare)
    {
        Id = id;
        MovementSpeed = movementSpeed;
        SetColorSwap(bodyHue, eyeHue, bodyShade, isRare);

        this.transform.position = startPosition;
        _isOut = false;
    }

    public override void Update()
    {
        if (_isOut)
            return;

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
        var calculatedSpeed = screenWidth / MovementSpeed;

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

    private void SetColorSwap(float bodyHue, float eyeHue, float bodyShade, bool isRare)
    {
        if (isRare)
        {
            _material.SetFloat("_ColorChangeEffectToggle", 1f);
            _material.SetFloat("_ColorChangeSpeed", Random.Range(3f, 10f));
        }
        else
        {
            _material.SetFloat("_ColorChangeEffectToggle", 0f);
            _material.SetFloat("_ColorChangeSpeed", 0f);
        }

        _material.SetVector("_HueShift", new Vector2(bodyHue, eyeHue));
        _material.SetVector("_ShadeShift", new Vector2(bodyShade, 0));
    }

    private bool IsOut()
    {
        var calculatedOut = transform.position.x - SpriteWidth / 2;
        return calculatedOut > CameraUtilities.GetEndPosition().x;
    }

    private void Out()
    {
        OutEvent?.Invoke(this);
        _isOut = true;
    }
}