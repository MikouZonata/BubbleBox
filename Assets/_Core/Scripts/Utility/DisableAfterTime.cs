using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    [SerializeField] float _secondsBeforeDisable = 2;

    float _timer = 0;

    void Awake ()
    {
        _timer = _secondsBeforeDisable;   
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0) {
            gameObject.SetActive(false);
        }
    }
}
