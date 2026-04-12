using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MyFirstLethalCompanyMod.Utils
{
	// patch here so we dont need explicit instances created in the plugin script
	//[HarmonyPatch(???)]
	public abstract class Singleton<T> : object where T : Singleton<T>
	{
		private static T instance;
		public static T Instance
		{
			get
			{
				return instance;
			}
			protected set
			{
				if (instance == value)
					return;
				instance = value;
			}
		}

		public static bool hasInstance => instance != null;
	}
}
