using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Orb))]
public class OrbCombining : MonoBehaviour, ICombinable
{
    public CombiningIngredients Ingredient => _ingredient;

    [SerializeField] CombiningIngredients _ingredient = default;

    Orb _orb;

    public void Activate (Orb orb)
    {
        _orb = orb;
    }

    public void Disable ()
    {
    }

    public void Combine ()
    {
        _orb.Disable();
    }
}
