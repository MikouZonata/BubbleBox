using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyShaking : MonoBehaviour, IShakable
{
    public delegate void Shaken ();
    public event Shaken OnShaken;

    public bool Shake () //Candidate for cleanup. This function returns a bool to fix OrbShaking.
    {
        OnShaken?.Invoke();
        return false; //Buddy cannot be destroyed by shake.
    }
}
