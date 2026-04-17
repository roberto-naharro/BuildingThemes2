using CitiesHarmony.API;
using System;

namespace BuildingThemes.HarmonyPatches.BuildingManagerPatch
{
    /// <summary>
    /// Hooks into BuildingManager.SimulationStep to run the auto-bulldoze scan once per tick.
    /// </summary>
    public static class AutoBulldozePatch
    {
        private static bool deployed;

        public static void Deploy()
        {
            if (!HarmonyHelper.IsHarmonyInstalled || deployed) return;

            PatchUtil.Patch(
                new PatchUtil.MethodDefinition(typeof(BuildingManager), "SimulationStep",
                    argumentTypes: new[] { typeof(int) }),
                postfix: new PatchUtil.MethodDefinition(typeof(AutoBulldozePatch), nameof(Postfix)));

            deployed = true;
            Debugger.Log("BuildingManager.SimulationStep patched (AutoBulldoze).");
        }

        public static void Revert()
        {
            if (!HarmonyHelper.IsHarmonyInstalled || !deployed) return;

            PatchUtil.Unpatch(
                new PatchUtil.MethodDefinition(typeof(BuildingManager), "SimulationStep",
                    argumentTypes: new[] { typeof(int) }));

            deployed = false;
            Debugger.Log("BuildingManager.SimulationStep unpatched (AutoBulldoze).");
        }

        public static void Postfix(int subStep)
        {
            // subStep cycles 0–3; run only on 0 to keep frequency reasonable
            if (subStep != 0) return;
            try
            {
                AutoBulldozeService.Tick();
            }
            catch (Exception e)
            {
                Debugger.LogException(e);
            }
        }
    }
}
