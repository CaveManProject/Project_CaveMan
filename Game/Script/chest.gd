class_name Chest extends Node2D

var player_in_area: bool = false

var player_body: PlayerBody = null

@onready var inv = $Inv_UI

func _on_openable_area_body_entered(body: CharacterBody2D):
	if body is PlayerBody:
		player_in_area = true
		player_body = body


func _on_openable_area_body_exited(body: CharacterBody2D):
	if body is PlayerBody:
		player_in_area = false
