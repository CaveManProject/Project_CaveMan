extends Control

@onready var check_button = $HBoxContainer/ON_OFF as CheckButton

func _on_check_button_toggled(toggled_on):
	if toggled_on == true:
		DisplayServer.VSYNC_ENABLED
	else:
		DisplayServer.VSYNC_DISABLED
