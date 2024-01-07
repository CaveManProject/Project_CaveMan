extends Node2D

var state = "no chunk"
var player_in_area = false


func _ready():
	if state == "no chunk":
		$Spawn_Timer.start()
		
func _process(delta):
	if state == "no chunk":
		$AnimatedSprite2D.play("no_chunk")
	if state == "chunk":
		$AnimatedSprite2D.play("chunk")
		if player_in_area:
			if Input.is_action_just_pressed("e"):
				state = "no chunk"
				$Spawn_Timer.start()


func _on_pickable_area_body_entered(body):
	if body.has_method("player"):
		player_in_area = true


func _on_pickable_area_body_exited(body):
	if body.has_method("player"):
		player_in_area = false
		
func _on_spawn_timer_timeout():
	if state == "no chunk":
		state = "chunk"
