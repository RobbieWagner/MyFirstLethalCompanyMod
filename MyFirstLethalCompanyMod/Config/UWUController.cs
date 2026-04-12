using MyFirstLethalCompanyMod.Models;
using MyFirstLethalCompanyMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstLethalCompanyMod.Config
{
	public class UWUController : Singleton<UWUController>
	{

		public static List<UWUWord> words {get; private set;}

		public static void LoadConfig()
		{

		}

		public static string GetRandomUWUWord(UWUWordTag tag = UWUWordTag.NONE)
		{
			List<string> validWords = words.Where(word => tag == UWUWordTag.NONE || word.tags.Contains(tag)).Select(word => word.word).ToList();
			Random random = new Random();
			return validWords[random.Next(0, validWords.Count)];
		}
	}
}
