using BuildingThemes.HarmonyPatches.BuildingManagerPatch;
using BuildingThemes.HarmonyPatches.DistrictManagerPatch;
using BuildingThemes.HarmonyPatches.DistrictWorldInfoPanelPatch;
using BuildingThemes.HarmonyPatches.ImmaterialResourceManagerPatch;
using BuildingThemes.HarmonyPatches.PoliciesPanelPatch;
using BuildingThemes.HarmonyPatches.PrivateBuildingAIPatch;
using BuildingThemes.HarmonyPatches.ZoneBlockPatch;

namespace BuildingThemes.HarmonyPatches
{
    public static class Patcher
    {
        public static void PatchAll()
        {
            GetRandomBuildingInfoPatch.Deploy();
            ReleaseDistrictImplementationPatch.Deploy();
            SimulationStepPatch.Deploy();
            SimulationStepPatch.SetUp();
            AddResourcePatch.Deploy();
            GetUpgradeInfoPatch.Deploy();
            PoliciesPanelPatches.Deploy();
            OnPoliciesClickPatch.Deploy();
            AutoBulldozePatch.Deploy();
        }

        public static void UnpatchAll()
        {
            AutoBulldozePatch.Revert();
            OnPoliciesClickPatch.Revert();
            PoliciesPanelPatches.Revert();
            GetUpgradeInfoPatch.Revert();
            AddResourcePatch.Revert();
            SimulationStepPatch.Revert();
            ReleaseDistrictImplementationPatch.Revert();
            GetRandomBuildingInfoPatch.Revert();
        }
    }
}
