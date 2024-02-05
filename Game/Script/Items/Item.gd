class_name Item extends Node2D

@export var item: InventoryItem
@onready var animation: AnimationPlayer = $AnimationPlayer
@onready var sprite: Sprite2D = $Sprite2D

static var TEXTURE_MAP = {
	Tile.Type.COAL:  load("res://Assets/Items/Coal.png"),
	Tile.Type.STONE:  load("res://Assets/Items/Stone_chunk.png"),
	Tile.Type.COPPER:  load("res://Assets/Items/Copper.png"),
	Tile.Type.IRON:  load("res://Assets/Items/Iron.png"),
	Tile.Type.ALUMINUM:  load("res://Assets/Items/Aluminum.png"),
	Tile.Type.DIAMOND:  load("res://Assets/Items/Diamond.png")
}

func drop(dir: Vector2i):
	if dir.x == 1:
		animation.play("drop_left")
	if dir.x == -1:
		animation.play("drop_right")
	if dir.y == 1:
		animation.play("drop_up")
	if dir.y == -1:
		animation.play("drop_down")

var player: Player = null

func _on_interactable_body_entered(body: CharacterBody2D):
	if body is Player:
		player = body

func _on_interactable_body_exited(body: CharacterBody2D):
	if body is Player:
		player = null

func _process(delta: float):
	if player and !animation.is_playing():
		player.collect(item)
		await get_tree().create_timer(0.1).timeout
		self.queue_free()
		
		

func _ready():
	sprite.texture = TEXTURE_MAP[item.type]
	


