# vmax

## Index

### empty.vmax

An empty Voxel Max file.

**Objects**:

1. "Object 0": an empty object

**Opened Editor**: "Object 0"

### energy-reactor.vmax

A Voxel Max file with multiple objects and materials.

**Objects**:

1. "energy-reactor": an object with a roughness, metalness, and emission materials
2. "energy-tank-1": an object with a roughness, metalness, and emission materials
3. "energy-tank-2": a reference to "energy-tank-1"
4. "energy-tank-3": a reference to "energy-tank-1"

**Opened Editor**: scene

### first-last-color.vmax

A Voxel Max file with an object with colors at the beginning and end of the palette.

**Objects**:

1. "Object 0": an object with pure red in its first color slot, pure blue in its last color slot, and pure green in every other color slot

**Opened Editor**: "Object 0"

### mixed-shapes-with-material.vmax

A Voxel Max file with a variety of shapes in a hierarchy and one material in use.

**Objects**:

1. "Outer Group" > "Inner Group" > "cube_sphere_16_subdivisions" > "test_sphere": A sphere that looks like the moon and has 50/100 roughness on one color
2. "Outer Group" > "Inner Group" > "Sphere": An empty object
3. "Outer Group" > "HalfCube": An all-yellow cube half which does not fill its bounds

**Opened Editor**: scene

### mixed-shapes.vmax

A Voxel Max file with a variety of shapes in a hierarchy.

**Objects**:

1. "Outer Group" > "Inner Group" > "cube_sphere_16_subdivisions" > "test_sphere": A sphere that looks like the moon
2. "Outer Group" > "Inner Group" > "Sphere": An empty object
3. "Outer Group" > "HalfCube": An all-yellow cube half which does not fill its bounds

**Opened Editor**: scene

### nothing.vmax

An empty Voxel Max file containing nothing.

**Opened Editor**: scene

### pivots.vmax

A Voxel Max file with two objects with different pivots but both at position 0.

**Objects**:

1. "Custom Pivot": An object with its pivot set to the bottom-left
2. "Default Pivot": An object with its pivot set to the default (center)

**Opened Editor**: scene

### rotation-30-y-40-z-origin.vmax

A Voxel Max file with one object rotated on two axes and moved to the origin.

**Objects**:

1. "Object 0": An object with position `[0, 0, 0]` and rotation `[0, 30, 40]`

**Opened Editor**: "Object 0"

### rotation-30-y-40-z.vmax

A Voxel Max file with one object rotated on two axes.

**Objects**:

1. "Object 0": An object with position `[0, 0, 16]` and rotation `[0, 30, 40]`

**Opened Editor**: "Object 0"

### rotation-40-z.vmax

A Voxel Max file with one object rotated on one axis.

**Objects**:

1. "Object 0": An object with position `[0, 0, 16]` and rotation `[0, 0, 40]`

**Opened Editor**: "Object 0"
