using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BuddyMovement), typeof(BuddyShaking))]
public class BuddySpeech : MonoBehaviour
{
	public enum SpeechDistances
	{
		CloseBy,
		FarAway
	}

	[Header("Speech Settings")]
	[SerializeField] BuddySpeechData _speechData = default;
	[SerializeField] float _secondsPerCharacter = .05f;

	[Header("Shaking Settings")]
	[SerializeField] int _shakesBeforeSpeaking = 4;
	[SerializeField] float _secondsBeforeShakesReset = 1;

	[Header("Displays")]
	[SerializeField] TextMeshPro _closeByTextDisplayLeft = default;
	[SerializeField] TextMeshPro _closeByTextDisplayRight = default;
	[SerializeField] TextMeshPro _farAwayTextDisplay = default;

	BuddyMovement _movement = default;
	BuddyShaking _shake = default;
	BuddyMovementModes _movementMode;

	Coroutine _activeTextRoutine = null;
	float _cooldownTimer = 0;
	int _accumulatedShakes = 0;
	float _shakeTimer = 0;

	bool _hasBeenPickedUp = false;

	#region Setup
	void Awake ()
	{
		_movement = GetComponent<BuddyMovement>();
		_shake = GetComponent<BuddyShaking>();

		StopAllSpeech();
	}

	void OnEnable ()
	{
		_shake.OnShaken += GotShaken;
	}

	void OnDisable ()
	{
		_shake.OnShaken -= GotShaken;
	}

	void Start ()
	{
		_movementMode = _movement.MovementMode;
	}
	#endregion

	#region Update Functions
	void Update ()
	{
		TimerBoundSpeeches();
		ShakeTimer();
	}

	void TimerBoundSpeeches ()
	{
		if (_movementMode != _movement.MovementMode) {
			StopAllSpeech();
			_movementMode = _movement.MovementMode;
		}

		if (_cooldownTimer <= 0 && _activeTextRoutine == null) {
			switch (_movementMode) {
				default:
					break;
				case BuddyMovementModes.Idle:
					if (!_hasBeenPickedUp) {
						SaySomething(SpeechDistances.FarAway, _speechData.pickMeUpData);
					} else {
						SaySomething(SpeechDistances.FarAway, _speechData.goExploreData);
					}
					break;
				case BuddyMovementModes.AttachedToHand:
					SaySomething(SpeechDistances.CloseBy, _speechData.niceToMeetYouData);
					_hasBeenPickedUp = true;
					break;
			}
		} else {
			_cooldownTimer -= Time.deltaTime;
		}
	}

	void ShakeTimer ()
	{
		if (_shakeTimer <= 0) {
			_accumulatedShakes = 0;
		} else {
			_shakeTimer -= Time.deltaTime;
		}
	}
	#endregion

	void SaySomething (SpeechDistances distance, BuddySpeechData.SpeechData speechData)
	{
		if (_activeTextRoutine != null) {
			StopAllSpeech();
		}

		switch (distance) {
			case SpeechDistances.CloseBy:
				_activeTextRoutine = StartCoroutine(SpeechRoutine(speechData, _closeByTextDisplayLeft, _closeByTextDisplayRight));
				break;
			case SpeechDistances.FarAway:
				_activeTextRoutine = StartCoroutine(SpeechRoutine(speechData, _farAwayTextDisplay));
				break;
		}
	}

	IEnumerator SpeechRoutine (BuddySpeechData.SpeechData speechData, params TextMeshPro[] displays)
	{
		foreach (TextMeshPro display in displays) {
			display.gameObject.SetActive(true);
		}

		for (int i = 0; i < speechData.texts.Length; i++) {
			foreach (TextMeshPro display in displays) {
				display.text = speechData.texts[i];
			}

			float waitTime = speechData.texts[i].Length * _secondsPerCharacter;
			yield return new WaitForSeconds(waitTime);
		}

		StopAllSpeech();
		_cooldownTimer = speechData.cooldownTime;
	}

	void GotShaken ()
	{
		_accumulatedShakes++;
		_shakeTimer = _secondsBeforeShakesReset;

		if (_accumulatedShakes >= _shakesBeforeSpeaking) {
			SaySomething(SpeechDistances.FarAway, _speechData.getShakenData);
			_accumulatedShakes = _shakesBeforeSpeaking - 1;
		}
	}

	void StopAllSpeech ()
	{
		if (_activeTextRoutine != null) {
			StopCoroutine(_activeTextRoutine);
			_activeTextRoutine = null;
		}
		_cooldownTimer = 0;

		_closeByTextDisplayLeft.text = "";
		_closeByTextDisplayLeft.gameObject.SetActive(false);
		_closeByTextDisplayRight.text = "";
		_closeByTextDisplayRight.gameObject.SetActive(false);
		_farAwayTextDisplay.text = "";
		_farAwayTextDisplay.gameObject.SetActive(false);
	}
}
