class_name World extends Node

@onready var tileMap: TileMap = $StoneTileMap
@onready var player: PlayerBody = $Player


var item_scene_factory = preload("res://Scene/Items/item.tscn")

enum TileType {
	STONE,
	COAL,
	COPPER,
	IRON,
	DIAMOND,
	ALUMINUM,
	BEDROCK,
	AIR
}


func get_random_ore_tile() -> TileType:
	match rng.randi_range(1, 7):
		2: return TileType.COPPER
		3: return TileType.IRON
		4: return TileType.DIAMOND
		5: return TileType.ALUMINUM
	return TileType.COAL
	

func type_to_cords(type: TileType) -> Vector2:
	match type:
		TileType.STONE: return Vector2(2, 2)
		TileType.COAL: return Vector2(1, 1)
		TileType.COPPER: return Vector2(3,1)
		TileType.IRON: return Vector2(0, 0)
		TileType.DIAMOND: return Vector2(1, 0)
		TileType.ALUMINUM: return Vector2(0, 1)
		TileType.BEDROCK: return Vector2(0, 2)
	return Vector2(-1, -1)

func cords_to_type(cords: Vector2) -> TileType: 
	match cords:
		Vector2(2, 2): return TileType.STONE
		Vector2(1, 1): return TileType.COAL
		Vector2(3, 1): return TileType.COPPER
		Vector2(0, 0): return TileType.IRON
		Vector2(1, 0): return TileType.DIAMOND
		Vector2(0, 1): return TileType.ALUMINUM
		Vector2(0, 2): return TileType.BEDROCK
	return TileType.AIR

var rng = RandomNumberGenerator.new()

@export var MAX_CHUNK_SIZE: int = 8

class ChunkGenerator:
	var pos: Vector2
	var dir: Vector2
	var chunk_size: int
	var tile_type: TileType

var CellSize = Vector2(16, 16)

@export var MAP_WIDTH: int = 20
@export var MAP_HEIGHT: int = 20

@export var SAFE_ZONE_RADIUS: int = 4

@export var GENERATOR_DESTROY_CHANCE: float = 0.1
@export var GENERATOR_SPAWN_CHANCE: float = 1

@export var MAX_ITERATION: int = 10
@export var MAX_GENERATORS: int = 10

var DIRECTIONS = {
	PlayerBody.PlayerRotation.LEFT: Vector2(-1, 0),
	PlayerBody.PlayerRotation.RIGHT: Vector2(1, 0),
	PlayerBody.PlayerRotation.UP: Vector2(0, -1),
	PlayerBody.PlayerRotation.DOWN: Vector2(0, 1),
	PlayerBody.PlayerRotation.IDLE: Vector2(0, 0)
}

var grid = []

var generators: Array[ChunkGenerator] = []

func is_in_safe_zone(x: int, y: int) -> bool:
	var t_x = x - MAP_WIDTH/2
	var t_y = y - MAP_HEIGHT/2
	var radius = sqrt(pow(t_x, 2) + pow(t_y,2))
	return radius < SAFE_ZONE_RADIUS

func get_random_direction() -> Vector2:
	return DIRECTIONS[rng.randi()%4]

func get_safe_random_position() -> Vector2:
	var ran_x = rng.randi_range(1, MAP_WIDTH-1)
	var ran_y = rng.randi_range(1, MAP_HEIGHT-1)
	while is_in_safe_zone(ran_x, ran_y):
		ran_x = rng.randi_range(1, MAP_WIDTH-1)
		ran_y = rng.randi_range(1, MAP_HEIGHT-1)
	return Vector2(ran_x, ran_y)

func init_grid():
	grid = []
	for x in MAP_WIDTH:
		grid.append([])
		for y in MAP_HEIGHT:
			if (x == 0 or x == MAP_WIDTH - 1) or (y == 0 or y == MAP_HEIGHT - 1):
				grid[x].append(TileType.BEDROCK)
			elif is_in_safe_zone(x,y):
				grid[x].append(TileType.AIR)
			else:
				grid[x].append(TileType.STONE)

func init_generators():
	var generator: ChunkGenerator = ChunkGenerator.new()
	generator.dir = get_random_direction()
	generator.pos = get_safe_random_position()
	generator.chunk_size = 0
	generator.tile_type = get_random_ore_tile()
	generators = [generator]

func create_chunks():
	init_generators()
	var iteration: int = 0
	while iteration < MAX_ITERATION:
		# Change direction, with chance
		for generator in generators:
			generator.dir = get_random_direction()
			
		# Random: Maybe destroy generator?
		for generator in generators:
			if rng.randf() < GENERATOR_DESTROY_CHANCE:
				generators.erase(generator);
				break; # Destroy only one generator per iteration
		
		# Spawn new generator, with chance
		for _g in generators:
			if rng.randf() < GENERATOR_SPAWN_CHANCE and generators.size() < MAX_GENERATORS:
				var generator = ChunkGenerator.new()
				generator.dir = get_random_direction()
				generator.pos = get_safe_random_position()
				generator.chunk_size = 0
				generator.tile_type = get_random_ore_tile()
				generators.append(generator)
	
		# Advance generator
		for generator in generators:
			if (generator.pos.x + generator.dir.x >= 1 and 
				generator.pos.x + generator.dir.x < MAP_WIDTH-1 and
				generator.pos.y + generator.dir.y >= 1 and
				generator.pos.y + generator.dir.y < MAP_HEIGHT-1):
					
					generator.pos += generator.dir
					if grid[generator.pos.x][generator.pos.y] == TileType.STONE:
						grid[generator.pos.x][generator.pos.y] = generator.tile_type
						generator.chunk_size += 1
						
						if generator.chunk_size == MAX_CHUNK_SIZE:
							generators.erase(generator)
		
		iteration += 1

func spawn_tiles():
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			var t_x = x - MAP_WIDTH/2
			var t_y = y - MAP_HEIGHT/2
			if grid[x][y] != TileType.AIR:
				tileMap.set_cell(0, Vector2(t_x, t_y), 0, type_to_cords(grid[x][y]))

# Called when the node enters the scene tree for the first time.
func _ready():
	init_grid()
	init_generators()
	create_chunks()
	spawn_tiles()

func block_in_radius(playerCords: Vector2, rotation: PlayerBody.PlayerRotation) -> bool:
	var target = playerCords + DIRECTIONS[rotation] - Vector2(MAP_WIDTH/2, MAP_HEIGHT/2)
	var tileType = grid[target.x][target.y]
	
	return tileType != TileType.AIR and tileType != TileType.BEDROCK

func mine_block(playerCords: Vector2, rotation: PlayerBody.PlayerRotation):
	tileMap.erase_cell(0, playerCords + DIRECTIONS[rotation])
	var target = playerCords + DIRECTIONS[rotation] - Vector2(MAP_WIDTH/2, MAP_HEIGHT/2)
	var tileType = grid[target.x][target.y]
	
	grid[target.x][target.y] = TileType.AIR
	
	var item = InventoryItem.new()
	item.amount = 1
	item.type = tileType
	
	var item_scene = item_scene_factory.instantiate()
	item_scene.item = item
	item_scene.global_position = player.global_position
	get_parent().add_child(item_scene)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var playerCoords = tileMap.local_to_map(player.global_position)
	if Input.is_action_just_pressed("e") and block_in_radius(playerCoords, player.player_direction) :
		mine_block(playerCoords, player.player_direction)
	
	if Input.is_action_just_pressed("space"):
		print("Generating")
		init_grid()
		init_generators()
		create_chunks()
		spawn_tiles()
		
		
