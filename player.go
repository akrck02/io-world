package main

import (
	"math"

	"github.com/akrck02/go-engine/engine"
)

// Player runs with the arrows (or the 3DS's cross and circle pad), speeding up and slowing down
// as io-world's did, jumps with Space, the up arrow or the 3DS's A while it stands on its floor
// (which a gravity field may turn), and starts the level again with Escape or the 3DS's B. Its
// tail wags while it runs: its children "Tail" and "Tail wag" take turns.
type Player struct {
	Speed        float64 `json:"speed"`        // metres a second, running at most
	Acceleration float64 `json:"acceleration"` // metres a second each second, speeding up or slowing down
	Jump         float64 `json:"jump"`         // metres a second, away from its floor, when it jumps

	wagging float64 // seconds into the tail's wag
}

// wagSeconds is how long the tail stays on each side.
const wagSeconds = 0.15

// Update runs every frame.
func (player *Player) Update(context *engine.Context) {
	if context.Input.Pressed(engine.KeyEscape) {
		context.RestartScene()
		return
	}
	body, found := context.Node.Body()
	if !found {
		return
	}
	direction := context.Input.Move().X
	body.Velocity.X = toward(body.Velocity.X, direction*player.Speed, player.Acceleration*context.Delta)
	jumping := context.Input.Pressed(engine.KeySpace) || context.Input.Pressed(engine.KeyUp)
	if jumping && body.OnFloor() {
		body.Velocity.Y = player.Jump
		if body.Pull().Y > 0 { // pulled up, its floor is above it: it jumps down
			body.Velocity.Y = -player.Jump
		}
	}
	player.wag(context, direction != 0)
}

// toward moves a value toward a target by at most a step.
func toward(value, target, step float64) float64 {
	if math.Abs(target-value) <= step {
		return target
	}
	if target > value {
		return value + step
	}
	return value - step
}

// wag shows one tail or the other in turns while the player runs, and the first while it stands.
func (player *Player) wag(context *engine.Context, running bool) {
	tail, found := context.Node.Find("Tail")
	wagged, foundWag := context.Node.Find("Tail wag")
	if !found || !foundWag {
		return
	}
	if !running {
		player.wagging = 0
	} else {
		player.wagging += context.Delta
	}
	second := int(player.wagging/wagSeconds)%2 == 1
	tail.Visible, wagged.Visible = !second, second
}
