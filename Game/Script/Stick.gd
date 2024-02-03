class_name Stick extends StaticBody2D

@export var item: Item
var player: PlayerBody = null

func _on_interactable_area_body_entered(body: CharacterBody2D):
	if body is PlayerBody:
		player = body
		playercollect()
		await get_tree().create_timer(0.1).timeout
		self.queue_free()
		
func playercollect():
	player.collect(item)
