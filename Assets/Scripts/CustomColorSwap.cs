using UnityEngine;

public class CustomColorSwap : MonoBehaviour
{
    [SerializeField] private float _bodyHueValue = 0.0F;
    [SerializeField] private float _eyeHueValue = 0.0F;
    [SerializeField] private float _bodyShadeValue = 0.0F;

    [SerializeField] private float _movementSpeed = 5f;

    [SerializeField] private float _animationAngle = 4.0f;
    [SerializeField] private float _animationSpeed = 3.0f;

    private Material _material;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        float screenWidth = CalculateScreenWidthInWorldUnits();
        var ms = screenWidth / _movementSpeed;

        Debug.Log($"Current speed: {ms}");
        transform.Translate(ms * Time.deltaTime * Vector3.right);

        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * ms * _animationSpeed) * _animationAngle);

        if (transform.position.x > GetEndPosition().x)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        _material.SetVector("_HueShift", new Vector2(_bodyHueValue, _eyeHueValue));
        _material.SetVector("_ShadeShift", new Vector2(_bodyShadeValue, 0));
    }

    public static float CalculateScreenWidthInWorldUnits()
    {
        float height = 2f * Camera.main.orthographicSize;
        return height * Camera.main.aspect;
    }

    public Vector3 GetStartPosition()
    {
        float screenWidth = CalculateScreenWidthInWorldUnits();
        return new Vector3(Camera.main.transform.position.x - screenWidth / 2, 0, 0);
    }

    private Vector3 GetEndPosition()
    {
        float screenWidth = CalculateScreenWidthInWorldUnits();
        return new Vector3(Camera.main.transform.position.x + screenWidth / 2, 0, 0);
    }
}