class_name Item extends Node2D

@export var item: InventoryItem
@onready var sprite: Sprite2D = $Sprite2D

static func get_texture(type: Tile.Type) -> Texture2D:
	match type:
		Tile.Type.COAL: return load("res://Assets/Items/Coal.png")
		Tile.Type.STONE: return load("res://Assets/Items/Stone_chunk.png")
		Tile.Type.COPPER: return load("res://Assets/Items/Copper.png")
		Tile.Type.IRON: return load("res://Assets/Items/Iron.png")
		Tile.Type.ALUMINUM: return load("res://Assets/Items/Aluminum.png")
		Tile.Type.DIAMOND: return load("res://Assets/Items/Diamond.png")
	return null


func _on_interactable_body_entered(body: CharacterBody2D):
	if body is Player:
		body.collect(item)
		await get_tree().create_timer(0.1).timeout
		self.queue_free()
		

func _ready():
	sprite.texture = get_texture(item.type)
	
