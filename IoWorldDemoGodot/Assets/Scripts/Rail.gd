extends Node2D

const Axis = preload("Mechanics/Axis.gd");
onready var platform = $Platform;

export var active : bool = false;
export var axis : String = Axis.X setget set_axis;
export var speed : float = float(100); 

const up = Vector2(0,-1);
const down = Vector2(0,1);
const left = Vector2(-1,0);
const right = Vector2(1,0);

## Update physhics
func _physics_process(_delta):
	move();
	pass;

## Set the axis to move on
func set_axis(ax : String):
	
	if(Axis.X == ax.to_lower()): 
		axis = Axis.X;
	else:
		axis = Axis.Y;


## Move the platform
func move():
	
	if active:
		
		if Axis.X == axis:
			pass
		else:
			var motion = Vector2(0,-speed);	
			platform.move_and_slide(motion,up);
