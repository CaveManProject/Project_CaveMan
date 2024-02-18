class_name World extends Node

@onready var tileMap: TileMap = $StoneTileMap
@onready var player: Player = $Player

var item_scene_factory = preload("res://Scene/Items/item.tscn")

var rng = RandomNumberGenerator.new()

@export var MAX_CHUNK_SIZE: int = 8

var CellSize = Vector2i(16, 16)

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
	var radius = sqrt(pow(x - MAP_WIDTH / 2, 2) + pow(y - MAP_HEIGHT / 2, 2))
	return radius < SAFE_ZONE_RADIUS


func get_safe_random_position() -> Vector2i:
	var ran_x = rng.randi_range(1, MAP_WIDTH - 1)
	var ran_y = rng.randi_range(1, MAP_HEIGHT - 1)
	while is_in_safe_zone(ran_x, ran_y):
		ran_x = rng.randi_range(1, MAP_WIDTH - 1)
		ran_y = rng.randi_range(1, MAP_HEIGHT - 1)
	return Vector2i(ran_x, ran_y)


func init_grid():
	grid = []
	for x in MAP_WIDTH:
		grid.append([])
		for y in MAP_HEIGHT:
			if (x == 0 or x == MAP_WIDTH - 1) or (y == 0 or y == MAP_HEIGHT - 1):
				grid[x].append(Tile.new(Tile.Type.BEDROCK))
			elif is_in_safe_zone(x, y):
				grid[x].append(Tile.new(Tile.Type.AIR))
			else:
				grid[x].append(Tile.new(Tile.Type.STONE))


func create_chunks():
	var iteration: int = 0
	while iteration < MAX_ITERATION:
		# Random: Maybe destroy generator?
		for generator in generators:
			if rng.randf() < GENERATOR_DESTROY_CHANCE:
				generators.erase(generator)
				break  # Destroy only one generator per iteration

		# Spawn new generator, with chance
		if rng.randf() < GENERATOR_SPAWN_CHANCE and generators.size() < MAX_GENERATORS:
			var generator = ChunkGenerator.new(get_safe_random_position())
			generators.append(generator)

		# Move generators
		for generator in generators:
			var target = generator.rotate()
			if (
				target.x >= 1
				and target.x < MAP_WIDTH - 1
				and target.y >= 1
				and target.y < MAP_HEIGHT - 1
				and grid[target.x][target.y].type == Tile.Type.STONE
			):
				generator.mine()
				grid[target.x][target.y].type = generator.tile_type
				if generator.chunk_size == MAX_CHUNK_SIZE:
					generators.erase(generator)
		iteration += 1


func render_tiles():
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			if grid[x][y].type == Tile.Type.STONE:
				var observation = get_observation(Vector2i(x, y))
				var variant =  Tile.get_stone_variant(observation)
				tileMap.set_cell(0, Vector2i(x, y),  1, variant)
			else:
				tileMap.set_cell(0, Vector2i(x, y), 0, grid[x][y].get_tileset_cords())


# Called when the node enters the scene tree for the first time.
func _ready():
	player.global_position += Vector2(Vector2i(MAP_WIDTH / 2, MAP_HEIGHT / 2) * CellSize)
	init_grid()
	create_chunks()
	render_tiles()


# Returns byte where bits are Left, Down, Right, Up
func get_observation(target: Vector2i) -> int:
	var observation = 0b0000
	if len(grid) > target.x + 1:
		observation |= Tile.Mask.RIGHT if grid[target.x+1][target.y].type == Tile.Type.AIR else 0
	if target.x - 1 > 0:
		observation |= Tile.Mask.LEFT if  grid[target.x-1][target.y].type == Tile.Type.AIR else 0
	if len(grid[target.x]) > target.y + 1:
		observation |= Tile.Mask.DOWN if grid[target.x][target.y + 1].type == Tile.Type.AIR else 0
	if target.y - 1 > 0:
		observation |= Tile.Mask.UP if grid[target.x][target.y - 1].type == Tile.Type.AIR else 0
	return observation


func update_tile(target: Vector2i):
	if target.x <= 0 or  target.x > len(grid) :
		print("Off map", target.x, "x", target.y, " Map width: ", len(grid))
		return
	if target.y <= 0 or target.y > len(grid[target.x]):
		print("Off map", target.x, "x", target.y, " Map height: ", len(grid[target.x]))
		return
	if grid[target.x][target.y].type != Tile.Type.STONE:
		print("Not stone", target.x, "x", target.y)
		return
	var observation = get_observation(target)
		
	var variant =  Tile.get_stone_variant(observation)
		
	tileMap.set_cell(0, target, 1, variant)
	


func update_surrounding(target: Vector2i):
	print("Updating surroundings RIGHT")
	update_tile(target + Vector2i(1,0))
	print("Updating surroundings LEFT")
	update_tile(target + Vector2i(-1,0))
	print("Updating surroundings DOWN")
	update_tile(target + Vector2i(0,1))
	print("Updating surroundings UP")
	update_tile(target + Vector2i(0,-1))


func mine_block(target: Vector2i, dir: Vector2i):
	player.animate_breaking()
	tileMap.set_cell(0, target, 0, Vector2(2, 2))
	var tile = grid[target.x][target.y]

	var item_scene = item_scene_factory.instantiate()
	item_scene.item = InventoryItem.new(tile.type)
	item_scene.global_position = player.global_position
	get_parent().add_child(item_scene)
	item_scene.drop(dir)

	grid[target.x][target.y].clear_tile()
	update_surrounding(target)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var playerCoords = tileMap.local_to_map(player.global_position)
	var target = playerCoords + player.get_direction()
	if Input.is_action_just_pressed("e") and grid[target.x][target.y].is_breakable():
		mine_block(target, player.get_direction())

	if Input.is_action_just_pressed("space"):
		init_grid()
		create_chunks()
		render_tiles()
