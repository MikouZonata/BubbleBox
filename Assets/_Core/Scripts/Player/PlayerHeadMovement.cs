using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class PlayerHeadMovement : MonoBehaviour
{
    [SerializeField] Transform _headTransform = default;
    [SerializeField] float _maxVerticalRotation = 55;
    [SerializeField] float _maxHorizontalRotation = 82;

    PlayerInputs _inputs;

    float _verticalAngle = 0;
    float _horizontalAngle = 0;

    private void Awake ()
    {
        _inputs = GetComponent<PlayerInputs>();
    }

    void Update()
    {
        HeadRotation();
    }

    void HeadRotation ()
    {
        _verticalAngle += _inputs.VerticalHeadRotation;
        _verticalAngle = Mathf.Clamp(_verticalAngle, -_maxVerticalRotation, _maxVerticalRotation);

        _horizontalAngle += _inputs.HorizontalHeadRotation;
        _horizontalAngle = Mathf.Clamp(_horizontalAngle, -_maxHorizontalRotation, _maxHorizontalRotation);

        _headTransform.localEulerAngles = new Vector3(_verticalAngle, _horizontalAngle, 0);
    }
}
