tool
extends Node2D

const Axis = preload("Axis.gd");
onready var platform = $Platform;
onready var collisionShape2D = $Platform/CollisionShape2D;

export var active : bool = true;
export var axis : String = Axis.X setget set_axis;
export var speed : float = float(300); 
export var distance : float = float(200) setget set_distance;

const up = Vector2(0,-1);
const down = Vector2(0,1);
const left = Vector2(-1,0);
const right = Vector2(1,0);

var _start : Vector2 = Vector2();

## On ready 
func _on_Rail_ready():
	_start = platform.position;

## Update physhics
func _physics_process(_delta):
	
	if Engine.editor_hint:
		return;
	
	move();
	pass;

## Set the axis to move on
func set_axis(ax : String):
	
	if(Axis.X == ax.to_lower()): 
		axis = Axis.X;
	else:
		axis = Axis.Y;
		
	update();

## Set the distance
func set_distance(dis : float):
	distance = dis;
	update();

## Move the platform
func move():
	
	if active:
		
		if Axis.X == axis:
			
			if(platform.position.x < _start.x - distance):
				speed = abs(speed);
			elif(platform.position.x > _start.x):
				speed = -abs(speed)

			var motion = Vector2(speed,0);	
			platform.move_and_slide(motion,right);
		else:
			
			if(platform.position.y < _start.y - distance):
				speed = abs(speed);
			elif(platform.position.y > _start.y):
				speed = -abs(speed)

			var motion = Vector2(0,speed);	
			platform.move_and_slide(motion,up);

func _draw():

	if not Engine.editor_hint:
		return;

	var size = collisionShape2D.get_shape().get_extents();
	var startPoint = Vector2(_start.x + size.x, _start.y + size.y);
	print(size);
	
	if Axis.X == axis:		
		draw_line(startPoint, Vector2(startPoint.x + distance, startPoint.y),Color.cornflower);
	if Axis.Y == axis:
		draw_line(startPoint, Vector2(startPoint.x, startPoint.y - distance),Color.cornflower);
		
