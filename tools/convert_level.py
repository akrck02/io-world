"""Makes Io World's assets and first level from io-world's Godot 3 project.

One-off tool: python3 tools/convert_level.py ../io-world/IoWorldDemo
It copies the pictures the level uses into assets/, composes the jump pad's and the rail's from
their tiles, writes a tileset file for each tile picture, and writes scenes/level-1.scene.json.

Godot measures in pixels with Y down; the scene is in metres, a tile (100 pixels) each, with Y up.
"""
import json
import os
import re
import shutil
import sys

from PIL import Image

PIXELS_PER_METRE = 100
TILE = 100
SKY = "#4d4d4d"  # Godot's own clear colour, which io-world kept

godot = sys.argv[1]
level_text = open(os.path.join(godot, "Assets/Scenes/Level1.tscn")).read()
sprites = os.path.join(godot, "Assets/Sprites")
os.makedirs("assets/sprites", exist_ok=True)
os.makedirs("scenes", exist_ok=True)


def vector(value):
    numbers = re.findall(r"-?[\d.]+", value[value.index("("):])
    return float(numbers[0]), float(numbers[1])


def metres(pixel_x, pixel_y):
    return [round(pixel_x / PIXELS_PER_METRE, 4), round(-pixel_y / PIXELS_PER_METRE, 4)]


def sections(text):
    """Each [header] with its key = value lines."""
    for match in re.finditer(r"^\[(.+?)\]\n(.*?)(?=^\[|\Z)", text, re.S | re.M):
        fields = {}
        for line in re.finditer(r"^([\w/]+) = (.+)$", match.group(2), re.M):
            fields[line.group(1)] = line.group(2)
        yield match.group(1), fields


def textures(text):
    """ext_resource id → the picture's path under Sprites/."""
    found = {}
    for match in re.finditer(r'\[ext_resource path="res://Assets/Sprites/([^"]+)" type="Texture" id=(\d+)\]', text):
        found[match.group(2)] = match.group(1)
    return found


def tiles_of(text):
    """Tile id → (picture, region x, region y, solid): the level's tile set."""
    pictures = textures(text)
    found = {}
    for header, fields in sections(text):
        if 'type="TileSet"' not in header:
            continue
        for key, value in fields.items():
            if not key.endswith("/region"):
                continue
            tile = int(key.split("/")[0])
            picture = pictures[re.search(r"\d+", fields[f"{tile}/texture"]).group(0)]
            x, y = vector(value)
            found[tile] = (picture, int(x), int(y), f"{tile}/shape" in fields)
    return found


def cells(tile_data):
    numbers = [int(number) for number in re.findall(r"-?\d+", tile_data)]
    for index in range(0, len(numbers), 3):
        packed, tile = numbers[index], numbers[index + 1]
        x, y = packed & 0xFFFF, packed >> 16
        if x >= 0x8000:
            x -= 0x10000
            y += 1
        yield x, y, tile


def node(name, position, depth, **parts):
    result = {"name": name, "position": position + [depth], "rotation": [0, 0, 0], "scale": [1, 1, 1]}
    result.update(parts)
    return result


def copy(picture, name):
    shutil.copy(os.path.join(sprites, picture), os.path.join("assets/sprites", name))
    return "assets/sprites/" + name


def compose(picture, regions, name):
    """A picture made of tiles of another, side by side: a prefab's own small tile map."""
    source = Image.open(os.path.join(sprites, picture)).convert("RGBA")
    made = Image.new("RGBA", (TILE * len(regions), TILE))
    for place, region in enumerate(regions):
        made.paste(source.crop((region, 0, region + TILE, TILE)), (place * TILE, 0))
    made.save(os.path.join("assets/sprites", name))
    return "assets/sprites/" + name


def tileset(picture, name, solid):
    path = f"assets/sprites/{name}.tileset.json"
    with open(path, "w") as output:
        json.dump({"picture": copy(picture, name + ".png"), "tileSize": TILE, "solid": sorted(solid)}, output, indent="\t")
        output.write("\n")
    return path


def tile_maps(fields, offset, tiles):
    """The level's tile map, split by picture: a tile map node for each."""
    origin = vector(fields.get("position", "Vector2( 0, 0 )"))
    origin = (origin[0] + offset[0], origin[1] + offset[1])
    by_picture = {}
    for x, y, tile in cells(fields["tile_data"]):
        by_picture.setdefault(tiles[tile][0], []).append((x, y, tile))
    made = []
    names = {"Tiles/FactoryTiles.png": "factory", "Gadgets/AttractorTiles.png": "attractor"}
    for picture, placed in sorted(by_picture.items()):
        columns_of_picture = Image.open(os.path.join(sprites, picture)).width // TILE
        lowest_x, highest_x = min(x for x, _, _ in placed), max(x for x, _, _ in placed)
        lowest_y, highest_y = min(y for _, y, _ in placed), max(y for _, y, _ in placed)
        columns, rows = highest_x - lowest_x + 1, highest_y - lowest_y + 1
        grid, solid = [-1] * (columns * rows), set()
        for x, y, tile in placed:
            _, region_x, region_y, is_solid = tiles[tile]
            number = (region_y // TILE) * columns_of_picture + region_x // TILE
            grid[(y - lowest_y) * columns + (x - lowest_x)] = number
            if is_solid:
                solid.add(number)
        name = names[picture]
        corner = metres(origin[0] + lowest_x * TILE, origin[1] + lowest_y * TILE)
        made.append(node(name.capitalize(), corner, 0.4,
                         tiles={"tileset": tileset(picture, name, solid), "columns": columns, "rows": rows, "tiles": grid}))
    return made


def player(position):
    # the body is the prefab's collision box, 73 by 96 pixels; its pictures sit 2 pixels higher
    parts = [("Body", "Player/PlayerBody.png", "player-body.png"), ("Tail", "Player/PlayerTail1.png", "player-tail-1.png"),
             ("Tail wag", "Player/PlayerTail2.png", "player-tail-2.png"), ("Eyes", "Player/PlayerEyes.png", "player-eyes.png")]
    children = [node(name, metres(0.5, -2), 0.01 * index, sprite=copy(picture, file))
                for index, (name, picture, file) in enumerate(parts)]
    return node("Player", metres(*position), 0.6, body={"kind": "moving", "size": [0.73, 0.96], "gravityScale": 9.2},
                behaviours=[{"name": "Player", "fields": {"speed": 7, "acceleration": 60, "jump": 20}}], children=children)


def jumper(name, position, scale):
    # three tiles of the pad (0, 1 and 3) on a solid base; the trigger reaches above the base's top,
    # where the feet of what stands on it are
    centre = (position[0] + 150 * scale[0], position[1] + 50)
    return node(name, metres(*centre), 0.5, sprite=compose("Gadgets/JumperTiles.png", [0, 100, 300], "jumper.png"),
                scale=[scale[0], 1, 1], body={"kind": "solid", "size": [3, 0.6]},
                children=[node("Pad", metres(0, -35), 0, body={"kind": "trigger", "size": [3, 0.3]},
                               behaviours=[{"name": "Jumper", "fields": {"speed": 26}}])])


def rail(position):
    # the platform's four tiles, riding up and down
    centre = (position[0] + 200, position[1] + 50)
    return node("Rail", metres(*centre), 0.5,
                sprite=compose("Gadgets/RailedPlatformTiles.png", [0, 100, 200, 300], "rail.png"),
                body={"kind": "platform", "size": [4, 1]},
                behaviours=[{"name": "Rail", "fields": {"speed": 1.5, "distance": 4}}])


def magnet(position):
    return node("Magnet", metres(*position), 0.55, sprite=copy("Gadgets/MagnetIdle.png", "magnet.png"),
                body={"kind": "pushable", "size": [0.66, 0.66]})


def field(position, scale):
    # the prefab's area is 201 by 474 pixels round (100, 233); it pulled up, and the level asked
    # for left, which never worked in Godot: here it pulls up, as the prefab meant, and the
    # inspector changes it
    centre = (position[0] + 100 * scale[0], position[1] + 233 * scale[1])
    picture = node("Glow", metres(0, (251 - 233) * scale[1]), -0.01, sprite=copy("Gadgets/MagneticField.png", "field.png"))
    picture["scale"] = [scale[0], scale[1], 1]
    return node("Field", metres(*centre), 0.3,
                body={"kind": "trigger", "size": [round(2.01 * scale[0], 4), round(4.74 * scale[1], 4)], "gravity": [0, 9.81]},
                children=[picture])


def convert():
    tiles = tiles_of(level_text)
    offset, nodes = (0.0, 0.0), []
    for header, fields in sections(level_text):
        if not header.startswith("node "):
            continue
        name = re.search(r'name="([^"]+)"', header).group(1)
        position = vector(fields.get("position", "Vector2( 0, 0 )"))
        scale = vector(fields.get("scale", "Vector2( 1, 1 )"))
        if 'parent=' not in header:
            offset = position
            continue
        position = (position[0] + offset[0], position[1] + offset[1])
        if name == "TileMap":
            nodes.extend(tile_maps(fields, offset, tiles))
        elif name == "Player":
            nodes.append(player(position))
        elif name.startswith("Jumper"):
            nodes.append(jumper("Jump pad " + (name[len("Jumper"):] or "1"), position, scale))
        elif name == "Rail":
            nodes.append(rail(position))
        elif name == "Magnet":
            nodes.append(magnet(position))
        elif name == "MagneticField":
            nodes.append(field(position, scale))
    nodes.append(node("Camera", [0, 0], 1, camera={"projection": "orthographic", "height": 10, "width": 16, "main": True,
                                                    "follow": "Player", "smoothing": 0.15, "ceiling": True}))
    return {"name": "Level 1", "mode": "2d", "sky": SKY, "pixelsPerMetre": PIXELS_PER_METRE, "nodes": nodes}


with open("scenes/level-1.scene.json", "w") as output:
    json.dump(convert(), output, indent="\t")
    output.write("\n")
