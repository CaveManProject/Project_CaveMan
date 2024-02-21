extends Control

@onready var back_button = $MarginContainer/VBoxContainer/Back_to_game as Button

@onready var ui_click = $ui_click as AudioStreamPlayer2D

func _ready(): 
	handle_connection_signals()

func on_resume_pressed() -> void:
	ui_click.play()
	get_tree().change_scene_to_file("res://scene/world.tscn")
	
func handle_connection_signals():
	back_button.button_down.connect(on_resume_pressed)