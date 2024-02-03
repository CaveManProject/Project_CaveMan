class_name PlayerBody extends CharacterBody2D

enum PlayerState {IDLE, WALKING}

@export var speed = 100
@export var player_state: PlayerState = PlayerState.IDLE

@export var inv: Inventory

func _physics_process(delta: float):
	var direction = Input.get_vector("left", "right", "up", "down")
	if direction.x == 0 and direction.y ==0:
		player_state = PlayerState.IDLE
	elif direction.x !=0 or direction.y !=0:
		player_state = PlayerState.WALKING
	
	velocity = direction * speed
	move_and_slide()
	
	play_anim(direction)
	
func play_anim(dir: Vector2):
	if player_state == PlayerState.IDLE:
		$AnimatedSprite2D.play("Idle")
	if player_state == PlayerState.WALKING:
		if dir.y == -1:
			$AnimatedSprite2D.play("Mv_up")
		if dir.x == 1:
			$AnimatedSprite2D.play("Mv_right")
		if dir.y == 1:
			$AnimatedSprite2D.play("Mv_down")
		if dir.x == -1:
			$AnimatedSprite2D.play("Mv_left")
		if dir.x > 0.5 and dir.y < -0.5:
			$AnimatedSprite2D.play("Mv_upR")
		if dir.x > 0.5 and dir.y > 0.5:
			$AnimatedSprite2D.play("Mv_downR")
		if dir.x < -0.5 and dir.y > 0.5:
			$AnimatedSprite2D.play("Mv_downL")
		if dir.x < -0.5 and dir.y < -0.5:
			$AnimatedSprite2D.play("Mv_upL")
			


func collect(item: InventoryItem):
	inv.insert(item)
	
