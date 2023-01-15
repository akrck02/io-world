extends KinematicBody2D

const Controls = preload("Mechanics/Util/Controls.gd");

const moveSpeed : float = float(300);
const maxSpeed : float = float(700);

const jumpHeight : float = float(-2000);
const up : Vector2 = Vector2(0,-1);
const gravity : float = float(150);

var isGravityAffecting = true;
var motion : Vector2 = Vector2();
var impulse : Vector2 = Vector2();
var inertia = 100;


func _physics_process(_delta):
	
	## If scene reset
	if Input.is_action_pressed(Controls.RESET_LEVEL):
		var _ok = get_tree().reload_current_scene();

	## If gravity was not canceled	
	if(isGravityAffecting):
		motion.y += gravity;	
	
	var friction = false;
	
	## Right movement
	if Input.is_action_pressed(Controls.RIGHT):
		motion.x = min(motion.x + moveSpeed, maxSpeed);
 
	## Left movement
	elif Input.is_action_pressed(Controls.LEFT):
		motion.x = max(motion.x - moveSpeed, -maxSpeed);

	## IdleState
	else:
		friction = true;
		
	## Ground detection
	if is_on_floor():
		
		if Input.is_action_pressed(Controls.JUMP):
			motion.y = jumpHeight;
		
		if friction == true: 
			motion.x = lerp(motion.x, 0, 0.5);
			

	## Air friction
	elif friction == true:
			motion.x = lerp(motion.x,0,0.1);
	
	motion = get_normalized_motion();
	motion = move_and_slide(motion,up, false, 4, PI/4 ,false);
	
	## Adding inertia to collider Rigidbodies
	for index in get_slide_count():
		var collision = get_slide_collision(index);
		if collision.collider.is_in_group("bodies"):
			collision.collider.apply_central_impulse(-collision.normal * inertia);
	
	impulse = Vector2();

## Artificial jump
func apply_impulse(force : Vector2):
	
	if Input.is_action_pressed(Controls.CANCEL):
		return;
	
	impulse = Vector2(force.x * 3, force.y * 3);


func get_normalized_motion():
	
	var x : float = motion.x + impulse.x;
	var y : float = motion.y + impulse.y;

	if impulse != Vector2.ZERO && y < impulse.y:
		print(impulse.y);
		print(y);
		y = impulse.y;

	return Vector2(x, y);
