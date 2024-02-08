class_name Tile extends Object

enum Type { STONE, COAL, COPPER, IRON, DIAMOND, ALUMINUM, BEDROCK, AIR }

static var TILE_SET_MAP = {
	Type.STONE: Vector2i(2, 2),
	Type.COAL: Vector2i(1, 1),
	Type.COPPER: Vector2i(3, 1),
	Type.IRON: Vector2i(0, 0),
	Type.DIAMOND: Vector2i(1, 0),
	Type.ALUMINUM: Vector2i(0, 1),
	Type.BEDROCK: Vector2i(0, 2),
	Type.AIR: Vector2i(-1, -1)
}

var type: Type
var durability: float  # 0 - 1 Information about block durability


func _init(_type: Type):
	type = _type
	durability = 1


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
