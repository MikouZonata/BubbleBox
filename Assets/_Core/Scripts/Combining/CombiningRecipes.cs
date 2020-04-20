using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipes", menuName = "ScriptableObjects/Recipes", order = 1)]
public class CombiningRecipes : ScriptableObject
{
    public Recipe[] recipes;

    [System.Serializable]
    public struct Recipe
    {
        public CombiningIngredients leftIngredient;
        public CombiningIngredients rightIngredient;
        public GameObject resultPrefab;
    }
}
