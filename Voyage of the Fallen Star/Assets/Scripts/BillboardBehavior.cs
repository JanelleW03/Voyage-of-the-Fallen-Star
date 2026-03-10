using UnityEngine;

public class BillboardBehavior : MonoBehaviour
{
    private Transform _mainCameraTransform;

    private void Start()
    {
        if (Camera.main)
        {
            _mainCameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (_mainCameraTransform)
        {
            transform.rotation = _mainCameraTransform.rotation;
        }
    }
}