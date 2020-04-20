using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyMovement : MonoBehaviour, IGrabbable
{
	public delegate void MovementModeChanged (BuddyMovementModes newMode);
	public event MovementModeChanged OnMovementModeChanged;

	public BuddyMovementModes MovementMode => _movementMode;

	[Header("General Settings")]
	[SerializeField] Transform _playerHeadTransform = default;
	[SerializeField] Vector3 _lookAtHeadOffset = new Vector3(0, -.4f, 0);
	[SerializeField] Transform _idlePositionTransform = default;

	[Header("Movement Settings")]
	[SerializeField] float _walkingSpeed = 4.2f;
	[SerializeField] float _walkingTurnRate = 200;
	[SerializeField] float _walkingDistanceToTargetBeforeIdle = 1;
	[SerializeField] float _floatingMaxSpeed = 3.5f;
	[SerializeField] float _floatingAcceleration = 7;
	[SerializeField] float _fallingSpeed = 4;
	[SerializeField] float _fallingDownwardTurnRate = 30;

	[Header("Grounded Check Settings")]
	[SerializeField] float _groundedRayLength = .1f;
	[SerializeField] LayerMask _groundedRayMask = default;


	BuddyMovementModes _movementMode = BuddyMovementModes.Walking;
	Transform _movementTargetTransform;
	float _movementSpeed = 0;
	Vector3 _movementDirection = Vector3.forward;

	void Awake ()
	{
		_idlePositionTransform = transform.parent;
		_movementTargetTransform = _idlePositionTransform;
		ChangeMovementMode(BuddyMovementModes.Idle);
	}

	#region Update Functions
	void Update ()
	{
		switch (_movementMode) {
			case BuddyMovementModes.AttachedToHand:
				AttachedMovement();
				break;
			case BuddyMovementModes.FloatingToHand:
				FloatingMovement();
				break;
			case BuddyMovementModes.Falling:
				transform.up = Vector3.up;
				FallingMovement();
				break;
			case BuddyMovementModes.Walking:
				WalkingMovement();
				break;
			case BuddyMovementModes.Idle:
				break;
		}
	}

	void AttachedMovement ()
	{
		transform.position = _movementTargetTransform.position;
		transform.LookAt(_playerHeadTransform.position + _lookAtHeadOffset);
	}

	void FloatingMovement ()
	{
		_movementSpeed = Mathf.MoveTowards(_movementSpeed, _floatingMaxSpeed, _floatingAcceleration * Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, _movementTargetTransform.position, _movementSpeed * Time.deltaTime);

		transform.LookAt(_movementTargetTransform);
	}

	void FallingMovement ()
	{
		if (GroundedCheck()) {
			ChangeMovementMode(BuddyMovementModes.Walking);
		} else {
			float actualTurnRate = Mathf.Deg2Rad * _fallingDownwardTurnRate * Time.deltaTime;
			_movementDirection = Vector3.RotateTowards(_movementDirection, Vector3.down, actualTurnRate, 0.0f);
			transform.position += _movementDirection.normalized * _movementSpeed * Time.deltaTime;
		}
	}

	void WalkingMovement ()
	{
		if (!GroundedCheck()) {
			ChangeMovementMode(BuddyMovementModes.Falling);
		}

		Vector3 selfLateralPosition = new Vector3(transform.position.x, 0, transform.position.z);
		Vector3 targetLateralPosition = new Vector3(_movementTargetTransform.position.x, 0, _movementTargetTransform.position.z);
		Vector3 desiredMovementDirection = targetLateralPosition - selfLateralPosition;

		float actualTurnRate = Mathf.Deg2Rad * _walkingTurnRate * Time.deltaTime;

		_movementDirection = Vector3.RotateTowards(_movementDirection, desiredMovementDirection, actualTurnRate, 0.0f);
		transform.forward = _movementDirection;
		transform.position += _movementDirection.normalized * _movementSpeed * Time.deltaTime;

		if (Vector3.Distance(selfLateralPosition, targetLateralPosition) <= _walkingDistanceToTargetBeforeIdle) {
			ChangeMovementMode(BuddyMovementModes.Idle);
		}
	}
	#endregion

	bool GroundedCheck ()
	{
		if (Physics.Raycast(transform.position, Vector3.down, _groundedRayLength, _groundedRayMask)) {
			return true;
		} else {
			return false;
		}
	}

	void ChangeMovementMode (BuddyMovementModes newMode)
	{
		//End previous state
		switch (_movementMode) {
			case BuddyMovementModes.AttachedToHand:
				break;
			case BuddyMovementModes.FloatingToHand:
				break;
			case BuddyMovementModes.Falling:
				break;
			case BuddyMovementModes.Idle:
				break;
			case BuddyMovementModes.Walking:
				break;
		}

		//Initiate new state
		switch (newMode) {
			case BuddyMovementModes.AttachedToHand:
				break;
			case BuddyMovementModes.FloatingToHand:
				_movementSpeed = 0;
				break;
			case BuddyMovementModes.Falling:
				break;
			case BuddyMovementModes.Idle:
				Vector3 lookAtPosition = new Vector3(_playerHeadTransform.position.x, transform.position.y, _playerHeadTransform.position.z);
				transform.LookAt(lookAtPosition);
				break;
			case BuddyMovementModes.Walking:
				_movementSpeed = _walkingSpeed;
				_movementDirection = transform.forward;
				_movementTargetTransform = _idlePositionTransform;
				break;
		}

		_movementMode = newMode;
		OnMovementModeChanged?.Invoke(newMode);
	}

	public bool RequestPull (Transform handTransform)
	{
		if (_movementMode == BuddyMovementModes.FloatingToHand || _movementMode == BuddyMovementModes.AttachedToHand) {
			return false;
		} else {
			_movementTargetTransform = handTransform;
			ChangeMovementMode(BuddyMovementModes.FloatingToHand);
			return true;
		}
	}

	public void Release ()
	{
		_movementDirection = Vector3.down;
		_movementSpeed = _fallingSpeed;

		ChangeMovementMode(BuddyMovementModes.Falling);
	}

	public void Attach (Transform handTransform)
	{
		_movementTargetTransform = handTransform;

		ChangeMovementMode(BuddyMovementModes.AttachedToHand);
	}

	public void Throw (Vector3 direction, float speed)
	{
		_movementDirection = direction;
		_movementSpeed = _fallingSpeed;

		ChangeMovementMode(BuddyMovementModes.Falling);
	}
}
