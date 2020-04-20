using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CombiningSpawnAnchors : SingletonMonoBehaviour<CombiningSpawnAnchors>
{
    [SerializeField] Transform[] _orbAnchors = default;

    public Transform GetRandomAnchor ()
    {
        return _orbAnchors[Random.Range(0, _orbAnchors.Length)];
    }
}
