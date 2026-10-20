package main

import (
	"math"

	"github.com/akrck02/go-engine/engine"
	"github.com/akrck02/go-engine/physics"
)

// Player runs and jumps as io-world's did: the arrows (or the 3DS's cross and circle pad) speed it
// up quickly to its top speed; let go, it stops at once on the ground and slowly in the air; while
// Space, the up arrow or the 3DS's A is held and it stands on its floor, it jumps. Escape or the
// 3DS's B starts the level again. Its tail wags while it runs: its children "Tail" and "Tail wag"
// take turns.
type Player struct {
	Speed        float64 `json:"speed"`        // metres a second, running at most
	Acceleration float64 `json:"acceleration"` // metres a second each second, speeding up
	GroundKeeps  float64 `json:"groundKeeps"`  // the share of its speed kept each original frame on the ground, let go
	AirKeeps     float64 `json:"airKeeps"`     // the same, in the air
	Jump         float64 `json:"jump"`         // metres a second, away from its floor, when it jumps

	wagging float64 // seconds into the tail's wag
}

// The original's physics ran at this many frames a second: its slowing down is per frame.
const originalFramesPerSecond = 60

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
	player.run(body, direction, context.Delta)
	jumping := context.Input.Down(engine.KeySpace) || context.Input.Down(engine.KeyUp)
	if jumping && body.OnFloor() {
		player.jump(body)
	}
	player.wag(context, direction != 0)
}

// run speeds the player up the way it is pushed, or slows it down when it is not.
func (player *Player) run(body *physics.Body, direction, seconds float64) {
	if direction != 0 {
		body.Velocity.X = toward(body.Velocity.X, direction*player.Speed, player.Acceleration*seconds)
		return
	}
	keeps := player.AirKeeps
	if body.OnFloor() {
		keeps = player.GroundKeeps
	}
	body.Velocity.X *= math.Pow(keeps, seconds*originalFramesPerSecond)
}

// jump throws the body away from its floor: against the way its gravity pulls it most.
func (player *Player) jump(body *physics.Body) {
	pull := body.Pull()
	if math.Abs(pull.X) > math.Abs(pull.Y) {
		body.Velocity.X = -math.Copysign(player.Jump, pull.X)
		return
	}
	body.Velocity.Y = player.Jump
	if pull.Y > 0 { // pulled up, its floor is above it: it jumps down
		body.Velocity.Y = -player.Jump
	}
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
