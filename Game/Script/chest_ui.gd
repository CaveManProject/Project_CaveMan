extends Control

var is_open = false


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("e"):
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
	
