package main

import (
	"os"
	"testing"

	"github.com/akrck02/go-renderer/input"

	"github.com/akrck02/go-engine/engine"
	"github.com/akrck02/go-engine/space"
)

// frame is a sixtieth of a second.
const frame = 1.0 / 60

func TestMain(main *testing.M) {
	register()
	os.Exit(main.Run())
}

// levelOne loads the first level and starts it, failing on any problem.
func levelOne(test *testing.T) *engine.Scene {
	loaded, err := engine.Load(os.DirFS("."), "scenes/level-1.scene.json")
	if err != nil {
		test.Fatal(err)
	}
	if problems := loaded.Start(); len(problems) > 0 {
		test.Fatal(problems)
	}
	return loaded
}

// play moves the level on for some seconds, with the keys held.
func play(level *engine.Scene, seconds float64, held ...input.Key) {
	keys := input.NewState()
	for _, key := range held {
		keys.SetKey(key, true)
	}
	for played := 0.0; played < seconds; played += frame {
		level.Update(frame, keys)
		keys.EndFrame()
	}
}

// find finds a node of the level, failing when it is not there.
func find(test *testing.T, level *engine.Scene, path string) *engine.Node {
	node, found := level.Find(path)
	if !found {
		test.Fatalf("%s is not in the level", path)
	}
	return node
}

// putAt moves a node to a place of the world, and stops its body.
func putAt(node *engine.Node, x, y float64) {
	node.Transform.Position = space.NewVector(x, y, node.Transform.Position.Z)
	if body, found := node.Body(); found {
		body.Velocity = space.Zero
	}
}

func TestThePlayerLandsAndRuns(test *testing.T) {
	level := levelOne(test)
	player := find(test, level, "Player")
	play(level, 1)
	body, _ := player.Body()
	if !body.OnFloor() {
		test.Fatalf("after a second the player stands nowhere, at %v", player.World().Position)
	}
	start := player.World().Position.X
	play(level, 0.5, input.KeyLeft)
	if moved := start - player.World().Position.X; moved < 1 {
		test.Fatalf("running left half a second moved the player %v metres", moved)
	}
}

func TestAJumpPadThrowsThePlayerUp(test *testing.T) {
	level := levelOne(test)
	player, pad := find(test, level, "Player"), find(test, level, "Jump pad 2")
	play(level, frame)
	putAt(player, pad.World().Position.X, pad.World().Position.Y+1.5)
	highest := player.World().Position.Y
	for played := 0.0; played < 1.5; played += frame {
		play(level, frame)
		highest = max(highest, player.World().Position.Y)
	}
	if rise := highest - (pad.World().Position.Y + 0.8); rise < 2 {
		test.Fatalf("the pad threw the player only %v metres above it", rise)
	}
}

func TestThePlayerPushesTheMagnet(test *testing.T) {
	level := levelOne(test)
	player, magnet := find(test, level, "Player"), find(test, level, "Magnet")
	play(level, 1)
	start := magnet.World().Position
	putAt(player, start.X+1.2, start.Y+0.2)
	play(level, 1, input.KeyLeft)
	if moved := start.X - magnet.World().Position.X; moved < 0.5 {
		test.Fatalf("walking into the magnet for a second moved it %v metres", moved)
	}
}

func TestTheRailRisesAndComesBack(test *testing.T) {
	level := levelOne(test)
	rail := find(test, level, "Rail")
	start := rail.World().Position.Y
	play(level, 2)
	if risen := rail.World().Position.Y - start; risen < 2 {
		test.Fatalf("in two seconds the rail rose %v metres", risen)
	}
	play(level, 4)
	if back := rail.World().Position.Y - start; back > 2 {
		test.Fatalf("six seconds in, the rail is still %v metres up: it did not come back", back)
	}
}

func TestTheFieldLiftsThePlayer(test *testing.T) {
	level := levelOne(test)
	player, field := find(test, level, "Player"), find(test, level, "Field")
	play(level, frame)
	centre := field.World().Position
	putAt(player, centre.X, centre.Y-1)
	play(level, 0.5)
	body, _ := player.Body()
	if player.World().Position.Y <= centre.Y-1 || body.Pull().Y <= 0 {
		test.Fatalf("in the field the player went to %v, pulled %v: it should rise", player.World().Position.Y, body.Pull())
	}
}
