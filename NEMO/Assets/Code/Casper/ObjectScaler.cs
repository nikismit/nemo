using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    public float minScale = 0.5f;
    public float maxScale = 1f;
    public float inhalingSeconds = 2;
    public float exhalingSeconds = 3;

    private Vector3 _startingScale;

    private float _scale = 0f;

    private float _timer = 0f;
    public float CurrentGrowTime { get => _timer; }

    private bool _isGrowing = true;
    public bool IsGrowing { get => _isGrowing; }

    void Start()
    {
        _startingScale = transform.localScale;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        // Grow or shrink
        if (_isGrowing)
        {
            _scale = Mathf.SmoothStep(minScale, maxScale, _timer / inhalingSeconds);
            // Change grow direction
            if (_timer >= inhalingSeconds)
            {
                _timer = 0;
                _isGrowing = (_isGrowing) ? false : true;
            }
        }
        else
        {
            _scale = Mathf.SmoothStep(maxScale, minScale, _timer / exhalingSeconds);
            // Change grow direction
            if (_timer >= exhalingSeconds)
            {
                _timer = 0;
                _isGrowing = (_isGrowing) ? false : true;
            }
        }

        transform.localScale = _startingScale * _scale;
    }
}
