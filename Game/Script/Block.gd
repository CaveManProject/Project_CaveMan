class_name Block extends Node2D

enum BlockState {PLAIN, WITH_ORE}

@export var state: BlockState = BlockState.WITH_ORE
@export var item: Item

var player_in_area: bool = false
var item_scene_factory = preload("res://Scene/Items/item.tscn")

var player: PlayerBody = null

func _ready():
	if state == BlockState.PLAIN:
		$Spawn_Timer.start()
		
func _process(delta):
	if state == BlockState.PLAIN:
		$AnimatedSprite2D.play("plain")
	if state == BlockState.WITH_ORE:
		$AnimatedSprite2D.play("with_ore")
		if player_in_area:
			if Input.is_action_just_pressed("e"):
				state = BlockState.PLAIN
				drop_chunk()


func _on_pickable_area_body_entered(body: CharacterBody2D):
	if body is PlayerBody:
		player_in_area = true
		player = body


func _on_pickable_area_body_exited(body: CharacterBody2D):
	if body is PlayerBody:
		player_in_area = false
		
func _on_spawn_timer_timeout():
	if state == BlockState.PLAIN:
		state = BlockState.WITH_ORE
		
func drop_chunk():
	var item_scene = item_scene_factory.instantiate()
	item_scene.global_position = $Marker2D.global_position
	get_parent().add_child(item_scene)
	player.collect(item)
	await get_tree().create_timer(3).timeout
	$Spawn_Timer.start()
