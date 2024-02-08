class_name Player extends CharacterBody2D

enum State { IDLE, WALKING }

enum Rotation { UP, DOWN, RIGHT, LEFT, IDLE }

static var DIRECTIONS = {
	Rotation.LEFT: Vector2i(-1, 0),
	Rotation.RIGHT: Vector2i(1, 0),
	Rotation.UP: Vector2i(0, -1),
	Rotation.DOWN: Vector2i(0, 1),
	Rotation.IDLE: Vector2i(0, 0)
}

@onready var animation: AnimatedSprite2D = $AnimatedSprite2D

@export var speed = 100
@export var player_state: State = State.IDLE
@export var inv: Inventory

var p_rotation: Rotation = Rotation.IDLE


func _physics_process(delta: float):
	var direction = Input.get_vector("left", "right", "up", "down")
	if direction.x == 0 and direction.y == 0:
		player_state = State.IDLE
	elif direction.x != 0 or direction.y != 0:
		player_state = State.WALKING

	velocity = direction * speed
	move_and_slide()

	animate_movement(direction)


func animate_movement(dir: Vector2):
	if animation.is_playing() and animation.animation.begins_with("Att_"):
		return
	p_rotation = Rotation.IDLE
	if player_state == State.IDLE:
		animation.play("Idle")
	if player_state == State.WALKING:
		if dir.x > 0.5 and dir.y < -0.5:
			animation.play("Mv_upR")
		elif dir.x > 0.5 and dir.y > 0.5:
			animation.play("Mv_downR")
		elif dir.x < -.05 and dir.y > 0.5:
			animation.play("Mv_downL")
		elif dir.x < -.05 and dir.y < -.05:
			animation.play("Mv_upL")
		elif dir.y < -.05:
			p_rotation = Rotation.UP
			animation.play("Mv_up")
		elif dir.x == 1:
			p_rotation = Rotation.RIGHT
			animation.play("Mv_right")
		elif dir.y == 1:
			p_rotation = Rotation.DOWN
			animation.play("Mv_down")
		elif dir.x == -1:
			p_rotation = Rotation.LEFT
			animation.play("Mv_left")


func animate_breaking():
	match p_rotation:
		Rotation.RIGHT:
			animation.play("Att_right")
		Rotation.LEFT:
			animation.play("Att_left")
		Rotation.UP:
			animation.play("Att_up")
		Rotation.DOWN:
			animation.play("Att_down")


func collect(item: InventoryItem):
	inv.insert(item)


func get_direction() -> Vector2i:
	return DIRECTIONS[p_rotation]
