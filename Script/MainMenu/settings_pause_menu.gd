extends Control

@onready var back_button = $MarginContainer/VBoxContainer/Back_to_game as Button

@onready var game = preload("res://scene/world.tscn") as PackedScene


func _ready(): 
	handle_connection_signals()

func on_resume_pressed() -> void:
	get_tree().change_scene_to_packed(game)
	
func handle_connection_signals():
	back_button.button_down.connect(on_resume_pressed)
