package main

import "github.com/akrck02/go-engine/engine"

// Rail moves its platform up a distance and down again, over and over, carrying what stands on it.
type Rail struct {
	Speed    float64 `json:"speed"`    // metres a second
	Distance float64 `json:"distance"` // metres it rises

	bottom  float64 // the height it started at
	started bool
	falling bool
}

// Update runs every frame.
func (rail *Rail) Update(context *engine.Context) {
	body, found := context.Node.Body()
	if !found {
		return
	}
	height := context.Node.World().Position.Y
	if !rail.started {
		rail.bottom, rail.started = height, true
	}
	if !rail.falling && height >= rail.bottom+rail.Distance {
		rail.falling = true
	}
	if rail.falling && height <= rail.bottom {
		rail.falling = false
	}
	body.Velocity.X, body.Velocity.Y = 0, rail.Speed
	if rail.falling {
		body.Velocity.Y = -rail.Speed
	}
}
