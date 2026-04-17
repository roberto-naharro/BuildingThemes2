using ColossalFramework;

namespace BuildingThemes
{
    /// <summary>
    /// Gradually demolishes growable buildings that are not valid for their district's active themes.
    /// Called once per BuildingManager.SimulationStep cycle (subStep == 0).
    /// Only runs when at least one district has autoBulldoze enabled.
    /// </summary>
    internal static class AutoBulldozeService
    {
        // How many buildings to inspect each tick. Low enough to be imperceptible, high enough
        // to clear a district in a reasonable number of game days.
        private const int BatchSize = 16;

        private static ushort s_cursor = 0;

        internal static void Tick()
        {
            var mgr = BuildingThemesManager.instance;
            if (mgr == null) return;

            var bm = Singleton<BuildingManager>.instance;
            var dm = Singleton<DistrictManager>.instance;
            if (bm == null || dm == null) return;

            int size = (int)bm.m_buildings.m_size;
            int scanned = 0;

            while (scanned < BatchSize)
            {
                if (++s_cursor >= size) s_cursor = 1;
                scanned++;

                ref Building b = ref bm.m_buildings.m_buffer[s_cursor];

                // Skip empty slots
                if ((b.m_flags & Building.Flags.Created) == 0) continue;

                BuildingInfo info = b.Info;
                if (info == null) continue;

                // Only auto-growable zone buildings (residential / commercial / industrial / office)
                if (info.m_placementStyle != ItemClass.Placement.Automatic) continue;
                if (ItemClass.GetPrivateServiceIndex(info.m_class.m_service) == -1) continue;

                byte districtId = dm.GetDistrict(b.m_position);

                if (!mgr.GetDistrictAutoBulldoze(districtId)) continue;
                if (!mgr.IsThemeManagementEnabled(districtId)) continue;
                if (mgr.IsBlacklistModeEnabled(districtId)) continue;

                if (!mgr.IsBuildingValidForDistrict(s_cursor, districtId))
                    bm.ReleaseBuilding(s_cursor);
            }
        }
    }
}
