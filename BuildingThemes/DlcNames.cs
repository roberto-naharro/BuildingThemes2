using System.Collections.Generic;
using ColossalFramework.Globalization;

namespace BuildingThemes
{
    // Resolves expansion / modder-pack masks to display names. Modder packs go through
    // Locale.Get when a key is known so the rendered text follows the game's language;
    // expansions fall back to hardcoded English (CS1 does not ship locale keys for them).
    internal static class DlcNames
    {
        public static string GetExpansionName(SteamHelper.ExpansionBitMask mask)
        {
            if (mask == SteamHelper.ExpansionBitMask.AfterDark)            return "After Dark";
            if (mask == SteamHelper.ExpansionBitMask.SnowFall)             return "Snowfall";
            if (mask == SteamHelper.ExpansionBitMask.NaturalDisasters)     return "Natural Disasters";
            if (mask == SteamHelper.ExpansionBitMask.InMotion)             return "Mass Transit";
            if (mask == SteamHelper.ExpansionBitMask.GreenCities)          return "Green Cities";
            if (mask == SteamHelper.ExpansionBitMask.Parks)                return "Parklife";
            if (mask == SteamHelper.ExpansionBitMask.Industry)             return "Industries";
            if (mask == SteamHelper.ExpansionBitMask.Campus)               return "Campus";
            if (mask == SteamHelper.ExpansionBitMask.SunsetHarbor)         return "Sunset Harbor";
            if (mask == SteamHelper.ExpansionBitMask.Airport)              return "Airports";
            if (mask == SteamHelper.ExpansionBitMask.PlazasAndPromenades)  return "Plazas & Promenades";
            if (mask == SteamHelper.ExpansionBitMask.FinancialDistricts)   return "Financial Districts";
            if (mask == SteamHelper.ExpansionBitMask.Hotel)                return "Hotels & Retreats";
            if (mask == SteamHelper.ExpansionBitMask.RacesAndParades)      return "Hubs & Transport";
            return "Expansion DLC";
        }

        // Hardcoded CCP names for all 31 packs (extracted from game DLL strings).
        // For packs that also have locale keys we try locale first for localization support.
        private static readonly Dictionary<SteamHelper.ModderPackBitMask, string> s_packNames =
            new Dictionary<SteamHelper.ModderPackBitMask, string>
        {
            { SteamHelper.ModderPackBitMask.Pack1,  "Art Deco"               },
            { SteamHelper.ModderPackBitMask.Pack2,  "High-Tech Buildings"    },
            { SteamHelper.ModderPackBitMask.Pack3,  "European Suburbia"      },
            { SteamHelper.ModderPackBitMask.Pack4,  "University City"        },
            { SteamHelper.ModderPackBitMask.Pack5,  "Modern City Center"     },
            { SteamHelper.ModderPackBitMask.Pack6,  "Modern Japan"           },
            { SteamHelper.ModderPackBitMask.Pack7,  "Train Stations"         },
            { SteamHelper.ModderPackBitMask.Pack8,  "Bridges & Piers"        },
            { SteamHelper.ModderPackBitMask.Pack9,  "Maps"                   },
            { SteamHelper.ModderPackBitMask.Pack10, "Vehicles of the World"  },
            { SteamHelper.ModderPackBitMask.Pack11, "Mid Century"            },
            { SteamHelper.ModderPackBitMask.Pack12, "Seaside Resorts"        },
            { SteamHelper.ModderPackBitMask.Pack13, "Skyscrapers"            },
            { SteamHelper.ModderPackBitMask.Pack14, "Heart of Korea"         },
            { SteamHelper.ModderPackBitMask.Pack15, "Maps 2"                 },
            { SteamHelper.ModderPackBitMask.Pack16, "Shopping Malls"         },
            { SteamHelper.ModderPackBitMask.Pack17, "Sports Venues"          },
            { SteamHelper.ModderPackBitMask.Pack18, "Africa in Miniature"    },
            { SteamHelper.ModderPackBitMask.Pack19, "Railroads of Japan"     },
            { SteamHelper.ModderPackBitMask.Pack20, "Industrial Evolution"   },
            { SteamHelper.ModderPackBitMask.Pack21, "Brooklyn and Queens"    },
            { SteamHelper.ModderPackBitMask.Pack22, "Mountain Village"       },
            { SteamHelper.ModderPackBitMask.Pack23, "Maps 3"                 },
            { SteamHelper.ModderPackBitMask.Pack24, "Countryside"            },
            { SteamHelper.ModderPackBitMask.Pack25, "Emerging Downtown"      },
            { SteamHelper.ModderPackBitMask.Pack26, "Shops of Shibuya"       },
            { SteamHelper.ModderPackBitMask.Pack27, "Maps 4"                 },
            { SteamHelper.ModderPackBitMask.Pack28, "Iconic Brutalism"       },
            { SteamHelper.ModderPackBitMask.Pack29, "Renewed History"        },
            { SteamHelper.ModderPackBitMask.Pack30, "Holiday Getaways"       },
            { SteamHelper.ModderPackBitMask.Pack31, "Mansions and Manors"    },
        };

        // Locale keys for packs that have in-game locale strings (confirmed via monodis on
        // Assembly-CSharp.dll — these are all the STYLES_* keys the game ships).
        // Pack3 (EuropeanSuburbia) uses kEuropeanSuburbiaStyleName instead of kModderPack3StyleName
        // but its buildings carry ModderPackBitMask.Pack3, so it belongs here.
        private static readonly Dictionary<SteamHelper.ModderPackBitMask, string> s_packLocale =
            new Dictionary<SteamHelper.ModderPackBitMask, string>
        {
            { SteamHelper.ModderPackBitMask.Pack3,  "STYLES_EUROPEANSUBURBIA"    },
            { SteamHelper.ModderPackBitMask.Pack5,  "STYLES_MODDERPACKFIVE"      },
            { SteamHelper.ModderPackBitMask.Pack11, "STYLES_MODDERPACKELEVEN"    },
            { SteamHelper.ModderPackBitMask.Pack14, "STYLES_MODDERPACKFOURTEEN"  },
            { SteamHelper.ModderPackBitMask.Pack16, "STYLES_MODDERPACKSIXTEEN"   },
            { SteamHelper.ModderPackBitMask.Pack18, "STYLES_MODDERPACKEIGHTEEN"  },
            { SteamHelper.ModderPackBitMask.Pack20, "STYLES_MODDERPACKTWENTY"    },
            { SteamHelper.ModderPackBitMask.Pack21, "STYLES_MODDERPACKTWENTYONE" },
            { SteamHelper.ModderPackBitMask.Pack24, "STYLES_MODDERPACKTWENTYFOUR"},
            { SteamHelper.ModderPackBitMask.Pack25, "STYLES_MODDERPACKTWENTYFIVE"},
            { SteamHelper.ModderPackBitMask.Pack26, "STYLES_MODDERPACKTWENTYSIX" },
            { SteamHelper.ModderPackBitMask.Pack31, "STYLES_MODDERPACKTHIRTYONE" },
        };

        public static string GetModderPackName(SteamHelper.ModderPackBitMask mask)
        {
            // Try locale key first for language support.
            string localeKey;
            if (s_packLocale.TryGetValue(mask, out localeKey))
            {
                try
                {
                    string name = Locale.Get(localeKey);
                    if (!string.IsNullOrEmpty(name)) return name;
                }
                catch { }
            }
            // Fall back to hardcoded English name.
            string hardcoded;
            if (s_packNames.TryGetValue(mask, out hardcoded))
                return hardcoded;
            return "CCP";
        }

        // Maps the game's DistrictStyle.Name constants (full strings with hash suffix, confirmed
        // via monodis) to locale keys. Used by AddStyleTheme to localise built-in style theme names.
        // All STYLES_* keys confirmed from Assembly-CSharp.dll strings table.
        private static readonly Dictionary<string, string> s_styleLocaleKeys =
            new Dictionary<string, string>
        {
            { DistrictStyle.kEuropeanStyleName,         "STYLES_EUROPEAN"            },
            { DistrictStyle.kEuropeanSuburbiaStyleName, "STYLES_EUROPEANSUBURBIA"    },
            { DistrictStyle.kModderPack5StyleName,      "STYLES_MODDERPACKFIVE"      },
            { DistrictStyle.kModderPack11StyleName,     "STYLES_MODDERPACKELEVEN"    },
            { DistrictStyle.kModderPack14StyleName,     "STYLES_MODDERPACKFOURTEEN"  },
            { DistrictStyle.kModderPack16StyleName,     "STYLES_MODDERPACKSIXTEEN"   },
            { DistrictStyle.kModderPack18StyleName,     "STYLES_MODDERPACKEIGHTEEN"  },
            { DistrictStyle.kModderPack20StyleName,     "STYLES_MODDERPACKTWENTY"    },
            { DistrictStyle.kModderPack21StyleName,     "STYLES_MODDERPACKTWENTYONE" },
            { DistrictStyle.kModderPack24StyleName,     "STYLES_MODDERPACKTWENTYFOUR"},
            { DistrictStyle.kModderPack25StyleName,     "STYLES_MODDERPACKTWENTYFIVE"},
            { DistrictStyle.kModderPack26StyleName,     "STYLES_MODDERPACKTWENTYSIX" },
            { DistrictStyle.kModderPack31StyleName,     "STYLES_MODDERPACKTHIRTYONE" },
        };

        /// <summary>
        /// Returns the locale key for a built-in DistrictStyle name, or null if unknown.
        /// Used by AddStyleTheme to set theme.localeKey for in-game language support.
        /// </summary>
        public static string GetStyleLocaleKey(string styleName)
        {
            string key;
            return s_styleLocaleKeys.TryGetValue(styleName, out key) ? key : null;
        }
    }
}
