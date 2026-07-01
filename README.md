# Building Themes 2

Apply building themes to districts: only buildings in the selected themes will grow in a
zone. Subscribe to theme mods from the Workshop, or create and manage your own themes
entirely in-game.

**Building Themes 2** is a community-maintained fork of the original
[Building Themes](https://github.com/boformer/BuildingThemes) by
[boformer (Sebastian Schöner)](https://github.com/boformer), updated to run on modern
Cities: Skylines with Harmony 2.x.

[Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3708105182) ·
[Report a bug](https://github.com/roberto-naharro/BuildingThemes2/issues) ·
[GitHub](https://github.com/roberto-naharro/BuildingThemes2)

> **Maintenance notice:** This mod is maintained by a single volunteer in their spare time.
> Bug reports and feature requests are read and appreciated, but fixes may take weeks or
> months depending on availability. If you hit a problem, please
> [open an issue](https://github.com/roberto-naharro/BuildingThemes2/issues) so it is
> tracked: nothing gets forgotten, just worked on when time allows.

---

## Requirements

- **[Harmony (Mod Dependency)](https://steamcommunity.com/sharedfiles/filedetails/?id=2040656402)**:
  subscribe and enable this first. The mod silently does nothing if Harmony is not installed.

---

## Quick Start

1. Open a **district policy panel** → go to the **Themes** tab.
2. Click **Theme Manager** → select a theme and include buildings (or import a style).
3. Back in the **Themes** tab, check **Enable Theme Management for this district**.
4. Enable one or more themes using the checkboxes.
5. Zone the area: only buildings in the active themes will grow.

If no theme is active for a district, any growable building can spawn (vanilla behavior).

---

## Features

- **Per-district themes**: assign different building sets to each district.
- **Theme Manager**: full UI to browse, filter, include/exclude individual buildings.
- **DLC & CCP filter**: filter the building list by installed DLC or Content Creator Pack.
- **Workshop Dependencies modal**: see exactly which assets are missing, copy their IDs,
  or subscribe to them directly via the Steam overlay.
- **Spawn Diagnostics**: see accepted/rejected building counts, missing-asset lists,
  and a live count of non-theme buildings currently placed in the district.
- **Auto-bulldoze**: gradually demolish growable buildings that don't belong to the
  district's active themes, replacing them with themed buildings over time. An optional
  sub-mode also removes themed buildings that don't match the district's active
  specialization (farming, eco, tourism, and so on).
- **Cluster against existing buildings (wall-to-wall)**: force new buildings to spawn next
  to existing ones and slide them along the road so their walls touch, removing the small
  gaps that appear when a building's mesh is narrower than its lot.
- **Prefer zones with electricity**: only spawn new buildings in zone cells already
  connected to the power grid, so growth follows your electricity network.
- **Asset Cloning**: duplicate a building and assign a different wealth level to it.
- **Missing asset handling**: three modes for how to respond when a subscribed building
  isn't loaded.
- **Level behavior**: control what happens when a building tries to level up but your
  theme has no building for that level.
- **Blacklist mode**: invert the theme: everything spawns except what you explicitly
  exclude.
- **Spawn weight**: control how often a building appears relative to others in the same
  zone, level, and footprint slot.
- **Size preference**: bias spawning toward larger, smaller, widest, or deepest footprints
  per zone type, with adjustable strength.
- **Rename themes**: rename any custom theme directly from the Theme Manager.
- **Import from District Styles Plus**: convert District Styles Plus styles into Building
  Themes 2 themes with one click (see [District Styles Plus](#district-styles-plus)).

---

## Theme Manager

Open from the **Themes** tab in the district policy panel.

### Filters

| Filter | Description |
| --- | --- |
| Zone toggles | Show/hide buildings by zone type |
| Display (Origin) | All / Default (vanilla/DLC) / Custom (Workshop) / Cloned |
| Display (Status) | All / Included / Excluded |
| Level | Filter by building level (1–5) |
| Size | Filter by footprint (width × depth); each axis is independent: set one to All and the other to a specific value |
| Height | Filter by building height in metres (min / max); unloaded assets are excluded when the filter is active |
| DLC / CCP | Filter by installed expansion or Content Creator Pack |
| Name | Filter by name or Steam Workshop ID |
| Show loaded / missing / DLC/Env | Toggle visibility of asset groups (loaded, not-loaded workshop assets, and DLC/environment-gated assets) |
| Spawnable only | Show only buildings that are loaded, included, and have zone-valid dimensions |
| Wall-to-wall | Show only buildings the mod classifies as wall-to-wall (flush against neighbours) |

### Theme Actions

- **New Theme**: create a blank theme with a custom name.
- **Copy Theme**: duplicate the selected theme (including a read-only built-in one) into a
  new local, editable theme.
- **Rename Theme**: rename the selected theme. Disabled for built-in themes
  (`[Vanilla]` / `[DLC]` / `[Custom]`); only local themes can be renamed.
- **Delete Theme**: permanently remove a local theme.
- **Workshop Dependencies**: lists all workshop assets in the selected theme (loaded /
  missing), with copy-to-clipboard and one-click Steam subscribe buttons.

### Bulk Actions

- **Include All / None**: include or exclude everything in the current filtered view.
- **Include Valid**: include all loaded, in-bounds-dimension buildings in the view.
- **Exclude Missing**: remove all unloaded assets from the current filtered view.

### Per-Building Options

- **Spawn weight** (1–100, default 10): see [Spawn Weight](#spawn-weight) below.
- **Upgrade building**: forces a specific building when this one levels up.
- **Clone Building**: creates a copy with a different wealth level.
- **Asset name**: the internal prefab name (read-only, click to select and copy). For Workshop assets the Steam ID is shown in the label.
- **Plop**: immediately places one instance of the building anywhere in the city (useful for testing).
- **Bulldoze All**: removes every placed instance of this building from the entire city at once.

### Building List Indicators

Each row uses text colour to signal asset status:

| Text colour | Meaning |
| --- | --- |
| White | Asset loaded and available |
| Yellow | Workshop asset not loaded (unsubscribed, disabled, or failed to load) |
| Grey | Asset unavailable (DLC not owned or wrong map environment) |
| Cyan | Cloned building (generated at level load) |

### Theme List Indicators

A **♦** badge next to a theme summarises what it needs to be usable (hover it for a
tooltip):

| Badge | Meaning |
| --- | --- |
| Green **♦** | Vanilla-only theme. No DLC or workshop assets required. |
| Red **♦** | Needs DLC, but no workshop subscriptions. (Owning the DLC is enough.) |
| *(no badge)* | The theme contains at least one workshop asset. |

The red badge is informational, not an error: it simply means the theme relies on DLC
content. If a theme that needs DLC you don't own would otherwise appear, it is hidden
instead, so a visible `[DLC]` theme always means you own that DLC.

### Building Preview Panel

Selecting a building shows a preview with its name, category, level, height, and footprint.
Below the name it also lists the building's **origin** (`Vanilla asset`, `Workshop`, or
`Included in <DLC name>`) and, when the building is classified as **wall-to-wall** by the
mod's mesh analysis, a **Wall to wall** label.

### Theme Prefixes: what's editable, and same-named themes

Theme names carry a prefix that tells you where the theme comes from and whether
you can edit it:

| Prefix | Source | Editable? |
| --- | --- | --- |
| *(no prefix)* | **Local** theme you created or copied | **Yes**: rename, delete, change includes / spawn rates / upgrades |
| `[Vanilla]` | Built-in base-game district style | No (read-only reference) |
| `[DLC]` | Built-in district style from a DLC / content pack (e.g. European) | No (read-only reference) |
| `[Custom]` | A theme registered by **another mod** (district style or `BuildingThemes.xml`) | No (read-only reference) |

Only **local** (un-prefixed) themes can be edited. The tagged themes are read-only
**references**: they always reflect the game's own content, so they're never
modified in place.

**Want to tweak a built-in theme?** Select it and use **Copy Theme** to make a local,
editable copy, then edit that.

**Same name, different theme:** because the tagged themes are separate from your local
ones, you can have a local theme with the **same name** as a tagged one (for example a
local `European` alongside the built-in `[DLC] European`). They are distinct themes; the
prefix is what tells them apart. Local theme names only need to be unique **among your
own local themes**. District assignments remember exactly which one you picked (see
[How Data Is Saved](#how-data-is-saved)), so a local theme stays selected after a reload
even when a built-in theme shares its name.

### Spawn Weight

Spawn weight controls how often a building is chosen relative to the other buildings
competing for the same **slot**: same zone type, level, and footprint size.

**How the selection works:**

The game picks a random building from a weighted pool. Each building is entered into
that pool once per unit of weight, so a building at weight 20 has twice as many entries
as one at weight 10 and is picked roughly twice as often.

| Weight | Effect |
| --- | --- |
| 1 | Appears very rarely relative to buildings with higher weight |
| 10 *(default)* | Baseline: all buildings at the default weight spawn with equal frequency |
| 20 | Appears roughly twice as often as a weight-10 building |
| 100 | Maximum: appears ten times as often as the default |

**Important:** weight is only meaningful when two or more buildings share the same slot.
A building that is alone in its slot always spawns regardless of its weight value.

**Example:** you have three 2×2 Level 1 Residential Low buildings in your theme:

| Building | Weight | Share of spawns |
| --- | --- | --- |
| House A | 10 | 25 % |
| House B | 10 | 25 % |
| House C | 20 | 50 % |

To make a building appear rarely, lower its weight toward 1. To effectively remove it
from a theme without deleting it, uncheck **Include** instead of setting weight to 1.

### How Spawning Works

A new zone always spawns a Level 1 building first: include one in your theme. It upgrades
when the zone's requirements are met. The Level 2 building must have the same width and
equal or smaller length than the Level 1.

Example: a 2×3 L1 Low Residential can upgrade to a 2×3, 2×2, or 2×1 L2.

By default, when a building model is smaller than the zone, the mod shrinks the plot to
the model's actual footprint (unless props or trees occupy the extra space). This enables
tight layouts like the UK Terraced Housing theme. To restore the original game behaviour —
where slightly smaller models can still be selected, and the spawned building always fills
the full zone area — enable the **Allow smaller building models to occupy larger lots**
option in District Options.

**Footprint coverage:** the game generates zone lots in many sizes (1×1, 1×2, 2×2, 2×3,
2×4, corner lots, and so on). Your theme only needs to cover the sizes you actually want,
but each lot must be matched by at least one theme building of that exact footprint (or
smaller, if the shrink fallback can reach it).

When a lot has **no matching theme building**, the mod tries progressively narrower
footprints (the shrink loop). If a smaller-width theme building is found, it is placed and
the plot is trimmed. If nothing fits (because your theme simply has no buildings for any
reachable footprint), **the lot stays empty**. Vanilla buildings are not used to fill
uncovered footprints; they are only used for missing workshop assets (see Missing Assets)
and for level-up behaviour when Level Behavior is set to Vanilla fallback.

Lots left empty this way are not permanent: when demand shifts and the game re-evaluates
the zone, a different lot layout may be tried, or a theme building of the right size added
later will fill them naturally.

If your district has many empty spots, check which lot sizes your theme covers using
**Spawn Diagnostics** (District Options). The "Accepted" count tells you which footprint
pools are active.

---

## District Options

Open via the **District Options** button in the Themes tab.

| Option | Description |
| --- | --- |
| Allow buildings not in any theme | Blacklist mode: anything not explicitly excluded can spawn |
| Auto-bulldoze non-theme buildings | Gradually demolishes growable buildings in the district that are not valid for the active themes. Replacements follow the normal themed spawn rules. |
| ↳ Also remove non-specialized buildings | Sub-option (only active when auto-bulldoze is on): also removes themed buildings whose sub-service does not match the district's active specialization (e.g. generic industry in a farming district). Use this to fully transition a district to its specialization. Has no effect when no specialization policy is active. |
| Prefer zones with electricity | Only spawn new buildings in zone cells that are already connected to the electricity grid. See [Prefer Zones With Electricity](#prefer-zones-with-electricity) below. |
| Cluster against existing buildings (wall-to-wall) | Force new buildings to spawn next to existing ones and slide them along the road so their walls touch, closing the small mesh-vs-cell gaps you'd otherwise see between buildings. See [Cluster Against Existing Buildings](#cluster-against-existing-buildings-wall-to-wall) below. |
| Allow smaller building models to occupy larger lots (vanilla footprint) | When enabled, slightly smaller models can still be selected, and the spawned building fills the full zone area — matching original game behavior. When disabled (default), the plot shrinks to the model's actual size, leaving remaining space for other buildings. See [How Spawning Works](#how-spawning-works). |
| Level behavior | What happens when a building levels up but the theme has no building for that level: **Vanilla fallback** (default) or **Strict** (freeze upgrades) |
| Missing asset handling | Per-district override of the global missing-asset mode |
| Size preference (4 dropdowns) | Bias spawning toward a particular footprint size or height for each zone type: see [Size Preference](#size-preference) |
| Preference strength | How strongly size preference overrides spawn weight: Gentle / Moderate / Strong |
| Spawn Diagnostics | Accepted/rejected counts, missing-asset list, and a live list of non-theme buildings currently placed in the district |

---

## Auto-Bulldoze

Enable **Auto-bulldoze non-theme buildings** in District Options to have the mod
gradually demolish any growable zone building in that district that is not part of the
active themes. Replacements spawn according to the normal themed-spawn rules.

- The scan runs in the background: buildings are removed a batch at a time so performance
  impact is minimal. The pace can be configured in **Mod Options → Building Themes →
  Auto-bulldoze pace**: Gentle (~26 s full pass), Normal (~6.5 s, default), or Aggressive
  (~1.6 s).
- Only active in districts where **Theme Management** is enabled and **Blacklist mode** is
  off (blacklist mode has no concept of "non-theme").
- After changing which buildings are included in a theme, the scan resets so newly-invalid
  buildings are caught quickly rather than waiting for the cursor to cycle through the
  whole city.
- Use **Spawn Diagnostics** (District Options) to see which non-theme buildings are
  currently present in the district before and after enabling auto-bulldoze.

### Enforce District Specialization

Auto-bulldoze only checks theme membership: a building that *is* in your theme is never
removed, even if it does not match the district's active specialization. This means a
generic industrial building in a farming-specialized district will stay indefinitely if
it is part of the active theme, because theme enforcement does not consider specialization.

Enable **Also remove non-specialized buildings** (sub-option, only available when
auto-bulldoze is on) to extend the scan: buildings in the theme whose sub-service does
not match the active specialization are also gradually removed. Examples:

- A farming district (Agriculture policy) → generic industrial buildings are removed,
  replaced by farming industry from the theme.
- A self-sufficient district (Green Cities policy) → non-eco residential buildings are
  removed, replaced by eco residential from the theme.
- A tourism district → non-tourist/leisure commercial buildings are removed.

The sub-service match uses the same position-based resolver the game itself uses
(`GetIndustryType` / `GetResidentialType` / `GetCommercialType`), so the result is
consistent with what would actually spawn on an empty lot in that district. If no
specialization policy is active the sub-option has no additional effect.

**What auto-bulldoze considers valid:** a building is valid for a district if and only if
its exact prefab appears in the active theme pool for its footprint and zone type. This
applies to all buildings regardless of origin (base-game, DLC, or Workshop). If you remove
a building from your theme, any placed copy of that building will be demolished the next
time the scan reaches it, even if it is a DLC or vanilla building.

**Uncovered footprints and empty lots:** if your theme has no buildings for a lot size,
that lot spawns empty (or a smaller theme building via the shrink loop). Auto-bulldoze
will demolish any building standing on such a lot that is no longer in the theme. Once
demolished, the lot stays empty until the game re-evaluates the zone layout or you add a
theme building that covers that footprint. Vanilla buildings do **not** fill uncovered
footprints: they are only used for missing workshop assets (see Missing Assets below).

**"Fill with vanilla" mode and auto-bulldoze:** when Missing Asset handling is set to
*Fill with vanilla*, vanilla buildings are added to the spawn pool to supplement slots
where theme buildings are missing. Those supplemented vanilla buildings are considered
valid by auto-bulldoze and will not be removed: they exist to replace the missing
workshop asset, not as a sign of an uncovered footprint.

---

## Prefer Zones With Electricity

Enable **Prefer zones with electricity** in District Options to make the mod skip zone
cells that are not yet connected to the electricity grid. Only cells with active
conductivity are used for new spawns; the rest wait until power reaches them.

> ⚠ **Spawning will be noticeably slower** while large parts of the district are
> unelectrified. Plan your power grid before zoning, or enable this option only after
> electricity infrastructure is in place.

**Automatic fallback:** after 40 consecutive zone blocks are skipped due to no
electricity, the filter suspends itself so growth is never permanently blocked. As soon
as the game finds an electrified block in the district the counter resets and the
preference re-activates automatically.

**Typical workflow:**

1. Lay out roads and power lines first.
2. Zone the area.
3. Enable **Prefer zones with electricity** in District Options.
4. New buildings only appear where electricity reaches: extend power lines to grow the
   district further.

---

## Cluster Against Existing Buildings (wall-to-wall)

Enable **Cluster against existing buildings (wall-to-wall)** in District Options to
force new buildings to grow against existing ones and physically touch their neighbours,
eliminating the small visible gaps that arise when a building's mesh is narrower than
its 8 m lot allocation.

The option does two things on every spawn attempt:

1. **Adjacency filter.** A building only spawns where it has a neighbour on the same
   street frontage to cluster against. Spawns that would land isolated are skipped and
   the game retries elsewhere, so growth radiates outward from existing construction
   instead of dropping lone buildings on far corners.
2. **Position snap.** When a spawn is accepted, the new building is slid **along the
   road it faces** until its wall meets the neighbour's wall. The slide distance is
   capped (roughly one cell) so it only closes real gaps, never makes large jumps. The
   snap measures each prefab's actual `mesh.bounds`, so buildings whose meshes are
   narrower than their cell footprint close their visible gap correctly.

### Corners

Corner buildings are clustered on **both** streets they front: the building is nudged
toward its neighbour down each road independently. A straight building next to a corner
also snaps flush to it. So intersections form continuous wall-to-wall blocks rather than
leaving a gap on one side.

### How the snap stays safe

- **Always moves along the building's own frontage.** Because the slide is parallel to
  the road the building faces, it can never push the building sideways into that road,
  and the capped distance keeps it from over-reaching.
- **Same frontage line only.** A candidate counts as a neighbour only when it sits on
  the same street frontage (overlapping perpendicular to the road). Buildings across the
  street or in a back row are ignored. A neighbour's own facing does not matter, which is
  what lets a building snap to a perpendicular corner.
- **Never snaps across a road.** Before committing, the snapped position is validated
  against the road network. If closing the gap would push the building into a
  perpendicular road or pedestrian street, the snap is reverted and the building stays at
  its normal position. (This covers the narrow wall-to-wall pedestrian streets from
  Plazas & Promenades.)
- **No-shift fallback.** If no qualifying neighbour is found, the building spawns at its
  normal cell-aligned position.

### What it works with

- **Wall-to-wall themes** are the prime use case: the snap closes the residual
  mesh gap that wall-to-wall walls were always supposed to hide.
- **Mixed themes work too.** The option is independent of the theme contents and is
  applied to whatever building the spawn pool produces, base-game, DLC, or Workshop.
- **Diagonal and curved roads** are handled: the snap works along the actual road
  direction, not just north–south / east–west streets.

### Empty-district fallback

When you first enable theme management on a district with no buildings yet, there's
nothing to cluster against. To avoid permanently blocking growth, the adjacency filter
suspends itself after a run of skipped spawns so the first building can land anywhere.
Once it appears, later spawns cluster against the growing footprint automatically.

### Typical workflow

1. Build a couple of seed buildings in the district (zone normally, or plop one).
2. Enable **Cluster against existing buildings** in District Options.
3. As demand spawns new buildings they appear adjacent to the seed and slide flush
   against it. Growth expands outward in a continuous wall.

---

## Missing Assets

**Missing asset handling** controls what happens when a workshop building in your theme is
not loaded (unsubscribed, disabled in Skyve, or failed to load). This setting applies to
**initial spawning only**: it has no effect on how existing buildings level up.

Set the default in **Mod Options → Building Themes**, or override per district in
**District Options**.

| Mode | Behaviour |
| --- | --- |
| **Skip** | The missing building is quietly dropped. Theme still applies using only loaded assets. Areas may be sparse if many assets are missing. |
| **Fill with vanilla** *(default)* | For each missing asset, vanilla buildings of the same zone type and size supplement the pool. Your loaded theme buildings still appear; vanilla fills the gaps. |
| **Fall back to vanilla** | If any building is missing in a size/level slot, that entire slot falls back to vanilla buildings only. No sparse areas, but theme coverage is reduced. |

---

## Level Behavior

**Level behavior** controls what happens when a building tries to level up but your theme
has **no buildings at all** for the target level. This setting applies to **upgrades only**
- it does not affect which buildings spawn on new zones.

Set the default in **Mod Options → Building Themes**, or override per district in
**District Options**.

| Mode | Behaviour |
| --- | --- |
| **Vanilla fallback** *(default)* | The game picks a vanilla building of the upgraded level. The building changes appearance on upgrade. |
| **Strict** | Upgrades are blocked entirely. The building stays at its current level indefinitely. |

> **Note:** These two settings are independent. "Fill with vanilla" (missing asset mode)
> fills gaps at spawn time; it does not affect upgrades. "Vanilla fallback" (level behavior)
> applies at upgrade time; it does not affect which buildings spawn on new zones.
>
> To control which specific building a level-up produces, use the **Upgrade building** field
> in the Theme Manager options panel for that building. That explicit mapping takes priority
> over the Level behavior setting.

---

## Size Preference

**Size preference** lets you bias which building footprint sizes spawn in a district, per
zone type. Set it in **District Options**: one dropdown each for Residential, Commercial,
Industrial, and Office.

| Mode | Effect |
| --- | --- |
| **Default** | Original game behaviour: tries the widest available lot first, shrinks width until a theme building fits |
| **Biggest first** | Prefers buildings with the largest total footprint (width × depth) |
| **Widest first** | Prefers widest buildings; tie-break by shallowest depth |
| **Deepest first** | Prefers deepest buildings; tie-break by narrowest width |
| **Random (weight only)** | No size bias: selection is purely by spawn weight |
| **Smallest first** | Prefers buildings with the smallest total footprint (width × depth) |
| **Tallest first** | Prefers the tallest buildings by mesh height |
| **Shortest first** | Prefers the shortest buildings by mesh height |

When any mode other than **Default** is active, all theme buildings that fit the available
lot are considered at once in a single weighted roll: the game's original shrink loop is
replaced by this selection.

### How the selection formula works

For each candidate building that fits the lot:

```text
score = spawn_weight / rank ^ α
```

- **rank**: position in the size ordering (rank 1 = most preferred). Buildings with the
  same size key share the same rank.
- **α**: set by **Preference strength**: Gentle = 0.5, Moderate = 1.0, Strong = 2.0. **Absolute** bypasses the formula entirely (see below).
- A single weighted random roll over all scores selects the building.

**Preference strength** controls how dominant the size preference is relative to spawn weight:

| Strength | α | Effect |
| --- | --- | --- |
| Gentle | 0.5 | Size gives a mild boost: spawn weight still matters a lot |
| Moderate *(default)* | 1.0 | Balanced: size and spawn weight both influence the result |
| Strong | 2.0 | Size strongly dominates: only top-ranked sizes appear regularly |
| Absolute | n/a | Only the highest-ranked size is ever chosen; spawn weight breaks ties within that size group |

**Example (Biggest first, Moderate strength, three candidates):**

| Building | Footprint | Spawn weight | Rank | Score |
| --- | --- | --- | --- | --- |
| Large house | 4×4 | 10 | 1 | 10 / 1¹ = **10** |
| Medium house | 2×4 | 30 | 2 | 30 / 2¹ = **15** |
| Small house | 2×2 | 10 | 3 | 10 / 3¹ ≈ **3.3** |

Total = 28.3. The medium house wins most often despite having a lower size rank, because its
higher spawn weight compensates.

### Notes

- **"Random (weight only)"** assigns every candidate rank 1, so α has no effect and selection
  is identical to pure spawn weight. This is useful when you want weighted variety without any
  size bias.
- **Lots with no matching theme building** stay empty regardless of the preference mode: the
  size preference only affects which building is chosen among those that fit, not whether a
  building spawns at all.
- Preference settings are saved per district in the save game and survive mod updates.

---

## Asset Cloning

Clone a building and assign a different wealth level: useful when you don't have enough
assets for every level, or to add variety.

To clone: select a building in the Theme Manager → **Clone Building** → enter a name and
choose the wealth level. The clone appears in the building list.

> **Important:** Clones are generated at level-load time. Return to the main menu and
> reload after creating clones.
>
> **Important:** If you delete a theme that contains clones, or disable the mod, those
> buildings disappear on next load. The save file itself is not broken.

Cloning can be disabled in mod options.

---

## District Styles Plus

[District Styles Plus](https://steamcommunity.com/sharedfiles/filedetails/?id=1204126181) is
a separate mod that lets you create and assign district styles in-game. Building Themes 2 can
import those styles as fully editable themes.

**How to import:**

1. Load a city that has District Styles Plus styles defined.
2. Open **Mod Options → Building Themes → District Styles Plus**.
3. Click **Import from District Styles Plus**.

Each imported style becomes a theme named **`[DSP] <style name>`**. Only growable
(zone-compatible) buildings are included: service buildings, parks, and unique buildings are
skipped automatically.

Running the import a second time is safe: styles already imported under the same `[DSP] …`
name are skipped; only new styles are added.

After importing, use **Rename Theme** in the Theme Manager to remove the `[DSP]` prefix or
give the theme a name that fits your workflow.

> **Compatibility note:** Do not enable BT2 theme management and assign a DSP style to the
> **same district**: the two mods filter the same spawning call and will interfere with each
> other, causing empty lots or unexpected vanilla spawns. Use one system per district. The
> recommended migration path is to import your DSP styles into BT2 (see above), then
> disable DSP and manage everything through BT2.
>
> **Note:** District Styles Plus does not need to be active for the import to work. If you
> previously used DSP and still have its `.crp` style packages on disk, the game loads them
> into memory when a city is open, and the import button will find them regardless of whether
> the DSP mod itself is enabled.

---

## Compatibility

**Compatible with Cities: Skylines 1.21.1-f9** (and later 1.x releases).

**Not compatible with:**

- **81 Tiles (original)**: use [81 Tiles 2](https://steamcommunity.com/sharedfiles/filedetails/?id=2862121823) instead (fully supported)
- **Building Themes (original)**: unsubscribe the original before enabling this fork
- **Building Simulation Overhaul**
- **Runways and Taxiways**: use [Airport Roads](http://steamcommunity.com/sharedfiles/filedetails/?id=465127441) instead

**Partially compatible (per-district rule):**

- **District Styles Plus**: both mods patch `BuildingManager.GetRandomBuildingInfo`. BT2
  runs a prefix that selects a themed building; DSP runs a postfix that rejects any building
  not in the district's assigned style. When both are active on the **same district**: BT2
  theme management enabled *and* a DSP style assigned: DSP will silently null out BT2's
  chosen buildings, causing empty lots or vanilla spawns. They are safe when used on
  **different districts**: disable BT2 theme management for districts managed by DSP, and
  do not assign a DSP style to districts managed by BT2. The [import feature](#district-styles-plus)
  is specifically designed to help you migrate styles from DSP into BT2 so you can manage
  everything through BT2 and drop DSP afterwards.

Save games made with the original Building Themes are **fully compatible**: district
assignments load normally.

For broader mod-compatibility diagnostics (conflicts, disabled assets, missing
dependencies) use [Skyve](https://steamcommunity.com/sharedfiles/filedetails/?id=2979559117).
If Skyve is installed, Spawn Diagnostics will note which missing assets may be disabled in
your playset and give you IDs to re-enable them.

---

## Troubleshooting: Buildings Not Spawning

Work through this list in order:

**1. Theme is empty or has no Level 1 buildings**
Open Theme Manager → select your theme → set the Status filter to **Included**. If the
list is empty, no buildings are in your theme. Every theme must include at least one
Level 1 building for the target zone type.

**2. Workshop assets are not loaded**
Yellow-text buildings in the Theme Manager indicate assets that are not active in your
playset. Use **Workshop Dependencies** to copy their IDs for resubscription or click
**Subscribe Missing** to subscribe from within the game.

**3. Wrong zone type**
The building's zone type must match the zone you placed. A commercial building will never
spawn in a residential zone.

**4. Theme not enabled for the district**
Open the district policy panel → Themes tab → confirm **Enable Theme Management** is
checked and your theme has a checkmark.

**5. Empty lots appearing after auto-bulldoze**
Auto-bulldoze removed buildings from lots that your theme does not cover (no theme building
exists for that exact footprint size). Vanilla buildings do not fill uncovered footprints:
those lots stay empty until the game re-evaluates zone demand or you add theme buildings for
that size. Open **Spawn Diagnostics** to see which footprint pools are active, then add
buildings for the missing sizes in the Theme Manager.

**6. DLC or vanilla buildings being demolished unexpectedly**
Auto-bulldoze checks whether a building's exact prefab is in the active theme pool:
origin (base-game, DLC, Workshop) does not matter. If you previously included a DLC
building in your theme and later removed it, auto-bulldoze will demolish it. Re-add it
to the theme to keep it.

**7. Run Spawn Diagnostics**
District policy panel → Themes tab → **District Options** → **Spawn Diagnostics**. Shows
accepted/rejected counts and lists missing assets by name.

---

## Theme Mods

These Workshop mods provide ready-made themes:

| Theme | Links |
| --- | --- |
| **UK Terraced Housing** | [Buildings](https://steamcommunity.com/sharedfiles/filedetails/?id=452704398) · [Theme mod](https://steamcommunity.com/sharedfiles/filedetails/?id=470539837) |
| **Neo-eclectic Homes** | [Buildings](https://steamcommunity.com/sharedfiles/filedetails/?id=464133310) · [Theme mod](https://steamcommunity.com/sharedfiles/filedetails/?id=471015559) |
| **American Trailer Homes** | [Buildings](https://steamcommunity.com/sharedfiles/filedetails/?id=437051479) · [Theme mod](https://steamcommunity.com/sharedfiles/filedetails/?id=471183698) |

---

## How Data Is Saved

- **Custom themes and options** are stored in `BuildingThemes.xml` in your Cities: Skylines
  install folder.
- **District theme assignments** are stored inside each save game
  (key: `BuildingThemes-SaveData`).

You do not need to edit these files: everything is configurable in-game.

### For developers: district theme references and tags

A district stores its enabled themes as a list of **names** (`District.themes`). Because a
local theme may legitimately share a name with a built-in one (e.g. `European`), names
alone are ambiguous. Since **save version 7** each district also stores an index-aligned
`District.themeTags` array, a compact discriminator per theme:

| Tag | Theme kind | Resolved by |
| --- | --- | --- |
| `L` | Local / user-created | `!isBuiltIn && name` |
| `C` | Built-in `[Custom]` (other mod, no style package) | `isBuiltIn && stylePackage == null && name` |
| `S:<pkg>` | Built-in `[Vanilla]`/`[DLC]` style theme | `stylePackage == <pkg>` (locale-name independent) |

On load, `GetThemeByNameAndTag(name, tag)` resolves the exact theme, and **falls back to the
name-only `GetThemeByName`** when the tag is empty or its target no longer exists (e.g. a DLC
later disabled). `GetThemeByName` itself, on a name collision, prefers the built-in default
(the one backed by a `stylePackage`).

**Backward compatibility:** `themeTags` is purely additive. Saves written before version 7
have no `themeTags` element, so it deserialises to `null`; the loader detects that and uses
the original name-only path, loading exactly as before. Older mod builds reading a version-7
save simply ignore the unknown element. The tag helpers live in
`BuildingThemesManager` (`GetThemeTag` / `GetThemeByNameAndTag`).

---

## Bug Reports

Please open an issue on GitHub with:

- Your game version
- A list of other active mods
- The relevant section of `output_log.txt` (search for `Building Themes`)
- Output from **Spawn Diagnostics** if buildings are not spawning

[► Open a bug report on GitHub](https://github.com/roberto-naharro/BuildingThemes2/issues)

---

## Translations

The mod UI is fully localizable. Language files live in
[`BuildingThemes/Locale/`](BuildingThemes/Locale/) — one plain-text file per language,
named after the game's locale code:

| File | Language |
| ---- | -------- |
| `en.txt` | English (source of truth) |
| `de.txt` | German |
| `es.txt` | Spanish |
| `fr.txt` | French |
| `it.txt` | Italian |
| `ja.txt` | Japanese |
| `ko.txt` / `kr.txt` | Korean (both codes used by Korean language mods) |
| `pl.txt` | Polish |
| `pt.txt` | Portuguese |
| `ru.txt` | Russian |
| `zh.txt` / `zh-cn.txt` | Chinese Simplified (`zh` is the game's built-in code) |
| `zh-tw.txt` | Chinese Traditional |

The mod picks the file matching the game's current language and falls back to English
for any missing key or unsupported language.

> ⚠️ **All translations except English (and the reviewed Spanish) are machine-generated
> and have not been reviewed by native speakers.** They may contain awkward phrasing,
> wrong game terminology, or text that does not fit the UI. Corrections from native
> speakers are very welcome — see below.

### How to update or fix a language

1. Open the file for your language in `BuildingThemes/Locale/` (any text editor, UTF-8).
2. Each line is `KEY translated text` — the **first space** separates the key from the
   text. Translate only the text after the key; **never change the key itself**.
3. Formatting rules:
   - `\n` inside the text is a line break — keep them in roughly the same places.
   - `{0}`, `{1}`, `{2}` are placeholders filled in by the mod (counts, names, IDs).
     Keep every placeholder; you may reorder them if your language needs it.
   - Keep button texts short — they share limited space in the UI.
   - Use `en.txt` as the reference for what each key means.
4. To test in-game without rebuilding the mod: copy your edited file into the mod's
   `Locale/` folder (local mod: `<game data>/Addons/Mods/BuildingThemes2/Locale/`;
   workshop copy: `workshop/content/255710/3708105182/Locale/`) and restart the game.

### How to contribute a translation

- **Preferred:** open a pull request on
  [GitHub](https://github.com/roberto-naharro/BuildingThemes2) editing the file in
  `BuildingThemes/Locale/`.
- Or open a [GitHub issue](https://github.com/roberto-naharro/BuildingThemes2/issues)
  with the corrected file attached, or post in the Workshop comments.

To add a **new language**, copy `en.txt` to `<locale-code>.txt` (the code your
game/language mod reports, e.g. `nl.txt`), translate it, and submit it the same way.

---

## Credits

**boformer (Sebastian Schöner)**: original mod concept, architecture, and all game logic.
Building Themes 2 would not exist without his work.

**BloodyPenguin**: prior compatibility fixes and contributions to the original mod.

**roberto-naharro**: Harmony 2.x migration, new features, and community maintenance of
this fork.

---

## Development

### Linux Build Setup (Windows game files on SMB share)

This project is developed on Linux, referencing the game assemblies from a Windows machine
where Cities: Skylines is installed via an SMB share.

Example setup (replace with your own values):

- Windows host: `YOUR-WINDOWS-PC`
- SMB share: `//192.168.x.x/Cities_Skylines`
- Linux mount point: `/mnt/cities_skylines`

The project file references managed DLLs from `/mnt/cities_skylines/Cities_Data/Managed`.

### Prerequisites

- Mono / xbuild
- `cifs-utils` (for SMB mounting)

### One-time setup

```bash
sudo apt-get install -y mono-complete mono-devel cifs-utils
sudo mkdir -p /mnt/cities_skylines

# Mount the Windows share
sudo mount -t cifs //192.168.x.x/Cities_Skylines /mnt/cities_skylines \
  -o username=YOUR_WINDOWS_USER,uid=$(id -u),gid=$(id -g),vers=3.0

# Verify
ls /mnt/cities_skylines/Cities_Data/Managed/Assembly-CSharp.dll
```

Copy `.env.example` to `.env` and fill in your values.

### Restore packages

```bash
mono ~/.local/tools/nuget.exe restore BuildingThemes.sln
```

### Build

```bash
xbuild BuildingThemes.sln                      # Debug (default)
xbuild BuildingThemes.sln /p:Configuration=Release
```

Output: `BuildingThemes/bin/Debug/BuildingThemes.dll`

### Deploy to game

```bash
./mount-cities.sh        # mount Windows SMB shares (first time / after reboot)
./deploy.sh              # Debug build → stage to dist/ → copy to game Mods folder
./deploy.sh --release    # Release build
```

`deploy.sh` also tails the last 60 lines of `output_log.txt` after deploying.

### Releasing a new version

Releases are managed by [Release Please](https://github.com/googleapis/release-please).
Write commits using [Conventional Commits](https://www.conventionalcommits.org/):

| Prefix | Semver bump | Example |
| --- | --- | --- |
| `fix:` | patch (2.0.0 → 2.0.1) | `fix: DLC filter overlap with Display row` |
| `feat:` | minor (2.0.0 → 2.1.0) | `feat: add Subscribe Missing button` |
| `feat!:` or `BREAKING CHANGE:` | major | `feat!: change save format` |
| `chore:`, `docs:`, `refactor:` | no bump | housekeeping |

**Release flow:**

1. Commit your changes using the prefixes above: Release Please opens a Release PR
   automatically and keeps it up to date.
2. Run `./deploy.sh --release` locally and commit `dist/` to `master`.
3. Merge the Release PR → tag `vX.Y.Z` is created → Workshop deploy fires automatically.

### Publish to Steam Workshop

```bash
./deploy.sh --release    # build & stage
./publish.sh             # upload dist/ to Steam Workshop via SteamCMD
./publish.sh "Fix: DLC filter overlap"   # with a change note
```

`publish.sh` requires `steamcmd` to be installed and reads `STEAM_USERNAME` from `.env`.
SteamCMD will prompt for your password and Steam Guard code interactively: no secrets
are stored.

For automated publishing via GitHub Actions, see
[`.github/workflows/workshop-deploy.yml`](.github/workflows/workshop-deploy.yml).

### Notes on cross-platform compatibility

- C# language version set to 7.2 (Mono compiler compatible).
- Windows-only `PostBuildEvent` is guarded with an OS condition in the `.csproj`.
- Game DLL references point to `/mnt/cities_skylines/Cities_Data/Managed`: do not commit
  these files (copyright).
