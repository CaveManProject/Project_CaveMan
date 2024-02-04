class_name Item extends Node2D

@export var item: InventoryItem
@onready var sprite: Sprite2D = $Sprite2D

static func get_texture(type: World.TileType) -> Texture2D:
	match type:
		World.TileType.COAL: return load("res://Assets/Items/Coal.png")
		World.TileType.STONE: return load("res://Assets/Items/Stone_chunk.png")
		World.TileType.COPPER: return load("res://Assets/Items/Copper.png")
		World.TileType.IRON: return load("res://Assets/Items/Iron.png")
		World.TileType.ALUMINUM: return load("res://Assets/Items/Aluminum.png")
		World.TileType.DIAMOND: return load("res://Assets/Items/Diamond.png")
	return null


func _on_interactable_body_entered(body: CharacterBody2D):
	if body is PlayerBody:
		body.collect(item)
		await get_tree().create_timer(0.1).timeout
		self.queue_free()
		

func _ready():
	sprite.texture = get_texture(item.type)
	
