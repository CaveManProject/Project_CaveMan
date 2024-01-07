extends Node2D

var state = "no chunk"
var player_in_area = false
var chunkpiece = preload("res://scene/chunk_collectable.tscn")

@export var item: InvItem
var player = null

func _ready():
	if state == "no chunk":
		$Spawn_Timer.start()
		
func _process(delta):
	if state == "no chunk":
		$AnimatedSprite2D.play("no_chunk")
	if state == "chunk":
		$AnimatedSprite2D.play("chunk")
		if player_in_area:
			if Input.is_action_just_pressed("e"):
				state = "no chunk"
				drop_chunk()


func _on_pickable_area_body_entered(body):
	if body.has_method("player"):
		player_in_area = true
		player = body


func _on_pickable_area_body_exited(body):
	if body.has_method("player"):
		player_in_area = false
		
func _on_spawn_timer_timeout():
	if state == "no chunk":
		state = "chunk"
		
func drop_chunk():
	var chunk_instance = chunkpiece.instantiate()
	chunk_instance.global_position = $Marker2D.global_position
	get_parent().add_child(chunk_instance)
	player.collect(item)
	await get_tree().create_timer(3).timeout
	$Spawn_Timer.start()
