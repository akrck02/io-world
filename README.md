# Io World

[io-world](https://github.com/akrck02/io-world)'s first level, a puzzle-platformer made with Godot,
made again with [Go Engine](https://github.com/akrck02/go-engine) to play on the Nintendo 3DS:
Io walks and jumps through a factory, bounced by jump pads, carried by a rail, pushing a magnet
and lifted by a magnetic field.

**Status:** an experiment, the engine's second example (its proposal 7).

## Play

```bash
go run .                                   # on the desktop: arrows, Space jumps, Escape restarts
go run github.com/akrck02/go-engine/cmd/go-engine .   # open it in the editor
```

On the 3DS: build it from the editor's Build workspace (TinyGo for the 3DS and devkitPro, both
from its Install panel), then send it to the Homebrew Launcher. The cross moves, A jumps, B
starts the level again.

## Made of

- `scenes/level-1.scene.json`: the level, made once from io-world's `Level1.tscn` by
  `tools/convert_level.py`, then edited in the editor.
- `assets/sprites/`: io-world's pictures; the jump pad's and the rail's are composed of their tiles.
- `player.go`, `jumper.go`, `rail.go`: the behaviours. The magnet is a pushable body and the field
  a trigger with a gravity of its own: no code.

## AI disclosure

Written by an AI assistant (Claude, by Anthropic) under the author's direction; the pictures are
io-world's, by the author. Pending human review.
