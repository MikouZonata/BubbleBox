using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] GameObject _orbPrefab = default;
	[SerializeField] Transform[] _orbAnchors = default;
	[SerializeField] int _maxOrbsActive = 2;
	[SerializeField] float _secondsBetweenSpawns = 6;

	List<Orb> _orbPool = new List<Orb>();
	int _orbsActive = 0;
	float _spawnTimer = 0;

	void Awake ()
	{
		for (int i = 0; i < _maxOrbsActive; i++) {
			_orbPool.Add(Instantiate(_orbPrefab, transform).GetComponent<Orb>());
			_orbPool[i].Disable();
		}
	}

	void Update ()
	{
		SpawnTimer();
	}

	void SpawnTimer ()
	{
		if (_spawnTimer > 0) {
			_spawnTimer -= Time.deltaTime;
		} else {
			if (_orbsActive < _maxOrbsActive) {
				SpawnOrb();
				_spawnTimer = _secondsBetweenSpawns;
			}
		}
	}

	void SpawnOrb ()
	{
		Orb newOrb = RetrieveOrbFromPool();
		newOrb.transform.position = transform.position;
		newOrb.Activate(_orbAnchors[Random.Range(0, _orbAnchors.Length)]);
		newOrb.OnDisabled += OrbWasDisabled;//
		_orbsActive++;
	}

	Orb RetrieveOrbFromPool ()
	{
		foreach (Orb orb in _orbPool) {
			if (!orb.IsActive) {
				return orb;
			}
		}

		//Insufficient orbs in pool, expanding pool
		Orb newOrb = Instantiate(_orbPrefab, transform).GetComponent<Orb>();
		_orbPool.Add(newOrb);
		return newOrb;
	}

	void OrbWasDisabled (Orb orb)
	{
		orb.OnDisabled -= OrbWasDisabled;//

		_orbsActive--;
		if (_spawnTimer <= 0) {
			_spawnTimer = _secondsBetweenSpawns;
		}
	}

}
