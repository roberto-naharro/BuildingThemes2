using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Xml.Serialization;
using ColossalFramework;
using ICities;
using UnityEngine;

namespace BuildingThemes
{
    // This extension handles the loading and saving of district theme data (which themes are assigned to a district).
    public class SerializableDataExtension : ISerializableDataExtension
    {
        public static ISerializableData SerializableData;

        // Stored during OnLoadData so the deferred style-import action in LoadingExtension
        // can re-apply district assignments after all mods finish OnLevelLoaded.
        internal static DistrictsConfiguration s_pendingConfiguration;

        public static string XMLSaveDataId = "BuildingThemes-SaveData";

        // support for legacy data
        public static string LegacyDataId = "BuildingThemes";

        public void OnCreated(ISerializableData serializableData)
        {
            SerializableData = serializableData;
        }

        public void OnReleased()
        {
        }

        public void OnLoadData()
        {
            try 
            {
                byte[] saveData = SerializableData.LoadData(XMLSaveDataId);

                if (saveData != null)
                {
                    if (Debugger.Enabled)
                        Debugger.LogFormat("Loading Save Data — {0} bytes.", saveData.Length);

                    DistrictsConfiguration configuration = null;

                    var xmlSerializer = new XmlSerializer(typeof(DistrictsConfiguration));
                    using (var memoryStream = new MemoryStream(saveData))
                    {
                        configuration = xmlSerializer.Deserialize(memoryStream) as DistrictsConfiguration;
                    }

                    if (Debugger.Enabled && configuration != null)
                        Debugger.LogFormat("Save data deserialized — {0} district(s).",
                            configuration.Districts.Count);

                    s_pendingConfiguration = configuration;
                    ApplyConfiguration(configuration);
                }
                else
                {
                    // search for legacy save data
                    byte[] legacyData = SerializableData.LoadData(LegacyDataId);

                    if (legacyData != null)
                    {
                        if (Debugger.Enabled)
                        {
                            Debugger.Log("Loading Legacy Save Data...");
                        }

                        var UniqueId = 0u;

                        for (var i = 0; i < legacyData.Length - 3; i++)
                        {
                            UniqueId = BitConverter.ToUInt32(legacyData, i);
                        }

                        var filepath = Path.Combine(Application.dataPath, String.Format("buildingThemesSave_{0}.xml", UniqueId));

                        Debugger.LogFormat("Legacy save UniqueId={0}, filepath={1}", UniqueId, filepath);

                        if (!File.Exists(filepath))
                        {
                            if (Debugger.Enabled)
                            {
                                Debugger.Log(filepath + " not found!");
                            }
                            return;
                        }

                        DistrictsConfiguration configuration;

                        var serializer = new XmlSerializer(typeof(DistrictsConfiguration));
                        try
                        {
                            using (var reader = new StreamReader(filepath))
                            {
                                configuration = (DistrictsConfiguration)serializer.Deserialize(reader);
                            }
                        }
                        catch
                        {
                            configuration = null;
                        }

                        s_pendingConfiguration = configuration;
                        ApplyConfiguration(configuration);
                    }
                    else
                    { 
                        if (Debugger.Enabled)
                        {
                            Debugger.Log("No legacy save data found!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("Error loading theme data");
                Debugger.LogException(ex);
            }
        }

        public void OnSaveData()
        {
            if (Debugger.Enabled)
            {
                Debugger.Log("Saving Data...");
            }
            
            try
            {
                var configuration = new DistrictsConfiguration();

                var themesManager = Singleton<BuildingThemesManager>.instance;
                for (byte i = 0; i < 128; i++)
                {
                    if (!themesManager.IsThemeManagementEnabled(i)) continue;

                    var themes = themesManager.GetDistrictThemes(i, false);
                    if (themes == null)
                    {
                        continue; ;
                    }
                    var themesNames = new string[themes.Count];
                    var themeTags = new string[themes.Count];
                    var j = 0;
                    foreach (var theme in themes)
                    {
                        themesNames[j] = theme.name;
                        themeTags[j] = BuildingThemesManager.GetThemeTag(theme);
                        j++;
                    }
                    configuration.Districts.Add(new DistrictsConfiguration.District()
                    {
                        id = i,
                        blacklistMode = themesManager.IsBlacklistModeEnabled(i),
                        themes = themesNames,
                        themeTags = themeTags,
                        missingAssetMode   = (int)themesManager.GetDistrictMissingAssetMode(i),
                        emptyLevelBehavior = (int)themesManager.GetDistrictEmptyLevelBehavior(i),
                        autoBulldoze            = themesManager.GetDistrictAutoBulldoze(i),
                        preferElectricity       = themesManager.GetDistrictPreferElectricity(i),
                        preferAdjacent          = themesManager.GetDistrictPreferAdjacent(i),
                        enforceSpecialization   = themesManager.GetDistrictEnforceSpecialization(i),
                        useVanillaFootprint    = themesManager.GetDistrictUseVanillaFootprint(i),
                        residentialSizePref = (int)themesManager.GetDistrictSizePreference(i, ItemClass.Service.Residential),
                        commercialSizePref  = (int)themesManager.GetDistrictSizePreference(i, ItemClass.Service.Commercial),
                        industrialSizePref  = (int)themesManager.GetDistrictSizePreference(i, ItemClass.Service.Industrial),
                        officeSizePref      = (int)themesManager.GetDistrictSizePreference(i, ItemClass.Service.Office),
                        strengthPref        = (int)themesManager.GetDistrictPreferenceStrength(i),
                    });
                    if (Debugger.Enabled)
                    {
                        Debugger.LogFormat("Saving: {0} themes enabled for district {1}", themes.Count, i);
                    }
                }

                byte[] configurationData;

                var xmlSerializer = new XmlSerializer(typeof(DistrictsConfiguration));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                using (var memoryStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(memoryStream, configuration, ns);
                    configurationData = memoryStream.ToArray();
                }
                SerializableData.SaveData(XMLSaveDataId, configurationData);
                if (Debugger.Enabled)
                    Debugger.LogFormat("Save complete — {0} district(s), {1} bytes written.",
                        configuration.Districts.Count, configurationData.Length);

                // output for debugging
                /*
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter("BuildingThemesData.xml"))
                {
                    xmlSerializer.Serialize(streamWriter, configuration, ns);
                }
                */

                if (Debugger.Enabled)
                {
                    Debugger.LogFormat("Serialization done.");
                    Debugger.AppendThemeList();
                }
            }

            catch (Exception ex)
            {
                Debugger.LogError("Error saving theme data");
                Debugger.LogException(ex);
            }
        }

        // Called from LoadingExtension's deferred action after all OnLevelLoaded calls complete.
        internal static void ApplyPendingConfiguration()
        {
            if (s_pendingConfiguration == null) return;
            var cfg = s_pendingConfiguration;
            s_pendingConfiguration = null;
            ApplyConfiguration(cfg);
        }

        internal static void ApplyConfiguration(DistrictsConfiguration configuration)
        {
            var buildingThemesManager = BuildingThemesManager.instance;
            // Import only mod themes here (XML-based). Style import (ImportStylesAsThemes) is
            // deferred to LoadingExtension so it runs after DSP and other mods finish OnLevelLoaded.
            buildingThemesManager.ImportThemesFromThemeMods();

            foreach (var district in configuration.Districts)
            {
                //skip districts which do not exist
                if (DistrictManager.instance.m_districts.m_buffer[district.id].m_flags == District.Flags.None) continue;

                var themes = new HashSet<Configuration.Theme>();

                if (district.themes == null) continue;

                // Tags disambiguate same-named themes (added in save version 7). Only trusted
                // when present and index-aligned with the names; older saves fall back to the
                // name-only lookup, preserving their original behaviour exactly.
                bool haveTags = district.themeTags != null
                                && district.themeTags.Length == district.themes.Length;

                for (int k = 0; k < district.themes.Length; k++)
                {
                    string themeName = district.themes[k];
                    var theme = haveTags
                        ? buildingThemesManager.GetThemeByNameAndTag(themeName, district.themeTags[k])
                        : buildingThemesManager.GetThemeByName(themeName);
                    if (theme == null)
                    {
                        Debugger.LogFormat("Theme {0} that was enabled in district {1} could not be found!", themeName, district.id);
                        continue;
                    }
                    themes.Add(theme);
                }

                if (Debugger.Enabled)
                {
                    Debugger.LogFormat("Loading: {0} themes enabled for district {1}", themes.Count, district.id);
                }

                buildingThemesManager.setThemeInfo(district.id, themes, district.blacklistMode);

                // Restore per-district behavior overrides (persisted since save version 2).
                if (configuration.version >= 2)
                    buildingThemesManager.RestoreDistrictBehavior(district.id, district.missingAssetMode, district.emptyLevelBehavior);

                // autoBulldoze, preferElectricity, preferAdjacent, enforceSpecialization default to false for old saves
                buildingThemesManager.SetDistrictAutoBulldoze(district.id, district.autoBulldoze);
                buildingThemesManager.SetDistrictPreferElectricity(district.id, district.preferElectricity);
                buildingThemesManager.SetDistrictPreferAdjacent(district.id, district.preferAdjacent);
                buildingThemesManager.SetDistrictEnforceSpecialization(district.id, district.enforceSpecialization);
                buildingThemesManager.SetDistrictUseVanillaFootprint(district.id, district.useVanillaFootprint);

                // Size preferences added in version 3; -1 means not saved → keep default
                if (configuration.version >= 3)
                {
                    if (district.residentialSizePref >= 0)
                        buildingThemesManager.SetDistrictSizePreference(district.id, ItemClass.Service.Residential, (SizePreference)district.residentialSizePref);
                    if (district.commercialSizePref >= 0)
                        buildingThemesManager.SetDistrictSizePreference(district.id, ItemClass.Service.Commercial, (SizePreference)district.commercialSizePref);
                    if (district.industrialSizePref >= 0)
                        buildingThemesManager.SetDistrictSizePreference(district.id, ItemClass.Service.Industrial, (SizePreference)district.industrialSizePref);
                    if (district.officeSizePref >= 0)
                        buildingThemesManager.SetDistrictSizePreference(district.id, ItemClass.Service.Office, (SizePreference)district.officeSizePref);
                    if (district.strengthPref >= 0)
                        buildingThemesManager.SetDistrictPreferenceStrength(district.id, (PreferenceStrength)district.strengthPref);
                }

                if (Debugger.Enabled)
                {
                    var themeNames = string.Join(", ", System.Array.ConvertAll(district.themes ?? new string[0], t => t));
                    Debugger.LogFormat("District {0} loaded — blacklist={1}, missingMode={2}, emptyMode={3}, themes=[{4}]",
                        district.id, district.blacklistMode,
                        district.missingAssetMode, district.emptyLevelBehavior, themeNames);
                }
            }
        }
    }

    public class DistrictsConfiguration
    {
        // version 2 adds missingAssetMode + emptyLevelBehavior per district.
        // version 3 adds size-preference fields per district.
        // version 4 adds preferElectricity per district.
        // version 5 adds enforceSpecialization per district.
        // version 6 adds preferAdjacent per district.
        // version 7 adds themeTags per district (disambiguates same-named themes).
        // version 8 adds useVanillaFootprint per district.
        // Old saves deserialise version as 0 — those fields are ignored on load.
        [System.Xml.Serialization.XmlAttribute("version")]
        public int version = 8;

        public class District
        {
            public byte id;
            public bool blacklistMode = false;
            public string[] themes;
            // Per-theme discriminator, index-aligned with `themes` (save version 7+). null on
            // older saves → loader falls back to name-only resolution. See GetThemeTag.
            public string[] themeTags;
            // -1 = not saved (old save / version < 2); >= 0 = enum ordinal.
            public int missingAssetMode = -1;
            public int emptyLevelBehavior = -1;
            public bool autoBulldoze = false;
            // -1 = not saved (old save / version < 3)
            public int residentialSizePref = -1;
            public int commercialSizePref  = -1;
            public int industrialSizePref  = -1;
            public int officeSizePref      = -1;
            public int strengthPref        = -1;
            // added in version 4; defaults to false for older saves
            public bool preferElectricity = false;
            // added in version 5; defaults to false for older saves
            public bool enforceSpecialization = false;
            // added in version 6; defaults to false for older saves
            public bool preferAdjacent = false;
            // added in version 8; defaults to false for older saves
            public bool useVanillaFootprint = false;
        }

        public List<District> Districts = new List<District>();
    }
}
