extends Node2D

export var screen : int = 0;

func _ready():
	OS.set_current_screen(screen);

