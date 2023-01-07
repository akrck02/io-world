extends KinematicBody2D

const moveSpeed = 300;
const maxSpeed = 700;

const jumpHeight = -2000;
const up = Vector2(0,-1);
const gravity = 150;

var isGravityAffecting = true;
var motion = Vector2();


func _physics_process(delta):

	## If gravity was canceled	
	if(isGravityAffecting):
		motion.y += gravity;	
	
	var friction = false;
	
	## Right movemente
	if Input.is_action_pressed("ui_right"):
		## sprite.flip_h = true;
		## animationPlayer.play("Walk");
		motion.x = min(motion.x + moveSpeed, maxSpeed);
 
	## Left movement
	elif Input.is_action_pressed("ui_left"):
		## sprite.flip_h = false;
		## animationPlayer.play("Walk");
		motion.x = max(motion.x - moveSpeed, -maxSpeed);

	## IdleState
	else:
		## animationPlayer.play("Idle");
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
			motion.x = lerp(motion.x,0,0.01)
			
	motion = move_and_slide(motion,up);

## Tells if player has a magnet
func hasMagnet():
	return true;
	
## Artificial jump
func jump(acceleration):
	isGravityAffecting = false;
	var velocity = Vector2();
	velocity += Vector2(0, -acceleration);
	velocity = move_and_slide(velocity)
	motion.y += velocity.y; 
	
## Restore physic default values
func restorePhysics():
	isGravityAffecting = true;
