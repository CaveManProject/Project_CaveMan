extends Node

@onready var resume_button = $PanelContainer/VBoxContainer/Resume as Button
@onready var settings_button = $PanelContainer/VBoxContainer/Settings as Button
@onready var quit_button = $PanelContainer/VBoxContainer/Quit as Button

@onready var PauseMenu = $"." as Control

#@onready var menu = preload("res://scene/MainMenu/main_menu.tscn") as PackedScene
@onready var settings = preload("res://scene/MainMenu/settings_pause_menu.tscn") as PackedScene

func _ready():
	handle_connection_signals()

func _process(delta):
	testESC()

func testESC() -> void:
	if Input.is_action_just_pressed("pause") and  !get_tree().paused:
		pause()
	elif Input.is_action_just_pressed("pause") and get_tree().paused:
		resume()

func  resume():
	get_tree().paused = false
	PauseMenu.hide()
	
func pause():
	get_tree().paused = true
	PauseMenu.show()
	

func _on_resume_pressed() -> void:
	resume()
	

func _on_settings_pressed()-> void:
	get_tree().paused = false
	get_tree().change_scene_to_packed(settings)
 

func _on_quit_pressed() -> void:
	#get_tree().paused = false
	#get_tree().change_scene_to_packed(menu)
	get_tree().quit()
	

func handle_connection_signals():
	resume_button.button_down.connect(_on_resume_pressed)
	settings_button.button_down.connect(_on_settings_pressed)
	quit_button.button_down.connect(_on_quit_pressed)


