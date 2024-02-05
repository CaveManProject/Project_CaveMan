class_name ChunkGenerator extends Object

enum Rotation {
	UP,
	DOWN,
	RIGHT,
	LEFT
}


var DIRECTIONS = {
	Player.Rotation.LEFT: Vector2(-1, 0),
	Player.Rotation.RIGHT: Vector2(1, 0),
	Player.Rotation.UP: Vector2(0, -1),
	Player.Rotation.DOWN: Vector2(0, 1)
}

var pos: Vector2
var dir: Vector2
var chunk_size: int
var tile_type: Tile.Type

var rng = RandomNumberGenerator.new()

func _init(_pos: Vector2, _tile_type: Tile.Type):
	tile_type = _tile_type
	pos = _pos
	dir = get_random_direction()
	chunk_size = 0
	


func get_random_direction() -> Vector2:
	return DIRECTIONS[rng.randi()%4]

func rotate():
	dir = get_random_direction()
	
func get_taget() -> Vector2:
	return pos + dir

func mine():
	pos += dir
	chunk_size += 1
