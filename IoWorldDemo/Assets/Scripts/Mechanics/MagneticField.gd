extends Area2D

const Direction = preload("Direction.gd");

export var direction : int = 0 setget set_direction;
export var attract_force = 100 setget set_force;

## NOT WORKING :( 
## Maybe overwriting sets gravity and its vector to initial values (?)
## Not sure...

func set_direction(dir : int):

	direction = dir;
	
	match dir :
		
		Direction.UP:
			gravity_vec = Vector2(0,-1);
			
		Direction.RIGHT:
			gravity_vec = Vector2(1,0);
			
		Direction.DOWN:
			gravity_vec = Vector2(0,1);
			
		Direction.LEFT:
			gravity_vec = Vector2(-1,0);
			
		_:	
			gravity_vec = Vector2(0,0);

func set_force(force : int):
	gravity = force;
