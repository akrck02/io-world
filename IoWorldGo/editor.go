//go:build !nintendo3ds && !release

package main

import "github.com/akrck02/go-engine/editor"

// The editor is part of the game while it is made: go-engine starts the game to edit it, and its
// view plays the behaviours compiled here. Builds for the 3DS and finished builds (-tags release)
// leave it out.
func init() { editor.Install() }
