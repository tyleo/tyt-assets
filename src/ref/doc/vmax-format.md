# VoxelMax External Parser Spec

This spec is for external tools that need to read VoxelMax project files.

It focuses on wire format: container shape, compression, serialization type, and key maps.

## 1. Container types

- `.vmax`
  - Apple package (directory-style document), UTI `com.ndreca.voxel.vmax`.
- `.vmax.zip`
  - Zipped form of the same package, UTI `com.ndreca.voxel.vmax.zip`.

## 2. Required/optional files in a `.vmax` package

Required:

- `scene.json`

Optional (depending on project content/edit history):

- `scene.vmaxhb` (scene-level history)
- `contents*.vmaxb` (voxel objects)
- `contents*.scndata` (external mesh objects)
- `history*.vmaxhb` (per-object history)
- `history*.vmaxhvsb` (per-object snapshot sidecar)
- `history*.vmaxhvsc` (per-object compressed-batches sidecar)
- `palette*.png` (palette image)
- `palette*.settings.vmaxpsb` (palette settings)
- `<texture-uuid>.vxtex` (external material textures)
- `QuickLook/*` (thumbnails only)

## 3. Payload envelopes by extension

This is the most important section for parser implementation.

| Extension | Outer encoding | Compression | Payload type |
|---|---|---|---|
| `.vmaxb` | binary plist | LZFSE (usually) | `VXObjectData` |
| `.vmaxhb` | binary plist | LZFSE (usually) | `VXHistory` |
| `.vmaxhvsb` | binary plist | LZFSE (usually) | `[VXVolumeSnapshot]` |
| `.vmaxhvsc` | binary plist | none (outer) | `[VXCompressedBatch]` |
| `.vmaxpsb` | binary plist | none | `VXMaterialPalette` |
| `.scndata` | NSKeyedArchive | LZFSE (usually) | `VXGeometry` |
| `.vxtex` | NSKeyedArchive | none | `VXTextureData` |

Important compatibility rule:

- VoxelMax decoders often do: `try lzfse-decompress` then fallback to raw if decompress fails.
- External parsers should do the same for `.vmaxb`/`.vmaxhb`/`.vmaxhvsb`/`.scndata`.

## 4. Parser pipeline (recommended)

1. Open package root.
2. Parse `scene.json`.
3. For each object in `scene.json.objects`:
   - Resolve `data` file (`*.vmaxb` or `*.scndata`).
   - Resolve `hist` file (`*.vmaxhb`) if present.
   - Resolve palette image `pal` and palette settings sidecar (`.settings.vmaxpsb`) if present.
   - Resolve sidecars derived from history name:
     - `.vmaxhvsb`
     - `.vmaxhvsc`
4. If `scene.json.trsc` exists, use it to map `contents*.scndata` -> texture UUIDs, then load `<uuid>.vxtex`.

## 5. `scene.json` schema

`scene.json` is plain JSON and is the package index.

Top-level keys (current):

- `v`: codable version (`0..4` currently used; latest is `4`)
- `cam`: camera object
- `objects`: array of object wrappers
- `groups`: array of group wrappers (optional)
- `ao`, `ag`, `af`: active object/group/face
- `trsc`: texture refs by contents (optional)
- renderer settings keys: `sat`, `cont`, `tint`, `temp`, `graint`, `vigint`, `vigpow`, `bloomint`, `bloomthr`, `bloombrad`, `outlineint`, `outlinesz`, `shadowint`, `eint`, `lint`, `lcolor`, `aint`, `background`, `ssr`

Camera keys (`cam`):

- `wa`, `ha`, `da`: camera angles
- `lwa`, `lha`, `lda`: light angles
- `px`, `py`: camera pan
- `z`: zoom
- `o`: origin `[x, y, z]`

Object entry keys (`objects[]`):

- `id`: UUID
- `data`: contents filename (`contents*.vmaxb` or `contents*.scndata`)
- `hist`: history filename (`history*.vmaxhb`)
- `pal`: palette image filename (`palette*.png`)
- `pid`: parent UUID (optional)
- `ind`: index triplet
- `s`: selected (optional bool)
- `h`: hidden (optional bool)
- `n`: name (optional)
- transform:
  - `t_p`: position `[x, y, z]`
  - `t_r`: rotation `[x, y, z, angle]`
  - `t_s`: scale `[x, y, z]`
  - `t_al`: alignment enum token
  - `t_pf`: pivot-face enum token
  - `t_pa`: pivot-align enum token
  - `t_po`: pivot offset (optional)
  - `e_c`: center
  - `e_mi`: bounds min
  - `e_ma`: bounds max
- legacy:
  - `t_a`: older Euler-angle rotation field (fallback only)

Group entry keys (`groups[]`) are similar but without `data/hist/pal`; name key is `name`.

## 6. Binary payload key maps

### 6.1 `.vmaxb` (`VXObjectData`)

Top-level keys:

- `uuid`
- `v`
- `brush`
- `cam`
- `tools`
- `snapshots`
- legacy: `chunks`, `voxels`

`snapshots` entries are `VXVolumeSnapshot` (`s` -> storage).

### 6.2 `.vmaxhb` (`VXHistory`)

Top-level keys:

- `sessions`
- `asid` (active session id)

Session (`VXEditSession`) keys:

- `sid`
- `steps`
- `snapshots` (volume snapshot identifiers; older files may contain full snapshots)
- `ssnapshots` (scene snapshots)
- `osnapshots` (object snapshots)

Snapshot identifier (`VXVolumeSnapshotIdentifier`) keys:

- `c` = chunk id
- `s` = session id
- `t` = type

Snapshot type values:

- `0`: undoRestore
- `1`: redoRestore
- `2`: undo
- `3`: redo
- `4`: checkpoint
- `5`: selection

### 6.3 `.vmaxhvsb` (`[VXVolumeSnapshot]`)

Array of snapshots.

Each element:

- `s` (storage object)

Storage (`VXVolumeSnapshotStorage`) keys:

- `id` (snapshot identifier)
- `st` (stats)
- voxel/stat blobs:
  - `ds` (voxel bytes)
  - `lc` / `dlc` in current writer
  - legacy files may contain `is` instead of newer usage-mask fields
  - these fields may be empty

### 6.4 `.vmaxhvsc` (`[VXCompressedBatch]`)

Array of compressed batches.

Batch keys:

- `d`: LZFSE-compressed binary plist payload (nested snapshots payload)
- `bid`: `[batchId, minSessionId, maxSessionId]`
- `ids`: snapshot identifiers included in this batch

Note:

- `.vmaxhvsc` itself is not outer-compressed by the writer.
- `d` is inner-compressed with LZFSE.

### 6.5 `.vmaxpsb` (`VXMaterialPalette`)

Primary keys:

- `indices`
- `lc`
- `current`
- `name`
- `type`
- `transparency`
- `r` (range)
- `rt` (range type)
- `cmt` (color mix type)
- `materials`
- `ali` (active layer index)
- `voxmats` (optional)
- legacy/fallback: `colors`, `gradient`

### 6.6 `.vxtex` (`VXTextureData`, NSKeyedArchive)

Keyed fields:

- `d`: raw texel bytes
- `w`: width (`Int32`)
- `h`: height (`Int32`)
- `c`: channel count (`UInt32`)
- `e`: channel encoding (`UInt32`)
- `r`: row stride (`Int`)
- `n`: texture name (optional string)

### 6.7 `.scndata` (`VXGeometry`, NSKeyedArchive)

Archived `VXGeometry`:

- `g`: `SCNGeometry` archived object graph
- `m`: material texture map (`"<materialIndex>_<propertyType>" -> "<textureUUID>"`)

Use `scene.json.trsc` plus `.vxtex` files to resolve actual texture payloads.

Implementation note:

- This payload is Apple `NSKeyedArchiver` data (not plain plist struct-only data).
- Cross-platform parsers need keyed-archive support.

## 7. Naming conventions

Generated names are deterministic:

- `contents.vmaxb`, `contents1.vmaxb`, ...
- `contents.scndata`, `contents1.scndata`, ...
- `history.vmaxhb`, `history1.vmaxhb`, ...
- `palette.png`, `palette1.png`, ...
- `palette.settings.vmaxpsb`, `palette1.settings.vmaxpsb`, ...
- `history*.vmaxhvsb`
- `history*.vmaxhvsc`

## 8. Versioning and compatibility

Version field (`v`) maps to:

- `0` = `v01Prerelease`
- `1` = `v02Extended`
- `2` = `v03Extended`
- `3` = `v04Extended`
- `4` = `v05Extended` (current latest)

Compatibility guidance:

- Accept unknown keys.
- Treat missing optional keys as defaults.
- Keep legacy fallback for `objects` shape in very old scene versions (`v03Extended` special-case existed internally).
- Try LZFSE decode first, then fallback to raw for file types listed above.

## 9. CLI decode examples (macOS)

Inspect `scene.json`:

```bash
cat MyScene.vmax/scene.json | jq .
```

Decode `.vmaxb`:

```bash
lzfse -decode -i MyScene.vmax/contents.vmaxb -o /tmp/contents.plist
plutil -p /tmp/contents.plist
```

Decode `.vmaxhb`:

```bash
lzfse -decode -i MyScene.vmax/history.vmaxhb -o /tmp/history.plist
plutil -p /tmp/history.plist
```

Decode `.vmaxhvsb`:

```bash
lzfse -decode -i MyScene.vmax/history.vmaxhvsb -o /tmp/hvsb.plist
plutil -p /tmp/hvsb.plist
```

Decode `.vmaxhvsc` outer plist:

```bash
plutil -p MyScene.vmax/history.vmaxhvsc
```

Then decode a batch `d` blob from `.vmaxhvsc`:

```bash
# Extract d blob with your plist tool, then:
lzfse -decode -i batch_d.bin -o /tmp/batch.plist
plutil -p /tmp/batch.plist
```

