package main

import "github.com/akrck02/go-engine/engine"

// Rail moves its platform up a distance and down again, over and over, waiting a while at each
// end, carrying what stands on it. One that starts still waits for something (a Button) to
// activate it.
type Rail struct {
	Speed       float64 `json:"speed"`       // metres a second
	Distance    float64 `json:"distance"`    // metres it rises
	Wait        float64 `json:"wait"`        // seconds it stays at each end
	StartsStill bool    `json:"startsStill"` // it waits to be activated

	bottom  float64 // the height it started at
	started bool
	active  bool
	falling bool
	waited  float64 // seconds waited at the end it reached
}

// Activate sets the rail moving.
func (rail *Rail) Activate() { rail.active = true }

// Update runs every frame.
func (rail *Rail) Update(context *engine.Context) {
	body, found := context.Node.Body()
	if !found {
		return
	}
	height := context.Node.World().Position.Y
	if !rail.started {
		rail.bottom, rail.started, rail.active = height, true, !rail.StartsStill
	}
	body.Velocity.X, body.Velocity.Y = 0, 0
	if !rail.active || rail.waiting(context.Delta) {
		return
	}
	switch {
	case !rail.falling && height >= rail.bottom+rail.Distance:
		rail.turn()
	case rail.falling && height <= rail.bottom:
		rail.turn()
	}
	body.Velocity.Y = rail.Speed
	if rail.falling {
		body.Velocity.Y = -rail.Speed
	}
}

// turn makes the rail go the other way, after its wait.
func (rail *Rail) turn() {
	rail.falling = !rail.falling
	rail.waited = 0
}

// waiting tells whether the rail still waits at the end it reached, counting the seconds.
func (rail *Rail) waiting(seconds float64) bool {
	if rail.waited >= rail.Wait {
		return false
	}
	rail.waited += seconds
	return true
}
