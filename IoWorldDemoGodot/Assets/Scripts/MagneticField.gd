extends Area2D

const Direction = preload("Mechanics/Direction.gd");

export var direction : int = 0 setget set_direction;
export var attract_force = 100 setget set_force;

## NOT WORKING :( 
## Maybe overwriting sets gravity and its vector to initial values (?)
## Not sure...

func set_direction(direction : int):
	
	match direction :
		
		Direction.UP:
			gravity_vec = Vector2(0,-1);
			print("up");
			
		Direction.RIGHT:
			gravity_vec = Vector2(1,0);
			print("right");
			
		Direction.DOWN:
			gravity_vec = Vector2(0,1);
			print("down");
			
		Direction.LEFT:
			gravity_vec = Vector2(-1,0);
			print("left");
			
		_:	
			gravity_vec = Vector2(0,0);
			print("none");

func set_force(force : int):
	gravity = force;
