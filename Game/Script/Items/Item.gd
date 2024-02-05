class_name Item extends Node2D

@export var item: InventoryItem
@onready var sprite: Sprite2D = $Sprite2D

static var TEXTURE_MAP = {
	Tile.Type.COAL:  load("res://Assets/Items/Coal.png"),
	Tile.Type.STONE:  load("res://Assets/Items/Stone_chunk.png"),
	Tile.Type.COPPER:  load("res://Assets/Items/Copper.png"),
	Tile.Type.IRON:  load("res://Assets/Items/Iron.png"),
	Tile.Type.ALUMINUM:  load("res://Assets/Items/Aluminum.png"),
	Tile.Type.DIAMOND:  load("res://Assets/Items/Diamond.png")
}

func _on_interactable_body_entered(body: CharacterBody2D):
	if body is Player:
		body.collect(item)
		await get_tree().create_timer(0.1).timeout
		self.queue_free()
		

func _ready():
	sprite.texture = TEXTURE_MAP[item.type]
	
