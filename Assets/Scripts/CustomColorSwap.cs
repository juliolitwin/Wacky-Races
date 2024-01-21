using UnityEngine;

public class CustomColorSwap : MonoBehaviour
{
    [SerializeField]
    private float _bodyHueValue = 0.0F;

    [SerializeField]
    private float _eyeHueValue = 0.0F;

    [SerializeField]
    private float _bodyShadeValue = 0.0F;

    private Material _material;

    [SerializeField]
    private float moveSpeed = 5f;
    private Vector3 moveDirection = Vector3.right;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        if (transform.position.x < GetEndPosition().x)
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

    private Vector3 GetStartPosition()
    {
        float screenWidth = CalculateScreenWidthInWorldUnits();
        return new Vector3(Camera.main.transform.position.x + screenWidth / 2, 0, 0);
    }

    private Vector3 GetEndPosition()
    {
        float screenWidth = CalculateScreenWidthInWorldUnits();
        return new Vector3(Camera.main.transform.position.x - screenWidth / 2, 0, 0);
    }
}