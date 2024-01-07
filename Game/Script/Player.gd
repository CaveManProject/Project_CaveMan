extends CharacterBody2D

var speed = 100
var player_state

@export var inv: Inv

func _physics_process(delta):
	var direction = Input.get_vector("left", "right", "up", "down")
	if direction.x == 0 and direction.y ==0:
		player_state = "idle"
	elif direction.x !=0 or direction.y !=0:
		player_state = "walking"
	
	velocity = direction * speed
	move_and_slide()
	
	play_anim(direction)
	
func play_anim(dir):
	if player_state == "idle":
		$AnimatedSprite2D.play("Idle")
	if player_state == "walking":
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
			
func player():
	pass


func collect(item):
	inv.insert(item)
	
