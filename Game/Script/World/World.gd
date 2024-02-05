class_name World extends Node

@onready var tileMap: TileMap = $StoneTileMap
@onready var player: Player = $Player


var item_scene_factory = preload("res://Scene/Items/item.tscn")

var rng = RandomNumberGenerator.new()

@export var MAX_CHUNK_SIZE: int = 8

var CellSize = Vector2(16, 16)

@export var MAP_WIDTH: int = 20
@export var MAP_HEIGHT: int = 20

@export var SAFE_ZONE_RADIUS: int = 4

@export var GENERATOR_DESTROY_CHANCE: float = 0.1
@export var GENERATOR_SPAWN_CHANCE: float = 1

@export var MAX_ITERATION: int = 10
@export var MAX_GENERATORS: int = 10

var grid: Array[Array] = []

var generators: Array[ChunkGenerator] = []

func is_in_safe_zone(x: int, y: int) -> bool:
	var t_x = x - MAP_WIDTH/2
	var t_y = y - MAP_HEIGHT/2
	var radius = sqrt(pow(t_x, 2) + pow(t_y,2))
	return radius < SAFE_ZONE_RADIUS



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
			if (x == 0 or x == MAP_WIDTH) or (y == 0 or y == MAP_HEIGHT):
				grid[x].append(Tile.new(Tile.Type.BEDROCK))
			elif is_in_safe_zone(x,y):
				grid[x].append(Tile.new(Tile.Type.AIR))
			else:
				grid[x].append(Tile.new(Tile.Type.STONE))

func init_generators():
	var generator = ChunkGenerator.new(get_safe_random_position(), Tile.get_random_ore_tile())
	generators = [generator]

func create_chunks():
	init_generators()
	var iteration: int = 0
	while iteration < MAX_ITERATION:
		# Random: Maybe destroy generator?
		for generator in generators:
			if rng.randf() < GENERATOR_DESTROY_CHANCE:
				generators.erase(generator);
				break; # Destroy only one generator per iteration
		
		# Spawn new generator, with chance
		for _g in generators:
			if rng.randf() < GENERATOR_SPAWN_CHANCE and generators.size() < MAX_GENERATORS:
				var generator = ChunkGenerator.new(get_safe_random_position(), Tile.get_random_ore_tile())
				generators.append(generator)
	
		# Advance generator
		for generator in generators:
			generator.rotate()
			var target = generator.get_taget()
			if (target.x >= 1 and 
				target.x < MAP_WIDTH-1 and
				target.y >= 1 and
				target.y < MAP_HEIGHT-1 and 
				grid[target.x][target.y].type == Tile.Type.STONE):
					generator.mine()
					grid[target.x][target.y].type = generator.tile_type
					if generator.chunk_size == MAX_CHUNK_SIZE:
						generators.erase(generator)
		
		iteration += 1

func spawn_tiles():
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			var t_x = x - MAP_WIDTH/2
			var t_y = y - MAP_HEIGHT/2
			if !grid[x][y].is_air():
				tileMap.set_cell(0, Vector2(t_x, t_y), 0, grid[x][y].get_cords())

# Called when the node enters the scene tree for the first time.
func _ready():
	init_grid()
	init_generators()
	create_chunks()
	spawn_tiles()

func mine_block(target: Vector2):
	player.animate_breaking()
	tileMap.erase_cell(0, target + Vector2(MAP_WIDTH/2, MAP_HEIGHT/2))
	var tile = grid[target.x][target.y]
	
	var item_scene = item_scene_factory.instantiate()
	item_scene.item = InventoryItem.new(tile.type)
	item_scene.global_position = player.global_position
	get_parent().add_child(item_scene)
	
	grid[target.x][target.y].type = Tile.Type.AIR

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var playerCoords = tileMap.local_to_map(player.global_position)
	var target = playerCoords + player.get_target() - Vector2i(MAP_WIDTH/2, MAP_HEIGHT/2)
	if Input.is_action_just_pressed("e") and grid[target.x][target.y].is_breakable():
		mine_block(target)
	
	if Input.is_action_just_pressed("space"):
		init_grid()
		init_generators()
		create_chunks()
		spawn_tiles()
		
		
