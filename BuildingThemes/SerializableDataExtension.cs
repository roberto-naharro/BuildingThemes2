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
                    var j = 0;
                    foreach (var theme in themes)
                    {
                        themesNames[j] = theme.name;
                        j++;
                    }
                    configuration.Districts.Add(new DistrictsConfiguration.District()
                    {
                        id = i,
                        blacklistMode = themesManager.IsBlacklistModeEnabled(i),
                        themes = themesNames,
                        missingAssetMode  = (int)themesManager.GetDistrictMissingAssetMode(i),
                        emptyLevelBehavior = (int)themesManager.GetDistrictEmptyLevelBehavior(i),
                        autoBulldoze = themesManager.GetDistrictAutoBulldoze(i)
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

        private static void ApplyConfiguration(DistrictsConfiguration configuration) 
        {
            var buildingThemesManager = BuildingThemesManager.instance;
            buildingThemesManager.ImportThemes();

            foreach (var district in configuration.Districts)
            {
                //skip districts which do not exist
                if (DistrictManager.instance.m_districts.m_buffer[district.id].m_flags == District.Flags.None) continue;

                var themes = new HashSet<Configuration.Theme>();

                if (district.themes == null) continue;

                foreach (var themeName in district.themes)
                {
                    var theme = buildingThemesManager.GetThemeByName(themeName);
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

                // autoBulldoze defaults to false for old saves (field missing = XML default)
                buildingThemesManager.SetDistrictAutoBulldoze(district.id, district.autoBulldoze);

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
        // Old saves deserialise version as 0 — those fields are ignored on load.
        [System.Xml.Serialization.XmlAttribute("version")]
        public int version = 2;

        public class District
        {
            public byte id;
            public bool blacklistMode = false;
            public string[] themes;
            // -1 = not saved (old save / version < 2); >= 0 = enum ordinal.
            public int missingAssetMode = -1;
            public int emptyLevelBehavior = -1;
            public bool autoBulldoze = false;
        }

        public List<District> Districts = new List<District>();
    }
}
