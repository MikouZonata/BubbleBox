using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(PlayerHandMotionView))]
public class PlayerHandInteractions : MonoBehaviour
{
	[Header("Hands Settings")]
	[SerializeField] Transform _stretchedLeftHandTransform = default;
	[SerializeField] Transform _stretchedRightHandTransform = default;
	[SerializeField] float _distanceToAttachGrabbable = .4f;

	[Header("Raycast Settings")]
	[SerializeField] Transform _cameraTransform = default;
	[SerializeField] LayerMask _orbDetectionMask = default;
	[SerializeField] float _rayMaxLength = 15;

	[Header("Pull & Release Settings")]
	[SerializeField] float _throwSpeed = 20;
	[SerializeField] LineRenderer _leftHandLineRenderer = default;
	[SerializeField] LineRenderer _rightHandLineRenderer = default;

	[Header("Combining Settings")]
	[SerializeField] float _timeBeforeCombine = 2.5f;
	[SerializeField] Transform _combiningSpawnPosition = default;
	[SerializeField] ParticleSystem _combiningInProgressParticles = default;
	[SerializeField] GameObject _combiningCompleteParticlesPrefab = default;

	PlayerInputs _inputs;
	PlayerHandMotionView _handMovementView;
	RaycastHit[] _rayHits = new RaycastHit[1];
	Ray _detectionRay;

	GrabbableData _leftGrabbable = null;
	bool _leftHandHasAttachment = false;
	GrabbableData _rightGrabbable = null;
	bool _rightHandHasAttachment = false;

	bool _combiningIsPossible = false;
	float _combiningTimer = 0;
	ParticleSystem.EmissionModule _combiningInProgressModule = default;
	float _combiningInProgressEmitRate;

	#region Setup
	void Awake ()
	{
		_inputs = GetComponent<PlayerInputs>();
		_handMovementView = GetComponent<PlayerHandMotionView>();

		_leftHandLineRenderer.SetPositions(new Vector3[] { _stretchedLeftHandTransform.position, _stretchedLeftHandTransform.position });
		_rightHandLineRenderer.SetPositions(new Vector3[] { _stretchedRightHandTransform.position, _stretchedRightHandTransform.position });

		_combiningInProgressModule = _combiningInProgressParticles.emission;
		_combiningInProgressEmitRate = _combiningInProgressModule.rateOverTime.constantMax;
		_combiningInProgressModule.rateOverTime = new ParticleSystem.MinMaxCurve(0);
	}

	void OnEnable ()
	{
		_handMovementView.OnShake += Shake;
	}

	void OnDisable ()
	{
		_handMovementView.OnShake -= Shake;
	}
	#endregion

	#region Update Functions
	void Update ()
	{
		Grabbing();
		Releasing();
		DetectAndAttach();
		Combine();
		DrawLinesToGrabbables();
	}

	void Grabbing ()
	{
		if (_inputs.LeftHandHeld && _leftGrabbable == null) {
			bool grabbableGrabbed = AttemptGrab(_stretchedLeftHandTransform, out _leftGrabbable);

			if (grabbableGrabbed && _leftGrabbable?.InstanceID == _rightGrabbable?.InstanceID) {
				_rightGrabbable = null;
			}
		} else if (_inputs.RightHandHeld && _rightGrabbable == null) {
			bool grabbableGrabbed = AttemptGrab(_stretchedRightHandTransform, out _rightGrabbable);

			if (grabbableGrabbed && _rightGrabbable?.InstanceID == _leftGrabbable?.InstanceID) {
				_leftGrabbable = null;
			}
		}
	}

	void Releasing ()
	{
		if (_inputs.LeftHandUp && _leftGrabbable != null) {
			if (_leftHandHasAttachment) {
				_leftGrabbable.grabbable.Throw(_handMovementView.HeadForward, _throwSpeed);
			} else {
				_leftGrabbable.grabbable.Release();
			}

			_leftGrabbable = null;
			_leftHandHasAttachment = false;
			UpdateCombinables();
		}

		if (_inputs.RightHandUp && _rightGrabbable != null) {
			if (_rightHandHasAttachment) {
				_rightGrabbable.grabbable.Throw(_handMovementView.HeadForward, _throwSpeed);
			} else {
				_rightGrabbable.grabbable.Release();
			}

			_rightGrabbable = null;
			_rightHandHasAttachment = false;
			UpdateCombinables();
		}
	}

	void DetectAndAttach ()
	{
		if (_leftGrabbable != null && !_leftHandHasAttachment && Vector3.Distance(_stretchedLeftHandTransform.position, _leftGrabbable.Position) < _distanceToAttachGrabbable) {
			_leftGrabbable.grabbable.Attach(_stretchedLeftHandTransform);
			_leftHandHasAttachment = true;
			if (_leftGrabbable.canBeCombined) {
				UpdateCombinables();
			}
		}

		if (_rightGrabbable != null && !_rightHandHasAttachment && Vector3.Distance(_stretchedRightHandTransform.position, _rightGrabbable.Position) < _distanceToAttachGrabbable) {
			_rightGrabbable.grabbable.Attach(_stretchedRightHandTransform);
			_rightHandHasAttachment = true;
			if (_rightGrabbable.canBeCombined) {
				UpdateCombinables();
			}
		}
	}

	void Combine ()
	{
		if (_combiningIsPossible) {
			if (_combiningTimer <= 0) {
				_combiningIsPossible = false;

				GameObject combiningResult = CombiningEngine.instance.CombineIngredients(_leftGrabbable.combinable.Ingredient, _rightGrabbable.combinable.Ingredient);
				Orb newOrb = Instantiate(combiningResult, _combiningSpawnPosition.position, Quaternion.identity).GetComponent<Orb>();
				newOrb.Activate(CombiningSpawnAnchors.instance.GetRandomAnchor());

				_leftGrabbable.combinable.Combine();
				_rightGrabbable.combinable.Combine();

				_leftGrabbable = null;
				_leftHandHasAttachment = false;
				_rightGrabbable = null;
				_rightHandHasAttachment = false;

				Instantiate(_combiningCompleteParticlesPrefab, _combiningSpawnPosition.position, Quaternion.identity);
				UpdateCombinables();
			} else {
				_combiningTimer -= Time.deltaTime;
			}
		}
	}

	void DrawLinesToGrabbables ()
	{
		_leftHandLineRenderer.SetPosition(0, _stretchedLeftHandTransform.position);
		if (_leftGrabbable != null) {
			_leftHandLineRenderer.SetPosition(1, _leftGrabbable.Position);
		} else {
			_leftHandLineRenderer.SetPosition(1, _stretchedLeftHandTransform.position);
		}

		_rightHandLineRenderer.SetPosition(0, _stretchedRightHandTransform.position);
		if (_rightGrabbable != null) {
			_rightHandLineRenderer.SetPosition(1, _rightGrabbable.Position);
		} else {
			_rightHandLineRenderer.SetPosition(1, _stretchedRightHandTransform.position);
		}
	}
	#endregion

	void UpdateCombinables ()
	{
		if (_leftHandHasAttachment && _rightHandHasAttachment && _leftGrabbable.canBeCombined && _rightGrabbable.canBeCombined) {
			_combiningIsPossible = true;
			_combiningTimer = _timeBeforeCombine;
			_combiningInProgressModule.rateOverTime = new ParticleSystem.MinMaxCurve(_combiningInProgressEmitRate);
		} else {
			_combiningIsPossible = false;
			_combiningInProgressModule.rateOverTime = new ParticleSystem.MinMaxCurve(0);
		}
	}

	void Shake ()
	{
		if (_leftHandHasAttachment) {
			bool orbWasDestroyedByShake = _leftGrabbable.shakable.Shake();

			if (orbWasDestroyedByShake) {
				_leftGrabbable = null;
				_leftHandHasAttachment = false;
				UpdateCombinables();
			}
		}
		if (_rightHandHasAttachment) {
			bool orbWasDestroyedByShake = _rightGrabbable.shakable.Shake();

			if (orbWasDestroyedByShake) {
				_rightGrabbable = null;
				_rightHandHasAttachment = false;
				UpdateCombinables();
			}
		}
	}



	bool AttemptGrab (Transform handTransform, out GrabbableData grabbableDataBuffer)
	{
		_detectionRay = new Ray(_cameraTransform.position, _cameraTransform.forward);
		int numberOfHits = Physics.RaycastNonAlloc(_detectionRay, _rayHits, _rayMaxLength, _orbDetectionMask);

		if (numberOfHits > 0) {
			IGrabbable hitGrabbable = _rayHits[0].transform.GetComponent<IGrabbable>();

			if (hitGrabbable == null)
				Debug.LogError("AttemptGrab attempted to pick up something that had no IGrabbable component attached", _rayHits[0].transform);

			bool pullIsSuccesful = hitGrabbable.RequestPull(handTransform);
			if (pullIsSuccesful) {
				grabbableDataBuffer = new GrabbableData(hitGrabbable, _rayHits[0].transform);
				return true;
			} else {
				grabbableDataBuffer = null;
				return false;
			}
		} else {
			grabbableDataBuffer = null;
			return false;
		}
	}
}
