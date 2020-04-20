﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombinable
{
	CombiningIngredients Ingredient
	{
		get;
	}

	void Combine ();
}
