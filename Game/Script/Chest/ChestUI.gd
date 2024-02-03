class_name ChestUI extends Control

var is_open: bool = false


# Called when the node enters the scene tree for the first time.
func _ready():
	visible = is_open


# Called every frame. 'delta' is the elapsed time since the previous frame.
func toggle():
	if is_open:
		close()
	else:
		open()

func close():
	visible = false
	is_open = false

func open():
	visible = true
	is_open = true
	
