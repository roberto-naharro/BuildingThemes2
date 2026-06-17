using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace BuildingThemes
{
    /// <summary>
    /// Controls what happens when a district theme has no buildings for a particular level.
    /// </summary>
    public enum EmptyLevelBehavior
    {
        /// <summary>Vanilla buildings spawn for levels not covered by the theme (current behavior).</summary>
        VanillaFallback = 0,
        /// <summary>Removed: caused infinite rebuild loops (L1 prefab returned for L2 slot → building never advances). Migrated to VanillaFallback on load.</summary>
        [System.Obsolete("Cascade mode removed — migrated to VanillaFallback on load.")]
        CascadeFromTheme = 1,
        /// <summary>Levels with no theme buildings stay empty — no vanilla fallback, no upgrades past the included levels.</summary>
        StrictThemeOnly = 2
    }

    /// <summary>
    /// Controls which lot sizes are preferred when selecting a theme building to spawn.
    /// </summary>
    public enum SizePreference
    {
        /// <summary>Existing 8-candidate SimulationStep loop — no size bias.</summary>
        Default = 0,
        /// <summary>Prefer buildings with the largest footprint area (width × length).</summary>
        BiggestFirst = 1,
        /// <summary>Prefer widest buildings first; tie-break by shortest depth.</summary>
        WidestFirst = 2,
        /// <summary>Prefer deepest buildings first; tie-break by narrowest width.</summary>
        DeepestFirst = 3,
        /// <summary>All candidates share rank 1 — selection is purely by spawn weight.</summary>
        Random = 4,
        /// <summary>Prefer buildings with the smallest footprint area (width × length).</summary>
        SmallestFirst = 5,
        /// <summary>Prefer tallest buildings first.</summary>
        TallestFirst = 6,
        /// <summary>Prefer shortest buildings first.</summary>
        ShortestFirst = 7,
    }

    /// <summary>
    /// Controls how strongly size preference biases selection relative to spawn weight.
    /// α exponent in: score = spawnWeight / rank^α
    /// </summary>
    public enum PreferenceStrength
    {
        Gentle = 0,  // α = 0.5
        Moderate = 1,  // α = 1.0
        Strong = 2,  // α = 2.0
        Absolute = 3,  // always pick highest-ranked size; spawn weight only breaks ties within the top rank
    }

    /// <summary>
    /// Controls how the spawning system handles theme buildings whose assets are not loaded.
    /// </summary>
    public enum MissingAssetMode
    {
        /// <summary>Current behavior: theme applies with only the loaded buildings. Buckets may be sparse.</summary>
        Skip = 0,
        /// <summary>Missing slots in each area bucket are supplemented with vanilla buildings from the same bucket.</summary>
        FillWithVanilla = 1,
        /// <summary>If an area bucket appears sparse (fewer entries than vanilla), vanilla spawning takes over for that bucket.</summary>
        FallbackToVanilla = 2
    }

    public class Configuration
    {
        public int version = 0;

        public bool UnlockPolicyPanel = true;

        public bool CreateBuildingDuplicates = false;

        [DefaultValue(true)]
        public bool ThemeValidityWarning = true;

        [XmlArray(ElementName = "Themes")]
        [XmlArrayItem(ElementName = "Theme")]
        public List<Theme> themes = new List<Theme>();

        public Theme getTheme(string name)
        {
            foreach (Theme theme in themes)
            {
                if (theme.name == name) return theme;
            }
            return null;
        }

        public class Theme
        {
            [XmlAttribute("name")]
            public string name;

            [XmlIgnoreAttribute]
            public bool isBuiltIn = false;

            /// <summary>True when this theme comes from a paid DLC (ModderPackBitMask != None).</summary>
            [XmlIgnore]
            public bool isDlc = false;

            /// <summary>Locale key set at import time (e.g. "STYLES_EUROPEANSUBURBIA"). Null for user/mod themes.</summary>
            [XmlIgnore]
            public string localeKey;

            [XmlIgnore]
            public string displayName
            {
                get
                {
                    // User-created themes keep their plain name.
                    if (!isBuiltIn) return name;

                    // Imported mod/workshop themes are read-only and shown as custom.
                    if (stylePackage == null) return "[Custom] " + name;

                    // Built-in district styles use Vanilla/DLC prefixes.
                    string prefix = isDlc ? "[DLC] " : "[Vanilla] ";
                    return prefix + ResolveLocaleName();
                }
            }

            /// <summary>
            /// True when this theme contains only base-game buildings — no DLC and no workshop
            /// assets required. Every building is inspected (no shortcut for built-in themes)
            /// so the badge is truthful even when a built-in theme's stored content is out of
            /// sync with its isDlc flag.
            /// </summary>
            [XmlIgnore]
            public bool isVanillaOnly
            {
                get
                {
                    if (buildings.Count == 0) return false;
                    foreach (var b in buildings)
                    {
                        if (BuildingNeedsDlc(b)) return false;
                        if (BuildingIsWorkshop(b.name)) return false;
                    }
                    return true;
                }
            }

            /// <summary>
            /// True when this theme contains no workshop assets — only vanilla and/or DLC content.
            /// Used together with isVanillaOnly to pick the badge: green when vanilla-only, red
            /// when no-workshop-but-has-DLC, none otherwise.
            /// </summary>
            [XmlIgnore]
            public bool hasNoWorkshopAssets
            {
                get
                {
                    if (buildings.Count == 0) return false;
                    foreach (var b in buildings)
                    {
                        if (BuildingIsWorkshop(b.name)) return false;
                    }
                    return true;
                }
            }

            // A building needs DLC when its own prefab flags say so (expansion or modder pack).
            // DistrictStyle membership is intentionally NOT checked here: a building shared
            // between e.g. the European DistrictStyle and the Sunny spawn pool has exp=None
            // and pack=None — it is vanilla by ownership. Whether the *theme* that contains
            // the building is DLC-gated is tracked separately on Theme.isDlc.
            private static bool BuildingNeedsDlc(Building b)
            {
                if (b == null) return false;
                if (b.dlc != null) return true;
                if (b.name == null) return false;

                var prefab = PrefabCollection<BuildingInfo>.FindLoaded(b.name);
                if (prefab == null) return false;
                return prefab.m_requiredExpansion != SteamHelper.ExpansionBitMask.None
                    || prefab.m_requiredModderPack != SteamHelper.ModderPackBitMask.None;
            }

            // Workshop assets follow the "<steamId>.Name_Data" pattern. Strip the optional
            // "{{steamId}}." namespace prefix some plugins inject before checking the dot.
            private static bool BuildingIsWorkshop(string name)
            {
                if (string.IsNullOrEmpty(name)) return false;
                string clean = System.Text.RegularExpressions.Regex.Replace(name, @"^\{\{.*?\}\}\.", "");
                return clean.Contains(".");
            }

            /// <summary>
            /// Returns the theme's display name without the [Vanilla] / [DLC] / [Custom] prefix.
            /// Resolves via the locale key when available so the rendered text follows the in-game
            /// language; falls back to the raw stored name otherwise. Suitable for per-building
            /// origin labels and any context where the prefix would be redundant.
            /// </summary>
            [XmlIgnore]
            public string localizedName
            {
                get { return ResolveLocaleName(); }
            }

            private string ResolveLocaleName()
            {
                if (localeKey != null)
                {
                    try
                    {
                        // Same call the game uses for style display names (see StylesHelper IL)
                        return ColossalFramework.Globalization.Locale.Get(localeKey);
                    }
                    catch { }
                }
                return name;
            }

            [XmlAttribute("style-package"), DefaultValue(null)]
            public string stylePackage = null;

            [XmlArray(ElementName = "Buildings")]
            [XmlArrayItem(ElementName = "Building")]
            public List<Building> buildings = new List<Building>();

            public bool containsBuilding(string name)
            {
                foreach (Building building in buildings)
                {
                    if (building.name == name) return true;
                }
                return false;
            }

            public IEnumerable<Building> getVariations(string baseName)
            {
                return from building in buildings where building.baseName == baseName select building;
            }

            public Building getBuilding(string name)
            {
                foreach (Building building in buildings)
                {
                    if (building.name == name) return building;
                }
                return null;
            }
        }

        public class Building
        {
            [XmlAttribute("name")]
            public string name;

            [XmlIgnoreAttribute]
            public Building builtInBuilding = null;

            [XmlIgnoreAttribute()]
            public bool fromStyle = false;

            [XmlAttribute("level"), DefaultValue(-1)]
            public int level = -1;

            [XmlAttribute("upgrade-name"), DefaultValue(null)]
            public string upgradeName = null;

            [XmlAttribute("base-name"), DefaultValue(null)]
            public string baseName = null;

            [XmlAttribute("spawn-rate"), DefaultValue(10)]
            public int spawnRate = 10;

            [XmlAttribute("include"), DefaultValue(true)]
            public bool include = true;

            [XmlAttribute("dlc"), DefaultValue(null)]
            public string dlc = null;

            [XmlAttribute("environments"), DefaultValue(null)]
            public string environments = null;

            /// <summary>
            /// User opt-in: treat this (normally straight-zoned) building as a corner building so
            /// it is also offered for corner lots. Only meaningful for buildings whose prefab
            /// m_zoningMode is Straight — true corner assets already fill corners natively.
            /// </summary>
            [XmlAttribute("mark-as-corner"), DefaultValue(false)]
            public bool markAsCorner = false;

            public bool Equals(Building other)
            {
                if (other == null) { return false; }
                if (object.ReferenceEquals(this, other)) { return true; }
                return this.name == other.name
                    && this.level == other.level
                    && this.upgradeName == other.upgradeName
                    && this.baseName == other.baseName
                    && this.spawnRate == other.spawnRate
                    && this.include == other.include
                    && this.markAsCorner == other.markAsCorner;
            }

            public Building(string name)
            {
                this.name = name;
            }

            public Building(Building builtInBuilding)
            {
                this.builtInBuilding = builtInBuilding;

                this.name = builtInBuilding.name;
                this.level = builtInBuilding.level;
                this.upgradeName = builtInBuilding.upgradeName;
                this.baseName = builtInBuilding.baseName;
                this.spawnRate = builtInBuilding.spawnRate;
                this.include = builtInBuilding.include;
                this.dlc = builtInBuilding.dlc;
                this.environments = builtInBuilding.environments;
                this.markAsCorner = builtInBuilding.markAsCorner;
            }

            public Building()
            {
            }
        }

        public static Configuration Deserialize(string filename)
        {
            if (!File.Exists(filename)) return null;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filename))
                {
                    return (Configuration)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception e)
            {
                Debugger.Log("Couldn't load configuration (XML malformed or serializer error): " + e.GetType().Name);
                throw;
            }
        }

        public static void Serialize(string filename, Configuration config)
        {
            var xmlSerializer = new XmlSerializer(typeof(Configuration));
            try
            {
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(filename))
                {
                    var configCopy = new Configuration();

                    configCopy.version = config.version;
                    configCopy.UnlockPolicyPanel = config.UnlockPolicyPanel;
                    configCopy.CreateBuildingDuplicates = config.CreateBuildingDuplicates;
                    configCopy.ThemeValidityWarning = config.ThemeValidityWarning;

                    foreach (var theme in config.themes)
                    {
                        var newTheme = new Theme
                        {
                            name = theme.name,
                            stylePackage = theme.stylePackage
                        };
                        foreach (var building in theme.buildings.Where(building =>
                            // a user-added building has to be included, or we don't need it in the config
                            (building.builtInBuilding == null && building.include)

                            // a built-in building that was modified by the user: Only add it to the config if the modification differs
                            || (building.builtInBuilding != null && !building.Equals(building.builtInBuilding))))
                        {
                            newTheme.buildings.Add(building);
                        }
                        if (!theme.isBuiltIn || newTheme.buildings.Count > 0)
                        {
                            configCopy.themes.Add(newTheme);
                        }
                    }

                    xmlSerializer.Serialize(streamWriter, configCopy);
                }
            }
            catch (Exception e)
            {
                Debugger.LogError("Couldn't write configuration file to \"" + Path.GetFullPath(filename) +
                    "\" (working dir: " + Directory.GetCurrentDirectory() + "): " + e.GetType().Name + " — " + e.Message);
                throw;
            }
        }
    }
}
