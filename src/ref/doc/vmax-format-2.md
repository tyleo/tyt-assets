# The `.vmax` Format

Working notes on the **Voxel Max** document format, scoped to what `tyt vmax print` needs:
**voxels, palette, materials, camera, and basic scene metadata**. Edit history, thumbnails,
and imported polygon meshes are out of scope and are described only enough to know what to skip.

Two sources informed this, and they're complementary:

- **`temp/vmax-format.md`** — a container/wire-format spec with the _real_ VoxelMax plist key
  names (`VXObjectData`, `VXVolumeSnapshotStorage`, `ds`, …). Authoritative for _naming_.
- **[`oomer/vmax2bella`](https://github.com/oomer/vmax2bella)** (`oomer_voxel_vmax.h`) — an
  open-source converter that actually _decodes_ the voxel byte stream. Authoritative for the
  _algorithm_. The decode loop, Morton math, and structs below are quoted from that header.

---

## 1. Package layout

A `.vmax` is **not a single file** — it's an Apple-style package (a directory; UTI
`com.ndreca.voxel.vmax`). A zipped variant exists (`.vmax.zip`, `…vmax.zip`). Files are named
deterministically: the first instance has no suffix, the rest count from `1`
(`contents.vmaxb`, `contents1.vmaxb`, …).

| File                                            | Contains                                                    | We care?        |
| ----------------------------------------------- | ----------------------------------------------------------- | --------------- |
| `scene.json`                                    | Scene metadata, camera, object/group tree, transforms       | **Yes**         |
| `contents*.vmaxb`                               | Per-object **voxel data** (bplist, usually LZFSE)           | **Yes**         |
| `palette*.png`                                  | 256×1 RGBA color palette                                    | **Yes**         |
| `palette*.settings.vmaxpsb`                     | Material settings (bplist)                                  | **Yes**         |
| `contents*.scndata`                             | Imported polygon mesh object (NSKeyedArchive `SCNGeometry`) | No (not voxels) |
| `*.vxtex`                                       | External material textures                                  | No              |
| `history*.vmaxhb` / `*.vmaxhvsb` / `*.vmaxhvsc` | Undo/redo timeline                                          | **No — skip**   |
| `scene.vmaxhb`                                  | Scene-level history                                         | No              |
| `QuickLook/*`                                   | Thumbnails                                                  | No              |

> The live voxel geometry lives in `contents*.vmaxb`, **not** in the history files. `tyt vmax pack`
> already strips the history sidecars, so a packed `.vmax` keeps only what `print` needs.

---

## 2. `scene.json`

Plain JSON. Keys are terse two/three-letter codes. The ones that matter:

**Top level**

- `v` — codable version, currently `0..4` (latest `4` = `v05Extended`).
- `cam` — camera (below).
- `objects` — array of voxel/mesh objects.
- `groups` — array of group nodes (folders; no geometry).
- `ao` / `ag` / `af` — active object / group / face (UI state).
- Renderer/post keys (`sat`, `cont`, `tint`, `bloom*`, `outline*`, `shadowint`, `background`, …)
  exist but are grading settings — ignore unless asked.

**Camera (`cam`)** — all simple scalars / arrays:

- `wa`, `ha`, `da` — camera orbit angles (width/height/distance angle).
- `lwa`, `lha`, `lda` — key-light angles.
- `px`, `py` — pan; `z` — zoom; `o` — orbit origin `[x, y, z]`.

**Object (`objects[]`)**

- `id` — UUID. `pid` — parent UUID (optional). `n` — name (optional). `ind` — index triplet.
- `data` — the `contents*.vmaxb` (or `contents*.scndata`) filename for this object.
- `pal` — the `palette*.png` filename. `hist` — history filename (ignore).
- `h` — hidden (bool). `s` — selected (bool).
- Transform: `t_p` position `[x,y,z]`, `t_r` rotation `[x,y,z,angle]` (axis-angle),
  `t_s` scale `[x,y,z]`. Legacy files may use `t_a` (Euler) instead of `t_r`.
- Pivot/alignment tokens: `t_al`, `t_pf`, `t_pa`, `t_po` (usually not needed for print).
- Bounds: `e_c` center, `e_mi` min, `e_ma` max.

**Group (`groups[]`)** — like an object but without `data`/`pal`/`hist`; its name key is `name`
(not `n`).

---

## 3. `contents*.vmaxb` — the voxel data

This is the important part. **Envelope:** Apple **binary plist**, usually **LZFSE-compressed**.
Read it as _try-LZFSE-decompress, fall back to raw bytes_ (VoxelMax itself does this).

### 3.1 Plist structure

Decoded plist is a `VXObjectData` dict. Relevant keys:

- `uuid`, `v` — id and version. `cam`, `brush`, `tools` — editor state (ignore).
- `snapshots` — **array of per-chunk volume snapshots** = the voxel geometry.
  (Legacy files may use `chunks` / `voxels` instead.)

Each `snapshots[]` entry is a `VXVolumeSnapshot` with key `s` → `VXVolumeSnapshotStorage`:

- `id` — snapshot identifier: `c` = **chunk id (0–511)**, `s` = session, `t` = type.
- `ds` — **the voxel byte stream** (decoded in §3.3).
- `st` — stats (min/max Morton bounds for the chunk).
- `lc` / `dlc` — layer-color usage masks (may be empty).

A model is **256×256×256** voxels, partitioned into an **8×8×8 = 512 chunk grid**. Each snapshot
carries one chunk's `ds`. To get the final model, **replay snapshots in array order and apply each
chunk's stream; later snapshots win** for the same chunk/position. (The array is effectively a
baked edit log; there is no separate "final state" blob.)

### 3.2 Coordinates: chunk grid + in-chunk Morton

- A `chunkID` (0–511) **Morton-decodes** into chunk-grid coords `(cx, cy, cz)`, each `0–7`.
- Within a chunk, voxels are addressed by a **local Morton code** in a **32×32×32** space — that
  code _is_ the slot index into `ds` (see §3.3). It decodes to local `(x, y, z)`.
- World position = chunk origin + local position. **Open question:** the reference converter
  computes the chunk origin as `chunkGridCoord * 24` and the author notes _"Don't know why we need
  to multiply by 24 // use to be 32."_ A 32³ chunk on a 32-pitch grid tiles 256³ cleanly; the `× 24`
  is unexplained. Treat the chunk pitch as **not fully nailed down** — when printing, it's safest to
  emit `chunkID` + chunk-grid coords + in-chunk `(x,y,z)` and let the consumer choose the pitch,
  rather than baking in `24`.

### 3.3 Decoding `ds`

`ds` is a flat byte array, **2 bytes per slot**, indexed by in-chunk Morton code. Byte 0 is the
material index, byte 1 is the color index. **Color `0` means empty** (no voxel):

```text
for i in 0, 2, 4, … < len(ds):
    material = ds[i]          # 0–7, selects a material slot (§4)
    color    = ds[i + 1]      # 0–255, indexes the palette PNG (§4); 0 = empty
    if color == 0: continue
    morton   = i/2 + mortonOffset      # slot index → local Morton code
    (x, y, z) = decodeMorton3D(morton) # local position in 0..31
    emit voxel { x, y, z, color, material }
```

3-bit-deinterleave Morton decode (verbatim constants from the header):

```c
uint32_t compactBits(uint32_t n) {        // gather every 3rd bit
    n &= 0x49249249;
    n = (n ^ (n >> 2))  & 0xc30c30c3;
    n = (n ^ (n >> 4))  & 0x0f00f00f;
    n = (n ^ (n >> 8))  & 0x00ff00ff;
    n = (n ^ (n >> 16)) & 0x0000ffff;
    return n;
}
void decodeMorton3D(uint32_t m, uint32_t& x, uint32_t& y, uint32_t& z) {
    x = compactBits(m);  y = compactBits(m >> 1);  z = compactBits(m >> 2);
}
```

The converter's voxel record, for reference:

```c
struct VmaxVoxel { uint8_t x, y, z;       // local position
                   uint8_t material;       // 0–7
                   uint8_t palette;        // color index 0–255
                   uint16_t chunkID;       // 0–511
                   uint16_t minMorton; };  // Morton base offset within the chunk
```

---

## 4. Palette & materials

**Color** comes from `palette*.png`: a **256×1 RGBA** image = 256 entries × 4 bytes (`r,g,b,a`).
A voxel's `color` byte (1–255) indexes it; **index 0 is reserved as empty**.

**Material** is separate and indirect. The voxel's `material` byte (0–7) selects one of **8
material slots** loaded from `palette*.settings.vmaxpsb` (a bplist, `VXMaterialPalette`; key
`materials`, plus `indices`, `name`, `type`, `transparency`, …). Per-slot properties
(from the converter's `VmaxMaterial`):

```c
struct VmaxMaterial { std::string materialName;
                      double transmission, roughness, metalness, emission;
                      bool   enableShadows, dielectric, volumetric; };
```

Converter-side semantics worth noting: `roughness == 0` → diffuse, `> 0` → plastic;
`emission > 0` → emitter; low alpha / transmission → glass-like. Voxel Max special-cases the
last slots as Glass/Liquid.

---

## 5. What we skip and why

- **History** (`*.vmaxhb`, `*.vmaxhvsb`, `*.vmaxhvsc`, `scene.vmaxhb`) — undo/redo timeline,
  sessions, checkpoints. The current geometry is already in `contents*.vmaxb`.
- **Thumbnails** (`QuickLook/*`) — preview PNGs.
- **Mesh objects** (`contents*.scndata`) — imported polygon meshes stored as Apple
  `NSKeyedArchiver` `SCNGeometry`; not voxels and needs keyed-archive support. Worth _detecting_
  (so we can report "object N is a mesh, not voxels") but not decoding.
- **Textures** (`*.vxtex`) — external material texture maps.

---

## 6. Open questions

1. **Chunk pitch (24 vs 32).** See §3.2 — the world-offset multiplier is unexplained in the only
   decoder we have. Validate against a real file (a known single voxel at a known chunk boundary)
   before trusting absolute world coordinates.
2. **Snapshot semantics.** We assume `contents.vmaxb`'s `snapshots` accumulate last-write-wins per
   chunk. If a file ever shows duplicate chunk ids that _shouldn't_ overwrite, revisit this.
3. **Morton offset base.** `mortonOffset` / `minMorton` (the per-chunk base added to the slot index)
   should be confirmed against `st` min/max bounds on real data.

These are the things to pin down by parsing an actual `.vmax` once the reader exists.
