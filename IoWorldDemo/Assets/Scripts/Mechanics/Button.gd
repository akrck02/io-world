extends Area2D

onready var animationPlayer = $AnimationPlayer 

var on : bool = false; 



func _on_Button_body_entered(body):
	
	if(on):
		return;
	
	if "Player" != body.name && "Magnet" != body.name:
		return;
		
	animationPlayer.play("ButtonOn");
	print(body.name);
	on = true;
