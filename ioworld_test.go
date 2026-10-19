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
func levelOne(test *testing.T) *engine.Scene { return levelFrom(test, "scenes/level-1.scene.json") }

// levelFrom loads a level's file and starts it, failing on any problem.
func levelFrom(test *testing.T, file string) *engine.Scene {
	loaded, err := engine.Load(os.DirFS("."), file)
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

func TestTheFieldLiftsTheMagnetButNotThePlayer(test *testing.T) {
	level := levelOne(test)
	player, magnet, field := find(test, level, "Player"), find(test, level, "Magnet"), find(test, level, "Field")
	play(level, frame)
	centre := field.World().Position
	putAt(magnet, centre.X, centre.Y-1)
	putAt(player, centre.X+1, centre.Y-1)
	play(level, 0.5)
	if magnet.World().Position.Y <= centre.Y-1 {
		test.Fatalf("in the field the magnet went to %v: it should rise", magnet.World().Position.Y)
	}
	if player.World().Position.Y >= centre.Y-1 {
		test.Fatalf("in the field the player went to %v: it should keep falling, as in io-world", player.World().Position.Y)
	}
}

func TestTheButtonStartsTheRailThatWaits(test *testing.T) {
	level := levelFrom(test, "scenes/level-1b.scene.json")
	rail, button, player := find(test, level, "Rail"), find(test, level, "Button"), find(test, level, "Player")
	start := rail.World().Position.Y
	play(level, 2)
	if moved := rail.World().Position.Y - start; moved > 1e-6 {
		test.Fatalf("before the button is pressed the rail moved %v metres", moved)
	}
	putAt(player, button.World().Position.X, button.World().Position.Y+1)
	play(level, 1)
	play(level, 3)
	if risen := rail.World().Position.Y - start; risen < 2 {
		test.Fatalf("pressed, the button did not start the rail: it rose %v metres", risen)
	}
}

func TestTheCubeIsPushed(test *testing.T) {
	level := levelFrom(test, "scenes/level-1b.scene.json")
	player, cube := find(test, level, "Player"), find(test, level, "Cube")
	play(level, 1)
	feet := player.World().Position
	putAt(cube, feet.X-2, feet.Y) // on the floor where the player stands: where it starts, it bounces on a pad
	play(level, 1)
	start := cube.World().Position
	putAt(player, start.X+1.2, start.Y+0.1)
	play(level, 1, input.KeyLeft)
	if moved := start.X - cube.World().Position.X; moved < 0.5 {
		test.Fatalf("walking into the cube for a second moved it %v metres", moved)
	}
}

func TestTheTestLevelTriesEverything(test *testing.T) {
	level := levelFrom(test, "scenes/test-level.scene.json")
	player := find(test, level, "Player")
	play(level, 1)
	if body, _ := player.Body(); !body.OnFloor() {
		test.Fatalf("the player stands nowhere at the start, at %v", player.World().Position)
	}
	// 2. the pad throws the player above the ledge
	pad := find(test, level, "Jump pad")
	putAt(player, pad.World().Position.X, 1.5)
	highest := 0.0
	for played := 0.0; played < 1.5; played += frame {
		play(level, frame)
		highest = max(highest, player.World().Position.Y)
	}
	if highest < 5.5 {
		test.Fatalf("the pad threw the player up to %v metres, not above the ledge at 5", highest)
	}
	// 3. the field lifts the magnet
	magnet, field := find(test, level, "Magnet"), find(test, level, "Field")
	putAt(magnet, field.World().Position.X, 1)
	play(level, 1)
	if magnet.World().Position.Y < 3 {
		test.Fatalf("the field lifted the magnet only to %v", magnet.World().Position.Y)
	}
	// 4, 5 and 6. the cube on the button starts the rail
	cube, button, rail := find(test, level, "Cube"), find(test, level, "Button"), find(test, level, "Rail")
	start := rail.World().Position.Y
	putAt(cube, button.World().Position.X, 1)
	play(level, 4)
	if risen := rail.World().Position.Y - start; risen < 3 {
		test.Fatalf("with the cube on the button the rail rose %v metres", risen)
	}
}

func TestTheStartScreenStartsTheGameOnA(test *testing.T) {
	start := levelFrom(test, "scenes/start.scene.json")
	snail := find(test, start, "Snail")
	from := snail.World().Position.X
	play(start, 2)
	if crept := snail.World().Position.X - from; crept <= 0 || crept > 2 {
		test.Fatalf("in two seconds the snail crept %v metres", crept)
	}
	keys := input.NewState()
	keys.SetKey(input.KeySpace, true)
	start.Update(frame, keys)
	next, replaced, err := start.TakeReplacement()
	if err != nil || !replaced || next.Name != "Level 1" {
		test.Fatalf("A on the start screen gives %v (replaced: %v, %v), not the first level", next, replaced, err)
	}
}
