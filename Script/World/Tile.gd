class_name Tile extends Object

enum Type { STONE, COAL, COPPER, IRON, DIAMOND, ALUMINUM, BEDROCK, AIR }

enum Mask {
	UP = 0b0001,
	RIGHT = 0b0010,
	DOWN = 0b0100,
	LEFT = 0b1000,
	TUNNEL_V = 0b1010,
	TUNNEL_H = 0b0101
}

static var TILE_SET_MAP = {
	Type.COAL: Vector2i(1, 1),
	Type.COPPER: Vector2i(3, 1),
	Type.IRON: Vector2i(0, 0),
	Type.DIAMOND: Vector2i(1, 0),
	Type.ALUMINUM: Vector2i(0, 1),
	Type.BEDROCK: Vector2i(0, 2),
	Type.AIR: Vector2i(2, 2)
}

var type: Type
var durability: float  # 0 - 1 Information about block durability


func _init(_type: Type):
	type = _type
	durability = 1
	

static func obs_to_str(observation: int):
	var res = ""
	if observation & Mask.UP:
		res += "UP "
	if observation & Mask.RIGHT:
		res += "RIGHT "
	if observation & Mask.DOWN:
		res += "DOWN "
	if observation & Mask.LEFT:
		res += "LEFT "
	return res

### Observation is byte with bits Left, Down, Right, Up
static func get_stone_variant(observation: int):
	var wall_count = 0
	var rotation = -1
	var gap = false
	for offset in range(4):
		if (observation >> offset) & 1:
			wall_count += 1
			if rotation == -1 or gap:
				rotation = offset
			gap = false
		elif wall_count > 0:
			gap = true
	
	var is_tunnel = observation & Mask.TUNNEL_H == Mask.TUNNEL_H or observation & Mask.TUNNEL_V == Mask.TUNNEL_V
	
	var x = 0
	rotation = rotation if rotation > -1 else 0
	match wall_count:
		0:
			x = 4
		1:
			x = 0
		2:
			x = 5 if is_tunnel else 1
			if x == 5:
				rotation = 1 if observation & Mask.TUNNEL_H == Mask.TUNNEL_H else 0
		3:
			x = 2
		4:
			x = 3
	
	print("Result vector: ", x, ":", rotation)
	return Vector2i(x, rotation)


static func get_random_ore_tile() -> Type:
	var rng = RandomNumberGenerator.new()
	match rng.randi_range(1, 7):
		2:
			return Type.COPPER
		3:
			return Type.IRON
		4:
			return Type.DIAMOND
		5:
			return Type.ALUMINUM
	return Type.COAL


func is_breakable() -> bool:
	return type != Type.AIR and type != Type.BEDROCK


func get_tileset_cords() -> Vector2i:
	return TILE_SET_MAP[type]


func clear_tile():
	type = Type.AIR
