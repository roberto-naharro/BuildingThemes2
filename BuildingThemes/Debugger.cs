using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BuildingThemes
{
    public static class Debugger
    {
        public const string ModTag = "Building Themes 2";
        private const string Prefix = ModTag + ": ";

        private static bool initialized = false;

        // Persisted via the game's settings file system (survives game restarts)
        private static readonly SavedBool s_enabled = new SavedBool("debugOutput", "BuildingThemes2", false, true);

        public static bool xmlCorrupt = false;

        private static bool loaded = false;
        private static string exceptions = "";

        public static bool Enabled
        {
            get
            {
                return s_enabled;
            }
            set
            {
                if ((bool)s_enabled == value) return;
                s_enabled.value = value;
                if (value)
                    Initialize();
                else
                    Deinitialize();
            }
        }

        public static void Initialize()
        {
            Deinitialize();
            
            if (Enabled) 
            {
                initialized = true;
            }
        }

        public static void Deinitialize()
        {
            if (initialized)
            {
                initialized = false;
                exceptions = "";
                loaded = false;
            }
        }

        public static void Log(string message)
        {
            if (initialized && Enabled)
                Debug.Log(Prefix + message);
        }

        // Always emitted, regardless of the Enabled toggle. Use sparingly — only for
        // diagnostics we must collect from users who don't have debug output turned on
        // (e.g. the theme save/load path, to investigate "my theme didn't save" reports).
        public static void LogAlways(string message)
        {
            Debug.Log(Prefix + message);
        }

        public static void LogFormat(string format, params object[] args)
        {
            Log(String.Format(format, args));
        }

        // Higher-verbosity tier for per-building / per-frame messages.
        // Same gate as LogFormat but prefixed with [VERBOSE] for easy grepping.
        public static void LogVerbose(string format, params object[] args)
        {
            if (initialized && Enabled)
                Debug.Log("[BT2:VERBOSE] " + String.Format(format, args));
        }

        public static void LogError(string error)
        {
            Debug.LogError(Prefix + error);
        }

        public static void LogException(Exception e)
        {
            string message = "ERROR: " + e.Message + "\n" + e.StackTrace  + "\n";
            
            Debug.LogException(e);

            exceptions += message; 

            if (loaded) ShowExceptions();
        }

        public static void OnLevelLoaded()
        {
            loaded = true;

            Debugger.AppendModList();
            Debugger.AppendThemeList();
            ShowExceptions();
        }

        public static void OnLevelUnloading()
        {
            loaded = false;
        }

        private static void ShowExceptions()
        {
            string text = null;

            if (xmlCorrupt)
            {
                text = "BuildingThemes is unable to read the BuildingThemes.xml file " +
                       "that stores your settings and themes!\n\n" +
                       "To fix it, delete the corrupted file and restart the game:\n" +
                       System.IO.Path.GetFullPath("BuildingThemes.xml") + "\n\n" +
                       "(Your self-made themes and other settings will be lost. Thank your PC for that)";
            }

            else if (exceptions != "")
            {
                text = "Please report this error on the Building Themes workshop page:\n" + exceptions;
            }

            if (text != null)
            {
                UIView.library?.ShowModal<ExceptionPanel>("ExceptionPanel")?.SetMessage("Building Themes Error",text,true);

                exceptions = "";
            }
        }

        public static void AppendModList()
        {
            if (!Enabled) return;

            string message = "Enabled Plugins:\n";

            foreach (var pluginInfo in Singleton<PluginManager>.instance.GetPluginsInfo())
            {
                if (pluginInfo.isEnabled)
                {
                    IUserMod mod = (IUserMod)pluginInfo.userModInstance;

                    message += String.Format("# {0}\n", mod.Name);
                }
            }

            Debug.Log(message);
        }

        public static void AppendThemeList()
        {
            if (!Enabled) return;

            try
            {
                string message = "Loaded Themes:\n";

                foreach (var theme in Singleton<BuildingThemesManager>.instance.GetAllThemes())
                {
                    message += String.Format("# {0}\n", theme.name);
                }

                Debug.Log(message);
            }
            catch
            {
            }
        }
    }
}
