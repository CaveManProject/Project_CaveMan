extends Node2D

var player_in_area = false

var player = null

@onready var inv = $Inv_UI

func _on_openable_area_body_entered(body):
	if body.has_method("player"):
		player_in_area = true
		player = body


func _on_openable_area_body_exited(body):
	if body.has_method("player"):
		player_in_area = false
