using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbMovement), typeof(OrbCombining), typeof(OrbShaking))]
public class Orb : MonoBehaviour
{
	public delegate void Disabled (Orb orb);
	public event Disabled OnDisabled;

	public bool IsActive => gameObject.activeSelf;

	OrbMovement _movement;
	OrbCombining _combining;

	private void Awake ()
	{
		_movement = GetComponent<OrbMovement>();
		_combining = GetComponent<OrbCombining>();

		//DEBUG
		Transform fakeAnchor = new GameObject("FakeOrbAnchor").transform;
		fakeAnchor.position = transform.position;
		Activate(fakeAnchor);
	}

	public void Activate (Transform anchorTransform)
	{
		_movement.Activate(anchorTransform);
		_combining.Activate(this);
		gameObject.SetActive(true);
	}

	public void Disable ()
	{
		OnDisabled?.Invoke(this);
		gameObject.SetActive(false);
	}
}
