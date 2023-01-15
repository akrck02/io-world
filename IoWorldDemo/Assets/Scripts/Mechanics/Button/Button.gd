extends Area2D

## Onready variables
onready var animationPlayer = $AnimationPlayer 

## Export variables
export var behaviour: Script;

## Common variables
var on : bool = false; 

## When button ready
func _on_Button_ready():
	if not behaviour:
		pass;
		

## Actions when body clicks button
func _on_Button_body_entered(body):
	
	if(on):
		return;
	
	if "Player" != body.name && "Magnet" != body.name:
		return;
		
	animationPlayer.play("ButtonOn");	
	behaviour.activate()
	on = true;


