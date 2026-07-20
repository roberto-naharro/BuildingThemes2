using System;
using System.Linq;
using BuildingThemes.GUI;
using BuildingThemes.HarmonyPatches.BuildingInfoPatch;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;

namespace BuildingThemes
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);

            Debugger.Initialize();

            // Unconditional — always visible in output_log.txt so the dev can verify whether
            // debug logging is active for this session without enabling it after the fact.
            UnityEngine.Debug.Log("[BT2] OnCreated. Debugger.Enabled = " + Debugger.Enabled);

            Debugger.Log("ON_CREATED");
            Debugger.Log("Initializing Mod...");
            Debugger.LogFormat("Version={0} HarmonyId={1} ConfigPath(user-data)={2} ConfigPath(game)={3}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
                BuildingThemesMod.HarmonyId,
                BuildingThemesManager.UserDataConfigPath,
                BuildingThemesManager.GameFolderConfigPath);

            try
            {
                PolicyPanelEnabler.Register();
                BuildingThemesManager.instance.Reset();
                BuildingVariationManager.instance.Reset();

                // Conditionally deploy the prefab-cloning patch based on saved config.
                UpdateConfig();

                Debugger.Log("Mod successfully initialized.");
            }
            catch (Exception e)
            {
                Debugger.LogError("Exception in OnCreated.");
                Debugger.LogException(e);
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            Debugger.Log("ON_LEVEL_LOADED");
            Debugger.OnLevelLoaded();

            try
            {
                Debugger.LogFormat("OnLevelLoaded mode={0} ({1})", mode, (int)mode);

                // Accept all in-game modes (not map/asset/scenario editors)
                bool isGameMode = mode == LoadMode.NewGame
                               || mode == LoadMode.LoadGame
                               || mode == LoadMode.NewGameFromScenario
                               || mode == LoadMode.LoadScenario;
                if (!isGameMode) return;

                RandomBuildings.ResetLogThrottle();

                // Synchronous import: vanilla/DLC styles are already populated by the game's
                // asset loader, so they are available immediately. DSP-created styles may still
                // be empty shells here if DSP's OnLevelLoaded runs after ours.
                BuildingThemesManager.instance.ImportThemes();
                Debugger.Log("ImportThemes done.");
                if (Debugger.Enabled)
                {
                    var allThemes = BuildingThemesManager.instance.GetAllThemes();
                    int totalBuildings = 0;
                    foreach (var t in allThemes) totalBuildings += t.buildings.Count;
                    Debugger.LogFormat("{0} theme(s) loaded, {1} building entries total.", allThemes.Count, totalBuildings);
                }

                // Deferred re-import: fires on the first simulation tick, which is guaranteed
                // to be AFTER every mod has returned from OnLevelLoaded (including DSP).
                // ImportStylesAsThemes() is called directly (bypasses the importedStyles flag)
                // so DSP styles that were empty on the first pass get their buildings now.
                // The fromStyle-clearing logic in AddImportedTheme makes this re-import safe.
                SimulationManager.instance.AddAction(() =>
                {
                    try
                    {
                        BuildingThemesManager.instance.ImportStylesAsThemes();

                        // Re-apply saved district config so DSP-style themed districts
                        // get their theme assignments (they failed name-lookup during OnLoadData
                        // because styles weren't imported yet at that stage).
                        SerializableDataExtension.ApplyPendingConfiguration();

                        // Rebuild spawn pools for any district whose theme now has buildings
                        // after the deferred DSP import.
                        BuildingThemesManager.instance.RefreshDistrictThemeInfos();

                        if (Debugger.Enabled)
                        {
                            var allThemes = BuildingThemesManager.instance.GetAllThemes();
                            int totalBuildings = 0;
                            foreach (var t in allThemes) totalBuildings += t.buildings.Count;
                            Debugger.LogFormat("Deferred re-import done — {0} theme(s), {1} building entries.", allThemes.Count, totalBuildings);
                        }
                        Debugger.Log("Deferred style re-import and district config restore complete.");

#if DEBUG
                        EnvironmentDump.RunIfRequested();
#endif
                    }
                    catch (Exception e) { Debugger.LogException(e); }
                });

                // Mod compatibility check — logs warnings and shows in-game panel for critical conflicts
                var conflicts = ModCompatibilityChecker.Check();
                foreach (var c in conflicts)
                {
                    string msg = "Building Themes 2 [" + c.Level + "] Mod conflict — "
                                 + c.ModName + ": " + c.Reason;
                    if (c.Level == ModCompatibilityChecker.Severity.Critical)
                        UnityEngine.Debug.LogError(msg);
                    else
                        UnityEngine.Debug.LogWarning(msg);
                }
                if (conflicts.Count > 0 && conflicts.Exists(c => c.Level == ModCompatibilityChecker.Severity.Critical))
                {
                    var criticals = conflicts.FindAll(c => c.Level == ModCompatibilityChecker.Severity.Critical);
                    // Queue to main thread so Unity UI is ready (level may still be loading)
                    SimulationManager.instance.m_ThreadingWrapper.QueueMainThread(() =>
                    {
                        try
                        {
                            string text = Localization.Get("CONFLICT_MESSAGE") + "\n\n" +
                                string.Join("\n\n", criticals.ConvertAll(c => "• " + c.ModName + ": " + c.Reason).ToArray());
                            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
                            if (panel != null)
                                panel.SetMessage(Localization.Get("CONFLICT_TITLE"), text, false);
                        }
                        catch (Exception ex)
                        {
                            Debugger.LogException(ex);
                        }
                    });
                }

                if (Debugger.Enabled)
                    BuildingThemesManager.instance.ValidateAllThemes();

                // Warn if themes have missing assets and Skyve is installed (Skyve may have disabled them)
                if (SkyveDetector.IsInstalled)
                {
                    var themes = BuildingThemesManager.instance.GetAllThemes();
                    int missingCount = 0;
                    foreach (var theme in themes)
                    {
                        foreach (var building in theme.buildings)
                        {
                            if (building.include && PrefabCollection<BuildingInfo>.FindLoaded(building.name) == null)
                                missingCount++;
                        }
                    }
                    if (missingCount > 0)
                    {
                        UnityEngine.Debug.LogWarning(string.Format(
                            "Building Themes 2: {0} theme building(s) are missing. " +
                            "Skyve is installed — some assets may be disabled. " +
                            "Use 'Workshop Dependencies' in the Theme Manager to identify missing assets.",
                            missingCount));
                    }
                }

                PolicyPanelEnabler.UnlockPolicyToolbarButton();
                Debugger.Log("PolicyPanelEnabler done.");

                UIThemeManager.Initialize();
                Debugger.LogFormat("UIThemeManager.Initialize done. instance={0}", UIThemeManager.instance != null ? "OK" : "NULL");

                UIStyleButtonReplacer.ReplaceStyleButton();
            }
            catch (Exception e)
            {
                Debugger.LogError("Exception in OnLevelLoaded.");
                Debugger.LogException(e);
            }
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            Debugger.Log("ON_LEVEL_UNLOADING");
            Debugger.OnLevelUnloading();

            if (Debugger.Enabled)
            {
                int themedDistricts = 0;
                for (byte d = 0; d < 128; d++)
                    if (BuildingThemesManager.instance.IsThemeManagementEnabled(d)) themedDistricts++;
                Debugger.LogFormat("OnLevelUnloading — {0} themed district(s) were active.", themedDistricts);
            }
            BuildingThemesManager.instance.Reset();
            ThemePolicyTab.RemoveThemesTab();
            UIThemeManager.Destroy();
            UIDistrictOptionsPanel.Cleanup();
            GUI.UIUtils.ClearAtlasCache();
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Debugger.Log("ON_RELEASED");

            BuildingThemesManager.instance.Reset();
            try { BuildingVariationManager.instance.Reset(); }
            catch (Exception e) { Debugger.LogException(e); }
            PolicyPanelEnabler.Unregister();

            // Revert the prefab-cloning patch (the other patches are unloaded via
            // BuildingThemesMod.OnDisabled which calls Patcher.UnpatchAll).
            try
            {
                InitializePrefabPatch.Revert();
            }
            catch (Exception e)
            {
                Debugger.LogException(e);
            }

            Debugger.Log("OnReleased — patches reverted, managers reset.");
            Debugger.Deinitialize();
        }

        private void UpdateConfig()
        {
            // If config version is 0, disable the cloning feature if it has never been used.
            if (BuildingVariationManager.Enabled)
            {
                bool cloneFeatureUsed = false;

                if (BuildingThemesManager.instance.Configuration.version == 0)
                {
                    foreach (var theme in BuildingThemesManager.instance.Configuration.themes)
                    {
                        foreach (var building in theme.buildings)
                        {
                            if (building.baseName != null)
                            {
                                cloneFeatureUsed = true;
                                break;
                            }
                        }
                        if (cloneFeatureUsed) break;
                    }
                }
                else cloneFeatureUsed = true;

                if (cloneFeatureUsed)
                {
                    try { InitializePrefabPatch.Deploy(); }
                    catch (Exception e) { Debugger.LogException(e); }
                }
                else
                {
                    BuildingVariationManager.Enabled = false;
                }
            }
            BuildingThemesManager.instance.Configuration.version = 1;
            BuildingThemesManager.instance.SaveConfig();
        }
    }
}
