class_name Tile extends Object


enum Type {
	STONE,
	COAL,
	COPPER,
	IRON,
	DIAMOND,
	ALUMINUM,
	BEDROCK,
	AIR
}

var type: Type
var durability: float # 0 - 1 Information about block durability

func _init(_type: Type):
	type = _type
	durability = 1

static func get_random_ore_tile() -> Type:
	var rng = RandomNumberGenerator.new()
	match rng.randi_range(1, 7):
		2: return Type.COPPER
		3: return Type.IRON
		4: return Type.DIAMOND
		5: return Type.ALUMINUM
	return Type.COAL
	

static func type_to_cords(type: Type) -> Vector2:
	match type:
		Type.STONE: return Vector2(2, 2)
		Type.COAL: return Vector2(1, 1)
		Type.COPPER: return Vector2(3,1)
		Type.IRON: return Vector2(0, 0)
		Type.DIAMOND: return Vector2(1, 0)
		Type.ALUMINUM: return Vector2(0, 1)
		Type.BEDROCK: return Vector2(0, 2)
	return Vector2(-1, -1)

static func cords_to_type(cords: Vector2) -> Type: 
	match cords:
		Vector2(2, 2): return Type.STONE
		Vector2(1, 1): return Type.COAL
		Vector2(3, 1): return Type.COPPER
		Vector2(0, 0): return Type.IRON
		Vector2(1, 0): return Type.DIAMOND
		Vector2(0, 1): return Type.ALUMINUM
		Vector2(0, 2): return Type.BEDROCK
	return Type.AIR

func is_air() -> bool:
	return type == Type.AIR
	
func is_breakable() -> bool:
	return type != Type.AIR and type != Type.BEDROCK
	
func get_cords() -> Vector2:
	return type_to_cords(type)
