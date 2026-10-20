// Io World, made with go-engine: io-world's first level (a Godot game) made again, to play on the
// Nintendo 3DS. Io walks and jumps through a factory, bounced by jump pads, carried by a rail,
// pushing a magnet and lifted by a magnetic field. Play it with `go run .` (arrows or the 3DS's
// cross move, Space or A jumps, Escape or B starts the level again).
package main

import (
	"embed"
	"log"

	"github.com/akrck02/go-engine/engine"
)

// files are the project's settings, scenes and assets, carried inside the game.
//
//go:embed engine.json scenes all:assets
var files embed.FS

func main() {
	register()
	engine.SetBottomScreen(bottomScreen)
	if err := engine.Run(files); err != nil {
		log.Fatal(err)
	}
}

// register registers each behaviour the scenes use, by its type's name.
func register() {
	engine.Register(Player{}, Jumper{}, Rail{}, Button{}, StartScreen{}, Snail{})
}
