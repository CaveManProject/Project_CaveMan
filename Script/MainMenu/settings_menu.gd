class_name SettingsMenu
extends Control

@onready var back_button = $MarginContainer/VBoxContainer/BACK as Button

signal exit_settings_menu

func _ready():
	back_button.button_down.connect(on_exit_pressed)
	set_process(false)
	
func on_exit_pressed() -> void:
		exit_settings_menu.emit()
		set_process(false)
	
