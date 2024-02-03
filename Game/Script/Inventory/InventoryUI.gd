class_name InventoryUI extends Control

@onready var inv_data: Inventory = preload("res://Data/inventory.tres")
@onready var items: Array = $NinePatchRect/GridContainer.get_children()

var is_open = false

func _ready():
	z_index = 100
	inv_data.update.connect(update_slots)
	update_slots()
	close()

func update_slots():
	for i in range(min(inv_data.items.size(), items.size())):
		items[i].update(inv_data.items[i])

func _process(delta: float):
	if Input.is_action_just_pressed("i"):
		if is_open:
			close()
		else:
			open()

func open():
	visible = true
	is_open = true

func close():
	visible = false
	is_open	= false
