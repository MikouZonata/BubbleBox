using UnityEngine;
using Utility;

public class CombiningEngine : SingletonMonoBehaviour<CombiningEngine>
{
	[SerializeField] CombiningRecipes _recipes = default;

	private void Start ()
	{
		//TestCombining();
	}

	public GameObject CombineIngredients (CombiningIngredients lhs, CombiningIngredients rhs)
	{
		foreach (CombiningRecipes.Recipe recipe in _recipes.recipes) {
			if (lhs == recipe.leftIngredient && rhs == recipe.rightIngredient) {
				return recipe.resultPrefab;
			} else if (lhs == recipe.rightIngredient && rhs == recipe.leftIngredient) {
				return recipe.resultPrefab;
			}
		}

		Debug.LogError("The combination of " + lhs + " & " + rhs + " yielded no result.");
		return new GameObject("Failed Combination!");
	}

	void TestCombining ()
	{
		Debug.Log("==Combining Test Start==");
		for (int i = 0; i < 50; i++) {
			CombiningIngredients leftIngredient = (CombiningIngredients) Random.Range(0, 4);
			CombiningIngredients rightIngredient = (CombiningIngredients) Random.Range(0, 4);
			GameObject result = CombineIngredients(leftIngredient, rightIngredient);
			Debug.Log("Result " + i + ", mixing " + leftIngredient + " with " + rightIngredient + " resulting in " + result.name + ".");
		}
	}
}
