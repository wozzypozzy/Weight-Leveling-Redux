using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using WeightLevelingRedux.Patches;

namespace WeightLevelingRedux
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "wozzy.spt.weightlevelingredux";
        public const string PluginName = "Weight Leveling Redux";
        public const string PluginVersion = "1.0.0";

        public static ManualLogSource LogSource;

        public static ConfigEntry<float> ReductionPerStep;
        public static ConfigEntry<int> LevelsPerStep;
        public static ConfigEntry<float> MaxReduction;

        private void Awake()
        {
            LogSource = Logger;

            ReductionPerStep = Config.Bind(
                "Weight",
                "Reduction Per Step (%)",
                1f,
                new ConfigDescription(
                    "Percent of item weight removed each time a step is reached. Total reduction = this value x number of completed steps (capped at 100%).",
                    new AcceptableValueRange<float>(0f, 100f)));

            LevelsPerStep = Config.Bind(
                "Weight",
                "Levels Per Step",
                1,
                new ConfigDescription(
                    "How many levels make up one step. 1 = reduction applies every level while 5 = every 5 levels (levels 1-4 give none, 5-9 give one step, etc.).",
                    new AcceptableValueRange<int>(1, 100)));

            MaxReduction = Config.Bind(
                "Weight",
                "Max Reduction (%)",
                100f,
                new ConfigDescription(
                    "Upper limit on the total weight reduction. 50 = items never weigh less than 50% of their normal weight, no matter how high your level is.",
                    new AcceptableValueRange<float>(0f, 100f)));

            ReductionPerStep.SettingChanged += (sender, args) => WeightPatch.Invalidate();
            LevelsPerStep.SettingChanged += (sender, args) => WeightPatch.Invalidate();
            MaxReduction.SettingChanged += (sender, args) => WeightPatch.Invalidate();

            LogSource.LogInfo("Loading Weight Patch");
            new WeightPatch().Enable();
        }
    }
}
