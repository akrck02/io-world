"""Makes Io World's start screen from io-world's Unity menu (MainMenu.unity), as it was in the
repository's history before io-world moved to Godot.

python3 tools/make_start_screen.py .. 713b9d1   (from IoWorldGo/: this repository, at the commit
before io-world moved to Godot)
Unity placed each picture with a scale, at 100 pixels a unit; here each is resized to the size it
showed at, 40 pixels a metre (the 3DS's top screen shows 24), so none is larger than it is drawn.

Its 2D lights are read from the scene too: the global light becomes the scene's ambient, the point
light a point light, and each sprite light a glow (a child that shines through its piece's
picture, so it blinks and moves with it; in Unity the picture is drawn and lit, here the parent
draws it and the child shines). The two parametric lights, thin four-sided shapes the engine has
no kind for, become small point lights of the same reach. Its music ("tranquil") is not here yet.
"""
import io
import json
import os
import re
import subprocess
import sys

from PIL import Image

PIXELS_PER_METRE = 40
UNITY_PIXELS_PER_UNIT = 100
SKY = "#314d79"  # the Unity camera's background

repository, commit = sys.argv[1], sys.argv[2]
os.makedirs("assets/sprites/start", exist_ok=True)

# name: (picture under Sprites/, Unity's scale, centre x and y in units, depth)
PIECES = {
    "Wallpaper": ("Wallpaper/MenuWallpaper.png", 0.46, 0, 0, 0.0),
    "Floor": ("Decoration/MenuFloor.png", 0.44, 0, -4.85, 0.1),
    "Lamp": ("Decoration/Lamp.png", 0.1, -6.79, 3.91, 0.2),
    "Logo": ("Decoration/Logo.png", 0.35, 0, 1.08, 0.3),
    "Demo": ("Decoration/Demo.png", 0.1, 2.79, 0.32, 0.4),
    "Press start": ("Decoration/PressStart.png", 0.25, 0, -0.40, 0.3),
    "Snail": ("Decoration/Snail.png", 0.35, -5.86, -4.12, 0.5),
}


def picture(path, scale, name):
    """The picture from the repository's history, resized to its size on screen."""
    data = subprocess.run(["git", "-C", repository, "show", f"{commit}:IOWorldDemo/Assets/Sprites/{path}"],
                          check=True, capture_output=True).stdout
    source = Image.open(io.BytesIO(data)).convert("RGBA")
    factor = scale * PIXELS_PER_METRE / UNITY_PIXELS_PER_UNIT
    size = (max(1, round(source.width * factor)), max(1, round(source.height * factor)))
    file = f"assets/sprites/start/{name.lower().replace(' ', '-')}.png"
    source.resize(size, Image.LANCZOS).save(file)
    return file


# Unity's Light2D types (m_LightType)
PARAMETRIC, SPRITE, POINT, GLOBAL = "0", "2", "3", "4"


def unity_scene():
    """The menu's scene file from the repository's history, as its documents: id -> (class, body)."""
    text = subprocess.run(["git", "-C", repository, "show", f"{commit}:IOWorldDemo/Assets/Scenes/MainMenu.unity"],
                          check=True, capture_output=True, text=True).stdout
    documents = {}
    for match in re.finditer(r"^--- !u!(\d+) &(\d+)[^\n]*\n(.*?)(?=^--- |\Z)", text, re.S | re.M):
        documents[match.group(2)] = (match.group(1), match.group(3))
    return documents


def field(body, name):
    found = re.search(r"^\s*" + name + r": (.*)$", body, re.M)
    return found.group(1) if found else ""


def numbers(value):
    return [float(number) for number in re.findall(r"[xyzrgba]: (-?[\d.e-]+)", value)]


def reference(value):
    return re.search(r"\d+", value).group(0)


def world_place(documents, game_object):
    """Where a game object is and how it is scaled, through its transform's parents."""
    transform = next(identifier for identifier, (kind, body) in documents.items()
                     if kind == "4" and reference(field(body, "m_GameObject")) == game_object)
    chain = []
    while transform != "0":
        body = documents[transform][1]
        chain.append((numbers(field(body, "m_LocalPosition")), numbers(field(body, "m_LocalScale"))))
        transform = reference(field(body, "m_Father"))
    position, scale = [0.0, 0.0], [1.0, 1.0]
    for local_position, local_scale in reversed(chain):
        position = [position[axis] + local_position[axis] * scale[axis] for axis in range(2)]
        scale = [scale[axis] * local_scale[axis] for axis in range(2)]
    return position, scale


def sprite_names():
    """The sprites' pictures by their Unity id, from their .meta files."""
    listing = subprocess.run(["git", "-C", repository, "ls-tree", "-r", "--name-only", commit],
                             check=True, capture_output=True, text=True).stdout.split()
    names = {}
    for file in listing:
        if file.endswith(".png.meta") and "/Sprites/" in file:
            meta = subprocess.run(["git", "-C", repository, "show", f"{commit}:{file}"], capture_output=True, text=True).stdout
            guid = re.search(r"guid: (\w+)", meta)
            if guid:
                names[guid.group(1)] = file.split("/Sprites/", 1)[1][:-len(".meta")]
    return names


def hex_colour(value):
    red, green, blue = numbers(value)[:3]
    return "#%02x%02x%02x" % tuple(round(channel * 255) for channel in (red, green, blue))


def unity_lights():
    """The menu's lights: (type, colour, intensity, inner, outer, position, scale, picture)."""
    documents = unity_scene()
    pictures = sprite_names()
    lights = []
    for kind, body in documents.values():
        if kind != "114" or not field(body, "m_LightType"):
            continue
        position, scale = world_place(documents, reference(field(body, "m_GameObject")))
        cookie = re.search(r"guid: (\w+)", field(body, "m_LightCookieSprite"))
        lights.append((field(body, "m_LightType"), hex_colour(field(body, "m_Color")), float(field(body, "m_Intensity")),
                       float(field(body, "m_PointLightInnerRadius")), float(field(body, "m_PointLightOuterRadius")),
                       position, scale, pictures.get(cookie.group(1)) if cookie else None,
                       float(field(body, "m_ShapeLightParametricRadius")), float(field(body, "m_ShapeLightFalloffSize"))))
    return lights


def node(name, x, y, depth, **parts):
    result = {"name": name, "position": [x, y, depth], "rotation": [0, 0, 0], "scale": [1, 1, 1]}
    result.update(parts)
    return result


behaviours = {
    "Press start": [{"name": "StartScreen", "fields": {"first": "scenes/level-1.scene.json", "blink": 0.6}}],
    "Snail": [{"name": "Snail", "fields": {"speed": 0.075, "rests": 0.25, "distance": 2}}],  # 0.0015 units a 50th of a second, 2 units
}
nodes = []
pieces = {}
for name, (path, scale, x, y, depth) in PIECES.items():
    parts = {"sprite": picture(path, scale, name)}
    if name in behaviours:
        parts["behaviours"] = behaviours[name]
    pieces[path] = node(name, x, y, depth, **parts)
    nodes.append(pieces[path])

ambient = None
small_lamps = 0
for kind, colour, intensity, inner, outer, (x, y), (scale_x, scale_y), cookie, reach, falloff in unity_lights():
    light = {"colour": colour, "intensity": round(intensity, 3)}
    if kind == GLOBAL:
        ambient = light
    elif kind == POINT:
        light.update(kind="point", inner=round(inner, 3), outer=round(outer, 3))
        nodes.append(node("Lamp light", round(x, 3), round(y, 3), 0.6, light=light))
    elif kind == SPRITE and cookie in pieces:
        piece = pieces[cookie]
        _, unity_scale, piece_x, piece_y, _ = PIECES[piece["name"]]
        glow = node(piece["name"] + " glow", round(x - piece_x, 3), round(y - piece_y, 3), 0, sprite=piece["sprite"], light={**light, "kind": "sprite"})
        glow["scale"] = [round(scale_x / unity_scale, 3), round(scale_y / unity_scale, 3), 1]
        piece.setdefault("children", []).append(glow)
    elif kind == PARAMETRIC:
        small_lamps += 1
        light.update(kind="point", inner=0, outer=round(reach * max(scale_x, scale_y) + falloff, 3))
        nodes.append(node(f"Small light {small_lamps}", round(x, 3), round(y, 3), 0.6, light=light))
nodes.append(node("Camera", 0, 0, 1, camera={"projection": "orthographic", "height": 10, "width": 17, "main": True}))
scene = {"name": "Start screen", "mode": "2d", "sky": SKY, "pixelsPerMetre": PIXELS_PER_METRE, "nodes": nodes}
if ambient:
    scene["ambient"] = ambient
with open("scenes/start.scene.json", "w") as output:
    json.dump(scene, output, indent="\t")
    output.write("\n")
