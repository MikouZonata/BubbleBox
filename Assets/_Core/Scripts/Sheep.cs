using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    void Start()
    {
        GetComponent<Orb>().enabled = false;
        GetComponent<OrbMovement>().enabled = false;
        GetComponent<OrbShaking>().enabled = false;
    }
}
