using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstLethalCompanyMod.Models
{
	public enum UWUWordTag
	{
		NONE = -1,
		HAPPY = 0,
		BASHFUL = 1,
		DEVIOUS = 2,
		SAD = 3
	}

	[System.Serializable]
	public class UWUWord
	{
		public string word;
		public List<UWUWordTag> tags = new List<UWUWordTag>();
	}
}
