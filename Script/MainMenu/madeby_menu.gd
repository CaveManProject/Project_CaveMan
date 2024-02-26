class_name MadeByMenu
extends Control

@onready var back_button = $MarginContainer/VBoxContainer/BACK as TextureButton

@onready var ui_click = $ui_click as AudioStreamPlayer2D

signal exit_madeby_menu

func _ready():
	back_button.button_down.connect(on_exit_pressed)
	set_process(false)
	
func on_exit_pressed() -> void:
		ui_click.play()
		exit_madeby_menu.emit()
		set_process(false)
