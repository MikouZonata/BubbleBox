﻿using UnityEngine;

namespace Utility
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
	{
		public static T instance
		{
			get; protected set;
		}

		protected virtual void Awake ()
		{
			if (instance != null && instance != this) {
				Destroy(this);
				throw new System.Exception("An instance of this singleton already exists.");
			} else {
				instance = (T) this;
			}
		}
	}
}