"""Makes Io World's start screen from io-world's Unity menu (MainMenu.unity), as it was in the
repository's history before io-world moved to Godot.

python3 tools/make_start_screen.py ../io-world 713b9d1
Unity placed each picture with a scale, at 100 pixels a unit; here each is resized to the size it
showed at, 40 pixels a metre (the 3DS's top screen shows 24), so none is larger than it is drawn.
Its 2D lights and its music ("tranquil") are not here: the engine has neither yet.
"""
import io
import json
import os
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


def node(name, x, y, depth, **parts):
    result = {"name": name, "position": [x, y, depth], "rotation": [0, 0, 0], "scale": [1, 1, 1]}
    result.update(parts)
    return result


behaviours = {
    "Press start": [{"name": "StartScreen", "fields": {"first": "scenes/level-1.scene.json", "blink": 0.6}}],
    "Snail": [{"name": "Snail", "fields": {"speed": 0.075, "rests": 0.25, "distance": 2}}],  # 0.0015 units a 50th of a second, 2 units
}
nodes = []
for name, (path, scale, x, y, depth) in PIECES.items():
    parts = {"sprite": picture(path, scale, name)}
    if name in behaviours:
        parts["behaviours"] = behaviours[name]
    nodes.append(node(name, x, y, depth, **parts))
nodes.append(node("Camera", 0, 0, 1, camera={"projection": "orthographic", "height": 10, "width": 17, "main": True}))
with open("scenes/start.scene.json", "w") as output:
    json.dump({"name": "Start screen", "mode": "2d", "sky": SKY, "pixelsPerMetre": PIXELS_PER_METRE, "nodes": nodes}, output, indent="\t")
    output.write("\n")
