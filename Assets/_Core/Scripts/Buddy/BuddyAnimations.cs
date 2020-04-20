using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuddyMovement))]
public class BuddyAnimations : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] Animator _animator = default;
	[SerializeField] string _floatingParameter = "Floating";
	[SerializeField] string _wavingParameter = "Waving";
	[SerializeField] string _walkingParameter = "Walking";
	[SerializeField] string _talkingParameter = "Talking";

	BuddyMovement _movement = default;

	void Awake ()
	{
		_movement = GetComponent<BuddyMovement>();
	}

	private void OnEnable ()
	{
		_movement.OnMovementModeChanged += MovementModeChanged;
	}

	private void OnDisable ()
	{
		_movement.OnMovementModeChanged -= MovementModeChanged;
	}

	void MovementModeChanged (BuddyMovementModes newMode)
	{
		switch (newMode) {
			case BuddyMovementModes.FloatingToHand:
				_animator.SetTrigger(_floatingParameter);
				break;
			case BuddyMovementModes.Falling:
				_animator.SetTrigger(_floatingParameter);
				break;
			case BuddyMovementModes.Idle:
				_animator.SetTrigger(_wavingParameter);
				break;
			case BuddyMovementModes.Walking:
				_animator.SetTrigger(_walkingParameter);
				break;
			case BuddyMovementModes.AttachedToHand:
				_animator.SetTrigger(_talkingParameter);
				break;
		}
	}
}
