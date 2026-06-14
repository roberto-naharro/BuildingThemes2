# Changelog

All notable changes to Building Themes 2 are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/);
versions follow [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.19.1](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.19.0...v2.19.1) (2026-06-14)


### Bug Fixes

* ship Locale folder in Workshop build (was showing raw localization keys) ([038af20](https://github.com/roberto-naharro/BuildingThemes2/commit/038af2060b571e85dfddafb195aabc42f0f59442))

## [2.19.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.18.0...v2.19.0) (2026-06-12)


### Features

* add localization support and translation framework ([019942e](https://github.com/roberto-naharro/BuildingThemes2/commit/019942ec9d955e4a3724f2b39e50d2f0dc1cb46e))

## [2.18.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.17.0...v2.18.0) (2026-06-12)


### Features

* **building:** add option to mark straight-zoned buildings for corner lots ([971f643](https://github.com/roberto-naharro/BuildingThemes2/commit/971f643135a638d627b9b6af2b1241b8d2bdc3c0))
* **ui:** add corner zoning label to building preview for better asset identification ([d4fee57](https://github.com/roberto-naharro/BuildingThemes2/commit/d4fee57de0a3cea845291db1ac098450f73377ea))

## [2.17.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.16.0...v2.17.0) (2026-06-07)


### Features

* **building:** allow building-extension mods to adjust service and level before selection ([691232d](https://github.com/roberto-naharro/BuildingThemes2/commit/691232d06823551c8c41dd06576f606393824759))

## [2.16.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.15.0...v2.16.0) (2026-05-29)


### Features

* **districts:** wall-to-wall corner clustering with road-overlap safety ([121ef87](https://github.com/roberto-naharro/BuildingThemes2/commit/121ef87de3c5da7b45458e03fd5d46dfe9590587))


### Bug Fixes

* **building:** remove Steam-prefix fallback for building lookup to ensure exact-name matching ([8ea8e49](https://github.com/roberto-naharro/BuildingThemes2/commit/8ea8e4965ca24025f4ce7bf02585fb78fd483877))
* **docs:** enhance description of clustering behavior for new buildings ([7a620ae](https://github.com/roberto-naharro/BuildingThemes2/commit/7a620aea164f634f4d1596c2aaefa4c968c66eac))
* **docs:** update and correct README.md ([917660e](https://github.com/roberto-naharro/BuildingThemes2/commit/917660e9248c4e07875c82090be864edeabdafed))
* **zone:** refine size preference handling for straight lots and exclude corner lots ([3c47fc3](https://github.com/roberto-naharro/BuildingThemes2/commit/3c47fc36f2e3891f25ba70889fc6170c3086148d))

## [2.15.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.14.0...v2.15.0) (2026-05-28)


### Features

* **theme:** enhance theme management with local theme distinction and tagging ([bd2e9cd](https://github.com/roberto-naharro/BuildingThemes2/commit/bd2e9cd0d64f62f972498f4e439ae2e9f08f35b1))

## [2.14.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.13.0...v2.14.0) (2026-05-28)


### Features

* **districts:** add preference for clustering buildings against existing ones ([f386d87](https://github.com/roberto-naharro/BuildingThemes2/commit/f386d87ca4d9920581049d3883754476c1f02e48))

## [2.13.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.12.2...v2.13.0) (2026-05-27)


### Features

* **districts:** add specialization enforcement for auto-bulldoze functionality ([9b7ef44](https://github.com/roberto-naharro/BuildingThemes2/commit/9b7ef445a43313c01efcf2a94986b54aa0e4c539))
* **filter:** add wall-to-wall building filter and detection logic ([91b1c62](https://github.com/roberto-naharro/BuildingThemes2/commit/91b1c622c0e3bc171f90d90cb806e58e53525a6b))
* **theme:** enhance wall detection logic with Y-range validation ([e61ee49](https://github.com/roberto-naharro/BuildingThemes2/commit/e61ee4941b0d7168ead1413302d65ad533f9b9df))


### Bug Fixes

* **theme:** reject multi-pack steam prefix matches in spawn pool ([07173c2](https://github.com/roberto-naharro/BuildingThemes2/commit/07173c23e69a5ae1287fb4694045262f210bde46))

## [2.12.2](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.12.1...v2.12.2) (2026-05-25)


### Bug Fixes

* **import:** prevent double-import of themes to avoid native crashes ([32c20cb](https://github.com/roberto-naharro/BuildingThemes2/commit/32c20cbbbfea65967f758e4a4fc36a62386137f0))

## [2.12.1](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.12.0...v2.12.1) (2026-05-25)


### Bug Fixes

* **configuration:** improve error handling during configuration loading ([17a1fff](https://github.com/roberto-naharro/BuildingThemes2/commit/17a1fffb5e9b95eabca42cb051549f09224636e4))

## [2.12.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.11.4...v2.12.0) (2026-05-23)


### Features

* **dev:** add environment dump tool; fix OnEnabled crash ([0a45035](https://github.com/roberto-naharro/BuildingThemes2/commit/0a4503526049db5a499644db09ba5cc836c13aff))
* **theme-manager:** label DistrictStyle-only buildings in the origin line ([1a93c6d](https://github.com/roberto-naharro/BuildingThemes2/commit/1a93c6dbe611de4dd6a3c6eb427954d45c7e9090))
* **theme-manager:** show DLC origin under each building name ([d4eed17](https://github.com/roberto-naharro/BuildingThemes2/commit/d4eed17f419e9416a40f8b24dd89a667d9de8506))
* **theme-manager:** single green/red badge driven by actual building content ([2336e3f](https://github.com/roberto-naharro/BuildingThemes2/commit/2336e3ffcee73ef365707e41ac09790d7f48f733))
* **themes:** mark European theme as [DLC] ([0b5914a](https://github.com/roberto-naharro/BuildingThemes2/commit/0b5914ae9b73a3ffec6085fc45702aa7012a498b))
* **themes:** replace stale bundled themes with single-source env themes ([89bc82d](https://github.com/roberto-naharro/BuildingThemes2/commit/89bc82de6ed9dc5e8736dbc411650d2e7fdf7c19))


### Bug Fixes

* **locale:** consolidate DLC name resolution into DlcNames; fix missing keys ([0447782](https://github.com/roberto-naharro/BuildingThemes2/commit/0447782c414dcd66569b5e2a673ed18d86400cfe))
* **spawn:** restore exact vanilla parity when no theme is engaged ([2c42477](https://github.com/roberto-naharro/BuildingThemes2/commit/2c42477f916a68a6a756a267350007d8df179a76))
* **theme-manager:** treat DistrictStyle-only buildings as DLC for badge ([99a8a21](https://github.com/roberto-naharro/BuildingThemes2/commit/99a8a215e9b4484c0aaa890ff69b61a49b018dae))
* **themes:** drop unloaded prefabs from bundled env theme imports ([d911969](https://github.com/roberto-naharro/BuildingThemes2/commit/d911969916a3d7305b36b78ce95e220f5ea9d1cb))

## [2.11.4](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.11.3...v2.11.4) (2026-05-21)


### Bug Fixes

* **workshop-deploy:** include BuildingThemes.xml in deployment package ([d78d148](https://github.com/roberto-naharro/BuildingThemes2/commit/d78d148f6b6516e6f1833de7c208e9af06921066))

## [2.11.3](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.11.2...v2.11.3) (2026-05-21)


### Bug Fixes

* **config:** show real path in XML error and don't mask write failures as corrupt XML ([02c08f5](https://github.com/roberto-naharro/BuildingThemes2/commit/02c08f5e12e482d3af607b28b04e13652d3ada2c))
* **ThemeManager:** enforce read-only for built-in themes; fix XML path/SaveConfig errors; deploy XML ([86e5b54](https://github.com/roberto-naharro/BuildingThemes2/commit/86e5b541cbaa67c90b4dd7dcba17428c044b99cf))

## [2.11.2](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.11.1...v2.11.2) (2026-05-09)


### Bug Fixes

* **spawn:** defer to vanilla when no BT2 theme is active for a district ([41377f3](https://github.com/roberto-naharro/BuildingThemes2/commit/41377f3a0d3fca44ca854735a8bec6867438b3cd))

## [2.11.1](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.11.0...v2.11.1) (2026-05-07)


### Bug Fixes

* **BuildingThemesManager:** add guard for areaIndex to prevent out-of-bounds access ([4314ffb](https://github.com/roberto-naharro/BuildingThemes2/commit/4314ffb793a036056be0f0cb4b959e880477bcab))

## [2.11.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.10.1...v2.11.0) (2026-05-06)


### Features

* **theme-filter:** add theme membership filter to building filter UI ([d50a8c7](https://github.com/roberto-naharro/BuildingThemes2/commit/d50a8c713445bb64f76cc553fa91ce940cf229b4))


### Bug Fixes

* **auto-bulldoze:** optimize Postfix method to run once per frame ([edf8b9c](https://github.com/roberto-naharro/BuildingThemes2/commit/edf8b9ced8b0e6a4eabb9c3977014d29e2844bfe))

## [2.10.1](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.10.0...v2.10.1) (2026-05-04)


### Bug Fixes

* **compat:** expose s_intentionalNull for strict-mode integration ([82a4333](https://github.com/roberto-naharro/BuildingThemes2/commit/82a4333d86c7d798721cbd151dd00be404b5d112))

## [2.10.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.9.0...v2.10.0) (2026-05-04)


### Features

* build mod in CI using committed game reference DLLs ([6b04657](https://github.com/roberto-naharro/BuildingThemes2/commit/6b046579cecf6a0cf2595d76c7c420ecf6276aed))

## [2.9.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.8.1...v2.9.0) (2026-05-03)


### Features

* **deploy:** inject version into AssemblyInfo for release builds ([0a6a009](https://github.com/roberto-naharro/BuildingThemes2/commit/0a6a0090693a0d87720a1c8a828435ea94f153e2))

## [2.8.1](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.8.0...v2.8.1) (2026-05-03)


### Bug Fixes

* **patch:** implement abandoned-building replacement logic and position tracking ([552567a](https://github.com/roberto-naharro/BuildingThemes2/commit/552567a0a8784178f55a88943b28e3f6dc3d5d5a))

## [2.8.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.7.2...v2.8.0) (2026-04-28)


### Features

* **ui:** add theme copy functionality and update UI layout ([2cedeb9](https://github.com/roberto-naharro/BuildingThemes2/commit/2cedeb9f63fbc5d9bbc9baccbfbe9f0e5d3e3dd9))

## [2.7.2](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.7.1...v2.7.2) (2026-04-23)


### Bug Fixes

* **compat:** fix empty DSP styles without breaking DLC themes ([7ea7298](https://github.com/roberto-naharro/BuildingThemes2/commit/7ea7298fb6a749fc1b6def345bc4c5d1f7b02e4f))

## [2.7.1](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.7.0...v2.7.1) (2026-04-22)


### Bug Fixes

* **compat:** defer district-style import to fix empty themes with DSP installed ([bd10b29](https://github.com/roberto-naharro/BuildingThemes2/commit/bd10b29982f5dce9b47bd14a04db3a73dd51fae8))

## [2.7.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.6.0...v2.7.0) (2026-04-20)


### Features

* add preference for electricity in district options and update serialization ([0aa8218](https://github.com/roberto-naharro/BuildingThemes2/commit/0aa8218058d18e8f6dd48bda161a21eff2e9edd2))

## [2.6.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.5.0...v2.6.0) (2026-04-20)


### Features

* DSP-inspired spawning improvements and Theme Manager UX ([3813bcf](https://github.com/roberto-naharro/BuildingThemes2/commit/3813bcfbf524341c1a5b72256d220b431194ddbb))


### Bug Fixes

* update VDF generation to use descriptionfile and improve escaping ([7e9c970](https://github.com/roberto-naharro/BuildingThemes2/commit/7e9c970700f17a8471b3e576cfd54ef1806f54bc))

## [2.5.0](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.7...v2.5.0) (2026-04-19)


### Features

* add theme renaming and import functionality from District Styles Plus ([c6a583a](https://github.com/roberto-naharro/BuildingThemes2/commit/c6a583a921c50058fd316acef812e656e1403500))

## [2.4.7](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.6...v2.4.7) (2026-04-19)


### Bug Fixes

* add TutorialUITag to Themes tab to prevent crashes during tab management ([ad63679](https://github.com/roberto-naharro/BuildingThemes2/commit/ad63679f048f8b66a55da6c62e0c63987771da21))

## [2.4.6](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.5...v2.4.6) (2026-04-19)


### Bug Fixes

* add cleanup method for UIDistrictOptionsPanel and invoke during level unloading ([e5f7bb1](https://github.com/roberto-naharro/BuildingThemes2/commit/e5f7bb1f8282606e515602eeaf01e195f6ca22cb))

## [2.4.5](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.4...v2.4.5) (2026-04-18)


### Bug Fixes

* compute spawn position for zone types based on size preference ([eb420d2](https://github.com/roberto-naharro/BuildingThemes2/commit/eb420d28503b4244a4a24b308dbd826605788360))

## [2.4.4](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.3...v2.4.4) (2026-04-18)


### Bug Fixes

* improve comments and documentation for footprint coverage and auto-bulldoze behavior ([840afbc](https://github.com/roberto-naharro/BuildingThemes2/commit/840afbcb89a73edc1549c598778865ef96a92f69))

## [2.4.3](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.2...v2.4.3) (2026-04-18)


### Bug Fixes

* enhance theme validation logic for unconfigured footprints ([9b1f791](https://github.com/roberto-naharro/BuildingThemes2/commit/9b1f7919214d10cf312ef161b6a85720c7fe2cce))
* update README with footprint coverage and vanilla building behavior details ([bc56c01](https://github.com/roberto-naharro/BuildingThemes2/commit/bc56c01b70b3fbe0a14ffda56b3cf43057dfe12a))

## [2.4.2](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.1...v2.4.2) (2026-04-17)


### Bug Fixes

* remove obsolete cascade level behavior and update related logic ([2a8ff49](https://github.com/roberto-naharro/BuildingThemes2/commit/2a8ff497a8a7f24df20e0457d6b3322012457987))
* update docs ([78a9a50](https://github.com/roberto-naharro/BuildingThemes2/commit/78a9a50d9b58034dcc52932f551bb39c47495ad9))

## [2.4.1](https://github.com/roberto-naharro/BuildingThemes2/compare/v2.4.0...v2.4.1) (2026-04-17)


### Bug Fixes

* migrate legacy spawn rate scale from 1–1000 to 1–100 ([db29379](https://github.com/roberto-naharro/BuildingThemes2/commit/db2937949b6188dec4d0dd7d878728914d245c1c))
* prevent null reference when submitting spawn rate in building options ([ab90a5a](https://github.com/roberto-naharro/BuildingThemes2/commit/ab90a5a9e212023bcb1b23b00fb37e80dc3520ae))
* remove numericalOnly restriction from spawn rate text field ([6307c4f](https://github.com/roberto-naharro/BuildingThemes2/commit/6307c4fc4e1550a2f6184960141f81b6f87be946))

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
