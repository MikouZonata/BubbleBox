using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
	public float HorizontalHeadRotation => _horizontalHeadRotation;
	public float VerticalHeadRotation => _verticalHeadRotation;
	public bool LeftHandDown => _leftHandDown;
	public bool LeftHandHeld => _leftHandHeld;
	public bool LeftHandUp => _leftHandUp;
	public bool RightHandDown => _rightHandDown;
	public bool RightHandHeld => _rightHandHeld;
	public bool RightHandUp => _rightHandUp;

	[Header("Look Settings")]
	[SerializeField] float _horizontalSensitivity = 1;
	[SerializeField] bool _invertHorizontal = false;
	[SerializeField] float _verticalSensitivity = 1;
	[SerializeField] bool _invertVertical = false;

	[Header("Hand Settings")]
	[SerializeField] KeyCode _leftHandKey = KeyCode.Mouse0;
	[SerializeField] KeyCode _rightHandKey = KeyCode.Mouse1;

	float _horizontalHeadRotation = 0;
	float _verticalHeadRotation = 0;
	bool _leftHandDown = false;
	bool _leftHandHeld = false;
	bool _leftHandUp = false;
	bool _rightHandDown = false;
	bool _rightHandHeld = false;
	bool _rightHandUp = false;

	void Update ()
	{
		UpdateHeadMovement();
		UpdateHandButtons();
	}

	void UpdateHeadMovement ()
	{
		_horizontalHeadRotation = Input.GetAxis("Mouse X") * _horizontalSensitivity * (_invertHorizontal ? -1 : 1);
		_verticalHeadRotation = Input.GetAxis("Mouse Y") * _verticalSensitivity * (_invertVertical ? 1 : -1);
	}

	void UpdateHandButtons ()
	{
		_leftHandDown = Input.GetKeyDown(_leftHandKey);
		_leftHandHeld = Input.GetKey(_leftHandKey);
		_leftHandUp = Input.GetKeyUp(_leftHandKey);
		_rightHandDown = Input.GetKeyDown(_rightHandKey);
		_rightHandHeld = Input.GetKey(_rightHandKey);
		_rightHandUp = Input.GetKeyUp(_rightHandKey);
	}
}
