using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float _secondsBeforeDestroy = 2;

    float _timer = 0;

    void Awake ()
    {
        _timer = _secondsBeforeDestroy;   
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0) {
            Destroy(gameObject);
        }
    }
}
