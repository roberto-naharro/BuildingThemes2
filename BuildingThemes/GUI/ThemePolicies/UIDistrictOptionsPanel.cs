using ColossalFramework;
using ColossalFramework.UI;
using System;
using UnityEngine;

namespace BuildingThemes.GUI
{
    /// <summary>
    /// Floating panel with per-district theme options.
    /// Opened via "District Options" in ThemePolicyTab.
    /// Updates every frame to reflect the currently selected district.
    /// </summary>
    public class UIDistrictOptionsPanel : UIPanel
    {
        private UICheckBox m_blacklistCheck;
        private UIDropDown m_levelDropdown;
        private UIDropDown m_missingDropdown;
        private UICheckBox m_autoBulldozeCheck;
        private UICheckBox m_enforceSpecializationCheck;
        private UICheckBox m_preferElectricityCheck;
        private UICheckBox m_preferAdjacentCheck;
        private UICheckBox m_useVanillaFootprintCheck;

        // Size-preference controls
        private UIDropDown m_resSizePref;
        private UIDropDown m_comSizePref;
        private UIDropDown m_indSizePref;
        private UIDropDown m_offSizePref;
        private UIDropDown m_strengthPref;

        // Prevents Update() sync from re-firing eventChanged callbacks
        private bool _updating;

        private static UIDistrictOptionsPanel _instance;

        public static UIDistrictOptionsPanel instance
        {
            get
            {
                if (_instance == null)
                    _instance = UIView.GetAView().AddUIComponent(typeof(UIDistrictOptionsPanel)) as UIDistrictOptionsPanel;
                return _instance;
            }
        }

        public static void Cleanup()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        public void Toggle()
        {
            if (isVisible)
                Hide();
            else
            {
                Debugger.LogFormat("DistrictOptionsPanel opened.");
                Show(true);
                BringToFront();
            }
        }

        public override void Start()
        {
            base.Start();

            // ── Layout constants ──────────────────────────────────────────────
            const float W       = 580f;
            const float TITLE_H = 40f;
            const float X       = 12f;
            const float GAP     = 12f;          // gap between two side-by-side columns
            const float CW      = W - X * 2f;  // 556 — full content width
            const float HW      = (CW - GAP) / 2f; // 272 — half-column width
            const float ROW_GAP = 10f;
            const float SEC_GAP = 5f;           // within a section (label → dropdown)
            const float DROP_H  = 28f;
            const float LBL_H   = 20f;
            const float CHK_H   = 20f;
            const float BTN_H   = 30f;
            const float TS      = 0.8f;         // label textScale

            float y = TITLE_H + 10f;

            backgroundSprite = "UnlockingPanel2";
            isVisible        = false;
            canFocus         = true;
            isInteractive    = true;
            clipChildren     = false; // allow dropdown popup lists to render outside panel bounds
            width            = W;

            relativePosition = new Vector3(
                Mathf.Floor((GetUIView().fixedWidth  - W)     / 2),
                Mathf.Floor((GetUIView().fixedHeight - 480f)  / 2));

            var titleBar = AddUIComponent<UITitleBar>();
            titleBar.title       = Localization.Get("DISTRICT_OPTIONS_TITLE");
            titleBar.iconSprite  = "ToolbarIconZoomOutDistrict";
            titleBar.isModal     = false;

            // ── Blacklist mode (full width) ───────────────────────────────────
            m_blacklistCheck = ThemePolicyTab.CreateCheckBox(this);
            m_blacklistCheck.width            = CW;
            m_blacklistCheck.relativePosition = new Vector3(X, y);
            m_blacklistCheck.text             = Localization.Get("DISTRICT_BLACKLIST_LABEL");
            m_blacklistCheck.tooltip = Localization.Get("DISTRICT_BLACKLIST_TOOLTIP");
            m_blacklistCheck.eventCheckChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                Debugger.LogFormat("[UserAction] district {0} — blacklist mode {1}.", districtId, val ? "ENABLED" : "DISABLED");
                Singleton<BuildingThemesManager>.instance.ToggleBlacklistMode(districtId, val);
            };
            y += CHK_H + ROW_GAP;

            // ── Level behavior  |  Missing asset  (two columns) ─────────────
            AddLabel(X,          y, HW, LBL_H, TS, Localization.Get("DISTRICT_LEVEL_UP_LABEL"));
            AddLabel(X + HW + GAP, y, HW, LBL_H, TS, Localization.Get("DISTRICT_NOT_LOADED_LABEL"));
            y += LBL_H + SEC_GAP;

            m_levelDropdown = UIUtils.CreateDropDown(this);
            m_levelDropdown.width            = HW;
            m_levelDropdown.size             = new Vector2(HW, DROP_H);
            m_levelDropdown.relativePosition = new Vector3(X, y);
            m_levelDropdown.items            = new string[] {
                Localization.Get("DISTRICT_LEVEL_VANILLA"),
                Localization.Get("DISTRICT_LEVEL_STRICT")
            };
            m_levelDropdown.tooltip = Localization.Get("DISTRICT_LEVEL_TOOLTIP");
            m_levelDropdown.eventSelectedIndexChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                if (!BuildingThemesManager.instance.IsThemeManagementEnabled(districtId)) return;
                var behavior = IndexToLevelBehavior(val);
                if (BuildingThemesManager.instance.GetDistrictEmptyLevelBehavior(districtId) == behavior) return;
                Debugger.LogFormat("[UserAction] district {0} — empty-level behavior set to '{1}'.", districtId, behavior);
                BuildingThemesManager.instance.SetDistrictEmptyLevelBehavior(districtId, behavior);
            };

            m_missingDropdown = UIUtils.CreateDropDown(this);
            m_missingDropdown.width            = HW;
            m_missingDropdown.size             = new Vector2(HW, DROP_H);
            m_missingDropdown.relativePosition = new Vector3(X + HW + GAP, y);
            m_missingDropdown.items            = new string[] {
                Localization.Get("DISTRICT_MISSING_SKIP"),
                Localization.Get("DISTRICT_MISSING_FILL"),
                Localization.Get("DISTRICT_MISSING_FALLBACK")
            };
            m_missingDropdown.tooltip = Localization.Get("DISTRICT_MISSING_TOOLTIP");
            m_missingDropdown.eventSelectedIndexChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                if (!BuildingThemesManager.instance.IsThemeManagementEnabled(districtId)) return;
                if ((int)BuildingThemesManager.instance.GetDistrictMissingAssetMode(districtId) == val) return;
                Debugger.LogFormat("[UserAction] district {0} — missing asset mode set to '{1}'.", districtId, (MissingAssetMode)val);
                BuildingThemesManager.instance.SetDistrictMissingAssetMode(districtId, (MissingAssetMode)val);
            };
            y += DROP_H + ROW_GAP;

            // ── Auto-bulldoze (full width) ────────────────────────────────────
            m_autoBulldozeCheck = ThemePolicyTab.CreateCheckBox(this);
            m_autoBulldozeCheck.width            = CW;
            m_autoBulldozeCheck.relativePosition = new Vector3(X, y);
            m_autoBulldozeCheck.text             = Localization.Get("DISTRICT_AUTO_BULLDOZE_LABEL");
            m_autoBulldozeCheck.tooltip = Localization.Get("DISTRICT_AUTO_BULLDOZE_TOOLTIP");
            m_autoBulldozeCheck.eventCheckChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                Debugger.LogFormat("[UserAction] district {0} — auto-bulldoze {1}.", districtId, val ? "ENABLED" : "DISABLED");
                BuildingThemesManager.instance.SetDistrictAutoBulldoze(districtId, val);
            };
            y += CHK_H + 4f;

            // ── Enforce specialization (sub-option of auto-bulldoze) ──────────
            const float INDENT = 20f;
            m_enforceSpecializationCheck = ThemePolicyTab.CreateCheckBox(this);
            m_enforceSpecializationCheck.width            = CW - INDENT;
            m_enforceSpecializationCheck.relativePosition = new Vector3(X + INDENT, y);
            m_enforceSpecializationCheck.text             = Localization.Get("DISTRICT_ENFORCE_SPEC_LABEL");
            m_enforceSpecializationCheck.tooltip = Localization.Get("DISTRICT_ENFORCE_SPEC_TOOLTIP");
            m_enforceSpecializationCheck.eventCheckChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                Debugger.LogFormat("[UserAction] district {0} — enforce specialization {1}.", districtId, val ? "ENABLED" : "DISABLED");
                BuildingThemesManager.instance.SetDistrictEnforceSpecialization(districtId, val);
            };
            y += CHK_H + ROW_GAP;

            // ── Prefer electricity ────────────────────────────────────────────
            m_preferElectricityCheck = ThemePolicyTab.CreateCheckBox(this);
            m_preferElectricityCheck.width            = CW;
            m_preferElectricityCheck.relativePosition = new Vector3(X, y);
            m_preferElectricityCheck.text             = Localization.Get("DISTRICT_PREFER_ELECTRICITY_LABEL");
            m_preferElectricityCheck.tooltip = Localization.Get("DISTRICT_PREFER_ELECTRICITY_TOOLTIP");
            m_preferElectricityCheck.eventCheckChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                Debugger.LogFormat("[UserAction] district {0} — prefer electricity {1}.", districtId, val ? "ENABLED" : "DISABLED");
                BuildingThemesManager.instance.SetDistrictPreferElectricity(districtId, val);
            };
            y += CHK_H + ROW_GAP;

            // ── Cluster against existing buildings (wall-to-wall snap) ────────
            m_preferAdjacentCheck = ThemePolicyTab.CreateCheckBox(this);
            m_preferAdjacentCheck.width            = CW;
            m_preferAdjacentCheck.relativePosition = new Vector3(X, y);
            m_preferAdjacentCheck.text             = Localization.Get("DISTRICT_CLUSTER_LABEL");
            m_preferAdjacentCheck.tooltip = Localization.Get("DISTRICT_CLUSTER_TOOLTIP");
            m_preferAdjacentCheck.eventCheckChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                Debugger.LogFormat("[UserAction] district {0} — prefer adjacent {1}.", districtId, val ? "ENABLED" : "DISABLED");
                BuildingThemesManager.instance.SetDistrictPreferAdjacent(districtId, val);
            };
            y += CHK_H + ROW_GAP;

            // ── Vanilla footprint (full zone area) ────────────────────────────
            m_useVanillaFootprintCheck = ThemePolicyTab.CreateCheckBox(this);
            m_useVanillaFootprintCheck.width            = CW;
            m_useVanillaFootprintCheck.relativePosition = new Vector3(X, y);
            m_useVanillaFootprintCheck.text             = "Allow smaller building models to occupy larger lots (original game behavior)";
            m_useVanillaFootprintCheck.tooltip =
                "When enabled: if no building model exactly matches the zone size, the\n" +
                "game selects a slightly smaller model as a substitute — but the spawned\n" +
                "building still occupies the full zone area. This is the original game logic.\n" +
                "\n" +
                "When disabled (default): the plot shrinks to the model's actual dimensions;\n" +
                "the remaining space is left unoccupied and available for other buildings.\n" +
                "This is Building Themes 2's behaviour — more land-efficient, but may leave\n" +
                "narrow gaps that are difficult to fill later.\n" +
                "\n" +
                "For example: a 3x3 model placed in a 3x4 zone. Enabled -> fills all\n" +
                "4 cells (model stays 3x3, the extra 1x3 becomes a yard). Disabled ->\n" +
                "occupies only 3 cells, leaving a 1x3 strip that other buildings can\n" +
                "later grow into.";
            m_useVanillaFootprintCheck.eventCheckChanged += (c, val) =>
            {
                if (_updating) return;
                byte districtId = GetDistrictId();
                BuildingThemesManager.instance.SetDistrictUseVanillaFootprint(districtId, val);
            };
            y += CHK_H + ROW_GAP;

            // ── Size preference section ───────────────────────────────────────
            AddLabel(X, y, CW, LBL_H, TS, Localization.Get("DISTRICT_SIZE_PREF_LABEL"));
            y += LBL_H + SEC_GAP;

            string[] sizePrefItems = new string[] {
                Localization.Get("DISTRICT_SIZE_DEFAULT"), Localization.Get("DISTRICT_SIZE_BIGGEST"), Localization.Get("DISTRICT_SIZE_WIDEST"),
                Localization.Get("DISTRICT_SIZE_DEEPEST"), Localization.Get("DISTRICT_SIZE_RANDOM"), Localization.Get("DISTRICT_SIZE_SMALLEST"),
                Localization.Get("DISTRICT_SIZE_TALLEST"), Localization.Get("DISTRICT_SIZE_SHORTEST")
            };
            string sizePrefTooltip = Localization.Get("DISTRICT_SIZE_PREF_TOOLTIP");

            // Row 1: Residential | Commercial
            AddLabel(X,            y, HW, LBL_H, TS, Localization.Get("DISTRICT_RESIDENTIAL_LABEL"));
            AddLabel(X + HW + GAP, y, HW, LBL_H, TS, Localization.Get("DISTRICT_COMMERCIAL_LABEL"));
            y += LBL_H + SEC_GAP;

            m_resSizePref = MakeSizePrefDrop(X,            y, HW, DROP_H, sizePrefItems, sizePrefTooltip);
            m_resSizePref.eventSelectedIndexChanged += (c, val) => {
                if (_updating) return;
                BuildingThemesManager.instance.SetDistrictSizePreference(GetDistrictId(), ItemClass.Service.Residential, (SizePreference)val);
            };
            m_comSizePref = MakeSizePrefDrop(X + HW + GAP, y, HW, DROP_H, sizePrefItems, sizePrefTooltip);
            m_comSizePref.eventSelectedIndexChanged += (c, val) => {
                if (_updating) return;
                BuildingThemesManager.instance.SetDistrictSizePreference(GetDistrictId(), ItemClass.Service.Commercial, (SizePreference)val);
            };
            y += DROP_H + SEC_GAP;

            // Row 2: Industrial | Office
            AddLabel(X,            y, HW, LBL_H, TS, Localization.Get("DISTRICT_INDUSTRIAL_LABEL"));
            AddLabel(X + HW + GAP, y, HW, LBL_H, TS, Localization.Get("DISTRICT_OFFICE_LABEL"));
            y += LBL_H + SEC_GAP;

            m_indSizePref = MakeSizePrefDrop(X,            y, HW, DROP_H, sizePrefItems, sizePrefTooltip);
            m_indSizePref.eventSelectedIndexChanged += (c, val) => {
                if (_updating) return;
                BuildingThemesManager.instance.SetDistrictSizePreference(GetDistrictId(), ItemClass.Service.Industrial, (SizePreference)val);
            };
            m_offSizePref = MakeSizePrefDrop(X + HW + GAP, y, HW, DROP_H, sizePrefItems, sizePrefTooltip);
            m_offSizePref.eventSelectedIndexChanged += (c, val) => {
                if (_updating) return;
                BuildingThemesManager.instance.SetDistrictSizePreference(GetDistrictId(), ItemClass.Service.Office, (SizePreference)val);
            };
            y += DROP_H + ROW_GAP;

            // ── Preference strength (half width, left) ────────────────────────
            AddLabel(X, y, HW, LBL_H, TS, Localization.Get("DISTRICT_STRENGTH_LABEL"));
            y += LBL_H + SEC_GAP;

            m_strengthPref = UIUtils.CreateDropDown(this);
            m_strengthPref.width            = HW;
            m_strengthPref.size             = new Vector2(HW, DROP_H);
            m_strengthPref.relativePosition = new Vector3(X, y);
            m_strengthPref.items = new string[] {
                Localization.Get("DISTRICT_STRENGTH_GENTLE"),
                Localization.Get("DISTRICT_STRENGTH_MODERATE"),
                Localization.Get("DISTRICT_STRENGTH_STRONG"),
                Localization.Get("DISTRICT_STRENGTH_ABSOLUTE")
            };
            m_strengthPref.tooltip = Localization.Get("DISTRICT_STRENGTH_TOOLTIP");
            m_strengthPref.eventSelectedIndexChanged += (c, val) => {
                if (_updating) return;
                BuildingThemesManager.instance.SetDistrictPreferenceStrength(GetDistrictId(), (PreferenceStrength)val);
            };
            y += DROP_H + ROW_GAP;

            // ── Spawn Diagnostics (bottom) ────────────────────────────────────
            UIButton diagnosticsBtn = UIUtils.CreateButton(this);
            diagnosticsBtn.width            = CW;
            diagnosticsBtn.size             = new Vector2(CW, BTN_H);
            diagnosticsBtn.relativePosition = new Vector3(X, y);
            diagnosticsBtn.text             = Localization.Get("DISTRICT_DIAGNOSTICS_BUTTON");
            diagnosticsBtn.tooltip = Localization.Get("DISTRICT_DIAGNOSTICS_TOOLTIP");
            diagnosticsBtn.eventClick += (c, p) =>
            {
                try { UIThemeDiagnosticsModal.instance.ShowForDistrict(GetDistrictId()); }
                catch (Exception e) { Debugger.LogException(e); }
            };
            y += BTN_H + 10f;

            height = y;

            // Pre-create diagnostics modal so its Start() runs before the first user click
            UIThemeDiagnosticsModal.instance.Hide();
        }

        public override void Update()
        {
            base.Update();
            if (!isVisible) return;
            if (ToolsModifierControl.policiesPanel == null) return;

            _updating = true;
            try
            {
                byte districtId = GetDistrictId();
                bool managed    = BuildingThemesManager.instance.IsThemeManagementEnabled(districtId);
                bool blacklist  = BuildingThemesManager.instance.IsBlacklistModeEnabled(districtId);

                if (m_blacklistCheck != null && m_blacklistCheck.isChecked != blacklist)
                    m_blacklistCheck.isChecked = blacklist;

                if (m_levelDropdown != null)
                {
                    m_levelDropdown.isEnabled = managed;
                    m_levelDropdown.opacity   = managed ? 1f : 0.5f;
                    if (managed)
                    {
                        int idx = LevelBehaviorToIndex(BuildingThemesManager.instance.GetDistrictEmptyLevelBehavior(districtId));
                        if (m_levelDropdown.selectedIndex != idx)
                            m_levelDropdown.selectedIndex = idx;
                    }
                }

                if (m_missingDropdown != null)
                {
                    m_missingDropdown.isEnabled = managed;
                    m_missingDropdown.opacity   = managed ? 1f : 0.5f;
                    if (managed)
                    {
                        int val = (int)BuildingThemesManager.instance.GetDistrictMissingAssetMode(districtId);
                        if (m_missingDropdown.selectedIndex != val)
                            m_missingDropdown.selectedIndex = val;
                    }
                }

                bool autoBulldozeOn = false;
                if (m_autoBulldozeCheck != null)
                {
                    bool effectiveBlacklist = BuildingThemesManager.instance.IsBlacklistModeEnabled(districtId);
                    m_autoBulldozeCheck.isEnabled = managed && !effectiveBlacklist;
                    m_autoBulldozeCheck.opacity   = (managed && !effectiveBlacklist) ? 1f : 0.5f;
                    autoBulldozeOn = BuildingThemesManager.instance.GetDistrictAutoBulldoze(districtId);
                    if (m_autoBulldozeCheck.isChecked != autoBulldozeOn)
                        m_autoBulldozeCheck.isChecked = autoBulldozeOn;
                }

                if (m_enforceSpecializationCheck != null)
                {
                    bool effectiveBlacklist = BuildingThemesManager.instance.IsBlacklistModeEnabled(districtId);
                    bool canEnforce = managed && !effectiveBlacklist && autoBulldozeOn;
                    m_enforceSpecializationCheck.isEnabled = canEnforce;
                    m_enforceSpecializationCheck.opacity   = canEnforce ? 1f : 0.5f;
                    bool enforce = BuildingThemesManager.instance.GetDistrictEnforceSpecialization(districtId);
                    if (m_enforceSpecializationCheck.isChecked != enforce)
                        m_enforceSpecializationCheck.isChecked = enforce;
                }

                if (m_preferElectricityCheck != null)
                {
                    m_preferElectricityCheck.isEnabled = managed;
                    m_preferElectricityCheck.opacity   = managed ? 1f : 0.5f;
                    bool prefElec = BuildingThemesManager.instance.GetDistrictPreferElectricity(districtId);
                    if (m_preferElectricityCheck.isChecked != prefElec)
                        m_preferElectricityCheck.isChecked = prefElec;
                }

                if (m_preferAdjacentCheck != null)
                {
                    m_preferAdjacentCheck.isEnabled = managed;
                    m_preferAdjacentCheck.opacity   = managed ? 1f : 0.5f;
                    bool prefAdj = BuildingThemesManager.instance.GetDistrictPreferAdjacent(districtId);
                    if (m_preferAdjacentCheck.isChecked != prefAdj)
                        m_preferAdjacentCheck.isChecked = prefAdj;
                }

                if (m_useVanillaFootprintCheck != null)
                {
                    m_useVanillaFootprintCheck.isEnabled = managed;
                    m_useVanillaFootprintCheck.opacity   = managed ? 1f : 0.5f;
                    bool vFoot = BuildingThemesManager.instance.GetDistrictUseVanillaFootprint(districtId);
                    if (m_useVanillaFootprintCheck.isChecked != vFoot)
                        m_useVanillaFootprintCheck.isChecked = vFoot;
                }

                SyncSizeDrop(m_resSizePref, managed, BuildingThemesManager.instance.GetDistrictSizePreference(districtId, ItemClass.Service.Residential));
                SyncSizeDrop(m_comSizePref, managed, BuildingThemesManager.instance.GetDistrictSizePreference(districtId, ItemClass.Service.Commercial));
                SyncSizeDrop(m_indSizePref, managed, BuildingThemesManager.instance.GetDistrictSizePreference(districtId, ItemClass.Service.Industrial));
                SyncSizeDrop(m_offSizePref, managed, BuildingThemesManager.instance.GetDistrictSizePreference(districtId, ItemClass.Service.Office));

                if (m_strengthPref != null)
                {
                    m_strengthPref.isEnabled = managed;
                    m_strengthPref.opacity   = managed ? 1f : 0.5f;
                    int si = (int)BuildingThemesManager.instance.GetDistrictPreferenceStrength(districtId);
                    if (m_strengthPref.selectedIndex != si) m_strengthPref.selectedIndex = si;
                }
            }
            finally { _updating = false; }
        }

        protected override void OnKeyDown(UIKeyEventParameter p)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                p.Use();
                Hide();
            }
            base.OnKeyDown(p);
        }

        private static byte GetDistrictId()
        {
            return ToolsModifierControl.policiesPanel != null
                ? ToolsModifierControl.policiesPanel.targetDistrict
                : (byte)0;
        }

        private static int LevelBehaviorToIndex(EmptyLevelBehavior b)
            => b == EmptyLevelBehavior.StrictThemeOnly ? 1 : 0;

        private static EmptyLevelBehavior IndexToLevelBehavior(int idx)
            => idx == 1 ? EmptyLevelBehavior.StrictThemeOnly : EmptyLevelBehavior.VanillaFallback;

        private UILabel AddLabel(float x, float y, float w, float h, float textScale, string text)
        {
            UILabel lbl = AddUIComponent<UILabel>();
            lbl.autoSize  = false;
            lbl.width     = w;
            lbl.height    = h;
            lbl.textScale = textScale;
            lbl.wordWrap  = true;
            lbl.text      = text;
            lbl.relativePosition = new Vector3(x, y);
            return lbl;
        }

        private UIDropDown MakeSizePrefDrop(float x, float y, float w, float h, string[] items, string tooltip)
        {
            var dd = UIUtils.CreateDropDown(this);
            dd.width            = w;
            dd.size             = new Vector2(w, h);
            dd.relativePosition = new Vector3(x, y);
            dd.items            = items;
            dd.tooltip          = tooltip;
            return dd;
        }

        private static void SyncSizeDrop(UIDropDown dd, bool managed, SizePreference pref)
        {
            if (dd == null) return;
            dd.isEnabled = managed;
            dd.opacity   = managed ? 1f : 0.5f;
            int idx = (int)pref;
            if (dd.selectedIndex != idx) dd.selectedIndex = idx;
        }
    }
}
