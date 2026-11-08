using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPT.Reflection.Utils;
using UnityEngine;

namespace WeightLevelingRedux.Patches
{
    internal class WeightPatch : ModulePatch
    {
        private static bool _cacheValid;
        private static int _cachedExperience;
        private static float _cachedMultiplier = 1f;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(Item), nameof(Item.Weight));
        }

        internal static void Invalidate()
        {
            _cacheValid = false;
        }

        [PatchPostfix]
        private static void Postfix(ref float __result)
        {
            if (!Singleton<GlobalConfiguration>.Instantiated)
            {
                return;
            }

            Profile profile = ((IProfileSession)ClientAppUtils.GetClientApp()?.Session)?.Profile;
            if (profile == null)
            {
                return;
            }

            __result *= GetMultiplier(profile.Experience);
        }

        private static float GetMultiplier(int experience)
        {
            if (_cacheValid && experience == _cachedExperience)
            {
                return _cachedMultiplier;
            }

            int level = ProfileInfo.GetLevel(experience);
            int steps = level / Plugin.LevelsPerStep.Value;
            float reduction = steps * Plugin.ReductionPerStep.Value * 0.01f;
            reduction = Mathf.Min(reduction, Plugin.MaxReduction.Value * 0.01f);

            _cachedMultiplier = Mathf.Clamp01(1f - reduction);
            _cachedExperience = experience;
            _cacheValid = true;
            return _cachedMultiplier;
        }
    }
}
