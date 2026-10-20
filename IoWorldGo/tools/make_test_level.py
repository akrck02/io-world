"""Makes Io World's test level: one short room that tries every mechanic, left to right.

python3 tools/make_test_level.py   (after convert_level.py, whose pictures and tileset it uses)

1. a low block, for the plain jump; 2. a jump pad up to a ledge too high to jump to; 3. on it the
magnet, to push into the field, which lifts it (Io walks through, unlifted); 4. the cube, to push
onto 5. the button, which starts 6. the rail, up to the exit ledge.
The room is in metres, a tile each, its floor's top at 0.
"""
import json

COLUMNS, ROWS = 40, 13  # the room's tiles; its top row is at 11 to 12, its floor's at -1 to 0
WALL, LEDGE = 3, 2      # factory.tileset.json's tiles


def cell_row(top):
    """The tile map's row whose top is at a height."""
    return ROWS - 1 - top


def tiles():
    grid = [-1] * (COLUMNS * ROWS)

    def put(column, row, tile):
        grid[row * COLUMNS + column] = tile

    for column in range(COLUMNS):
        put(column, cell_row(0), WALL)   # the floor
        put(column, cell_row(12), WALL)  # the ceiling
    for top in range(1, 12):
        put(0, cell_row(top), WALL)
        put(COLUMNS - 1, cell_row(top), WALL)
    put(5, cell_row(1), WALL)                  # 1. the low block
    for column in range(10, 16):
        put(column, cell_row(5), LEDGE)        # 2. the ledge the pad reaches
    for column in range(33, COLUMNS - 1):
        put(column, cell_row(7), LEDGE)        # 6. the exit ledge the rail reaches
    return grid


def node(name, x, y, depth, **parts):
    result = {"name": name, "position": [x, y, depth], "rotation": [0, 0, 0], "scale": [1, 1, 1]}
    result.update(parts)
    return result


def player():
    parts = [("Body", "player-body.png"), ("Tail", "player-tail-1.png"), ("Tail wag", "player-tail-2.png"), ("Eyes", "player-eyes.png")]
    children = [node(name, 0.005, 0.02, 0.01 * index, sprite="assets/sprites/" + file) for index, (name, file) in enumerate(parts)]
    return node("Player", 2.5, 0.48, 0.6,
                body={"kind": "moving", "size": [0.73, 0.96], "gravityScale": 9.2, "ignoresZones": True},
                behaviours=[{"name": "Player", "fields": {"speed": 7, "acceleration": 180, "groundKeeps": 0.5,
                                                          "airKeeps": 0.9, "jump": 20}}], children=children)


def scene():
    nodes = [
        node("Room", 0, ROWS - 1, 0.4, tiles={"tileset": "assets/sprites/factory.tileset.json", "columns": COLUMNS, "rows": ROWS, "tiles": tiles()}),
        node("Jump pad", 8.5, 0.3, 0.5, sprite="assets/sprites/jumper.png", body={"kind": "solid", "size": [3, 0.6]},
             children=[node("Pad", 0, 0.35, 0, body={"kind": "trigger", "size": [3, 0.3]}, behaviours=[{"name": "Jumper", "fields": {"speed": 30}}])]),
        node("Magnet", 12.5, 5.33, 0.55, sprite="assets/sprites/magnet.png", body={"kind": "pushable", "size": [0.66, 0.66], "gravityScale": 0.8}),
        node("Field", 17.5, 5.5, 0.3, body={"kind": "trigger", "size": [3, 11], "pull": [0, 22.2]},
             children=[node("Glow", 0, 0, -0.01, sprite="assets/sprites/field.png")]),
        node("Cube", 21.5, 0.42, 0.55, sprite="assets/sprites/cube.png", body={"kind": "pushable", "size": [0.87, 0.84], "gravityScale": 1}),
        node("Button", 26, 0.21, 0.5, sprite="assets/sprites/button.sprite.json", body={"kind": "trigger", "size": [1.56, 0.42]},
             behaviours=[{"name": "Button", "fields": {"target": "Rail"}}]),
        node("Rail", 31, 0.5, 0.5, sprite="assets/sprites/rail.png", body={"kind": "platform", "size": [4, 1]},
             behaviours=[{"name": "Rail", "fields": {"speed": 2, "distance": 6, "wait": 2, "startsStill": True}}]),
        player(),
        node("Camera", 2.5, 3, 1, camera={"projection": "orthographic", "height": 6, "main": True,
                                          "follow": "Player", "smoothing": 0.15, "ceiling": True}),
    ]
    nodes[3]["children"][0]["scale"] = [1.5, 2.2, 1]  # the glow, 2 by 5 metres, over the 3 by 11 field
    return {"name": "Test level · everything", "mode": "2d", "sky": "#4d4d4d", "pixelsPerMetre": 100, "nodes": nodes}


with open("scenes/test-level.scene.json", "w") as output:
    json.dump(scene(), output, indent="\t")
    output.write("\n")
