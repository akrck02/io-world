package main

import (
	"math/rand/v2"

	"github.com/akrck02/go-engine/engine"
)

// StartScreen makes its node ("Press start") blink, and starts the game on A, Space or Enter.
type StartScreen struct {
	First string  `json:"first"` // the scene the game starts with, as "scenes/level-1.scene.json"
	Blink float64 `json:"blink"` // seconds it shows, then hides

	shown float64 // seconds into its blink
}

// Update runs every frame.
func (start *StartScreen) Update(context *engine.Context) {
	if context.Input.Pressed(engine.KeySpace) || context.Input.Pressed(engine.KeyEnter) {
		context.ChangeScene(start.First)
		return
	}
	start.shown += context.Delta
	if start.Blink > 0 && start.shown >= 2*start.Blink {
		start.shown = 0
	}
	context.Node.Visible = start.Blink <= 0 || start.shown < start.Blink
}

// Snail creeps right a distance, as io-world's menu's did, resting now and then.
type Snail struct {
	Speed    float64 `json:"speed"`    // metres a second, creeping
	Rests    float64 `json:"rests"`    // the share of moments it rests in, from 0 to 1
	Distance float64 `json:"distance"` // metres it creeps, then stays

	crept float64
}

// Update runs every frame.
func (snail *Snail) Update(context *engine.Context) {
	if snail.crept >= snail.Distance || rand.Float64() < snail.Rests {
		return
	}
	step := min(snail.Speed*context.Delta, snail.Distance-snail.crept)
	snail.crept += step
	context.Node.Transform.Position.X += step
}
