class_name HotkeyRebindButton
extends Control

@onready var label = $HBoxContainer/action_button as Label
@onready var button = $HBoxContainer/Button as Button

@export var action_name : String = "up"

func _ready():
	set_process_unhandled_key_input(false)
	set_action_name()
	set_text_for_key()
	
func set_action_name() -> void:
	label.text = "unassigned" 
	
	match action_name:
		"up":
			label.text = "UP"
		"down":
			label.text = "DOWN"
		"left":
			label.text = "LEFT"
		"right":
			label.text = "RIGHT"
		"mb_left":
			label.text = "MB LEFT"
		"mb_right":
			label.text = "MB RIGHT"
		"i":
			label.text = "I"
		"e":
			label.text = "E"
		"space":
			label.text = "SPACEBAR"
		"pause":
			label.text = "ESC" 
 			
func set_text_for_key() -> void:
	var action_events = InputMap.action_get_events(action_name)
	var action_event = action_events[0]
	if action_event is InputEventKey:
		var action_keycode = OS.get_keycode_string(action_event.physical_keycode)
		button.text = "%s" % action_keycode
	elif action_event is InputEventMouseButton:
		var action_keycode = OS.get_keycode_string(action_event.button_index)
		button.text = "%s" % "Mouse Button " + str(action_event.button_index)

func _on_button_toggled(button_pressed):
	if button_pressed:
		button.text = "Press any key....."
		set_process_unhandled_key_input(button_pressed)
		
		for i in get_tree().get_nodes_in_group("hotkey_button"):
			if i.action_name != self.action_name:
				i.button.toggle_mode = false
				i.set_process_unhandled_key_input(false)
	else:
		for i in get_tree().get_nodes_in_group("hotkey_button"):
			if i.action_name != self.action_name:
				i.button.toggle_mode = true
				i.set_process_unhandled_key_input(false)
		set_text_for_key()
		
func _unhandled_key_input(event):
	_rebind_action_key(event)
	button.button_pressed = false
	
func _rebind_action_key(event) -> void:
	var is_duplicate = false
	var action_event = event
	var action_keycode = OS.get_keycode_string(action_event.physical_keycode)
	for i in get_tree().get_nodes_in_group("hotkey_button"):
			if i.action_name != self.action_name:
				if i.button.text == "%s" %action_keycode:
					is_duplicate = true
					break
	if not is_duplicate:
		InputMap.action_erase_events(action_name)
		InputMap.action_add_event(action_name,event)
		set_process_unhandled_key_input(false)
		set_text_for_key()
		set_action_name()
	








