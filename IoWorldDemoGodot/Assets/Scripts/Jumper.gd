extends CollisionShape2D

const force = -1000;

func _on_Area2D_body_entered(body):
	
	if("Player" != body.name && "Magnet" != body.name):
		return;
		
	if("Player" == body.name):
		body.apply_impulse_y(force / 850);
		return;
		
	body.apply_impulse(Vector2(), Vector2(0,force));
	print(body.name);	
