package main

import (
	"image"

	"github.com/akrck02/go-renderer/draw2d"
	ui "github.com/akrck02/go-ui"

	"github.com/akrck02/go-engine/engine"
)

// The 3DS's bottom screen is a menu of the levels made of the levels' own pieces: square bricks
// above and below, Io's face by the title, each level a block of the factory's bricks with a
// button's face beside its name (the green, pleased one for the level playing, the blue, cross
// one for the others), and under the stylus the jump pads' blue drips along its top.

// The menu's colours, taken from the level's pictures.
var (
	menuBackground = draw2d.Hex("#4d4d4d") // the level's sky
	padBlue        = draw2d.Hex("#7db3bf") // the jump pads
	menuInk        = draw2d.Hex("#ffffff")
	opaque         = draw2d.Color{1, 1, 1, 1}
)

// The menu's measures, in the bottom screen's pixels.
const (
	brickBand   = 24
	faceSide    = 34
	levelHeight = 40
	buttonWidth = 52 // a button's face, twice as wide as tall, as its picture
	dripsHeight = 12
	menuPad     = 10
	menuGap     = 6
	titleSize   = 20
	captionSize = 12
	levelSize   = 15
)

// The pictures the menu is made of.
const (
	brickPicture  = "assets/sprites/factory.png"
	bodyPicture   = "assets/sprites/player-body.png"
	eyesPicture   = "assets/sprites/player-eyes.png"
	buttonPicture = "assets/sprites/button.png" // two frames side by side: off, then on
	dripsPicture  = "assets/sprites/jumper.png"
)

// menuArt are the pictures the menu draws, read once from the scene's.
type menuArt struct {
	read                               bool
	bricks, body, eyes, drips, off, on *draw2d.Image
}

// art is the menu's pictures.
var art menuArt

// readFrom reads the pictures from a scene, the first time.
func (pictures *menuArt) readFrom(scene *engine.Scene) {
	if pictures.read {
		return
	}
	pictures.read = true
	pictures.bricks, _ = scene.Picture(brickPicture) // a picture missing is drawn as nothing
	pictures.body, _ = scene.Picture(bodyPicture)
	pictures.eyes, _ = scene.Picture(eyesPicture)
	pictures.drips, _ = scene.Picture(dripsPicture)
	if buttons, err := scene.Picture(buttonPicture); err == nil {
		pictures.off, pictures.on = halves(buttons)
	}
}

// halves are the left and right halves of a picture: the two frames of the button's.
func halves(picture *draw2d.Image) (left, right *draw2d.Image) {
	bounds := picture.Pixels.Bounds()
	middle := bounds.Min.X + bounds.Dx()/2
	left = draw2d.NewImage(picture.Pixels.SubImage(image.Rect(bounds.Min.X, bounds.Min.Y, middle, bounds.Max.Y)))
	right = draw2d.NewImage(picture.Pixels.SubImage(image.Rect(middle, bounds.Min.Y, bounds.Max.X, bounds.Max.Y)))
	return left, right
}

// draw draws a picture filling an area, when there is one.
func draw(list *draw2d.List, picture *draw2d.Image, area draw2d.Rect) {
	if picture != nil {
		list.Image(picture, area, 0, opaque)
	}
}

// bottomScreen draws the menu of levels.
func bottomScreen(context *ui.Context, menu engine.Menu) {
	art.readFrom(menu.Playing())
	whole := ui.Layout{Direction: ui.Vertical, Width: ui.Grow(), Height: ui.Grow()}
	context.Frame(whole, ui.Look{Fill: menuBackground}, func() {
		bricks(context)
		context.Box(ui.Layout{Direction: ui.Vertical, Width: ui.Grow(), Height: ui.Grow(), Padding: ui.Pad(menuPad), Gap: menuGap}, func() {
			title(context)
			for _, scene := range menu.Scenes() {
				level(context, menu, scene)
			}
		})
		bricks(context)
	})
}

// bricks is a band of the factory's bricks across the screen.
func bricks(context *ui.Context) {
	context.Paint(ui.Layout{Width: ui.Grow(), Height: ui.Fixed(brickBand)}, func(area draw2d.Rect, list *draw2d.List) {
		draw(list, art.bricks, area)
	})
}

// title is Io's face and the game's name.
func title(context *ui.Context) {
	context.Box(ui.Layout{Width: ui.Grow(), Gap: menuGap, AlignY: ui.Center}, func() {
		context.Paint(ui.Layout{Width: ui.Fixed(faceSide), Height: ui.Fixed(faceSide)}, func(area draw2d.Rect, list *draw2d.List) {
			draw(list, art.body, area)
			draw(list, art.eyes, area)
		})
		context.Box(ui.Layout{Direction: ui.Vertical}, func() {
			context.Text("IO WORLD", ui.TextStyle{Size: titleSize, Weight: draw2d.Bold, Color: menuInk})
			context.Text("Touch a level to play it", ui.TextStyle{Size: captionSize, Color: padBlue})
		})
	})
}

// level is a level's block of bricks: touched, it plays. Under the stylus it drips the pads' blue.
func level(context *ui.Context, menu engine.Menu, scene engine.MenuScene) {
	face := art.off
	if menu.Playing().Name == scene.Name {
		face = art.on
	}
	square := ui.Look{}
	touched := ui.Look{Border: padBlue, BorderWidth: 2}
	looks := ui.Looks{Normal: square, Hovered: touched, Pressed: touched}
	if context.Clickable(scene.File, ui.Layout{Width: ui.Grow(), Height: ui.Fixed(levelHeight)}, looks, func() {
		context.Paint(ui.Layout{Width: ui.Grow(), Height: ui.Grow()}, func(area draw2d.Rect, list *draw2d.List) {
			paintLevel(list, area, face, scene.Name)
		})
	}) {
		menu.Play(scene.File)
	}
}

// paintLevel paints a level's block: the bricks, the button's face at its left, and its name.
func paintLevel(list *draw2d.List, area draw2d.Rect, face *draw2d.Image, name string) {
	draw(list, art.bricks, area)
	button := draw2d.Rect{X: area.X + menuGap, Y: area.Y + (area.Height-buttonWidth/2)/2, Width: buttonWidth, Height: buttonWidth / 2}
	draw(list, face, button)
	baseline := area.Y + area.Height/2 + levelSize/3
	list.Text(name, button.X+button.Width+menuGap, baseline, levelSize, draw2d.Bold, menuInk)
}
