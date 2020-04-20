using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Orb))]
public class OrbShaking : MonoBehaviour, IShakable
{
	[Header("Settings")]
	[SerializeField] GameObject _explosionPrefab = default;
	[SerializeField] int _shakesBeforeExploding = 6;
	[SerializeField] float _secondsBeforeShakeCountReset = 1.5f;

	Orb _orb;
	int _accumulatedShakes = 0;
	float _timer = 0;

	void Awake ()
	{
		_orb = GetComponent<Orb>();
	}

	void Update ()
	{
		if (_timer <= 0) {
			_accumulatedShakes = 0;
		} else {
			_timer -= Time.deltaTime;
		}
	}

    public bool Shake () //Candidate for cleanup. Should be clearer why this returns a bool to PlayerHandInteractions
	{
		_accumulatedShakes++;
		_timer = _secondsBeforeShakeCountReset;

		if (_accumulatedShakes >= _shakesBeforeExploding) {
			Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
			_orb.Disable();
			return true; //Orb was destroyed by the shake.
		} else {
			return false; //Orb was NOT destroyed by the shake.
		}
	}
}
