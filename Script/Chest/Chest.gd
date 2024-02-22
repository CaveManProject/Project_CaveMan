class_name Chest extends Node2D

var player_in_area: bool = false
@onready var chest_ui = $chest_ui

func _on_openable_area_body_entered(body: CharacterBody2D):
	if body is PlayerNode:
		player_in_area = true


func _on_openable_area_body_exited(body: CharacterBody2D):
	if body is PlayerNode:
		player_in_area = false

func _process(delta: float):
	if player_in_area and Input.is_action_just_pressed("e"):
		chest_ui.toggle()
	elif not player_in_area:
		chest_ui.close()
