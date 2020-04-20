using UnityEngine;

public class PlayerHandMotionView : MonoBehaviour
{
	public delegate void Shake ();
	public event Shake OnShake;

	public Vector3 HeadForward => _headTransform.forward;

	[Header("Settings")]
	[SerializeField] Transform _headTransform = default;
	[SerializeField] float _rotationPerSecondShakeThreshold = 300;

	Quaternion _headRotation;
	bool _wasShakeFiredInCurrentDirection = false;

	private void Awake ()
	{
		_headRotation = _headTransform.localRotation;
	}

	void Update ()
	{
		Quaternion oldHeadRotation = _headRotation;
		_headRotation = _headTransform.localRotation;
		float angleRotated = Quaternion.Angle(_headRotation, oldHeadRotation);

		if (angleRotated > _rotationPerSecondShakeThreshold * Time.deltaTime) {
			if (!_wasShakeFiredInCurrentDirection) {
				_wasShakeFiredInCurrentDirection = true;
				OnShake?.Invoke();
			}
		} else {
			_wasShakeFiredInCurrentDirection = false;
		}
	}
}
