using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JellyAnimation : MonoBehaviour
{
    private Material _material;

    [Header("Shape Noise")]
    public float _noiseScale;
    public float _noiseFrequency;
    [Header("Jelly Anim Settings")]
    public float _speed;
    public Vector3 _N3;
    public float _x;
    [Header("Jelly Pump")]
    public float _pump1;
    public float _pump2;
    public float _pump3;
    public float _pump4;
    [Header("Jelly Sway")]
    public float _swayY1;
    public float _swayY2;
    public float _swayY3;
    public float _swayXZ1;
    public float _swayXZ2;

    // Start is called before the first frame update
    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (_material)
        {
            _material.SetFloat("_speed", _speed);
            _material.SetFloat("_NoiseScale", _noiseScale);
            _material.SetFloat("_NoiseFrequency", _noiseFrequency);
            _material.SetVector("_N3", _N3);
            _material.SetFloat("_x", _x);
            _material.SetFloat("_pump1", _pump1);
            _material.SetFloat("_pump2", _pump2);
            _material.SetFloat("_pump3", _pump3);
            _material.SetFloat("_pump4", _pump4);
            _material.SetFloat("_swayY1", _swayY1);
            _material.SetFloat("_swayY2", _swayY2);
            _material.SetFloat("_swayY3", _swayY3);
            _material.SetFloat("_swayXZ1", _swayXZ1);
            _material.SetFloat("_swayXZ2", _swayXZ2);
        }

    }
}
