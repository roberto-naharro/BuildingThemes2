# Changelog

All notable changes to Building Themes 2 are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/);
versions follow [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
