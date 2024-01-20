using UnityEngine;

public class CustomColorSwap : MonoBehaviour
{
    [SerializeField]
    private float _bodyHueValue = 0.0F;

    [SerializeField]
    private float _eyeHueValue = 0.0F;

    [SerializeField]
    private float _bodyShadeValue = 0.0F;

    [SerializeField]
    private float _eyeShadeValue = 0.0F;

    private Material _material;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void LateUpdate()
    {
        _material.SetVector("_HueShift", new Vector2(_bodyHueValue, _eyeHueValue));
        _material.SetVector("_ShadeShift", new Vector2(_bodyShadeValue, _eyeShadeValue));
    }
}