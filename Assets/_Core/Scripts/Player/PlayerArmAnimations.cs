using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class PlayerArmAnimations : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] GameObject _leftArmResting = default;
    [SerializeField] GameObject _rightArmResting = default;
    [SerializeField] GameObject _leftArmStretched = default;
    [SerializeField] GameObject _rightArmStretched = default;

    PlayerInputs _inputs;

    void Awake ()
    {
        _inputs = GetComponent<PlayerInputs>();

        _leftArmResting.SetActive(true);
        _leftArmStretched.SetActive(false);
        _rightArmResting.SetActive(true);
        _rightArmStretched.SetActive(false);
    }

    void Update ()
    {
        if (_inputs.LeftHandDown) {
            _leftArmStretched.SetActive(true);
            _leftArmResting.SetActive(false);
        } else if (_inputs.LeftHandUp) {
            _leftArmResting.SetActive(true);
            _leftArmStretched.SetActive(false);
        }

        if (_inputs.RightHandDown) {
            _rightArmStretched.SetActive(true);
            _rightArmResting.SetActive(false);
        } else if (_inputs.RightHandUp) {
            _rightArmResting.SetActive(true);
            _rightArmStretched.SetActive(false);
        }
    }
}
