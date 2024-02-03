class_name Item extends Node2D

@export var item: InventoryItem

func _on_interactable_body_entered(body: CharacterBody2D):
	print("Collision")
	if body is PlayerBody:
		body.collect(item)
		await get_tree().create_timer(0.1).timeout
		self.queue_free()
