class_name MainMenu
extends Control

@onready var start_button = $MarginContainer/HBoxContainer/VBoxContainer/PLAY as Button
@onready var settings_button = $MarginContainer/HBoxContainer/VBoxContainer/SETTINGS as Button
@onready var exit_button = $MarginContainer/HBoxContainer/VBoxContainer/EXIT as Button
@onready var madeby_button = $MarginContainer/HBoxContainer/VBoxContainer/MADEBY as Button

@onready var margin_container = $MarginContainer as MarginContainer

@onready var settings_menu = $SettingsMenu as SettingsMenu
@onready var madeby_menu = $madeby_menu as MadeByMenu

@onready var start_lvl = preload("res://Scene/world.tscn") as PackedScene

func _ready():
	handle_connection_signals()
	
func on_start_pressed() -> void:
	get_tree().change_scene_to_packed(start_lvl)

func on_settings_pressed() -> void:
	margin_container.hide()
	settings_menu.set_process(true)
	settings_menu.show()
	
	
func on_madeby_pressed() -> void:
	margin_container.hide()
	madeby_menu.set_process(true)
	madeby_menu.show()

func on_exit_pressed() -> void:
	get_tree().quit()

func on_exit_settings_menu() -> void:
	margin_container.show()
	settings_menu.hide()
	
func on_exit_madeby_menu() -> void:
	margin_container.show()
	madeby_menu.hide()

func handle_connection_signals() -> void:
	start_button.button_down.connect(on_start_pressed)
	settings_button.button_down.connect(on_settings_pressed)
	madeby_button.button_down.connect(on_madeby_pressed)
	exit_button.button_down.connect(on_exit_pressed)
	settings_menu.exit_settings_menu.connect(on_exit_settings_menu)
	madeby_menu.exit_madeby_menu.connect(on_exit_madeby_menu)
