using UnityEngine;

[RequireComponent(typeof(Orb), typeof(Rigidbody))]
public class OrbMovement : MonoBehaviour, IGrabbable
{
	public MovementModes MovementMode => _movementMode;

	public enum MovementModes
	{
		FreeFloating, FloatingToHand, AttachedToHand, Thrown
	}

	[Header("Movement Settings")]
	[SerializeField] float _baseMovementSpeed = 2;
	[SerializeField] float _accelleration = 1.2f;
	[SerializeField] float _decelleration = 3;
	[SerializeField] float _turnRate = 130;
	[SerializeField] float _thrownStateDuration = 1.2f;

	Transform _anchorTransform = default;
	Transform _movementTargetTransform = default;
	Rigidbody _rigidbody;

	MovementModes _movementMode = MovementModes.FreeFloating;
	float _movementSpeed = 0;
	Vector3 _movementDirection = Vector3.forward;
	float _thrownTimer = 0;

	public void Activate (Transform anchorTransform)
	{
		_anchorTransform = anchorTransform;
		_movementTargetTransform = anchorTransform;
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = true;

		ChangeMovementMode(MovementModes.FreeFloating);
	}

	#region Update Functions
	protected virtual void Update ()
	{
		switch (_movementMode) {
			case MovementModes.AttachedToHand:
				AttachedMovement();
				break;
			case MovementModes.FloatingToHand:
				FloatingMovement();
				break;
			case MovementModes.FreeFloating:
				FloatingMovement();
				break;
			case MovementModes.Thrown:
				ThrownMovement();
				break;
		}
	}

	protected virtual void AttachedMovement ()
	{
		transform.position = _movementTargetTransform.position;
	}

	protected virtual void FloatingMovement ()
	{
		if (_movementSpeed < _baseMovementSpeed) {
			_movementSpeed += _accelleration * Time.deltaTime;
		} else {
			_movementSpeed -= _decelleration * Time.deltaTime;
		}

		Vector3 desiredDirection = _movementTargetTransform.position - transform.position;
		float actualTurnRate = _turnRate * Mathf.Deg2Rad * Time.deltaTime;
		_movementDirection = Vector3.RotateTowards(_movementDirection, desiredDirection, actualTurnRate, 0);

		transform.position += _movementDirection.normalized * _movementSpeed * Time.deltaTime;
	}

	protected virtual void ThrownMovement ()
	{
		if (_thrownTimer > 0) {
			_thrownTimer -= Time.deltaTime;
		} else {

			ChangeMovementMode(MovementModes.FreeFloating);
		}
	}
	#endregion

	void ChangeMovementMode (MovementModes newMode)
	{
		switch (_movementMode) {
			case MovementModes.AttachedToHand:
				_movementTargetTransform = _anchorTransform;
				break;
			case MovementModes.FloatingToHand:
				break;
			case MovementModes.FreeFloating:
				break;
			case MovementModes.Thrown:
				_movementSpeed = _rigidbody.velocity.magnitude;
				_movementDirection = _rigidbody.velocity.normalized;
				_rigidbody.isKinematic = true;
				break;
		}

		switch (newMode) {
			case MovementModes.AttachedToHand:
				break;
			case MovementModes.FloatingToHand:
				break;
			case MovementModes.FreeFloating:
				_movementTargetTransform = _anchorTransform;
				break;
			case MovementModes.Thrown:
				_rigidbody.isKinematic = false;
				_rigidbody.velocity = _movementDirection.normalized * _movementSpeed;
				_thrownTimer = _thrownStateDuration;
				break;
		}

		_movementMode = newMode;
	}

	#region Grabbable Functions
	public void Shake ()
	{
		Debug.LogWarning("Shake not yet implemented");
	}

	public bool RequestPull (Transform handTransform)
	{
		if (_movementMode == MovementModes.FloatingToHand || _movementMode == MovementModes.AttachedToHand) {
			return false;
		} else {
			_movementTargetTransform = handTransform;
			ChangeMovementMode(MovementModes.FloatingToHand);
			return true;
		}
	}

	public void Release ()
	{
		ChangeMovementMode(MovementModes.FreeFloating);
	}

	public void Attach (Transform handTransform)
	{
		_movementTargetTransform = handTransform;

		ChangeMovementMode(MovementModes.AttachedToHand);
	}

	public void Throw (Vector3 direction, float speed)
	{
		_movementDirection = direction;
		_movementSpeed = speed;

		ChangeMovementMode(MovementModes.Thrown);
	}
	#endregion
}
