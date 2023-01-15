tool
extends Node2D

## Preload 
const Axis = preload("../Util/Axis.gd")
const Direction = preload("../Util/Direction.gd")

## Onready variables
onready var platform = $Platform
onready var collisionShape2D = $Platform/CollisionShape2D
onready var _start = platform.position

## Export variables
export var active : bool = true
export var axis : String = Axis.X setget set_axis
export var speed : float = float(300) 
export var distance : float = float(200) setget set_distance
export var wait_time : float = float(3)

## Common constants / variables
const y = Vector2(0,-1)
const x = Vector2(-1,0)

var _direction : int;
var _top_time = 0;

## Update physhics
func _physics_process(_delta):
	
	if Engine.editor_hint:
		return
	
	move()
	pass

## Set the axis to move on
func set_axis(ax : String):
	
	if(Axis.X == ax.to_lower()): 
		axis = Axis.X
		_direction = Direction.RIGHT
	else:
		axis = Axis.Y
		_direction = Direction.UP
		
	update()

## Set the distance
func set_distance(dis : float):
	distance = dis
	update()

## Move the platform
func move():
	
	if not active:
		return;
	
	if _top_time == 0:
		_calculate_axis_values();
		
	## Calculate speed based on direction4
	if(_direction == Direction.UP || _direction == Direction.LEFT):
		speed = -abs(speed)
	elif (_direction == Direction.DOWN || _direction == Direction.RIGHT):
		speed = abs(speed)
		
	## If top is 0 move
	if _top_time == 0:	
		
		if _is_axis_x():
			var motion = Vector2(speed,0)	
			platform.move_and_slide(motion,x)
		else : 
			var motion = Vector2(0,speed)	
			platform.move_and_slide(motion,y)
					
		return
	
	## If waiting time passed move again
	if OS.get_unix_time() > _top_time + wait_time:
		var motion = Vector2(0,speed)	
		platform.move_and_slide(motion,x)
		_top_time = 0


## Calculate values evaluating 
func _calculate_axis_values():
	if _is_axis_x():
		
		## Reached the right
		if platform.position.x < _start.x - distance:
			_direction = Direction.RIGHT;
			_top_time = OS.get_unix_time()
			
		## Reached the left
		elif platform.position.x > _start.x:
			_direction = Direction.LEFT;
			_top_time = OS.get_unix_time()
			
	else:
		
		## Reached the top
		if platform.position.y < _start.y - distance:
			_direction = Direction.DOWN;
			_top_time = OS.get_unix_time()
			
		## Reached the bottom
		elif platform.position.y > _start.y:
			_direction = Direction.UP;
			_top_time = OS.get_unix_time()

## Get if the current axis is X
func _is_axis_x():
	return Axis.X == axis
	
## Draw gizmos 
func _draw():

	if not Engine.editor_hint:
		return
		
	if not collisionShape2D:
		return;

	var size = collisionShape2D.get_shape().get_extents()
	var startPoint = Vector2(_start.x + size.x, _start.y + size.y)
	
	if Axis.X == axis:		
		draw_line(startPoint, Vector2(startPoint.x + distance, startPoint.y),Color.cornflower)
	if Axis.Y == axis:
		draw_line(startPoint, Vector2(startPoint.x, startPoint.y - distance),Color.cornflower)
