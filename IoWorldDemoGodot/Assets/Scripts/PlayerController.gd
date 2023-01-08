extends KinematicBody2D

const moveSpeed = 300;
const maxSpeed = 700;

const jumpHeight = -2000;
const up = Vector2(0,-1);
const gravity = 150;

var isGravityAffecting = true;
var motion = Vector2();

var impulseY = 0;


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
	
	motion.y += impulseY;
	
	motion = move_and_slide(motion,up);
	impulseY = 0;

## Artificial jump
func apply_impulse_y(force):
	print(force);
	
	if(force > 0): 
		impulseY = -jumpHeight * abs(force);
		return;
	
	impulseY = jumpHeight * abs(force); 
	
## Restore physic default values
func restorePhysics():
	isGravityAffecting = true;
