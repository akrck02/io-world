extends Area2D

export var force: int = 1000;
export var direction: Vector2 = Vector2();
export var allowed_names = ["Player","Magnet","CompanionCube"];

func _get_direction(rotation : int):
	
	match rotation:
		0:
			return Vector2(0,-1); 
		45:
			return Vector2(1,-1); 
		90:
			return Vector2(1,0); 
		135:
			return Vector2(1,1); 
		180:
			return Vector2(0,1); 
		225:
			return Vector2(-1,1); 
		270:
			return Vector2(-1,0); 
		315:
			return Vector2(-1,-1); 
	
	return Vector2(0,0);

func _on_jumper_body_entered(body):
	
	direction = _get_direction(int(rotation_degrees));
	
	if(not allowed_names.has(body.name)):
		return;
		
	var new_direction = Vector2(direction.x * force, direction.y * force);
		
	if("Player" == body.name):
		body.apply_impulse(new_direction);
		return;
	
	body.apply_impulse(Vector2(), new_direction);
