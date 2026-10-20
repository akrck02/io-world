package main

import (
	"github.com/akrck02/go-engine/engine"
	"github.com/akrck02/go-engine/physics"
)

// Jumper throws upwards what lands on its pad: the player, or the magnet.
type Jumper struct {
	Speed float64 `json:"speed"` // metres a second, upwards, given to what lands on it
}

// Touched runs when a body begins touching the pad.
func (jumper *Jumper) Touched(context *engine.Context, other *engine.Node) {
	body, found := other.Body()
	if found && (body.Kind == physics.Moving || body.Kind == physics.Pushable) {
		body.Velocity.Y = jumper.Speed
	}
}
