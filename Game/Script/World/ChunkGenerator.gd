class_name ChunkGenerator extends Object

enum Rotation {
	UP,
	DOWN,
	RIGHT,
	LEFT
}

static var DIRECTIONS = {
	Player.Rotation.LEFT: Vector2i(-1, 0),
	Player.Rotation.RIGHT: Vector2i(1, 0),
	Player.Rotation.UP: Vector2i(0, -1),
	Player.Rotation.DOWN: Vector2i(0, 1)
}

var pos: Vector2i
var dir: Vector2i
var tile_type: Tile.Type
var chunk_size: int

var rng = RandomNumberGenerator.new()

func _init(_pos: Vector2i):
	pos = _pos
	dir = get_random_direction()
	tile_type = Tile.get_random_ore_tile()
	chunk_size = 0
	


func get_random_direction() -> Vector2i:
	return DIRECTIONS[rng.randi()%4]

func rotate() -> Vector2i:
	dir = get_random_direction()
	return pos + dir

func mine():
	pos += dir
	chunk_size += 1
