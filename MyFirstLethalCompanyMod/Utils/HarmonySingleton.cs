using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;

namespace MyFirstLethalCompanyMod.Utils
{
    [HarmonyPatch]
    public abstract class HarmonySingleton<T> where T : HarmonySingleton<T>
    {
        private static T? instance;
        protected static bool configLoaded = false;

        public static T? Instance => instance;

        static HarmonySingleton()
        {
            instance = Activator.CreateInstance<T>();
        }

        protected abstract Type TargetClass { get; }
        protected abstract string TargetMethodName { get; }
        protected virtual Type[] TargetMethodParameters => Type.EmptyTypes;

        static MethodBase? TargetMethod()
        {
            return AccessTools.Method(instance!.TargetClass, instance!.TargetMethodName, instance!.TargetMethodParameters);
        }

        [HarmonyPrefix]
        [HarmonyPatch]
        private static void ConfigPatch()
        {
            if (!configLoaded)
            {
                Plugin.Logger!.LogDebug("load config - harmony singleton");
                instance!.LoadConfig();
                configLoaded = true;
            }
        }

        public abstract void LoadConfig();
    }
}