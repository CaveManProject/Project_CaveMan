class_name ItemBody extends StaticBody2D

func _ready():
	createItem()

func createItem():
	$AnimationPlayer.play("Loot_Anim")
	await get_tree().create_timer(1.5).timeout
	$AnimationPlayer.play("fade")
	get_tree().create_timer(0.3).timeout
	queue_free()
