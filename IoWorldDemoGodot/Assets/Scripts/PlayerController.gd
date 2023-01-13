extends KinematicBody2D

const moveSpeed : float = float(300);
const maxSpeed : float = float(700);

const jumpHeight : float = float(-2000);
const up : Vector2 = Vector2(0,-1);
const gravity : float = float(150);

var isGravityAffecting = true;
var motion : Vector2 = Vector2();
var impulse : Vector2 = Vector2();


func _physics_process(_delta):
	
	## If scene reset
	if Input.is_action_pressed("ui_reset"):
		var _ok = get_tree().reload_current_scene();

	## If gravity was not canceled	
	if(isGravityAffecting):
		motion.y += gravity;	
	
	var friction = false;
	
	## Right movement
	if Input.is_action_pressed("ui_right"):
		motion.x = min(motion.x + moveSpeed, maxSpeed);
 
	## Left movement
	elif Input.is_action_pressed("ui_left"):
		motion.x = max(motion.x - moveSpeed, -maxSpeed);

	## IdleState
	else:
		friction = true;
		
	## Ground detection
	if is_on_floor():
		
		if Input.is_action_pressed("ui_accept"):
			motion.y = jumpHeight;
		
		if friction == true: 
			motion.x = lerp(motion.x, 0, 0.5);
	
	## Air friction
	else:
		if friction == true:
			motion.x = lerp(motion.x,0,0.01);
	
	motion += impulse;
	motion = move_and_slide(motion,up);
	
	impulse = Vector2();

## Artificial jump
func apply_impulse(force : Vector2):
	
	## TODO insert here magic jump formula
	impulse = Vector2(force.x * 3, force.y * 3);
