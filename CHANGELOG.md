# Changelog

All notable changes to Building Themes 2 are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/);
versions follow [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.4.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.3.0...v2.4.0) (2026-04-17)


### Features

* update spawn rate handling and diagnostics for building themes ([c007002](https://github.com/roberto-naharro/BuildingThemes2/commit/c007002fec83e152931890863cec431f75e6b1bd))


### Bug Fixes

* add Steam BBCode conversion for release notes in workshop deployment ([b551877](https://github.com/roberto-naharro/BuildingThemes2/commit/b551877e9a221a7ca01ecf8e479283b0e55b8e15))
* implement Steam BBCode conversion and VDF generation for workshop deployment ([eca65f9](https://github.com/roberto-naharro/BuildingThemes2/commit/eca65f950e4d1520fe15062a51418b4cf2189975))

## [2.3.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.2.0...v2.3.0) (2026-04-17)


### Features

* add Auto-bulldoze feature to gradually replace non-theme buildings with themed ones ([860c5a1](https://github.com/roberto-naharro/BuildingThemes2/commit/860c5a1f7c4fe7024749fd6f1babde01d06df961))
* enhance AutoBulldoze functionality with improved logging and reset mechanism ([ec59f40](https://github.com/roberto-naharro/BuildingThemes2/commit/ec59f40c0b670af5fd58509f749b0696cc9fb145))


### Bug Fixes

* update last-release-sha in release-please configuration ([80c7041](https://github.com/roberto-naharro/BuildingThemes2/commit/80c70418053c190159f541fb6f388acd325a4bf0))

## [2.2.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.1.0...v2.2.0) (2026-04-17)


### Features

* add About image to Workshop directory ([327aa86](https://github.com/roberto-naharro/BuildingThemes2/commit/327aa86e97eb01cb54591d3579f862d4a90ce185))
* add asset loading status and filtering options in theme manager UI ([c4125d7](https://github.com/roberto-naharro/BuildingThemes2/commit/c4125d7d6448d3b71af33ddc4900ce49c4549b9b))
* add asset name display and resize functionality in theme manager UI ([4ecb39a](https://github.com/roberto-naharro/BuildingThemes2/commit/4ecb39ab837e75e35b753b09dc037ef6c1872658))
* add compatibility with current game and mods versions ([e6a4421](https://github.com/roberto-naharro/BuildingThemes2/commit/e6a44217238d51b33171389189373e69341192dd))
* add configurable behavior for empty levels and missing assets in theme management ([a7a3a53](https://github.com/roberto-naharro/BuildingThemes2/commit/a7a3a53d2c0de16257c8aa29dce4b02812fd4305))
* add deployment and mounting scripts with environment configuration ([ee6afee](https://github.com/roberto-naharro/BuildingThemes2/commit/ee6afeeb3484d099e1e55e27aa2ea7a9473c5845))
* add environment specification for deployment job ([b30bfef](https://github.com/roberto-naharro/BuildingThemes2/commit/b30bfef7516c14bd2f4f0b6f35962b3877fe4c82))
* add locale key support for themes and improve UI display names ([4df408e](https://github.com/roberto-naharro/BuildingThemes2/commit/4df408ea8375f115403b2e6091a34c1c098eafa7))
* add mod compatibility checker and Skyve detection functionality ([650af43](https://github.com/roberto-naharro/BuildingThemes2/commit/650af43648a59369186f57aa187dbb737f9a8c4b))
* add preparation steps for Steam home directories in deployment workflow ([8429eff](https://github.com/roberto-naharro/BuildingThemes2/commit/8429effca282c3febc5fd87498df286691605401))
* Add Release Please workflow for automated versioning and deployment ([fc28e24](https://github.com/roberto-naharro/BuildingThemes2/commit/fc28e24389f8f632430213bff5c7d53968294b84))
* Add Steam Workshop publishing workflow and update documentation ([f81a1b9](https://github.com/roberto-naharro/BuildingThemes2/commit/f81a1b97a023c803e6ec88ea7998f86fe1576ddf))
* add TOTP secret for Steam Workshop deployment ([d3f6470](https://github.com/roberto-naharro/BuildingThemes2/commit/d3f647056a38fb8f26614e4301fc88c27830fb6a))
* **diagnostics:** add spawn diagnostics and enhanced theme validation ([e21f9b8](https://github.com/roberto-naharro/BuildingThemes2/commit/e21f9b84fe26ef5a83ca523518c9b31c468f30cf))
* enhance building info retrieval and logging in theme manager ([00d72ff](https://github.com/roberto-naharro/BuildingThemes2/commit/00d72ff988416e2e7561569ea3150406512121fa))
* enhance DLC handling and UI display for themes ([39ad42f](https://github.com/roberto-naharro/BuildingThemes2/commit/39ad42f895cb3b2a134734fea00e20d6b0886009))
* enhance logging with verbose output and detailed save/load messages ([0209280](https://github.com/roberto-naharro/BuildingThemes2/commit/0209280cd8a7151036f6acd071b9d6d553e278ad))
* enhance UI layout and tooltip descriptions in theme management panels ([9873a05](https://github.com/roberto-naharro/BuildingThemes2/commit/9873a053fc967cea24e3772814a036838923e431))
* implement auto-bulldoze feature for non-theme buildings with UI options ([1b8ff82](https://github.com/roberto-naharro/BuildingThemes2/commit/1b8ff82ef8c7ad2a1cb34a33f4050bcdbf9b5c7d))
* implement missing asset handling with configurable behavior in theme manager ([853dc5e](https://github.com/roberto-naharro/BuildingThemes2/commit/853dc5e720d98355a11541d5437d701a465eb6b4))
* improve warning message for prefab cloning and clarify save risk ([431f8a5](https://github.com/roberto-naharro/BuildingThemes2/commit/431f8a5235f0c5c611fbddfa3ec4f72996e18f41))
* per-district persistence, code quality ([7b1035c](https://github.com/roberto-naharro/BuildingThemes2/commit/7b1035cc111936b222aafe53e5da2b35dd645581))
* unify Harmony ID usage across modules and enhance error handling in patching ([801eaf2](https://github.com/roberto-naharro/BuildingThemes2/commit/801eaf2e42b87df9ad8ed6ebebfeefee31188017))
* update mod description for clarity and maintenance notice ([2e8e2f9](https://github.com/roberto-naharro/BuildingThemes2/commit/2e8e2f97d4af2a5250b74acc38c6b1bf250096c7))
* update Steam Workshop deployment to use description from file ([57f84de](https://github.com/roberto-naharro/BuildingThemes2/commit/57f84de1ae1a5bc1a9c51e9627bf7536b2a085e3))


### Bug Fixes

* 85 NRE ([5b7ab30](https://github.com/roberto-naharro/BuildingThemes2/commit/5b7ab30ce4f418a8529ce786570b4d5217a4f14d))
* add detailed logging for theme and building operations in various UI components ([e920682](https://github.com/roberto-naharro/BuildingThemes2/commit/e920682166b1cdf9f00e7185aee4a73592c89df3))
* add environment variable to enforce Node.js version for release-please job ([6f27ccf](https://github.com/roberto-naharro/BuildingThemes2/commit/6f27ccf513159395f65ec0dea2a30863c24972bc))
* add path for About.jpg in deploy script ([93091f3](https://github.com/roberto-naharro/BuildingThemes2/commit/93091f3249634ac3ed2b7ff886f0d7ff1079cf3a))
* add source filter for building items and update UI components for better filtering options ([1d838bc](https://github.com/roberto-naharro/BuildingThemes2/commit/1d838bc22ffef403ea1b964483e14bf7a6b32716))
* adjust DLC filter layout and checkbox positions for improved UI consistency ([d026e9b](https://github.com/roberto-naharro/BuildingThemes2/commit/d026e9be9079f992045e202233ca76a4c9ca7ae3))
* centralize mod tag in Debugger — auto-prefix all log output ([9c9f3a2](https://github.com/roberto-naharro/BuildingThemes2/commit/9c9f3a2c4550475a9eb61d003e11ea3164a1f69d))
* correct YAML parse error in workshop-deploy workflow ([eac78c3](https://github.com/roberto-naharro/BuildingThemes2/commit/eac78c3dd6ac9c733985b7c07e3d771b1b6f09b6))
* enhance log messages in ThemeDiagnostics and PolicyPanelEnabler for clarity ([b42f810](https://github.com/roberto-naharro/BuildingThemes2/commit/b42f8103aed98091449535e518b5c190c1fed46c))
* gate all unconditional Debug.Log calls behind Debugger.Enabled ([8ef4133](https://github.com/roberto-naharro/BuildingThemes2/commit/8ef4133bae20a5d8ee284044582376e573db353d))
* implement fallback for renamed workshop assets using Steam prefix in building lookups ([2b3d28f](https://github.com/roberto-naharro/BuildingThemes2/commit/2b3d28f25526e04f00db81e12903df8801e770cf))
* improve error logging and handling in UI components ([d99d829](https://github.com/roberto-naharro/BuildingThemes2/commit/d99d82957bab2980cca6936eee5f7e55005629ad))
* pass change note as env var to prevent backtick command substitution ([37d9c82](https://github.com/roberto-naharro/BuildingThemes2/commit/37d9c820b28cefea2d952858ed560ba92d6f23a9))
* remove description.txt and PreviewImage.png from dist/ content folder ([8208baa](https://github.com/roberto-naharro/BuildingThemes2/commit/8208baa62f62004e937b020e4376c35a1024a6ea))
* replace broken steam-workshop-deploy action with direct steamcmd steps ([8476c6a](https://github.com/roberto-naharro/BuildingThemes2/commit/8476c6a315a2325965112da9efd38aa8fc9758db))
* replace source filter with DLC filter for improved building item filtering ([77e6391](https://github.com/roberto-naharro/BuildingThemes2/commit/77e6391dd7974cd06323b3f73e40b8f5ab982b43))
* replace UnityEngine.Debug.LogException with Debugger.LogException for improved error logging ([1d38ed3](https://github.com/roberto-naharro/BuildingThemes2/commit/1d38ed3a198bfbf319c274bd55dbfc9b9aba71f7))
* throttle per-building spawn/upgrade debug logs in RandomBuildings ([6473711](https://github.com/roberto-naharro/BuildingThemes2/commit/6473711b1491d84a592ea998f1044ef6ce789d88))
* update .gitattributes to include .dll files as binary ([d3e3e7a](https://github.com/roberto-naharro/BuildingThemes2/commit/d3e3e7aaf72e4bf7081dfe0ee986715483b741be))
* update AutoBulldozePatch to use AccessTools for method patching and remove unnecessary file copies in deploy script ([0a0270a](https://github.com/roberto-naharro/BuildingThemes2/commit/0a0270a8cb04a35de26c92e479b32450758a939b))
* update hardcoded pack names and adjust UI element positions for improved layout ([3e1e446](https://github.com/roberto-naharro/BuildingThemes2/commit/3e1e4462953d53affa2f7f207b80e02ee72c810c))
* update steamcmd installation process and directory structure ([7e81262](https://github.com/roberto-naharro/BuildingThemes2/commit/7e81262e56f7e62bf6fc9ad9cd449c64c70ca973))

## [2.1.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.0.0...v2.1.0) (2026-04-17)


### Features

* add environment specification for deployment job ([b30bfef](https://github.com/roberto-naharro/BuildingThemes2/commit/b30bfef7516c14bd2f4f0b6f35962b3877fe4c82))
* add preparation steps for Steam home directories in deployment workflow ([8429eff](https://github.com/roberto-naharro/BuildingThemes2/commit/8429effca282c3febc5fd87498df286691605401))
* add TOTP secret for Steam Workshop deployment ([d3f6470](https://github.com/roberto-naharro/BuildingThemes2/commit/d3f647056a38fb8f26614e4301fc88c27830fb6a))
* implement auto-bulldoze feature for non-theme buildings with UI options ([1b8ff82](https://github.com/roberto-naharro/BuildingThemes2/commit/1b8ff82ef8c7ad2a1cb34a33f4050bcdbf9b5c7d))
* update Steam Workshop deployment to use description from file ([57f84de](https://github.com/roberto-naharro/BuildingThemes2/commit/57f84de1ae1a5bc1a9c51e9627bf7536b2a085e3))


### Bug Fixes

* add environment variable to enforce Node.js version for release-please job ([6f27ccf](https://github.com/roberto-naharro/BuildingThemes2/commit/6f27ccf513159395f65ec0dea2a30863c24972bc))
* correct YAML parse error in workshop-deploy workflow ([eac78c3](https://github.com/roberto-naharro/BuildingThemes2/commit/eac78c3dd6ac9c733985b7c07e3d771b1b6f09b6))
* pass change note as env var to prevent backtick command substitution ([37d9c82](https://github.com/roberto-naharro/BuildingThemes2/commit/37d9c820b28cefea2d952858ed560ba92d6f23a9))
* remove description.txt and PreviewImage.png from dist/ content folder ([8208baa](https://github.com/roberto-naharro/BuildingThemes2/commit/8208baa62f62004e937b020e4376c35a1024a6ea))
* replace broken steam-workshop-deploy action with direct steamcmd steps ([8476c6a](https://github.com/roberto-naharro/BuildingThemes2/commit/8476c6a315a2325965112da9efd38aa8fc9758db))
* update .gitattributes to include .dll files as binary ([d3e3e7a](https://github.com/roberto-naharro/BuildingThemes2/commit/d3e3e7aaf72e4bf7081dfe0ee986715483b741be))
* update steamcmd installation process and directory structure ([7e81262](https://github.com/roberto-naharro/BuildingThemes2/commit/7e81262e56f7e62bf6fc9ad9cd449c64c70ca973))

## [2.0.0] — 2026-04-17

Complete modernisation of the original Building Themes mod by boformer.

### Added

- **Harmony 2.x migration** — replaced the old Detour/Redirection system with
  CitiesHarmony 2.x patches; no longer conflicts with mods that patch the same methods
- **Missing asset handling** — three configurable modes (Skip / Fill with vanilla /
  Fall back to vanilla), set globally in Mod Options or overridden per district
- **Empty level behavior** — Vanilla fallback / Cascade / Strict freeze when a theme has
  no buildings for a particular level
- **Blacklist mode** — invert a district theme so anything not explicitly excluded can
  spawn
- **Spawn Diagnostics modal** — shows accepted/rejected building counts and lists missing
  assets for the selected district
- **Workshop Dependencies modal** — lists all workshop assets in a theme by status
  (loaded / missing), with copy-to-clipboard and Steam overlay subscribe buttons
- **DLC / CCP filter** in the Theme Manager — filter buildings by installed expansion or
  Content Creator Pack (all 29 CCPs named)
- **Asset loading status filter** — Show loaded / Show missing / Show DLC-locked /
  Spawnable only checkboxes
- **Mod Compatibility Checker** — warns on startup if known incompatible mods
  (81 Tiles original, Building Simulation Overhaul, Runways and Taxiways) are active;
  detects Skyve and notes disabled assets in diagnostics output
- **Skyve integration** — Spawn Diagnostics identifies which missing assets are disabled
  in the active Skyve playset
- **Unified Harmony ID** — all patches registered under a single consistent ID
  (`com.github.roberto-naharro.BuildingThemes2`)
- **Per-district persistence** — missing-asset mode and empty-level behavior are now
  saved inside the save game and restored on load
- **Verbose debug logging** — all significant lifecycle, spawn, UI, and save/load events
  are logged when "Generate Debug Output" is enabled in Mod Options
- **Steam Workshop publishing** — `publish.sh` script and GitHub Actions workflow for
  automated deployment to the Workshop

### Fixed

- Double `MemoryStream` allocation on save-game deserialisation
- `debugCount` not reset between level loads (throttle fired immediately on second load)
- Null-reference exception on `<District>` elements with no `<themes>` child in save XML
- `OnDisabled` Harmony unpatch not guarded against shutdown-phase exceptions
- All stray `UnityEngine.Debug.LogException` calls unified to `Debugger.LogException`
- Workshop assets renamed by their creators now resolved by Steam package ID prefix

### Changed

- Minimum supported game version: Cities: Skylines 1.18.x
- Save data format bumped to version 2 (fully backwards-compatible with version 1 saves)
- Theme Manager filter bar reorganised: DLC filter on its own row; checkboxes row below it
