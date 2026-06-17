using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using System;
using ColossalFramework.Plugins;
using System.IO;
using System.Text;
using UnityEngine;
using BuildingThemes.Diagnostics;

namespace BuildingThemes
{
    public class BuildingThemesManager : Singleton<BuildingThemesManager>
    {
        // From BuildingManager.
        private const int AreaBuildingsLength = 4480;

        private const string ModConfigPath = "BuildingThemes.xml";
        private readonly DistrictThemeInfo[] districtThemeInfos = new DistrictThemeInfo[128];
        private readonly FastList<ushort>[] m_areaBuildings = new FastList<ushort>[AreaBuildingsLength];
        private bool m_areaBuildingsDirty = true;
        private bool importedModThemes = false;
        private bool importedStyles = false;

        // Missing asset behavior — persisted to game settings (same mechanism as Debugger.Enabled)
        private static readonly ColossalFramework.SavedInt s_missingAssetMode =
            new ColossalFramework.SavedInt("missingAssetMode", "BuildingThemes2", (int)MissingAssetMode.FillWithVanilla, true);

        public static MissingAssetMode MissingAssetBehavior
        {
            get { return (MissingAssetMode)(int)s_missingAssetMode; }
            set { s_missingAssetMode.value = (int)value; }
        }

        // Empty level behavior — what to do when a theme has no buildings for a given level
        private static readonly ColossalFramework.SavedInt s_emptyLevelMode =
            new ColossalFramework.SavedInt("emptyLevelMode", "BuildingThemes2", (int)EmptyLevelBehavior.VanillaFallback, true);

        public static EmptyLevelBehavior EmptyLevelBehavior
        {
            get
            {
                var v = (EmptyLevelBehavior)(int)s_emptyLevelMode;
#pragma warning disable CS0618
                return v == EmptyLevelBehavior.CascadeFromTheme ? EmptyLevelBehavior.VanillaFallback : v;
#pragma warning restore CS0618
            }
            set { s_emptyLevelMode.value = (int)value; }
        }

        /// <summary>
        /// Returns true when the given district (or the city-wide district 0) has theme management
        /// enabled AND at least one theme assigned. Districts that are toggled on with no themes
        /// selected behave like vanilla: the spawn patch defers to the original code, leaving the
        /// district's native m_Style filter intact.
        /// </summary>
        public bool IsEffectivelyThemed(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info != null && info.isEnabled && info.themes != null && info.themes.Count > 0) return true;
            if (districtId != 0)
            {
                var c = districtThemeInfos[0];
                if (c != null && c.isEnabled && c.themes != null && c.themes.Count > 0) return true;
            }
            return false;
        }

        // --- Per-district setting accessors ---

        public MissingAssetMode GetDistrictMissingAssetMode(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info != null) return info.missingAssetMode;
            if (districtId != 0) { var c = districtThemeInfos[0]; if (c != null) return c.missingAssetMode; }
            return MissingAssetBehavior;
        }

        public void SetDistrictMissingAssetMode(byte districtId, MissingAssetMode mode)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || info.missingAssetMode == mode) return;
            info.missingAssetMode = mode;
            if (info.isEnabled) CompileDistrictThemes(districtId);
        }

        public EmptyLevelBehavior GetDistrictEmptyLevelBehavior(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info != null) return info.emptyLevelBehavior;
            if (districtId != 0) { var c = districtThemeInfos[0]; if (c != null) return c.emptyLevelBehavior; }
            return EmptyLevelBehavior;
        }

        public void SetDistrictEmptyLevelBehavior(byte districtId, EmptyLevelBehavior behavior)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || info.emptyLevelBehavior == behavior) return;
            info.emptyLevelBehavior = behavior;
            if (info.isEnabled) CompileDistrictThemes(districtId);
        }

        public bool GetDistrictAutoBulldoze(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info != null) return info.autoBulldoze;
            if (districtId != 0) { var c = districtThemeInfos[0]; if (c != null) return c.autoBulldoze; }
            return false;
        }

        public void SetDistrictAutoBulldoze(byte districtId, bool enabled)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || info.autoBulldoze == enabled) return;
            info.autoBulldoze = enabled;
        }

        public bool GetDistrictPreferElectricity(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info != null) return info.preferElectricity;
            if (districtId != 0) { var c = districtThemeInfos[0]; if (c != null) return c.preferElectricity; }
            return false;
        }

        public void SetDistrictPreferElectricity(byte districtId, bool enabled)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || info.preferElectricity == enabled) return;
            info.preferElectricity = enabled;
        }

        public bool GetDistrictPreferAdjacent(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info != null) return info.preferAdjacent;
            if (districtId != 0) { var c = districtThemeInfos[0]; if (c != null) return c.preferAdjacent; }
            return false;
        }

        public void SetDistrictPreferAdjacent(byte districtId, bool enabled)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || info.preferAdjacent == enabled) return;
            info.preferAdjacent = enabled;
        }

        public bool GetDistrictEnforceSpecialization(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info != null) return info.enforceSpecialization;
            if (districtId != 0) { var c = districtThemeInfos[0]; if (c != null) return c.enforceSpecialization; }
            return false;
        }

        public void SetDistrictEnforceSpecialization(byte districtId, bool enabled)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || info.enforceSpecialization == enabled) return;
            info.enforceSpecialization = enabled;
            AutoBulldozeService.ResetCursor();
        }

        public SizePreference GetDistrictSizePreference(byte districtId, ItemClass.Service service)
        {
            var info = districtThemeInfos[districtId];
            if (info == null) return SizePreference.Default;
            switch (service)
            {
                case ItemClass.Service.Residential: return info.residentialSizePref;
                case ItemClass.Service.Commercial: return info.commercialSizePref;
                case ItemClass.Service.Industrial: return info.industrialSizePref;
                case ItemClass.Service.Office: return info.officeSizePref;
                default: return SizePreference.Default;
            }
        }

        public void SetDistrictSizePreference(byte districtId, ItemClass.Service service, SizePreference pref)
        {
            var info = districtThemeInfos[districtId];
            if (info == null) return;
            switch (service)
            {
                case ItemClass.Service.Residential: info.residentialSizePref = pref; break;
                case ItemClass.Service.Commercial: info.commercialSizePref = pref; break;
                case ItemClass.Service.Industrial: info.industrialSizePref = pref; break;
                case ItemClass.Service.Office: info.officeSizePref = pref; break;
            }
        }

        public PreferenceStrength GetDistrictPreferenceStrength(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            return info != null ? info.strengthPref : PreferenceStrength.Moderate;
        }

        public void SetDistrictPreferenceStrength(byte districtId, PreferenceStrength s)
        {
            var info = districtThemeInfos[districtId];
            if (info != null) info.strengthPref = s;
        }

        public static float PreferenceStrengthToAlpha(PreferenceStrength s)
        {
            switch (s)
            {
                case PreferenceStrength.Gentle: return 0.5f;
                case PreferenceStrength.Strong: return 2.0f;
                default: return 1.0f;
            }
        }

        /// <summary>
        /// Returns the flat building entry list for a district (used by the size-preference path).
        /// Null when the district has no theme management or no entries.
        /// </summary>
        internal FastList<DistrictBuildingEntry> GetDistrictBuildingEntries(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || !info.isEnabled) return null;
            return info.buildingEntries;
        }

        // Flat per-building entry used by the size-preference selection path.
        internal struct DistrictBuildingEntry
        {
            public ushort prefabIndex;
            public int spawnWeight;
            public int cellWidth;
            public int cellLength;
            public float height;
            public ItemClass.SubService subService;
            public ItemClass.Level level;
            public BuildingInfo.ZoningMode zoningMode;
        }

        private class DistrictThemeInfo
        {
            /// <summary>
            /// False when theme management has been toggled off for this district in-session.
            /// The info object is kept so themes and options survive a disable/re-enable cycle.
            /// Set to null via ClearDistrictData() only when the district is actually deleted.
            /// </summary>
            public bool isEnabled = true;

            public bool blacklistMode = false;

            // Per-district behavior settings (default to global at creation time)
            public MissingAssetMode missingAssetMode = MissingAssetMode.FillWithVanilla;
            public EmptyLevelBehavior emptyLevelBehavior = EmptyLevelBehavior.VanillaFallback;

            /// <summary>When true, buildings not belonging to any active theme are gradually demolished.</summary>
            public bool autoBulldoze = false;

            /// <summary>When true, zone blocks without electricity conductivity are skipped during spawn.</summary>
            public bool preferElectricity = false;

            /// <summary>When true, spawn attempts at cells with no adjacent existing building are skipped
            /// until a fallback threshold lapses — so new buildings cluster against existing ones.</summary>
            public bool preferAdjacent = false;

            /// <summary>
            /// When true (and autoBulldoze is on), buildings that are in the active theme but whose
            /// sub-service does not match the district's active specialization are also demolished.
            /// e.g. a generic industrial building in a farming-specialised district gets removed.
            /// </summary>
            public bool enforceSpecialization = false;

            // Size preference per zone type
            public SizePreference residentialSizePref = SizePreference.Default;
            public SizePreference commercialSizePref = SizePreference.Default;
            public SizePreference industrialSizePref = SizePreference.Default;
            public SizePreference officeSizePref = SizePreference.Default;
            public PreferenceStrength strengthPref = PreferenceStrength.Moderate;

            public readonly HashSet<Configuration.Theme> themes = new HashSet<Configuration.Theme>();

            // similar to BuildingManager.m_areaBuildings, but separate for every district
            public readonly FastList<ushort>[] areaBuildings = new FastList<ushort>[AreaBuildingsLength];

            // Flat list for size-preference selection (rebuilt alongside areaBuildings)
            public FastList<DistrictBuildingEntry> buildingEntries = new FastList<DistrictBuildingEntry>();

            // building upgrade mapping (prefabLevel1 --> prefabLevel2) for realistic building upgrades
            public readonly Dictionary<ushort, ushort> upgradeBuildings = new Dictionary<ushort, ushort>();
        }

        private const string userConfigPath = "BuildingThemes.xml";
        private Configuration _configuration;
        internal Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    string fullPath;
                    try { fullPath = Path.GetFullPath(userConfigPath); }
                    catch { fullPath = userConfigPath; }

                    try
                    {
                        _configuration = Configuration.Deserialize(userConfigPath);
                        Debugger.xmlCorrupt = false;

                        // Always-on so we can compare the LOAD path/count against the SAVE path
                        // logged by SaveConfig — a mismatch explains "my theme didn't save".
                        if (_configuration != null)
                        {
                            Debugger.LogAlways("[BT2:Persistence] Loaded " + _configuration.themes.Count +
                                " theme(s) from: " + fullPath);
                        }
                        else
                        {
                            Debugger.LogAlways("[BT2:Persistence] No existing config file at: " + fullPath +
                                " — a new one will be created here on the first save.");
                        }
                    }
                    catch (Exception e)
                    {
                        Debugger.xmlCorrupt = true;
                        Debugger.LogError("[BT2:Persistence] Configuration load FAILED from: " + fullPath +
                            " — the file may be corrupt. Check Player.log for details.");
                        Debugger.LogException(e);
                    }

                    if (_configuration == null && !Debugger.xmlCorrupt)
                    {
                        _configuration = new Configuration();
                        try { SaveConfig(); }
                        catch (Exception e)
                        {
                            Debugger.LogError("Could not create BuildingThemes.xml at: " + fullPath);
                            Debugger.LogException(e);
                        }
                    }
                }

                return _configuration;
            }
        }

        internal void SaveConfig()
        {
            if (_configuration == null) return;

            // Resolve the absolute target so the log shows exactly where the file goes.
            // userConfigPath is relative, so it depends on the game's working directory.
            string fullPath;
            try { fullPath = Path.GetFullPath(userConfigPath); }
            catch { fullPath = userConfigPath; }

            try
            {
                Configuration.Serialize(userConfigPath, _configuration);
                Debugger.LogAlways("[BT2:Persistence] Saved " + _configuration.themes.Count +
                    " theme(s) to: " + fullPath);
            }
            catch (Exception e)
            {
                // Never let a save failure propagate into the UI update loop. Log it loudly
                // (always-on) so affected users can share their log: this is the smoking gun
                // for "my custom theme disappears after restarting the game".
                Debugger.LogError("[BT2:Persistence] FAILED to save themes to: " + fullPath +
                    " — your custom themes will be lost on reload. Most likely the game's " +
                    "working directory is not writable. Exception follows.");
                Debugger.LogException(e);
            }
        }

        // Maps prefab name -> themes imported from a built-in DistrictStyle that contain it.
        // Populated during AddStyleTheme. Used by the per-building origin label to surface the
        // owning style when the prefab itself carries no expansion / modder-pack mask (the
        // canonical case is base-game European content on PC, which has neither flag set but
        // still belongs to the European DistrictStyle).
        private static readonly System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Configuration.Theme>> s_prefabBuiltInThemes
            = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Configuration.Theme>>();

        public static System.Collections.Generic.IList<Configuration.Theme> GetBuiltInThemesForPrefab(string prefabName)
        {
            System.Collections.Generic.List<Configuration.Theme> list;
            return s_prefabBuiltInThemes.TryGetValue(prefabName, out list) ? list : null;
        }

        public void Reset()
        {
            for (int d = 0; d < districtThemeInfos.Length; d++)
            {
                districtThemeInfos[d] = null;
            }

            for (int i = 0; i < m_areaBuildings.Length; i++)
            {
                m_areaBuildings[i] = null;
            }

            _configuration = null;
            m_areaBuildingsDirty = true;
            importedModThemes = false;
            importedStyles = false;
            s_prefabBuiltInThemes.Clear();
            ThemeDiagnostics.Reset();
        }

        public void ImportThemes()
        {
            if (!importedModThemes) ImportThemesFromThemeMods();
            if (!importedStyles) ImportStylesAsThemes();
        }


        public void ImportThemesFromThemeMods()
        {
            // Guard against double-import: ApplyConfiguration calls this directly AND
            // ApplyPendingConfiguration calls it again via a second ApplyConfiguration.
            // The second call is redundant and dangerous — FilterEnvThemeBuildings calls
            // PrefabCollection.FindLoaded(), which triggers LSM's patched ResolveLegacyPrefab.
            // That patch lazily constructs a Texture2D, which must run on the main thread; the
            // deferred SimulationStep context is NOT the main thread, causing a native crash.
            if (importedModThemes) return;

            foreach (var pluginInfo in Singleton<PluginManager>.instance.GetPluginsInfo().Where(pluginInfo => pluginInfo.isEnabled))
            {
                try
                {
                    var config = Configuration.Deserialize(Path.Combine(pluginInfo.modPath, ModConfigPath));
                    if (config == null)
                    {
                        continue;
                    }
                    bool isBt2Bundle = pluginInfo.userModInstance is BuildingThemesMod;
                    foreach (var theme in config.themes)
                    {
                        AddModTheme(theme, pluginInfo.name, isBt2Bundle);
                    }
                }
                catch (Exception e)
                {
                    Debugger.Log("Error while parsing BuildingThemes.xml of mod " + pluginInfo.name);
                    Debugger.LogException(e);
                }
            }

            importedModThemes = true;
        }

        public void ImportStylesAsThemes()
        {
            var styles = DistrictManager.instance.m_Styles;
            if (styles == null)
            {
                return;
            }
            bool hasEuropeanStyle = false;
            foreach (var style in styles)
            {
                try
                {
                    if (IsEuropeanStyle(style)) hasEuropeanStyle = true;
                    AddStyleTheme(style);
                }
                catch (Exception e)
                {
                    Debugger.Log("Error while importing style " + style.FullName);
                    Debugger.LogException(e);
                }
            }

            if (!hasEuropeanStyle)
            {
                Debugger.Log("European district style not detected in active styles. " +
                             "Skipping European theme import (likely disabled in Content Manager or DLC not active).");
            }

            // No auto-enable on load: a district with a vanilla m_Style set keeps its vanilla
            // behaviour. The user opts in to theme management explicitly via the Policies tab,
            // and the per-district opt-in code clears m_Style at that point. This restores
            // exact vanilla parity for players who never touch the mod's UI.

            importedStyles = true;
        }

        // BT2's bundled env themes: each maps to exactly one expansion source. Buildings whose
        // prefab requires a different DLC (or a modder pack) are filtered out at import time so
        // each theme is a clean, single-source representation. International is base-game-only
        // Maps every bundled theme name to its required expansion for ownership gating.
        // Env themes (International, Snowfall) are a subset — they also run through
        // FilterEnvThemeBuildings to enforce single-DLC-source purity.
        // DLC-only themes (AfterDark, GreenCities, etc.) are pre-filtered in the XML.
        // European comes from AddStyleTheme via the real DistrictStyle, not from this table.
        private static readonly System.Collections.Generic.Dictionary<string, SteamHelper.ExpansionBitMask> s_envThemeExpansion =
            new System.Collections.Generic.Dictionary<string, SteamHelper.ExpansionBitMask>
        {
            { "International",       SteamHelper.ExpansionBitMask.None                },
            { "Snowfall",            SteamHelper.ExpansionBitMask.SnowFall            },
            { "AfterDark",           SteamHelper.ExpansionBitMask.AfterDark           },
            { "GreenCities",         SteamHelper.ExpansionBitMask.GreenCities         },
            { "PlazasAndPromenades", SteamHelper.ExpansionBitMask.PlazasAndPromenades },
            { "FinancialDistricts",  SteamHelper.ExpansionBitMask.FinancialDistricts  },
        };

        private void AddModTheme(Configuration.Theme modTheme, string modName, bool isBundled = false)
        {
            if (modTheme == null)
            {
                return;
            }
            // Bundled XML themes pass the theme name as stylePackage so displayName resolves to
            // "[Vanilla] <name>" / "[DLC] <name>" rather than "[Custom] <name>". Other mods'
            // themes keep stylePackage=null → "[Custom]".
            string stylePackage = isBundled ? modTheme.name : null;

            // Check ownership gate for all recognised bundled themes.
            SteamHelper.ExpansionBitMask requiredExpansion = SteamHelper.ExpansionBitMask.None;
            bool isBundledKnown = isBundled && s_envThemeExpansion.TryGetValue(modTheme.name, out requiredExpansion);
            // Env-sourced themes additionally need DLC-source filtering; DLC themes are pre-filtered in XML.
            bool isEnvTheme = isBundledKnown && (modTheme.name == "International" || modTheme.name == "Snowfall");

            if (isBundledKnown && requiredExpansion != SteamHelper.ExpansionBitMask.None)
            {
                // Hide the theme entirely when the player does not own the required expansion.
                // Matches vanilla behaviour for DLC-locked content (the styles simply do not appear).
                var owned = SteamHelper.GetOwnedExpansionMask();
                if ((owned & requiredExpansion) == SteamHelper.ExpansionBitMask.None)
                {
                    Debugger.LogFormat(
                        "Skipping bundled theme \"{0}\": required expansion not owned.",
                        modTheme.name);
                    return;
                }
            }
            var buildingsToImport = modTheme.buildings;

            if (isEnvTheme)
            {
                buildingsToImport = FilterEnvThemeBuildings(modTheme.buildings, requiredExpansion);
            }

            // For DLC-expansion themes, use the official DLC display name instead of the
            // internal XML key (e.g. "After Dark" instead of "AfterDark").
            string themeName = (!isEnvTheme && isBundledKnown && requiredExpansion != SteamHelper.ExpansionBitMask.None)
                ? DlcNames.GetExpansionName(requiredExpansion)
                : modTheme.name;

            var theme = AddImportedTheme(buildingsToImport, themeName, stylePackage);

            if (isBundledKnown)
            {
                theme.isDlc = requiredExpansion != SteamHelper.ExpansionBitMask.None;
            }

            Debugger.LogFormat(
                "Imported theme from mod \"{0}\" as theme \"{1}\". Buildings in mod: {2}. Buildings in theme: {3} ",
                modName, theme.name, modTheme.buildings.Count, theme.buildings.Count);
        }

        // Keep only buildings whose runtime prefab matches the env theme's single DLC source.
        // Modder-pack-tagged prefabs are always dropped (they belong to their pack's own
        // DistrictStyle theme). Unloaded prefabs are always kept regardless of expansion type:
        // base-game buildings absent on the current map type (e.g. EU buildings on Sunny) should
        // surface as missing assets rather than silently emptying the theme.
        private static List<Configuration.Building> FilterEnvThemeBuildings(
            List<Configuration.Building> input, SteamHelper.ExpansionBitMask requiredExpansion)
        {
            var result = new List<Configuration.Building>(input.Count);
            int droppedModderPack = 0, droppedWrongExpansion = 0;
            foreach (var b in input)
            {
                if (b == null || string.IsNullOrEmpty(b.name)) continue;
                var prefab = PrefabCollection<BuildingInfo>.FindLoaded(b.name);
                if (prefab == null)
                {
                    result.Add(b);
                    continue;
                }
                if (prefab.m_requiredModderPack != SteamHelper.ModderPackBitMask.None) { droppedModderPack++; continue; }
                if (prefab.m_requiredExpansion != requiredExpansion) { droppedWrongExpansion++; continue; }
                result.Add(b);
            }
            Debugger.LogFormat(
                "FilterEnvThemeBuildings (expansion={0}): in={1} kept={2} | dropped: modderPack={3} wrongExpansion={4}",
                requiredExpansion, input.Count, result.Count,
                droppedModderPack, droppedWrongExpansion);
            return result;
        }

        private static bool IsEuropeanStyle(DistrictStyle style)
        {
            return style != null && style.BuiltIn &&
                   (style.Name == DistrictStyle.kEuropeanStyleName
                    || style.PackageName == DistrictStyle.kEuropeanStyleName
                    || style.FullName == DistrictStyle.kEuropeanStyleName);
        }

        private void AddStyleTheme(DistrictStyle style)
        {
            if (style == null)
            {
                return;
            }

            var allBuildingInfos = style.GetBuildingInfos();

            // Derive DLC masks for this style by OR-ing the bits of all its buildings
            // (including non-growable ones, which reliably carry the correct DLC bits).
            var packMask = SteamHelper.ModderPackBitMask.None;
            var expansionMask = SteamHelper.ExpansionBitMask.None;
            if (allBuildingInfos != null)
                foreach (var b in allBuildingInfos)
                {
                    if (b == null) continue;
                    packMask |= b.m_requiredModderPack;
                    expansionMask |= b.m_requiredExpansion;
                }

            // Collect growable zone buildings (Placement.Automatic) from the style.
            // Non-growable buildings (unique, service, parks, sub-buildings) are skipped:
            // they cannot spawn in zones and would show as "(Not Loaded)" in the Theme Manager.
            var styleNames = new System.Collections.Generic.HashSet<string>();
            var buildings = new System.Collections.Generic.List<Configuration.Building>();
            if (allBuildingInfos != null)
            {
                foreach (var b in allBuildingInfos)
                {
                    if (b == null || b.m_placementStyle != ItemClass.Placement.Automatic) continue;
                    if (styleNames.Add(b.name))
                        buildings.Add(new Configuration.Building { name = b.name, fromStyle = true });
                }
            }

            // If this is a paid DLC pack, skip it entirely when the player doesn't own it.
            bool isModderPackDlc = packMask != SteamHelper.ModderPackBitMask.None;
            bool isExpansionDlc = expansionMask != SteamHelper.ExpansionBitMask.None;

            if (isModderPackDlc)
            {
                var ownedMask = SteamHelper.GetOwnedModderPackMask();
                if ((ownedMask & packMask) == SteamHelper.ModderPackBitMask.None)
                {
                    Debugger.LogFormat("Skipping style \"{0}\": modder pack DLC not owned (mask {1}).", style.FullName, packMask);
                    return;
                }
            }

            if (isExpansionDlc)
            {
                var ownedMask = SteamHelper.GetOwnedExpansionMask();
                if ((ownedMask & expansionMask) == SteamHelper.ExpansionBitMask.None)
                {
                    Debugger.LogFormat("Skipping style \"{0}\": expansion DLC not owned (mask {1}).", style.FullName, expansionMask);
                    return;
                }
            }

            // Some DLC packs tag their growable buildings with m_requiredModderPack but do not
            // register them all in the DistrictStyle (e.g. Heart of Korea KR R* residential).
            // Scan PrefabCollection for any Placement.Automatic building that carries this
            // style's pack bit and hasn't been added yet. Works for any current or future DLC.
            if (packMask != SteamHelper.ModderPackBitMask.None)
            {
                uint prefabCount = (uint)PrefabCollection<BuildingInfo>.PrefabCount();
                for (uint i = 0; i < prefabCount; i++)
                {
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetPrefab(i);
                    if (prefab == null) continue;
                    if (prefab.m_placementStyle != ItemClass.Placement.Automatic) continue;
                    if ((prefab.m_requiredModderPack & packMask) == SteamHelper.ModderPackBitMask.None) continue;
                    if (!styleNames.Add(prefab.name)) continue;
                    buildings.Add(new Configuration.Building { name = prefab.name, fromStyle = true });
                }
            }

            if (buildings.Count == 0)
                Debugger.LogFormat(
                    "WARNING: Style \"{0}\" imported with 0 growable buildings. " +
                    "If District Styles Plus is installed, this style may not yet be initialized — " +
                    "it will be re-imported after level load.", style.FullName);

            bool isEuropeanStyle = IsEuropeanStyle(style);

            string themeName = isEuropeanStyle ? "European" : FormatStyleName(style);
            string stylePackage = isEuropeanStyle ? DistrictStyle.kEuropeanStyleName : style.PackageName;

            var theme = AddImportedTheme(buildings, themeName, stylePackage);
            // European is shipped as an enable-able content pack (free on PC but still a
            // separate pack the player must turn on), so it carries the [DLC] prefix even
            // though its expansion/modder-pack masks resolve to None.
            theme.isDlc = isModderPackDlc || isExpansionDlc || isEuropeanStyle;

            // Record style membership so the per-building origin label can surface this style
            // even for buildings whose prefab masks are both None (e.g. base-game European
            // content on PC). General mechanism: any built-in DistrictStyle a prefab belongs to
            // contributes to its "Included in ..." label.
            if (style.BuiltIn)
            {
                foreach (var b in buildings)
                {
                    System.Collections.Generic.List<Configuration.Theme> list;
                    if (!s_prefabBuiltInThemes.TryGetValue(b.name, out list))
                    {
                        list = new System.Collections.Generic.List<Configuration.Theme>();
                        s_prefabBuiltInThemes[b.name] = list;
                    }
                    if (!list.Contains(theme)) list.Add(theme);
                }
            }

            // Wire up the locale key so the UI can show the localized style name.
            theme.localeKey = DlcNames.GetStyleLocaleKey(style.Name);

            Debugger.LogFormat(
                "Imported style \"{0}\" as theme \"{1}\". isDlc={2}. Style buildings: {3}. Theme buildings: {4}.",
                style.FullName, theme.name, theme.isDlc,
                allBuildingInfos != null ? allBuildingInfos.Length : 0,
                theme.buildings.Count);
        }

        private static string FormatStyleName(DistrictStyle style)
        {
            if (style.BuiltIn)
            {
                if (style.Name == DistrictStyle.kEuropeanStyleName) return "European";
                if (style.Name == DistrictStyle.kEuropeanSuburbiaStyleName) return "European Suburbia";
            }

            return style.Name;
        }

        private Configuration.Theme AddImportedTheme(List<Configuration.Building> builtInBuildings, string themeName, string stylePackage)
        {
            // Lookup strategy: built-in style / env themes match by stylePackage so a user theme
            // that happens to share the name (e.g. user created a custom "International") is left
            // alone instead of being silently adopted as a built-in. Themes without a stylePackage
            // (user themes, other mods' themes) keep the legacy name-based lookup.
            Configuration.Theme theme = stylePackage != null
                ? GetThemeByStylePackage(stylePackage)
                : Configuration.getTheme(themeName);

            if (theme == null)
            {
                theme = new Configuration.Theme
                {
                    name = themeName,
                    stylePackage = stylePackage
                };
                Configuration.themes.Add(theme);
            }
            theme.isBuiltIn = true;

            // For style-based themes, re-import is authoritative: clear any buildings that came
            // from a previous (possibly empty) style import before adding the new list.
            if (stylePackage != null)
                theme.buildings.RemoveAll(b => b.fromStyle);

            if (builtInBuildings != null)
            {
                foreach (var builtInBuilding in builtInBuildings)
                {
                    var building = theme.getBuilding(builtInBuilding.name);
                    if (building == null)
                    {
                        building = new Configuration.Building(builtInBuilding);
                        theme.buildings.Add(building);
                    }
                    else if (building.builtInBuilding == null)
                    {
                        building.builtInBuilding = builtInBuilding;
                    }
                    building.fromStyle = builtInBuilding.fromStyle;
                }
            }

            return theme;
        }

        public void EnableTheme(byte districtId, Configuration.Theme theme)
        {
            if (!IsThemeManagementEnabled(districtId)) return;

            if (Debugger.Enabled)
            {
                Debugger.LogFormat("BuildingThemesManager. Enabling theme {0} for district {1}.", theme.name, districtId);
            }
            var themes = GetDistrictThemes(districtId, true);

            if (!districtThemeInfos[districtId].themes.Add(theme))
            {
                if (Debugger.Enabled)
                {
                    Debugger.LogFormat("BuildingThemesManager. Theme {0} was already enabled for district {1}.", theme.name, districtId);
                }
                return;
            }

            CompileDistrictThemes(districtId);
        }

        public void DisableTheme(byte districtId, Configuration.Theme theme)
        {
            if (!IsThemeManagementEnabled(districtId)) return;

            if (Debugger.Enabled)
            {
                Debugger.LogFormat("BuildingThemesManager. Disabling theme {0} for district {1}.", theme.name, districtId);
            }

            if (!districtThemeInfos[districtId].themes.Remove(theme))
            {
                if (Debugger.Enabled)
                {
                    Debugger.LogFormat("BuildingThemesManager. Theme {0} was already disabled for district {1}.", theme.name, districtId);
                }
                return;
            }

            CompileDistrictThemes(districtId);
        }

        public void RefreshDistrictThemeInfos()
        {
            for (byte d = 0; d < districtThemeInfos.Length; d++)
            {
                var info = districtThemeInfos[d];
                if (info == null || !info.isEnabled) continue;

                // Remove themes which are no longer listed in the configuration
                info.themes.RemoveWhere(theme => !Configuration.themes.Contains(theme));

                CompileDistrictThemes(d);
            }
        }

        /// <summary>
        /// Restores saved per-district behavior overrides after setThemeInfo has been called.
        /// Values of -1 are ignored (sentinel for "not saved / use global default").
        /// Triggers one recompile only if a value actually changed.
        /// </summary>
        public void RestoreDistrictBehavior(byte districtId, int savedMissingMode, int savedEmptyMode)
        {
            var info = districtThemeInfos[districtId];
            if (info == null) return;
            bool changed = false;
            if (savedMissingMode >= 0 && (int)info.missingAssetMode != savedMissingMode)
            {
                info.missingAssetMode = (MissingAssetMode)savedMissingMode;
                changed = true;
            }
            if (savedEmptyMode >= 0 && (int)info.emptyLevelBehavior != savedEmptyMode)
            {
#pragma warning disable CS0618
                if (savedEmptyMode == (int)EmptyLevelBehavior.CascadeFromTheme)
                    savedEmptyMode = (int)EmptyLevelBehavior.VanillaFallback;
#pragma warning restore CS0618
                info.emptyLevelBehavior = (EmptyLevelBehavior)savedEmptyMode;
                changed = true;
            }
            if (changed) CompileDistrictThemes(districtId);
        }

        public void setThemeInfo(byte districtId, HashSet<Configuration.Theme> themes, bool blacklistMode)
        {
            var info = districtThemeInfos[districtId];

            if (info == null)
            {
                info = new DistrictThemeInfo();
                // Inherit global defaults so a new district starts with the user's preferred behaviour
                info.missingAssetMode = MissingAssetBehavior;
                info.emptyLevelBehavior = EmptyLevelBehavior;
                districtThemeInfos[districtId] = info;
            }
            else
            {
                info.themes.Clear();
                info.upgradeBuildings.Clear();
            }

            info.blacklistMode = blacklistMode;

            // Add the themes to the district theme info
            info.themes.UnionWith(themes);

            CompileDistrictThemes(districtId);
        }

        //TODO move this method to DistrictThemeInfo class?
        public void CompileDistrictThemes(byte districtId)
        {
            var info = districtThemeInfos[districtId];

            if (info == null || !info.isEnabled) return;

            HashSet<Configuration.Theme> enabledThemes = info.themes;
            HashSet<Configuration.Theme> blacklistedThemes = null;

            if (info.blacklistMode)
            {
                blacklistedThemes = new HashSet<Configuration.Theme>(GetAllThemes());
                blacklistedThemes.ExceptWith(info.themes);
            }

            if (Debugger.Enabled)
            {
                Debugger.LogFormat("Compiling theme data for district {0}. Enabled Themes: {1}, Blacklist Themes: {2}",
                    districtId, enabledThemes.Count, blacklistedThemes == null ? 0 : blacklistedThemes.Count);
                ThemeDiagnostics.BeginCompile(districtId);
            }

            // Ensure the vanilla area-buildings list is populated.
            // Required for FillWithVanilla / FallbackToVanilla miss-asset handling.
            if (m_areaBuildingsDirty)
            {
                RefreshAreaBuildings(m_areaBuildings, null, null, false);
                m_areaBuildingsDirty = false;
            }

            // Create custom areaBuildings fastlist array for this district
            RefreshAreaBuildings(info.areaBuildings, enabledThemes, blacklistedThemes, true, districtId, info.buildingEntries);

            // Create upgrade mapping
            info.upgradeBuildings.Clear();
            foreach (var theme in enabledThemes)
            {
                foreach (var building in theme.buildings)
                {
                    if (building.upgradeName == null) continue;

                    var fromPrefab = PrefabCollection<BuildingInfo>.FindLoaded(building.name)
                        ?? FindLoadedBySteamPrefix(building.name);
                    var toPrefab = PrefabCollection<BuildingInfo>.FindLoaded(building.upgradeName)
                        ?? FindLoadedBySteamPrefix(building.upgradeName);

                    if (fromPrefab != null && toPrefab != null && !info.upgradeBuildings.ContainsKey((ushort)fromPrefab.m_prefabDataIndex))
                    {
                        info.upgradeBuildings.Add((ushort)fromPrefab.m_prefabDataIndex, (ushort)toPrefab.m_prefabDataIndex);
                    }
                }
            }

            if (Debugger.Enabled)
            {
                Debugger.LogFormat("Upgrade Mappings in district {0}: {1}", districtId, info.upgradeBuildings.Count);
                ThemeDiagnostics.LogReport(districtId);
            }

            // Reset the auto-bulldoze cursor so newly-invalid buildings are caught in the
            // next scan cycle rather than waiting for the cursor to loop around naturally.
            AutoBulldozeService.ResetCursor();
        }

        public void ToggleThemeManagement(byte districtId, bool enabled)
        {
            if (enabled == IsThemeManagementEnabled(districtId)) return;

            if (enabled)
            {
                var info = districtThemeInfos[districtId];
                if (info != null)
                {
                    // Restore from preserved state — themes and options are still intact
                    info.isEnabled = true;
                    CompileDistrictThemes(districtId);
                }
                else
                {
                    // First time enabling — create info with defaults
                    setThemeInfo(districtId, getDefaultThemes(districtId), IsBlacklistModeEnabled(0));
                }
            }
            else
            {
                var info = districtThemeInfos[districtId];
                if (info != null) info.isEnabled = false;
            }
        }

        public bool IsThemeManagementEnabled(byte districtId)
        {
            var info = districtThemeInfos[districtId];
            return info != null && info.isEnabled;
        }

        /// <summary>
        /// Permanently wipes all theme data for a district.
        /// Called only when the district is deleted by the player.
        /// Use ToggleThemeManagement(id, false) to disable while preserving data for re-enable.
        /// </summary>
        public void ClearDistrictData(byte districtId)
        {
            districtThemeInfos[districtId] = null;
        }

        public void ToggleBlacklistMode(byte districtId, bool enabled)
        {
            if (!IsThemeManagementEnabled(districtId) || enabled == IsBlacklistModeEnabled(districtId)) return;

            districtThemeInfos[districtId].blacklistMode = enabled;
            CompileDistrictThemes(districtId);
        }

        public bool IsBlacklistModeEnabled(byte districtId)
        {
            var info = districtThemeInfos[districtId];

            if (info != null)
            {
                return info.blacklistMode;
            }
            else if (districtId != 0)
            {
                return IsBlacklistModeEnabled(0);
            }
            else
            {
                return false;
            }
        }

        public BuildingInfo GetUpgradeBuildingInfo(ushort prefabIndex, byte districtId)
        {
            var info = districtThemeInfos[districtId];

            if (info == null)
            {
                return null;
            }

            ushort upgradePrefabIndex;
            if (!info.upgradeBuildings.TryGetValue(prefabIndex, out upgradePrefabIndex))
            {
                return null;
            }

            return PrefabCollection<BuildingInfo>.GetPrefab(upgradePrefabIndex);
        }

        private void RefreshAreaBuildings(FastList<ushort>[] m_areaBuildings, HashSet<Configuration.Theme> enabledThemes, HashSet<Configuration.Theme> blacklistedThemes, bool includeVariations, byte diagnosticsDistrictId = 255, FastList<DistrictBuildingEntry> buildingEntries = null)
        {
            bool recordDiagnostics = Debugger.Enabled && diagnosticsDistrictId != 255;
            if (buildingEntries != null) buildingEntries.Clear();

            int areaBuildingsLength = m_areaBuildings.Length;
            for (int i = 0; i < areaBuildingsLength; i++)
            {
                m_areaBuildings[i] = null;
            }
            int prefabCount = PrefabCollection<BuildingInfo>.PrefabCount();
            for (int j = 0; j < prefabCount; j++)
            {
                BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetPrefab((uint)j);
                if (prefab != null && prefab.m_class.m_service != ItemClass.Service.None && prefab.m_placementStyle == ItemClass.Placement.Automatic && prefab.m_class.m_service <= ItemClass.Service.Office)
                {
                    int privateServiceIndex = ItemClass.GetPrivateServiceIndex(prefab.m_class.m_service);

                    if (privateServiceIndex != -1)
                    {
                        if (prefab.m_cellWidth < 1 || prefab.m_cellWidth > 4 || prefab.m_cellLength < 1 || prefab.m_cellLength > 4)
                        {
                            continue;
                        }
                        else
                        {
                            // mod begin
                            if (!includeVariations && BuildingVariationManager.instance.IsVariation(prefab.name))
                            {
                                if (recordDiagnostics) ThemeDiagnostics.RecordBuilding(diagnosticsDistrictId, prefab.name, RejectionReason.Variation);
                                continue;
                            }

                            // Determine spawn weight from the first enabled theme that includes this building.
                            // -1  = not found in any theme
                            // 1–100 = accepted; building is added N times to the pool
                            int spawnRate = -1;
                            bool markCorner = false;  // user opted this building in as a corner building

                            if (enabledThemes != null && enabledThemes.Count > 0)
                            {
                                foreach (var theme in enabledThemes)
                                {
                                    // Exact-name match only. The Steam-prefix fallback was removed:
                                    // a workshop ID identifies a PACK, not a building, so it leaked
                                    // sibling assets (e.g. "Small mediterranean 4" matching a theme
                                    // that only includes "…3"). A renamed asset now surfaces as a
                                    // visible "missing" row instead, which is correct and re-addable.
                                    var building = theme.getBuilding(prefab.name);

                                    if (building != null && building.include)
                                    {
                                        markCorner = building.markAsCorner;
                                        // Migrate legacy spawnRate=0: old behaviour silently excluded
                                        // the building; now we make that explicit via include=false.
                                        if (building.spawnRate == 0)
                                        {
                                            Debugger.LogFormat(
                                                "[Migration] Building '{0}' in theme '{1}' had spawnRate=0 — disabling it (include=false, spawnRate reset to 10).",
                                                prefab.name, theme.name);
                                            building.include = false;
                                            building.spawnRate = 10;
                                            break; // spawnRate stays -1 → treated as excluded
                                        }

                                        // Migrate legacy 1–1000 scale: original mod used max=1000,
                                        // new scale is max=100. Values 1–100 are valid as-is;
                                        // values >100 are divided by 10 (1000→100, 500→50, etc.).
                                        if (building.spawnRate > 100)
                                        {
                                            int oldRate = building.spawnRate;
                                            building.spawnRate = Mathf.Max(1, building.spawnRate / 10);
                                            Debugger.LogFormat(
                                                "[Migration] Building '{0}' in theme '{1}' had spawnRate={2} (old 1\u20131000 scale) \u2014 remapped to {3}.",
                                                prefab.name, theme.name, oldRate, building.spawnRate);
                                        }

                                        spawnRate = Mathf.Clamp(building.spawnRate, 1, 100);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // No themes configured — treat as weight 1 (vanilla behaviour)
                                spawnRate = 1;
                            }

                            if (spawnRate == -1 && blacklistedThemes != null)
                            {
                                bool onBlacklist = false;

                                foreach (var theme in blacklistedThemes)
                                {
                                    // Exact-name match only (see note on the enabled-themes loop above).
                                    var building = theme.getBuilding(prefab.name);

                                    if (building != null && building.include)
                                    {
                                        onBlacklist = true;
                                        break;
                                    }
                                }

                                if (onBlacklist)
                                {
                                    if (recordDiagnostics) ThemeDiagnostics.RecordBuilding(diagnosticsDistrictId, prefab.name, RejectionReason.NotInTheme);
                                    continue;
                                }
                                else
                                {
                                    // Not blacklisted — use default weight
                                    spawnRate = 10;
                                }
                            }

                            if (spawnRate == -1)
                            {
                                if (recordDiagnostics) ThemeDiagnostics.RecordBuilding(diagnosticsDistrictId, prefab.name, RejectionReason.NotInTheme);
                                continue;
                            }

                            if (recordDiagnostics) ThemeDiagnostics.RecordBuilding(diagnosticsDistrictId, prefab.name, RejectionReason.Accepted);

                            // mod end

                            // Append to flat entry list for size-preference selection path
                            if (buildingEntries != null)
                            {
                                float entryHeight = 0f;
                                var gen = prefab.m_generatedInfo;
                                if (gen != null && gen.m_heights != null)
                                    for (int h = 0; h < gen.m_heights.Length; h++)
                                        if (gen.m_heights[h] > entryHeight) entryHeight = gen.m_heights[h];

                                buildingEntries.Add(new DistrictBuildingEntry
                                {
                                    prefabIndex = (ushort)j,
                                    spawnWeight = spawnRate,
                                    cellWidth = prefab.m_cellWidth,
                                    cellLength = prefab.m_cellLength,
                                    height = entryHeight,
                                    subService = prefab.m_class.m_subService,
                                    level = prefab.m_class.m_level,
                                    zoningMode = prefab.m_zoningMode,
                                });
                            }

                            int areaIndex = GetAreaIndex(prefab.m_class.m_service, prefab.m_class.m_subService, prefab.m_class.m_level, prefab.m_cellWidth, prefab.m_cellLength, prefab.m_zoningMode);

                            // Corner census: log every CornerLeft/CornerRight building that enters
                            // the pool, so we can confirm a theme's "corner buildings" actually have
                            // corner zoning (only those can fill corner lots). If this prints nothing
                            // for a district, the theme has no true corner-zoned assets and corners
                            // will always fall back to straight buildings.
                            if (recordDiagnostics && prefab.m_zoningMode != BuildingInfo.ZoningMode.Straight)
                                Debugger.LogFormat("[CornerPool] district {0}: {1} zoning={2} size={3}x{4} sub={5} -> areaIndex={6}",
                                    diagnosticsDistrictId, prefab.name, prefab.m_zoningMode,
                                    prefab.m_cellWidth, prefab.m_cellLength, prefab.m_class.m_subService, areaIndex);

                            if (m_areaBuildings[areaIndex] == null)
                            {
                                m_areaBuildings[areaIndex] = new FastList<ushort>();
                            }

                            // mod begin — add building N times so random selection reflects spawn weight
                            for (uint s = 0; s < spawnRate; s++)
                            {
                                // mod end
                                m_areaBuildings[areaIndex].Add((ushort)j);
                                // mod begin
                            }
                            // mod end

                            // "Mark as corner" opt-in: a normally straight-zoned building the user
                            // flagged as a corner is ALSO injected into the CornerLeft and CornerRight
                            // buckets for its size, so the vanilla corner cases can pick it for corner
                            // lots (it stays available on straight lots too). True corner-zoned assets
                            // already live in a corner bucket, so they're skipped here.
                            if (markCorner && prefab.m_zoningMode == BuildingInfo.ZoningMode.Straight)
                            {
                                int cornerLeftIndex  = GetAreaIndex(prefab.m_class.m_service, prefab.m_class.m_subService, prefab.m_class.m_level, prefab.m_cellWidth, prefab.m_cellLength, BuildingInfo.ZoningMode.CornerLeft);
                                int cornerRightIndex = GetAreaIndex(prefab.m_class.m_service, prefab.m_class.m_subService, prefab.m_class.m_level, prefab.m_cellWidth, prefab.m_cellLength, BuildingInfo.ZoningMode.CornerRight);
                                foreach (int ci in new[] { cornerLeftIndex, cornerRightIndex })
                                {
                                    if (m_areaBuildings[ci] == null) m_areaBuildings[ci] = new FastList<ushort>();
                                    for (uint s = 0; s < spawnRate; s++) m_areaBuildings[ci].Add((ushort)j);
                                }
                                if (recordDiagnostics)
                                    Debugger.LogFormat("[CornerPool] district {0}: {1} MARKED-AS-CORNER size={2}x{3} -> L={4} R={5}",
                                        diagnosticsDistrictId, prefab.name, prefab.m_cellWidth, prefab.m_cellLength, cornerLeftIndex, cornerRightIndex);
                            }
                        }
                    }
                }
            }
            int num3 = 24;
            for (int k = 0; k < num3; k++)
            {
                for (int l = 0; l < 5; l++)
                {
                    for (int m = 0; m < 4; m++)
                    {
                        for (int n = 1; n < 4; n++)
                        {
                            int num4 = k;
                            num4 = num4 * 5 + l;
                            num4 = num4 * 4 + m;
                            num4 = num4 * 4 + n;
                            num4 *= 2;
                            FastList<ushort> fastList = m_areaBuildings[num4];
                            FastList<ushort> fastList2 = m_areaBuildings[num4 - 2];
                            if (fastList2 != null)
                            {
                                if (fastList == null)
                                {
                                    m_areaBuildings[num4] = fastList2;
                                }
                                else
                                {
                                    for (int num5 = 0; num5 < fastList2.m_size; num5++)
                                    {
                                        fastList.Add(fastList2.m_buffer[num5]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (enabledThemes != null && enabledThemes.Count > 0)
            {
                // Resolve per-district settings; fall back to global defaults.
                var distInfo = (diagnosticsDistrictId != 255) ? districtThemeInfos[diagnosticsDistrictId] : null;
                MissingAssetMode missingMode = distInfo != null ? distInfo.missingAssetMode : MissingAssetBehavior;

                // Phase 3 — missing-asset fill/fallback.
                // Uses this.m_areaBuildings (vanilla field, not the parameter) as reference.
                // A non-null bucket smaller than vanilla is treated as "has missing assets".
                if (missingMode != MissingAssetMode.Skip)
                {
                    FastList<ushort>[] vanillaBuildings = this.m_areaBuildings;
                    for (int i = 0; i < areaBuildingsLength; i++)
                    {
                        if (m_areaBuildings[i] == null) continue;
                        FastList<ushort> vanillaBucket = vanillaBuildings[i];
                        if (vanillaBucket == null || vanillaBucket.m_size == 0) continue;
                        if (m_areaBuildings[i].m_size < vanillaBucket.m_size)
                        {
                            if (missingMode == MissingAssetMode.FillWithVanilla)
                            {
                                int deficit = vanillaBucket.m_size - m_areaBuildings[i].m_size;
                                int vanillaSize = vanillaBucket.m_size;
                                for (int s = 0; s < deficit; s++)
                                    m_areaBuildings[i].Add(vanillaBucket.m_buffer[s % vanillaSize]);
                            }
                            else // FallbackToVanilla
                            {
                                m_areaBuildings[i] = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the building at <paramref name="buildingId"/> is a valid spawn candidate
        /// for the active themes in <paramref name="districtId"/>.
        /// Always returns true for non-growable buildings (services, unique, etc.).
        /// </summary>
        public bool IsBuildingValidForDistrict(ushort buildingId, byte districtId)
        {
            var building = BuildingManager.instance.m_buildings.m_buffer[buildingId];
            var info = building.Info;
            if (info == null) return true;

            // Only enforce themes on auto-growable zone buildings
            if (info.m_placementStyle != ItemClass.Placement.Automatic) return true;
            if (ItemClass.GetPrivateServiceIndex(info.m_class.m_service) == -1) return true;

            int areaIndex = GetAreaIndex(
                info.m_class.m_service, info.m_class.m_subService, info.m_class.m_level,
                info.m_cellWidth, info.m_cellLength, info.m_zoningMode);

            var valid = GetAreaBuildings(districtId, areaIndex);

            if (Debugger.Enabled)
            {
                var names = new System.Text.StringBuilder();
                if (valid == null || valid.m_size == 0)
                {
                    names.Append("(empty)");
                }
                else
                {
                    for (int i = 0; i < valid.m_size; i++)
                    {
                        var pi = PrefabCollection<BuildingInfo>.GetPrefab(valid.m_buffer[i]);
                        if (i > 0) names.Append(", ");
                        names.Append(pi != null ? pi.name : valid.m_buffer[i].ToString());
                    }
                }
                Debugger.LogVerbose("[ValidCheck] district={0} building=\"{1}\" areaIndex={2} validList=[{3}]",
                    districtId, info.name, areaIndex, names);
            }

            if (valid == null || valid.m_size == 0)
            {
                // No theme buildings for this footprint — the building is not part of the
                // active theme regardless of spawn mode. FillWithVanilla only supplements
                // existing pool entries (line 883 skips null pools), so a null pool here
                // means the footprint is genuinely uncovered: the shrink loop in
                // SimulationStepPatch will find a smaller theme building or leave the lot
                // empty. Either way, demolishing the standing building causes no infinite
                // loop — return false so auto-bulldoze and diagnostics are consistent.
                return false;
            }

            ushort prefab = (ushort)info.m_prefabDataIndex;
            bool inTheme = false;
            for (int i = 0; i < valid.m_size; i++)
                if (valid.m_buffer[i] == prefab) { inTheme = true; break; }

            if (!inTheme) return false;

            // Secondary check: if the district has specialization enforcement enabled, also
            // verify that the building's sub-service matches what the game would spawn at
            // this position given the district's active specialization policies.
            if (GetDistrictEnforceSpecialization(districtId) && !IsCompatibleWithSpecialization(building, info))
            {
                Debugger.LogVerbose("[ValidCheck] district={0} building=\"{1}\" rejected: sub-service mismatch with active specialization.",
                    districtId, info.name);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the building's sub-service matches what the game's zone-type resolver
        /// would choose at the building's world position given current district policies.
        /// Used by IsBuildingValidForDistrict when enforceSpecialization is on.
        /// </summary>
        private static bool IsCompatibleWithSpecialization(Building building, BuildingInfo info)
        {
            var pos = building.m_position;
            var sub = info.m_class.m_subService;
            ItemClass.SubService expected;
            ItemClass.Level dummy;

            switch (info.m_class.m_service)
            {
                case ItemClass.Service.Industrial:
                    ZoneBlock.GetIndustryType(pos, out expected, out dummy);
                    return sub == expected;

                case ItemClass.Service.Residential:
                {
                    var zone = (sub == ItemClass.SubService.ResidentialHigh ||
                                sub == ItemClass.SubService.ResidentialHighEco)
                        ? ItemClass.Zone.ResidentialHigh
                        : ItemClass.Zone.ResidentialLow;
                    ZoneBlock.GetResidentialType(pos, zone, info.m_cellWidth, info.m_cellLength, out expected, out dummy);
                    return sub == expected;
                }

                case ItemClass.Service.Commercial:
                {
                    var zone = (sub == ItemClass.SubService.CommercialHigh ||
                                sub == ItemClass.SubService.CommercialLeisure ||
                                sub == ItemClass.SubService.CommercialTourist)
                        ? ItemClass.Zone.CommercialHigh
                        : ItemClass.Zone.CommercialLow;
                    ZoneBlock.GetCommercialType(pos, zone, info.m_cellWidth, info.m_cellLength, out expected, out dummy);
                    return sub == expected;
                }

                case ItemClass.Service.Office:
                    ZoneBlock.GetOfficeType(pos, ItemClass.Zone.Office, info.m_cellWidth, info.m_cellLength, out expected, out dummy);
                    return sub == expected;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Size-preference building selection. Returns null when no theme building fits the lot.
        /// Called from SimulationStepPatch when the district has a non-Default SizePreference.
        /// </summary>
        public BuildingInfo GetRandomBuildingInfoWithPreference(
            byte districtId,
            ItemClass.Service service, ItemClass.SubService subService, ItemClass.Level level,
            int maxWidth, int maxDepth, BuildingInfo.ZoningMode zoningMode,
            ref ColossalFramework.Math.Randomizer r)
        {
            var info = districtThemeInfos[districtId];
            if (info == null || !info.isEnabled || info.buildingEntries == null) return null;

            SizePreference pref = GetDistrictSizePreference(districtId, service);

            // 1 — filter candidates that fit the lot
            var candidates = new System.Collections.Generic.List<DistrictBuildingEntry>();
            for (int i = 0; i < info.buildingEntries.m_size; i++)
            {
                var e = info.buildingEntries.m_buffer[i];
                if (e.subService != subService || e.level != level) continue;
                if (!ZoningModeCompatible(e.zoningMode, zoningMode)) continue;
                if (e.cellWidth > maxWidth || e.cellLength > maxDepth) continue;
                candidates.Add(e);
            }
            if (candidates.Count == 0) return null;

            // 2 — sort by preference and assign ranks (ties share rank)
            RankByPreference(candidates, pref);

            // Absolute mode: keep only rank-1 candidates, then pick by spawn weight alone.
            if (info.strengthPref == PreferenceStrength.Absolute)
            {
                var top = new System.Collections.Generic.List<DistrictBuildingEntry>();
                for (int i = 0; i < candidates.Count; i++)
                    if (GetCandidateRank(candidates, i, pref) == 1) top.Add(candidates[i]);
                if (top.Count > 0) candidates = top;

                int totalW = 0;
                foreach (var c in candidates) totalW += Mathf.Max(c.spawnWeight, 1);
                int roll2 = (int)r.Int32((uint)totalW);
                int acc2 = 0;
                foreach (var c in candidates)
                {
                    acc2 += Mathf.Max(c.spawnWeight, 1);
                    if (roll2 < acc2)
                        return PrefabCollection<BuildingInfo>.GetPrefab(c.prefabIndex);
                }
                return PrefabCollection<BuildingInfo>.GetPrefab(candidates[candidates.Count - 1].prefabIndex);
            }

            // 3 — weighted roll: score = spawnWeight / rank^alpha
            float alpha = PreferenceStrengthToAlpha(info.strengthPref);
            float total = 0f;
            var scores = new float[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                float rank = (float)GetCandidateRank(candidates, i, pref);
                scores[i] = Mathf.Max(candidates[i].spawnWeight, 1) / Mathf.Pow(rank, alpha);
                total += scores[i];
            }
            if (total <= 0f) return null;

            float roll = (r.Int32(100000) / 100000f) * total;
            float acc = 0f;
            for (int i = 0; i < candidates.Count; i++)
            {
                acc += scores[i];
                if (roll <= acc)
                    return PrefabCollection<BuildingInfo>.GetPrefab(candidates[i].prefabIndex);
            }
            return PrefabCollection<BuildingInfo>.GetPrefab(candidates[candidates.Count - 1].prefabIndex);
        }

        private static bool ZoningModeCompatible(BuildingInfo.ZoningMode buildingMode, BuildingInfo.ZoningMode lotMode)
        {
            if (lotMode == BuildingInfo.ZoningMode.Straight)
                return buildingMode == BuildingInfo.ZoningMode.Straight;
            // Corner lot — building must be a corner of the matching handedness
            return buildingMode == lotMode;
        }

        private static void RankByPreference(System.Collections.Generic.List<DistrictBuildingEntry> list, SizePreference pref)
        {
            switch (pref)
            {
                case SizePreference.BiggestFirst:
                    list.Sort((a, b) => (b.cellWidth * b.cellLength).CompareTo(a.cellWidth * a.cellLength));
                    break;
                case SizePreference.WidestFirst:
                    list.Sort((a, b) =>
                    {
                        int c = b.cellWidth.CompareTo(a.cellWidth);
                        return c != 0 ? c : a.cellLength.CompareTo(b.cellLength);
                    });
                    break;
                case SizePreference.DeepestFirst:
                    list.Sort((a, b) =>
                    {
                        int c = b.cellLength.CompareTo(a.cellLength);
                        return c != 0 ? c : a.cellWidth.CompareTo(b.cellWidth);
                    });
                    break;
                case SizePreference.SmallestFirst:
                    list.Sort((a, b) => (a.cellWidth * a.cellLength).CompareTo(b.cellWidth * b.cellLength));
                    break;
                case SizePreference.TallestFirst:
                    list.Sort((a, b) => b.height.CompareTo(a.height));
                    break;
                case SizePreference.ShortestFirst:
                    list.Sort((a, b) => a.height.CompareTo(b.height));
                    break;
                    // Random / Default: no sort — all will get rank 1
            }
        }

        // Returns 1-based rank for candidate i, sharing rank with preceding entries of equal size key.
        private static int GetCandidateRank(System.Collections.Generic.List<DistrictBuildingEntry> list, int idx, SizePreference pref)
        {
            if (pref == SizePreference.Random || pref == SizePreference.Default) return 1;
            int rank = 1;
            for (int i = 0; i < idx; i++)
            {
                if (!SameSizeKey(list[i], list[idx], pref)) rank++;
            }
            return rank;
        }

        private static bool SameSizeKey(DistrictBuildingEntry a, DistrictBuildingEntry b, SizePreference pref)
        {
            switch (pref)
            {
                case SizePreference.BiggestFirst:
                case SizePreference.SmallestFirst:
                    return (a.cellWidth * a.cellLength) == (b.cellWidth * b.cellLength);
                case SizePreference.WidestFirst:
                    return a.cellWidth == b.cellWidth && a.cellLength == b.cellLength;
                case SizePreference.DeepestFirst:
                    return a.cellLength == b.cellLength && a.cellWidth == b.cellWidth;
                case SizePreference.TallestFirst:
                case SizePreference.ShortestFirst:
                    return a.height == b.height;
                default: return true;
            }
        }

        public static int GetAreaIndex(ItemClass.Service service, ItemClass.SubService subService, ItemClass.Level level, int width, int length, BuildingInfo.ZoningMode zoningMode)
        {
            int privateSubServiceIndex = ItemClass.GetPrivateSubServiceIndex(subService);
            int num = (int)((privateSubServiceIndex == -1 ? ItemClass.GetPrivateServiceIndex(service) : 8 + privateSubServiceIndex) * 5 + level);
            return zoningMode != BuildingInfo.ZoningMode.CornerRight ? (int)(((num * 4 + width - 1) * 4 + length - 1) * 2 + zoningMode) : ((num * 4 + length - 1) * 4 + width - 1) * 2 + 1;

        }

        public FastList<ushort> GetAreaBuildings(byte districtId, int areaIndex)
        {
            // Guard: a third-party mod (e.g. via m_BuildingWrapper.OnCalculateSpawn) may pass
            // a service/subService combination that maps outside the array bounds.  Return null
            // so the caller falls back to vanilla rather than crashing the simulation thread.
            if (areaIndex < 0 || areaIndex >= AreaBuildingsLength) return null;

            var info = districtThemeInfos[districtId];

            // Theme management enabled in district? return custom fastlist for district
            if (info != null && info.isEnabled)
            {
                return info.areaBuildings[areaIndex];
            }
            // Theme management not enabled in district? return fastlist for city-wide "district"
            else if (districtId != 0)
            {
                return GetAreaBuildings(0, areaIndex);
            }
            // Theme management not enabled in city-wide district? return fastlist of the game
            else
            {
                if (m_areaBuildingsDirty)
                {
                    RefreshAreaBuildings(m_areaBuildings, null, null, false);
                    m_areaBuildingsDirty = false;
                }
                return m_areaBuildings[areaIndex];
            }
        }

        private HashSet<Configuration.Theme> getDefaultThemes(uint districtIdx)
        {
            // Theme management is opt-in. Enabling the toggle without selecting a theme leaves
            // the district behaving like vanilla (IsEffectivelyThemed returns false for empty
            // theme sets, so the spawn patch defers to the original code). The built-in env
            // themes are references in the manager for the player to inspect or copy from.
            if (Debugger.Enabled)
                Debugger.LogFormat("Theme management enabled for district {0}; no default theme assigned.", districtIdx);

            return new HashSet<Configuration.Theme>();
        }

        public HashSet<Configuration.Theme> GetDistrictThemes(byte districtId, bool initializeIfNull)
        {
            if (IsThemeManagementEnabled(districtId))
            {
                return districtThemeInfos[districtId].themes;
            }
            else
            {
                return initializeIfNull ? getDefaultThemes(districtId) : null;
            }
        }

        public List<Configuration.Theme> GetAllThemes()
        {
            return Configuration.themes;
        }

        // Sort order: user-created → [Custom] → [Vanilla] → [DLC].
        // User themes keep creation order; built-in groups sort by displayName.
        public List<Configuration.Theme> GetAllThemesSorted()
        {
            var all = Configuration.themes;
            var idx = new Dictionary<Configuration.Theme, int>(all.Count);
            for (int i = 0; i < all.Count; i++) idx[all[i]] = i;

            var sorted = new List<Configuration.Theme>(all);
            sorted.Sort((a, b) =>
            {
                int ga = ThemeSortGroup(a), gb = ThemeSortGroup(b);
                if (ga != gb) return ga.CompareTo(gb);
                if (ga == 0) return idx[b].CompareTo(idx[a]);
                return string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase);
            });
            return sorted;
        }

        private static int ThemeSortGroup(Configuration.Theme t)
        {
            if (!t.isBuiltIn)           return 0; // user-created
            if (t.stylePackage == null) return 1; // [Custom] (other mods)
            if (!t.isDlc)               return 2; // [Vanilla]
            return 3;                              // [DLC]
        }

        /// <summary>
        /// Logs a validation summary for all themes to the debug log.
        /// Called after ImportThemes() on level load. Requires Debugger.Enabled.
        /// </summary>
        public void ValidateAllThemes()
        {
            var themes = GetAllThemes();
            var sb = new StringBuilder();
            sb.AppendLine("Theme validation summary:");

            int prefabCount = PrefabCollection<BuildingInfo>.PrefabCount();
            var loadedNames = new HashSet<string>();
            for (int i = 0; i < prefabCount; i++)
            {
                var prefab = PrefabCollection<BuildingInfo>.GetPrefab((uint)i);
                if (prefab != null) loadedNames.Add(prefab.name);
            }

            foreach (var theme in themes)
            {
                int total = 0, loaded = 0, missing = 0;
                foreach (var building in theme.buildings)
                {
                    if (!building.include) continue;
                    total++;
                    if (loadedNames.Contains(building.name)) loaded++;
                    else missing++;
                }

                if (total == 0)
                    sb.AppendFormat("  {0}: EMPTY (no buildings)\n", theme.name);
                else if (missing == 0)
                    sb.AppendFormat("  {0}: {1} buildings, all loaded\n", theme.name, total);
                else
                    sb.AppendFormat("  {0}: {1} buildings, {2} loaded, {3} MISSING\n",
                        theme.name, total, loaded, missing);
            }

            Debugger.Log(sb.ToString());
        }

        // Local (user-created, editable) theme lookup by name. Built-in/tagged themes
        // ([Vanilla]/[DLC]/[Custom]) are excluded, so a user may create a local theme that
        // shares a display name with a tagged one — they are distinct entities and only the
        // local one is editable. Used by the create/rename/copy name-uniqueness checks, which
        // must only enforce uniqueness among local themes.
        public Configuration.Theme GetLocalThemeByName(string themeName)
        {
            return Configuration.themes.FirstOrDefault(theme => !theme.isBuiltIn && theme.name == themeName);
        }

        public Configuration.Theme GetThemeByName(string themeName)
        {
            var matches = Configuration.themes.Where(theme => theme.name == themeName).ToList();
            if (matches.Count <= 1) return matches.FirstOrDefault();
            // Name collision (e.g. a custom "European" plus the built-in DistrictStyle "European"):
            // prefer the built-in default (the one backed by a stylePackage). It's the canonical
            // theme that's always present when its DLC/style is active, so saved district
            // references resolve to it rather than to a same-named custom theme that happened to
            // load first.
            return matches.FirstOrDefault(t => t.isBuiltIn && t.stylePackage != null) ?? matches[0];
        }

        // Compact discriminator saved alongside a district's enabled theme so a display name
        // shared by several themes (e.g. a local "European" plus the built-in DistrictStyle
        // "European") resolves back to the EXACT same theme on reload:
        //   "L"        local / user-created (the only editable kind)
        //   "C"        built-in but no style package ([Custom], imported from another mod)
        //   "S:<pkg>"  built-in style / env theme, keyed by its unique stylePackage
        public static string GetThemeTag(Configuration.Theme theme)
        {
            if (theme == null) return "";
            if (!theme.isBuiltIn) return "L";
            if (theme.stylePackage == null) return "C";
            return "S:" + theme.stylePackage;
        }

        // Resolves a district's saved theme reference using the tag from GetThemeTag. Falls back
        // to the name-only lookup when the tag is empty (old saves, which never wrote tags) or
        // when the tagged target no longer exists (e.g. a DLC/style now disabled).
        public Configuration.Theme GetThemeByNameAndTag(string themeName, string tag)
        {
            if (string.IsNullOrEmpty(tag)) return GetThemeByName(themeName);
            if (tag == "L")
                return Configuration.themes.FirstOrDefault(t => !t.isBuiltIn && t.name == themeName)
                       ?? GetThemeByName(themeName);
            if (tag == "C")
                return Configuration.themes.FirstOrDefault(t => t.isBuiltIn && t.stylePackage == null && t.name == themeName)
                       ?? GetThemeByName(themeName);
            if (tag.StartsWith("S:"))
            {
                string sp = tag.Substring(2);
                return Configuration.themes.FirstOrDefault(t => t.stylePackage == sp)
                       ?? GetThemeByName(themeName);
            }
            return GetThemeByName(themeName);
        }

        // Built-in style/env themes are matched ONLY by their stylePackage. A custom or
        // third-party theme that merely shares the display name (e.g. another mod's "European")
        // has stylePackage == null and is intentionally left alone, so the built-in DistrictStyle
        // import always produces its own unique [DLC]/[Vanilla] theme instead of being merged
        // into the custom one.
        private Configuration.Theme GetThemeByStylePackage(string stylePackage)
        {
            if (stylePackage == null) return null;
            return Configuration.themes.FirstOrDefault(theme => theme.stylePackage == stylePackage);
        }

        /// <summary>
        /// Fallback prefab lookup for renamed workshop assets.
        /// Extracts the numeric Steam workshop ID prefix from <paramref name="buildingName"/>
        /// (format "12345678.LocalName_Data") and returns the first loaded prefab whose name
        /// shares that prefix.  Returns null if the name has no numeric prefix.
        /// </summary>
        private static BuildingInfo FindLoadedBySteamPrefix(string buildingName)
        {
            if (buildingName == null) return null;
            int dotIdx = buildingName.IndexOf('.');
            if (dotIdx <= 0) return null;

            string prefix = buildingName.Substring(0, dotIdx);
            foreach (char c in prefix) if (!char.IsDigit(c)) return null;

            uint count = (uint)PrefabCollection<BuildingInfo>.PrefabCount();
            for (uint i = 0; i < count; i++)
            {
                BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetPrefab(i);
                if (prefab == null) continue;
                int pDot = prefab.name.IndexOf('.');
                if (pDot > 0 && prefab.name.Substring(0, pDot) == prefix)
                    return prefab;
            }
            return null;
        }
    }
}
