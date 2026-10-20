package main

import (
	"github.com/akrck02/go-engine/engine"
	"github.com/akrck02/go-engine/physics"
)

// Button is pressed once by what lands on it (the player, the magnet, the cube): it lights up
// (its sprite's "on" animation) and activates the rail its Target names.
type Button struct {
	Target string `json:"target"` // the path of the node whose Rail it activates, as "Rail"

	pressed bool
}

// Touched runs when a body begins touching the button.
func (button *Button) Touched(context *engine.Context, other *engine.Node) {
	body, found := other.Body()
	if button.pressed || !found || (body.Kind != physics.Moving && body.Kind != physics.Pushable) {
		return
	}
	button.pressed = true
	context.Node.Play("on")
	target, found := context.Find(button.Target)
	if !found {
		return
	}
	if rail, found := engine.Behaviour[Rail](target); found {
		rail.Activate()
	}
}
