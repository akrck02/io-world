extends Area2D

var attract_force = 100;

func _on_MagneticField_body_entered(body):
	
	if("Player" == body.name):
		if(body.hasMagnet()):
			body.jump(1000);
			print("player has magnet!");

func _on_MagneticField_body_exited(body):
	if("Player" == body.name):
		body.restorePhysics();
